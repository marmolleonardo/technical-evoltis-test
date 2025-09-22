namespace technical_tests_backend_ssr.Dtos
{
    public class ProductReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ProductCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class ProductUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
