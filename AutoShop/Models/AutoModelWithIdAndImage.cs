namespace AutoShop.Models
{
    public class AutoModelWithIdAndImage
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Info { get; set; }

        public byte[] Photo { get; set; }

        public float Price { get; set; }
    }
}
