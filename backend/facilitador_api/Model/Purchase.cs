namespace facilitador_api.Model
{
    public class Purchase : BaseModel
    {
        public int ClientId { get; set; }
        public decimal Value { get; set; }
        public string? Description { get; set; }
    }
}
