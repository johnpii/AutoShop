﻿namespace AutoShop.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public byte[] Photo { get; set; }

        public float Price { get; set; }
    }
}
