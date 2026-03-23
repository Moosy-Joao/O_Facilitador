namespace facilitador_api.Model
{
    public class Purchase : BaseModel
    {
        private int ClientId { get; set; }
        private decimal Value { get; set; }
        private string? Description { get; set; }
    }
}
