using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RockyApp.Models
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Наименование")]
        [Required] // Обязательное заполнение этого поля
        public string Name { get; set; }
    }
}