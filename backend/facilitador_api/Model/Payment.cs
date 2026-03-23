namespace facilitador_api.Model
{
    public class Payment : BaseModel
    {
        private int ClientId { get; set; }
        private decimal PaymentValue { get; set; }
        private string? Observation { get; set; }
    }
}
