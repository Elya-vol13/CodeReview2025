/*
    TODO FIXME SUMMARY:
        - Между блоками кода должны быть 2 пустые строки;
        - В конце кода должна быть 1 пустая строка;
        - Убрать лишние очевидные комментарии;
        - Переименовать название файла;
        - Переименовать поля;
        - Переименовать названия методов и их параметры;
*/


using System;
using System.IO;

namespace ConsoleApp1_csharp
{
    internal class DatabaseController
    {
        private readonly DatabaseService _databaseService;

        public DatabaseController(string filePath) => _databaseService = new DatabaseService(filePath);

        //Вывод базы данных
        public void ViewDatabase()
        {
            var animals = _databaseService.GetAnimals(); //Получаем всех животных

            Console.WriteLine("\nЖивотные:");
            Console.WriteLine("{0,-5} {1,-15} {2,-20}", "ID", "Вид", "Порода");
            foreach (var animal in animals)
                Console.WriteLine(animal); //Выводим информацию о каждом животном

            var customers = _databaseService.GetCustomers(); //Получаем всех покупателей

            Console.WriteLine("\nПокупатели:");
            Console.WriteLine("{0,-5} {1,-30} {2,-9} {3, -30}", "ID", "Имя", "Возраст", "Адрес");
            foreach (var customer in customers)
                Console.WriteLine(customer); //Выводим информацию о каждом покупателе

            var sales = _databaseService.GetSales(); //Получаем все продажи

            Console.WriteLine("\nПродажи:");
            Console.WriteLine("{0,-5} {1,-15} {2,-15} {3, -12} {4, -6}", "ID", "ID Животного", "ID Покупателя", "Дата", "Цена");
            foreach (var sale in sales)
                Console.WriteLine(sale); //Выводим информацию о каждой продаже
            LogAction("Выведена база данных.");
        }

