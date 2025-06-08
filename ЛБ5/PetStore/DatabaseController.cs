using System;
using System.IO;

namespace PetStore
{
    internal class DatabaseController
    {
        private readonly DatabaseService databaseService;

        public DatabaseController(string _filePath) => databaseService = new DatabaseService(_filePath);


        public void viewDatabase()
        {
            var animals = databaseService.getAnimals();

            Console.WriteLine("\nЖивотные:");
            Console.WriteLine("{0,-5} {1,-15} {2,-20}", "ID", "Вид", "Порода");
            foreach (var animal in animals)
                Console.WriteLine(animal);

            var customers = databaseService.getCustomers();

            Console.WriteLine("\nПокупатели:");
            Console.WriteLine("{0,-5} {1,-30} {2,-9} {3, -30}", "ID", "Имя", "Возраст", "Адрес");
            foreach (var customer in customers)
                Console.WriteLine(customer);

            var sales = databaseService.getSales();

            Console.WriteLine("\nПродажи:");
            Console.WriteLine("{0,-5} {1,-15} {2,-15} {3, -12} {4, -6}", "ID", "ID Животного", "ID Покупателя", "Дата", "Цена");
            foreach (var sale in sales)
                Console.WriteLine(sale);
            logAction("Выведена база данных.");
        }


        public void deleteAnimal()
        {
            Console.WriteLine("Введите ID животного для удаления:");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var animal = databaseService.getAnimalById(id);
                if (animal != null)
                {
                    databaseService.deleteAnimal(id);
                    logAction($"Удалено животное с ID {id}");
                    Console.WriteLine($"Животное с ID {id} удалено.");
                }
                else
                {
                    Console.WriteLine($"Животное с ID {id} не найдено.");
                }
            }
            else
                Console.WriteLine("Некорректный ID.");
        }


