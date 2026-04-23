namespace facilitador_api.Model
{
    public class Address : BaseModel
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public int? Number { get; set; }
        public int CEP { get; set; }
    }
}