        //Животные
        //Удаление животного по ID
        public void DeleteAnimal()
        {
            Console.WriteLine("Введите ID животного для удаления:");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var animal = _databaseService.GetAnimalById(id);
                if (animal != null)
                {
                    _databaseService.DeleteAnimal(id); //Удаляем животное по ID
                    LogAction($"Удалено животное с ID {id}");
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

        //Корректировка животного по ID
        public void UpdateAnimal()
        {
            Console.WriteLine("Введите ID животного для корректировки:");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var animal = _databaseService.GetAnimalById(id);
                if (animal != null)
                {
                    Console.WriteLine("Введите новый вид:");
                    string species = Console.ReadLine();
                    Console.WriteLine("Введите новую породу:");
                    string breed = Console.ReadLine();

                    var updatedAnimal = new Animal(id, species, breed);
                    _databaseService.UpdateAnimal(id, updatedAnimal); //Обновляем данные о животном
                    LogAction($"Обновлено животное с ID:{id}:  Вид: {species}, Порода: {breed}");
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

        //Добавление нового животного
        public void AddAnimal()
        {
            Console.WriteLine("Введите ID нового животного:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var animal = _databaseService.GetAnimalById(id);
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
                    _databaseService.AddAnimal(newAnimal); //Добавляем новое животное в базу данных
                    LogAction($"Добавлено новое животное: {id}  Вид: {species}, Порода: {breed}");
                    Console.WriteLine($"Животное с ID {id} добавлено.");
                }
            }
            else
                Console.WriteLine("Некорректный ID.");
        }

        //Покупатели
        //Удаление покупателя по ID
        public void DeleteCustomer()
        {
            Console.WriteLine("Введите ID покупателя для удаления:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = _databaseService.GetCustomerById(id);
                if (customer != null)
                {
                    _databaseService.DeleteCustomer(id); //Удаляем покупателя по ID
                    LogAction($"Удален покупатель с ID {id}");
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

        //Корректировка покупателя по ID
        public void UpdateCustomer()
        {
            Console.WriteLine("Введите ID покупателя для корректировки:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = _databaseService.GetCustomerById(id);
                if (customer != null)
                {
                    Console.WriteLine("Введите новое имя:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Введите новый возраст:");
                    int age;
                    while (true)
                    {
                        if(int.TryParse(Console.ReadLine(), out age))
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
                    _databaseService.UpdateCustomer(id, updatedCustomer); //Обновляем данные о покупателе
                    LogAction($"Обновлён покупатель с ID:{id}:  Имя: {name}, Возраст: {age}, Адрес: {address}");
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

        //Добавление нового покупателя
        public void AddCustomer()
        {
            Console.WriteLine("Введите ID нового покупателя:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = _databaseService.GetCustomerById(id);
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
                    _databaseService.AddCustomer(newCustomer); //Добавляем нового покупателя в базу данных
                    LogAction($"Добавлен новый покупатель: {id},  Имя: {name}, Возраст: {age}, Адрес: {address}");
                    Console.WriteLine($"Покупатель с ID {id} добавлен.");
                }
            }
            else
                Console.WriteLine("Некорректный ID.");   
        }

        //Продажи
        //Удаление продажи по ID
        public void DeleteSale()
        {
            Console.WriteLine("Введите ID продажи для удаления:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var sale = _databaseService.GetSaleById(id);
                if (sale != null)
                {
                    _databaseService.DeleteSale(id); //Удаляем продажу по ID
                    LogAction($"Удалена продажа с {id}");
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

        //Корректировка продажи по ID
        public void UpdateSale()
        {
            Console.WriteLine("Введите ID продажи для корректировки:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var sale = _databaseService.GetSaleById(id);
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
                    _databaseService.UpdateSale(id, updatedSale); //Обновляем данные о продаже
                    LogAction($"Обновлена продажа с ID:{id}:  ID животного: {animalId}, ID покупателя: {customerId}, Дата: {date}, Цена: {price}");
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

        //Добавление новой продажи
        public void AddSale()
        {
            Console.WriteLine("Введите ID новой продажи:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var sale = _databaseService.GetSaleById(id);
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
                    _databaseService.AddSale(newSale); //Добавляем новую продажу в базу данных
                    LogAction($"Добавлена новая продажа: {id}, ID животного: {animalId}, ID покупателя: {customerId}, Дата: {date}, Цена: {price}");
                    Console.WriteLine($"Продажа с ID {id} добавлена.");
                }
            }
            else
                Console.WriteLine("Некорректный ID.");
        }
        
        //Запросы
        public void ExecuteQueries(int i)
        {
            switch (i)
            {
                case 1:
                    //Запрос 1: Получение всех животных вида "Собака"
                    var animalsOfType = _databaseService.GetAnimalsBySpecies("Собака");
                    Console.WriteLine("\nЖивотные вида 'Собака':");
                    Console.WriteLine("{0,-5} {1,-15} {2,-20}", "ID", "Вид", "Порода");
                    foreach (var animal in animalsOfType)
                    {
                        Console.WriteLine(animal);
                    }
                    LogAction("Выполнен запрос 1.");
                    break;
                case 2:
                    //Запрос 2: Получение количества покупок для покупателя с именем Amanda Mclaughlin
                    int salesCount = _databaseService.GetSalesCountByCustomerName("Amanda Mclaughlin");
                    Console.WriteLine($"\nКоличество продаж для покупателя 'Amanda Mclaughlin': {salesCount}");
                    LogAction("Выполнен запрос 2.");
                    break;
                case 3:
                    //Запрос 3: Получение видов животных, которых покупали люди в возрасте больше 50
                    var speciesPurchasedBySeniors = _databaseService.GetAnimalSpeciesPurchasedBySeniors();
                    Console.WriteLine("\nВиды животных, которых покупали люди в возрасте больше 50:");
                    foreach (var species in speciesPurchasedBySeniors)
                    {
                        Console.WriteLine(species);
                    }
                    LogAction("Выполнен запрос 3.");
                    break;
                case 4:
                    //Запрос 4: Получение суммы купленных пород "Ара" людьми из Уфы
                    decimal totalAraPrice = _databaseService.GetTotalPriceOfAraPurchasedByUfaResidents();
                    Console.WriteLine($"\nСумма купленных пород 'Ара' людьми с адресом 'Уфа': {totalAraPrice}");
                    LogAction("Выполнен запрос 4.");
                    break;
            }
            
        }

        private void LogAction(string message) => File.AppendAllText("log.txt", $"{DateTime.Now}: {message}\n");
    }
}