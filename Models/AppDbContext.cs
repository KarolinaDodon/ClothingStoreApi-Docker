using Microsoft.EntityFrameworkCore;
using ClothingStoreApi.Models;

namespace ClothingStoreApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // DbSet представляет таблицу в БД
        public DbSet<ClothingItem> ClothingItems { get; set; } = null!;

        // Инициализация тестовыми данными
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClothingItem>().HasData(
                new ClothingItem
                {
                    Id = 1,
                    Name = "Платье вечернее «Алиса»",
                    Category = "Платья",
                    Price = 4990.00m,
                    Size = "M",
                    Color = "Чёрный",
                    InStock = true
                },
                new ClothingItem
                {
                    Id = 2,
                    Name = "Юбка карандаш «Элегант»",
                    Category = "Юбки",
                    Price = 2490.00m,
                    Size = "S",
                    Color = "Тёмно-синий",
                    InStock = true
                },
                new ClothingItem
                {
                    Id = 3,
                    Name = "Блузка шёлковая «Нежность»",
                    Category = "Блузки",
                    Price = 3290.00m,
                    Size = "L",
                    Color = "Бежевый",
                    InStock = false
                }
            );
        }
    }
}