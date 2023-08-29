using AutoShop.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AutoShop.ViewModels
{
    public class AutoModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(30, ErrorMessage = "Имя не должно превышать 30 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(300, ErrorMessage = "Информация не должна превышать 300 символов")]
        public string Info { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [MaxFileSize(1048576)]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [Range(0, float.MaxValue, ErrorMessage = "Цена должна быть выше 0")]
        public float Price { get; set; }
    }
}
