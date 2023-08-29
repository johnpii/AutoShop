﻿using System.ComponentModel.DataAnnotations;

namespace AutoShop.ViewModels
{
    public class RegistModel
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [EmailAddress(ErrorMessage = "Это не валидный email")]
        [StringLength(30, ErrorMessage = "email не должен превышать 30 символов")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Имя пользователя не должно превышать 30 символов и быть меньше 3 символов")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Пароль не должен превышать 30 символов и быть меньше 4 символов")]
        public string Password { get; set; }
    }
}
