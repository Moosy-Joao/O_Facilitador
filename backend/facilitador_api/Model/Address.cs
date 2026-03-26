namespace facilitador_api.Model
{
    public class Address : BaseModel
    {
        public string Country { get; private set; }
        public string State { get; private set; }
        public string City { get; private set; }
        public string Neighborhood { get; private set; }
        public string Street { get; private set; }
        public int? Number { get; private set; }
        public int CEP { get; private set; }
    }
}
