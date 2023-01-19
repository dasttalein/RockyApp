using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RockyApp.Models
{
    public class Category
    {
        [Key] // Ключ
        public int Id { get; set; }
        [DisplayName("Наименование")] // Наименование
        [Required] // Обязательное заполнение этого поля
        public string Name { get; set; }
        [DisplayName("Категория")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Порядок отображения категории должен быть > 0")] // Валидация поля 
        public string DisplayOrder { get; set; }
    }
}