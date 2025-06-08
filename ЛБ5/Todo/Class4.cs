/*
    TODO FIXME SUMMARY:
        - Между блоками кода должны быть 2 пустые строки;
        - В конце кода должна быть 1 пустая строка;
        - Убрать лишние очевидные комментарии;
        - Переименовать название файла;
        - Переименовать названия методов и их параметры;
        - Переименовать поля;
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Cells; 

namespace ConsoleApp1_csharp
{
    public class DatabaseService
    {
        private readonly string _filePath;

        public DatabaseService(string filePath) => _filePath = filePath;

        //Чтение таблиц
        //Чтение всех животных из таблицы
        public List<Animal> GetAnimals() => ReadAnimals();
        //Чтение всех покупателей из таблицы
        public List<Customer> GetCustomers() => ReadCustomers();
        //Чтение всех продаж из таблицы
        public List<Sale> GetSales() => ReadSales();

        //Удаление элементов
        //Удаление животного по ID
        public void DeleteAnimal(int id) => RemoveAnimalFromTable(id);
        //Удаление покупателя по ID
        public void DeleteCustomer(int id) => RemoveCustomerFromTable(id);
        //Удаление продажи по ID
        public void DeleteSale(int id) => RemoveSaleFromTable(id);

        //Корректировка элементов
        //Корректировка животного по ID
        public void UpdateAnimal(int id, Animal updatedAnimal) => UpdateAnimalInTable(id, updatedAnimal);
        //Корректировка покупателя по ID
        public void UpdateCustomer(int id, Customer updatedCustomer) => UpdateCustomerInTable(id, updatedCustomer);
        //Корректировка продажи по ID
        public void UpdateSale(int id, Sale updatedSale) => UpdateSaleInTable(id, updatedSale);

        //Добавление новых элементов
        //Добавление нового животного 
        public void AddAnimal(Animal animal) => AddAnimalToTable(animal);
        //Добавление нового покупателя 
        public void AddCustomer(Customer customer) => AddCustomerToTable(customer);
        //Добавление новой продажи 
        public void AddSale(Sale sale) => AddSaleToTable(sale);

        //Запросы
        //Запрос 1: Получение всех животных определенного вида (одна таблица, возвращает перечень)
        public List<Animal> GetAnimalsBySpecies(string species) =>
            ReadAnimals().Where(a => a.Species.Equals(species, StringComparison.OrdinalIgnoreCase)).ToList();

        //Запрос 2: Получение количества покупок для покупателя с именем Amanda Mclaughlin(две таблицы, возвращает одно значение)
        //Получение ID покупателя по имени
        public int? GetCustomerIdByName(string name)
        {
            var customers = ReadCustomers();
            var customer = customers.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return customer?.Id; // Возвращаем ID покупателя или null, если не найден
        }

        //Получение количества продаж для покупателя по его ID
        public int GetSalesCountByCustomerName(string customerName)
        {
            int? customerId = GetCustomerIdByName(customerName);
            return ReadSales().Count(s => s.CustomerId == customerId.Value);
        }

        //Запрос 3: Получение видов животных, которых покупали люди в возрасте больше 50(три таблицы, возвращает перечень)
        public List<string> GetAnimalSpeciesPurchasedBySeniors()
        {
            var sales = ReadSales();
            var customers = ReadCustomers();
            var animals = ReadAnimals();

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

        //Запрос 4: Получение суммы купленных пород "Ара" людьми из Уфы(три таблицы, возвращает одно значение)
        public decimal GetTotalPriceOfAraPurchasedByUfaResidents()
        {
            var sales = ReadSales();
            var customers = ReadCustomers();
            var animals = ReadAnimals();

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

        //Животные
        //Чтение животных из таблицы
        private List<Animal> ReadAnimals()
        {
            var animals = new List<Animal>();
            var workbook = new Workbook(_filePath);
            var worksheet = workbook.Worksheets[0];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++) // Пропускаем заголовок
            {
                int id = Convert.ToInt32(worksheet.Cells[i, 0].Value);
                string species = worksheet.Cells[i, 1].Value.ToString();
                string breed = worksheet.Cells[i, 2].Value.ToString();
                animals.Add(new Animal(id, species, breed));
            }
            return animals;
        }

        //Удаление животного из таблицы
        private void RemoveAnimalFromTable(int id)
        {
            var workbook = new Workbook(_filePath);
            var worksheet = workbook.Worksheets[0];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                if (Convert.ToInt32(worksheet.Cells[i, 0].Value) == id)
                {
                    worksheet.Cells.DeleteRow(i);
                    break;
                }
            }
            workbook.Save(_filePath);
        }
        
        //Обновление животного в таблице
        private void UpdateAnimalInTable(int id, Animal updatedAnimal)
        {
            var workbook = new Workbook(_filePath);
            var worksheet = workbook.Worksheets[0];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                if (Convert.ToInt32(worksheet.Cells[i, 0].Value) == id)
                {
                    worksheet.Cells[i, 1].Value = updatedAnimal.Species;
                    worksheet.Cells[i, 2].Value = updatedAnimal.Breed;
                    break;
                }
            }
            workbook.Save(_filePath);
        }

        //Добавление животного в таблицу
        private void AddAnimalToTable(Animal animal)
        {
            var workbook = new Workbook(_filePath);
            var worksheet = workbook.Worksheets[0];
            int lastRow = worksheet.Cells.MaxDataRow + 1;

            worksheet.Cells[lastRow, 0].Value = animal.Id;
            worksheet.Cells[lastRow, 1].Value = animal.Species;
            worksheet.Cells[lastRow, 2].Value = animal.Breed;

            workbook.Save(_filePath);
        }
        
        //Поиск животного с указанным ID
        public Animal GetAnimalById(int id)
        {
            var animals = ReadAnimals(); //Получаем список всех животных
            return animals.FirstOrDefault(a => a.Id == id); //Ищем животное с указанным ID
        }

        //Покупатели
        //Чтение покупателей из таблицы
        private List<Customer> ReadCustomers()
        {
            var customers = new List<Customer>();
            var workbook = new Workbook(_filePath);
            var worksheet = workbook.Worksheets[1];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++) //Пропускаем заголовок
            {
                int id = Convert.ToInt32(worksheet.Cells[i, 0].Value);
                string name = worksheet.Cells[i, 1].Value.ToString();
                int age = Convert.ToInt32(worksheet.Cells[i, 2].Value);
                string address = worksheet.Cells[i, 3].Value.ToString();
                customers.Add(new Customer(id, name, age, address));
            }
            return customers;
        }

        //Удаление покупателя из таблицы
        private void RemoveCustomerFromTable(int id)
        {
            var workbook = new Workbook(_filePath);
            var worksheet = workbook.Worksheets[1];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                if (Convert.ToInt32(worksheet.Cells[i, 0].Value) == id)
                {
                    worksheet.Cells.DeleteRow(i);
                    break;
                }
            }
            workbook.Save(_filePath);
        }

        //Обновление покупателя в таблице
        private void UpdateCustomerInTable(int id, Customer updatedCustomer)
        {
            var workbook = new Workbook(_filePath);
            var worksheet = workbook.Worksheets[1];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                if (Convert.ToInt32(worksheet.Cells[i, 0].Value) == id)
                {
                    worksheet.Cells[i, 1].Value = updatedCustomer.Name;
                    worksheet.Cells[i, 2].Value = updatedCustomer.Age;
                    worksheet.Cells[i, 3].Value = updatedCustomer.Address;
                    break;
                }
            }
            workbook.Save(_filePath);
        }

        //Добавление покупателя в таблицу
        private void AddCustomerToTable(Customer customer)
        {
            var workbook = new Workbook(_filePath);
            var worksheet = workbook.Worksheets[1];
            int lastRow = worksheet.Cells.MaxDataRow + 1;

            worksheet.Cells[lastRow, 0].Value = customer.Id;
            worksheet.Cells[lastRow, 1].Value = customer.Name;
            worksheet.Cells[lastRow, 2].Value = customer.Age;
            worksheet.Cells[lastRow, 3].Value = customer.Address;

            workbook.Save(_filePath);
        }

        //Поиск покупателя с указанным ID
        public Customer GetCustomerById(int id)
        {
            var customers = ReadCustomers(); //Получаем список всех покупателей
            return customers.FirstOrDefault(a => a.Id == id); //Ищем покупателя с указанным ID
        }

        //Продажи
        //Чтение продаж из таблицы
        private List<Sale> ReadSales()
        {
            var sales = new List<Sale>();
            var workbook = new Workbook(_filePath);
            var worksheet = workbook.Worksheets[2];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++) //Пропускаем заголовок
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

        //Удаление продажи из таблицы
        private void RemoveSaleFromTable(int id)
        {
            var workbook = new Workbook(_filePath);
            var worksheet = workbook.Worksheets[2];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                if (Convert.ToInt32(worksheet.Cells[i, 0].Value) == id)
                {
                    worksheet.Cells.DeleteRow(i);
                    break;
                }
            }
            workbook.Save(_filePath);
        }

        //Обновление продажи в таблице
        private void UpdateSaleInTable(int id, Sale updatedSale)
        {
            var workbook = new Workbook(_filePath);
            var worksheet = workbook.Worksheets[2];

            for (int i = 1; i <= worksheet.Cells.MaxDataRow; i++)
            {
                if (Convert.ToInt32(worksheet.Cells[i, 0].Value) == id)
                {
                    worksheet.Cells[i, 1].Value = updatedSale.AnimalId;
                    worksheet.Cells[i, 2].Value = updatedSale.CustomerId;
                    worksheet.Cells[i, 3].Value = updatedSale.Date;
                    worksheet.Cells[i, 4].Value = updatedSale.Price;
                    break;
                }
            }
            workbook.Save(_filePath);
        }

        //Добавление продажи в таблицу
        private void AddSaleToTable(Sale sale)
        {
            var workbook = new Workbook(_filePath);
            var worksheet = workbook.Worksheets[2];
            int lastRow = worksheet.Cells.MaxDataRow + 1;

            worksheet.Cells[lastRow, 0].Value = sale.Id;
            worksheet.Cells[lastRow, 1].Value = sale.AnimalId;
            worksheet.Cells[lastRow, 2].Value = sale.CustomerId;
            worksheet.Cells[lastRow, 3].Value = sale.Date;
            worksheet.Cells[lastRow, 3].SetStyle(new Style() { Number = 14 });
            worksheet.Cells[lastRow, 4].Value = sale.Price;

            workbook.Save(_filePath);
        }

        //Поиск продажи с указанным ID
        public Sale GetSaleById(int id)
        {
            var sales = ReadSales(); //Получаем список всех продаж
            return sales.FirstOrDefault(a => a.Id == id); //Ищем продажу с указанным ID
        }
    }
}