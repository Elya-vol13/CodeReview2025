namespace PetStore
{
    public class Sale
    {
        private int id { get; set; }
        private int animalId { get; set; }
        private int customerId { get; set; }
        private DateTime date { get; set; }
        private decimal price { get; set; }


        public int Id => id;
        public int AnimalId => animalId;
        public int CustomerId => customerId;
        public DateTime Date => date;
        public decimal Price => price;

        public Sale(int _id, int _animalId, int _customerId, DateTime _date, decimal _price)
        {
            id = _id;
            animalId = _animalId;
            customerId = _customerId;
            date = _date;
            price = _price;
        }


        public override string ToString()
        {
            return string.Format("{0,-5} {1,-15} {2,-15} {3, -12} {4, -6}", id, animalId, customerId, date.ToShortDateString(), price);
        }
    }
}
