namespace facilitador_api.Model
{
    public class Purchase : BaseModel
    {
        public int ClientId { get; private set; }
        public decimal Value { get; private set; }
        public string? Description { get; private set; }
    }
}
