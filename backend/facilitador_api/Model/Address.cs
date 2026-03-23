namespace facilitador_api.Model
{
    public class Address : BaseModel
    {
        private string Country { get; set; }
        private string State { get; set; }
        private string City { get; set; }
        private string Neighborhood { get; set; }
        private string Street { get; set; }
        private int? Number { get; set; }
        private int CEP { get; set; }
    }
}
