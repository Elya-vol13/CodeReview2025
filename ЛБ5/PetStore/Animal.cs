namespace PetStore
{
    public class Animal
    {
        private int id { get; set; }
        private string species { get; set; }
        private string breed { get; set; }


        public int Id => id;
        public string Species => species;
        public string Breed => breed;



        public Animal(int _id, string _species, string _breed)
        {
            id = _id;
            species = _species;
            breed = _breed;
        }


        public override string ToString()
        {
            return string.Format("{0,-5} {1,-15} {2,-20}", id, species, breed);
        }
    }
}
