namespace SampleWebApiAspNetCore.Entities
{
    public class CSGOEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Damage { get; set; }
        public DateTime Created { get; set; }
    }
}
