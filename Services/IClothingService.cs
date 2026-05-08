using ClothingStoreApi.Models;

namespace ClothingStoreApi.Services
{
    public interface IClothingService
    {
        Task<IEnumerable<ClothingItem>> GetAllItemsAsync();
        Task<ClothingItem?> GetItemByIdAsync(int id);
        IEnumerable<ClothingItem> FormatItemsForDisplay(IEnumerable<ClothingItem> items);
    }
}