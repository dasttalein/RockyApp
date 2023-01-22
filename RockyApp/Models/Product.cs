using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RockyApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1, double.MaxValue)]
        public double Price { get; set; }
        public string Image { get; set; }
        
        // Связь между Product и Category
        [DisplayName("Тип категории")]
        public int CategoryId { get; set; }
        // Явно указываем что CategoryId ключ из таблицы Category
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}