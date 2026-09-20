namespace PDMSRestServices.Models
{
    public class EncryptionPyalod
    {
        public string KeyValue { get; set; }

        public string KeySecret { get; set; }

        public string KeyName { get; set; }
    }


    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
