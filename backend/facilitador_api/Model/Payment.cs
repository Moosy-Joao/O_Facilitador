namespace facilitador_api.Model
{
    public class Payment : BaseModel
    {
        public int ClientId { get; private set; }
        public decimal PaymentValue { get; private set; }
        public string? Observation { get; private set; }
    }
}
