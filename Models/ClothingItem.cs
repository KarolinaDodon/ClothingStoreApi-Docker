namespace ClothingStoreApi.Models
{
    public class ClothingItem
    {
        public int Id { get; set; }                 
        public string Name { get; set; } = string.Empty; 
        public string Category { get; set; } = string.Empty; 
        public decimal Price { get; set; }           
        public string Size { get; set; } = string.Empty;   
        public string Color { get; set; } = string.Empty;  
        public bool InStock { get; set; } = true;     
    }
}