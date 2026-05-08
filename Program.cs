using Microsoft.EntityFrameworkCore;
using ClothingStoreApi.Models;
using ClothingStoreApi.Services;

var builder = WebApplication.CreateBuilder(args);

// === 1. Подключение к БД ===
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// === 2. Регистрация сервиса ===
builder.Services.AddScoped<IClothingService, ClothingService>();

// === 3. Добавление контроллеров (для Swagger) ===
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === 4. Конвейер запросов ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// === 5. Инициализация БД при запуске ===
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated(); // Создаёт БД, если не существует
}

// === 6. Endpoint для вывода конфигурации ===
app.MapGet("/api/config", (IConfiguration config) =>
{
    return new
    {
        appName = config["AppSettings:AppName"],
        version = config["AppSettings:Version"],
        maxItems = config.GetValue<int>("AppSettings:MaxItemsPerPage")
    };
});

// === 7. Главный endpoint - получение всех товаров ===
app.MapGet("/api/clothing", async (IClothingService clothingService) =>
{
    // 1. Получаем данные из БД через DbContext
    var items = await clothingService.GetAllItemsAsync();
    
    // 2. Обрабатываем через кастомный сервис
    var formattedItems = clothingService.FormatItemsForDisplay(items);
    
    // 3. Возвращаем JSON
    return Results.Ok(formattedItems);
});

// === 8. Endpoint для получения товара по ID ===
app.MapGet("/api/clothing/{id}", async (IClothingService clothingService, int id) =>
{
    var item = await clothingService.GetItemByIdAsync(id);
    
    if (item == null)
        return Results.NotFound(new { message = $"Товар с ID {id} не найден" });
    
    return Results.Ok(item);
});

app.Run();