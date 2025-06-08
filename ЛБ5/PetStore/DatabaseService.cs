using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Cells;

namespace PetStore
{
    public class DatabaseService
    {
        private readonly string filePath;

        public DatabaseService(string _filePath) => filePath = _filePath;

        public List<Animal> getAnimals() => readAnimals();

        public List<Customer> getCustomers() => readCustomers();

        public List<Sale> getSales() => readSales();

        public void deleteAnimal(int _id) => removeAnimalFromTable(_id);

        public void deleteCustomer(int _id) => removeCustomerFromTable(_id);

        public void deleteSale(int _id) => removeSaleFromTable(_id);

        public void updateAnimal(int _id, Animal _updatedAnimal) => updateAnimalInTable(_id, _updatedAnimal);

        public void updateCustomer(int _id, Customer _updatedCustomer) => updateCustomerInTable(_id, _updatedCustomer);

        public void updateSale(int _id, Sale _updatedSale) => updateSaleInTable(_id, _updatedSale);

        public void addAnimal(Animal _animal) => addAnimalToTable(_animal);

        public void addCustomer(Customer _customer) => addCustomerToTable(_customer);

        public void addSale(Sale _sale) => addSaleToTable(_sale);


        public List<Animal> getAnimalsBySpecies(string _species)
        {
            return readAnimals().Where(a => a.Species.Equals(_species, StringComparison.OrdinalIgnoreCase)).ToList();

        }


        public int? getCustomerIdByName(string _name)
        {
            var customers = readCustomers();
            var customer = customers.FirstOrDefault(c => c.Name.Equals(_name, StringComparison.OrdinalIgnoreCase));
            return customer?.Id;
        }


        public int getSalesCountByCustomerName(string _customerName)
        {
            int? customerId = getCustomerIdByName(_customerName);
            return readSales().Count(s => s.CustomerId == customerId.Value);
        }


        public List<string> getAnimalSpeciesPurchasedBySeniors()
        {
            var sales = readSales();
            var customers = readCustomers();
            var animals = readAnimals();

            var speciesPurchasedBySeniors = new List<string>();

            var seniorCustomers = customers.Where(c => c.Age > 50).Select(c => c.Id).ToHashSet();

            foreach (var sale in sales)
            {
                if (seniorCustomers.Contains(sale.CustomerId))
                {
                    var animal = animals.FirstOrDefault(a => a.Id == sale.AnimalId);
                    if (animal != null && !speciesPurchasedBySeniors.Contains(animal.Species))
                    {
                        speciesPurchasedBySeniors.Add(animal.Species);
                    }
                }
            }

            return speciesPurchasedBySeniors;
        }


        public decimal getTotalPriceOfAraPurchasedByUfaResidents()
        {
            var sales = readSales();
            var customers = readCustomers();
            var animals = readAnimals();

            decimal totalPrice = 0;

            var ufaResidents = customers.Where(c => c.Address.Equals("Уфа", StringComparison.OrdinalIgnoreCase)).Select(c => c.Id).ToHashSet();

            foreach (var sale in sales)
            {
                if (ufaResidents.Contains(sale.CustomerId))
                {
                    var animal = animals.FirstOrDefault(a => a.Id == sale.AnimalId);
                    if (animal != null && animal.Breed.Equals("Ара", StringComparison.OrdinalIgnoreCase))
                    {
                        totalPrice += sale.Price;
                    }
                }
            }

            return totalPrice;
        }


        private List<Animal> readAnimals()
        {
            var animals = new List<Animal>();
            var workbook = new Workbook(filePath);
            var worksheet = workbook.Worksheets[0];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                int id = Convert.ToInt32(worksheet.Cells[i, 0].Value);
                string species = worksheet.Cells[i, 1].Value.ToString();
                string breed = worksheet.Cells[i, 2].Value.ToString();
                animals.Add(new Animal(id, species, breed));
            }
            return animals;
        }


