using ClothingStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreApi.Services
{
    public class ClothingService : IClothingService
    {
        private readonly AppDbContext _context;

        // Внедрение зависимости через конструктор
        public ClothingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClothingItem>> GetAllItemsAsync()
        {
            return await _context.ClothingItems.ToListAsync();
        }

        public async Task<ClothingItem?> GetItemByIdAsync(int id)
        {
            return await _context.ClothingItems.FindAsync(id);
        }

        // Кастомная обработка: форматирование данных для вывода
        public IEnumerable<ClothingItem> FormatItemsForDisplay(IEnumerable<ClothingItem> items)
        {
            return items
                .Where(item => item.InStock) // Фильтруем только товары в наличии
                .OrderBy(item => item.Price) // Сортируем по цене
                .Select(item => new ClothingItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Category = item.Category,
                    Price = Math.Round(item.Price, 2),
                    Size = item.Size,
                    Color = item.Color,
                    InStock = item.InStock
                });
        }
    }
}