namespace PetStore
{
    public class Customer
    {
        private int id { get; set; }
        private string name { get; set; }
        private int age { get; set; }
        private string address { get; set; }


        public int Id => id;
        public string Name => name;
        public int Age => age;
        public string Address => address;



        public Customer(int _id, string _name, int _age, string _address)
        {
            id = _id;
            name = _name;
            age = _age;
            address = _address;
        }


        public override string ToString()
        {
            return string.Format("{0,-5} {1,-30} {2,-9} {3, -30}", id, name, age, address);
        }
    }
}