        private void removeAnimalFromTable(int _id)
        {
            var workbook = new Workbook(filePath);
            var worksheet = workbook.Worksheets[0];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                if (Convert.ToInt32(worksheet.Cells[i, 0].Value) == _id)
                {
                    worksheet.Cells.DeleteRow(i);
                    break;
                }
            }
            workbook.Save(filePath);
        }


        private void updateAnimalInTable(int _id, Animal _updatedAnimal)
        {
            var workbook = new Workbook(filePath);
            var worksheet = workbook.Worksheets[0];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                if (Convert.ToInt32(worksheet.Cells[i, 0].Value) == _id)
                {
                    worksheet.Cells[i, 1].Value = _updatedAnimal.Species;
                    worksheet.Cells[i, 2].Value = _updatedAnimal.Breed;
                    break;
                }
            }
            workbook.Save(filePath);
        }


        private void addAnimalToTable(Animal _animal)
        {
            var workbook = new Workbook(filePath);
            var worksheet = workbook.Worksheets[0];
            int lastRow = worksheet.Cells.MaxDataRow + 1;

            worksheet.Cells[lastRow, 0].Value = _animal.Id;
            worksheet.Cells[lastRow, 1].Value = _animal.Species;
            worksheet.Cells[lastRow, 2].Value = _animal.Breed;

            workbook.Save(filePath);
        }


        public Animal getAnimalById(int _id)
        {
            var animals = readAnimals();
            return animals.FirstOrDefault(a => a.Id == _id);
        }


        private List<Customer> readCustomers()
        {
            var customers = new List<Customer>();
            var workbook = new Workbook(filePath);
            var worksheet = workbook.Worksheets[1];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                int id = Convert.ToInt32(worksheet.Cells[i, 0].Value);
                string name = worksheet.Cells[i, 1].Value.ToString();
                int age = Convert.ToInt32(worksheet.Cells[i, 2].Value);
                string address = worksheet.Cells[i, 3].Value.ToString();
                customers.Add(new Customer(id, name, age, address));
            }
            return customers;
        }


        private void removeCustomerFromTable(int _id)
        {
            var workbook = new Workbook(filePath);
            var worksheet = workbook.Worksheets[1];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                if (Convert.ToInt32(worksheet.Cells[i, 0].Value) == _id)
                {
                    worksheet.Cells.DeleteRow(i);
                    break;
                }
            }
            workbook.Save(filePath);
        }


        private void updateCustomerInTable(int _id, Customer _updatedCustomer)
        {
            var workbook = new Workbook(filePath);
            var worksheet = workbook.Worksheets[1];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                if (Convert.ToInt32(worksheet.Cells[i, 0].Value) == _id)
                {
                    worksheet.Cells[i, 1].Value = _updatedCustomer.Name;
                    worksheet.Cells[i, 2].Value = _updatedCustomer.Age;
                    worksheet.Cells[i, 3].Value = _updatedCustomer.Address;
                    break;
                }
            }
            workbook.Save(filePath);
        }


        private void addCustomerToTable(Customer _customer)
        {
            var workbook = new Workbook(filePath);
            var worksheet = workbook.Worksheets[1];
            int lastRow = worksheet.Cells.MaxDataRow + 1;

            worksheet.Cells[lastRow, 0].Value = _customer.Id;
            worksheet.Cells[lastRow, 1].Value = _customer.Name;
            worksheet.Cells[lastRow, 2].Value = _customer.Age;
            worksheet.Cells[lastRow, 3].Value = _customer.Address;

            workbook.Save(filePath);
        }


        public Customer getCustomerById(int _id)
        {
            var customers = readCustomers();
            return customers.FirstOrDefault(a => a.Id == _id);
        }


        private List<Sale> readSales()
        {
            var sales = new List<Sale>();
            var workbook = new Workbook(filePath);
            var worksheet = workbook.Worksheets[2];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                int id = Convert.ToInt32(worksheet.Cells[i, 0].Value);
                int animalId = Convert.ToInt32(worksheet.Cells[i, 1].Value);
                int customerId = Convert.ToInt32(worksheet.Cells[i, 2].Value);
                DateTime date = Convert.ToDateTime(worksheet.Cells[i, 3].Value);
                decimal price = Convert.ToDecimal(worksheet.Cells[i, 4].Value);
                sales.Add(new Sale(id, animalId, customerId, date, price));
            }
            return sales;
        }


        private void removeSaleFromTable(int _id)
        {
            var workbook = new Workbook(filePath);
            var worksheet = workbook.Worksheets[2];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                if (Convert.ToInt32(worksheet.Cells[i, 0].Value) == _id)
                {
                    worksheet.Cells.DeleteRow(i);
                    break;
                }
            }
            workbook.Save(filePath);
        }


        private void updateSaleInTable(int _id, Sale _updatedSale)
        {
            var workbook = new Workbook(filePath);
            var worksheet = workbook.Worksheets[2];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                if (Convert.ToInt32(worksheet.Cells[i, 0].Value) == _id)
                {
                    worksheet.Cells[i, 1].Value = _updatedSale.AnimalId;
                    worksheet.Cells[i, 2].Value = _updatedSale.CustomerId;
                    worksheet.Cells[i, 3].Value = _updatedSale.Date;
                    worksheet.Cells[i, 4].Value = _updatedSale.Price;
                    break;
                }
            }
            workbook.Save(filePath);
        }


        private void addSaleToTable(Sale _sale)
        {
            var workbook = new Workbook(filePath);
            var worksheet = workbook.Worksheets[2];
            int lastRow = worksheet.Cells.MaxDataRow + 1;

            worksheet.Cells[lastRow, 0].Value = _sale.Id;
            worksheet.Cells[lastRow, 1].Value = _sale.AnimalId;
            worksheet.Cells[lastRow, 2].Value = _sale.CustomerId;
            worksheet.Cells[lastRow, 3].Value = _sale.Date;
            worksheet.Cells[lastRow, 3].SetStyle(new Style() { Number = 14 });
            worksheet.Cells[lastRow, 4].Value = _sale.Price;

            workbook.Save(filePath);
        }


        public Sale getSaleById(int _id)
        {
            var sales = readSales();
            return sales.FirstOrDefault(a => a.Id == _id);
        }
    }
}
