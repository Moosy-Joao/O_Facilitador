namespace facilitador_api.Model
{
    public class Payment : BaseModel
    {
        public int ClientId { get; set; }
        public decimal PaymentValue { get; set; }
        public string? Observation { get; set; }
    }
}