        public void updateAnimal()
        {
            Console.WriteLine("Введите ID животного для корректировки:");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var animal = databaseService.getAnimalById(id);
                if (animal != null)
                {
                    Console.WriteLine("Введите новый вид:");
                    string species = Console.ReadLine();
                    Console.WriteLine("Введите новую породу:");
                    string breed = Console.ReadLine();

                    var updatedAnimal = new Animal(id, species, breed);
                    databaseService.updateAnimal(id, updatedAnimal);
                    logAction($"Обновлено животное с ID:{id}:  Вид: {species}, Порода: {breed}");
                    Console.WriteLine($"Животное с ID {id} обновлено.");
                }
                else
                {
                    Console.WriteLine($"Животное с ID {id} не найдено.");
                }
            }
            else
                Console.WriteLine("Некорректный ID.");
        }


        public void addAnimal()
        {
            Console.WriteLine("Введите ID нового животного:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var animal = databaseService.getAnimalById(id);
                if (animal != null)
                {
                    Console.WriteLine("Животное с таким ID уже есть!");
                }
                else
                {
                    Console.WriteLine("Введите вид:");
                    string species = Console.ReadLine();
                    Console.WriteLine("Введите породу:");
                    string breed = Console.ReadLine();

                    var newAnimal = new Animal(id, species, breed);
                    databaseService.addAnimal(newAnimal);
                    logAction($"Добавлено новое животное: {id}  Вид: {species}, Порода: {breed}");
                    Console.WriteLine($"Животное с ID {id} добавлено.");
                }
            }
            else
                Console.WriteLine("Некорректный ID.");
        }


        public void deleteCustomer()
        {
            Console.WriteLine("Введите ID покупателя для удаления:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = databaseService.getCustomerById(id);
                if (customer != null)
                {
                    databaseService.deleteCustomer(id);
                    logAction($"Удален покупатель с ID {id}");
                    Console.WriteLine($"Покупатель с ID {id} удален.");
                }
                else
                {
                    Console.WriteLine($"Покупатель с ID {id} не найден.");
                }
            }
            else
                Console.WriteLine("Некорректный ID.");
        }


        public void updateCustomer()
        {
            Console.WriteLine("Введите ID покупателя для корректировки:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = databaseService.getCustomerById(id);
                if (customer != null)
                {
                    Console.WriteLine("Введите новое имя:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Введите новый возраст:");
                    int age;
                    while (true)
                    {
                        if (int.TryParse(Console.ReadLine(), out age))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                    }
                    Console.WriteLine("Введите новый адрес:");
                    string address = Console.ReadLine();

                    var updatedCustomer = new Customer(id, name, age, address);
                    databaseService.updateCustomer(id, updatedCustomer);
                    logAction($"Обновлён покупатель с ID:{id}:  Имя: {name}, Возраст: {age}, Адрес: {address}");
                    Console.WriteLine($"Покупатель с ID {id} обновлён.");
                }
                else
                {
                    Console.WriteLine($"Покупатель с ID {id} не найден.");
                }
            }
            else
                Console.WriteLine("Некорректный ID.");
        }


        public void addCustomer()
        {
            Console.WriteLine("Введите ID нового покупателя:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = databaseService.getCustomerById(id);
                if (customer != null)
                {
                    Console.WriteLine("Покупатель с таким ID уже есть!");
                }
                else
                {
                    Console.WriteLine("Введите имя:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Введите возраст:");
                    int age;
                    while (true)
                    {
                        if (int.TryParse(Console.ReadLine(), out age))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                    }
                    Console.WriteLine("Введите адрес:");
                    string address = Console.ReadLine();

                    var newCustomer = new Customer(id, name, age, address);
                    databaseService.addCustomer(newCustomer);
                    logAction($"Добавлен новый покупатель: {id},  Имя: {name}, Возраст: {age}, Адрес: {address}");
                    Console.WriteLine($"Покупатель с ID {id} добавлен.");
                }
            }
            else
                Console.WriteLine("Некорректный ID.");
        }


        public void deleteSale()
        {
            Console.WriteLine("Введите ID продажи для удаления:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var sale = databaseService.getSaleById(id);
                if (sale != null)
                {
                    databaseService.deleteSale(id);
                    logAction($"Удалена продажа с {id}");
                    Console.WriteLine($"Продажа с ID {id} удалена.");
                }
                else
                {
                    Console.WriteLine($"Продажа с ID {id} не найдена.");
                }
            }
            else
                Console.WriteLine("Некорректный ID.");
        }


        public void updateSale()
        {
            Console.WriteLine("Введите ID продажи для корректировки:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var sale = databaseService.getSaleById(id);
                if (sale != null)
                {
                    Console.WriteLine("Введите новый ID животного:");
                    int animalId;
                    while (true)
                    {
                        if (int.TryParse(Console.ReadLine(), out animalId))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                    }
                    Console.WriteLine("Введите новый ID покупателя:");
                    int customerId;
                    while (true)
                    {
                        if (int.TryParse(Console.ReadLine(), out customerId))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                    }
                    Console.WriteLine("Введите новую дату (в формате ГГГГ-ММ-ДД):");
                    DateTime date;
                    while (true)
                    {
                        if (DateTime.TryParse(Console.ReadLine(), out date))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                    }
                    Console.WriteLine("Введите новую цену:");
                    decimal price;
                    while (true)
                    {
                        if (decimal.TryParse(Console.ReadLine(), out price))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                    }

                    var updatedSale = new Sale(id, animalId, customerId, date, price);
                    databaseService.updateSale(id, updatedSale);
                    logAction($"Обновлена продажа с ID:{id}:  ID животного: {animalId}, ID покупателя: {customerId}, Дата: {date}, Цена: {price}");
                    Console.WriteLine($"Продажа с ID {id} обновлена.");
                }
                else
                {
                    Console.WriteLine($"Покупатель с ID {id} не найден.");
                }
            }
            else
                Console.WriteLine("Некорректный ID.");
        }


        public void addSale()
        {
            Console.WriteLine("Введите ID новой продажи:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var sale = databaseService.getSaleById(id);
                if (sale != null)
                {
                    Console.WriteLine("Продажа с таким ID уже есть!");
                }
                else
                {
                    Console.WriteLine("Введите ID животного:");
                    int animalId;
                    while (true)
                    {
                        if (int.TryParse(Console.ReadLine(), out animalId))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                    }
                    Console.WriteLine("Введите ID покупателя:");
                    int customerId;
                    while (true)
                    {
                        if (int.TryParse(Console.ReadLine(), out customerId))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                    }
                    Console.WriteLine("Введите дату (в формате ГГГГ-ММ-ДД):");
                    DateTime date;
                    while (true)
                    {
                        if (DateTime.TryParse(Console.ReadLine(), out date))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                    }
                    Console.WriteLine("Введите цену:");
                    decimal price;
                    while (true)
                    {
                        if (decimal.TryParse(Console.ReadLine(), out price))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Неверный ввод!");
                        }
                    }

                    var newSale = new Sale(id, animalId, customerId, date, price);
                    databaseService.addSale(newSale);
                    logAction($"Добавлена новая продажа: {id}, ID животного: {animalId}, ID покупателя: {customerId}, Дата: {date}, Цена: {price}");
                    Console.WriteLine($"Продажа с ID {id} добавлена.");
                }
            }
            else
                Console.WriteLine("Некорректный ID.");
        }


        public void executeQueries(int _i)
        {
            switch (_i)
            {
                case 1:
                    var animalsOfType = databaseService.getAnimalsBySpecies("Собака");
                    Console.WriteLine("\nЖивотные вида 'Собака':");
                    Console.WriteLine("{0,-5} {1,-15} {2,-20}", "ID", "Вид", "Порода");
                    foreach (var animal in animalsOfType)
                    {
                        Console.WriteLine(animal);
                    }
                    logAction("Выполнен запрос 1.");
                    break;
                case 2:
                    int salesCount = databaseService.getSalesCountByCustomerName("Amanda Mclaughlin");
                    Console.WriteLine($"\nКоличество продаж для покупателя 'Amanda Mclaughlin': {salesCount}");
                    logAction("Выполнен запрос 2.");
                    break;
                case 3:
                    var speciesPurchasedBySeniors = databaseService.getAnimalSpeciesPurchasedBySeniors();
                    Console.WriteLine("\nВиды животных, которых покупали люди в возрасте больше 50:");
                    foreach (var species in speciesPurchasedBySeniors)
                    {
                        Console.WriteLine(species);
                    }
                    logAction("Выполнен запрос 3.");
                    break;
                case 4:
                    decimal totalAraPrice = databaseService.getTotalPriceOfAraPurchasedByUfaResidents();
                    Console.WriteLine($"\nСумма купленных пород 'Ара' людьми с адресом 'Уфа': {totalAraPrice}");
                    logAction("Выполнен запрос 4.");
                    break;
            }

        }


        private void logAction(string message)
        {
            File.AppendAllText("log.txt", $"{DateTime.Now}: {message}\n");
        }
    }
}
