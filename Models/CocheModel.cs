namespace WpfApp_Concesionario.Models
{
    public class CocheModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string CarColor { get; set; }
        public int YearOfManufacture { get; set; }
        public string CreditCardType { get; set; }

        public CocheModel(int id,string firstName, string lastName, string country, string carBrand, string carModel, string carColor, int yearOfManufacture, string creditCardType)
        {
            this.Id = id;
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            CarBrand = carBrand;
            CarModel = carModel;
            CarColor = carColor;
            YearOfManufacture = yearOfManufacture;
            CreditCardType = creditCardType;
        }

        public CocheModel()
        {
        }
    }
}
