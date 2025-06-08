using System;
using System.IO;


namespace PetStore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "LR5-var2.xls";

            var databaseController = new DatabaseController(filePath);

            string isNewLogFile;
            while (true)
            {
                Console.WriteLine("Записывать ли в новый файл или дописывать в существующий? (n/d)");
                isNewLogFile = Console.ReadLine();
                if (isNewLogFile == "n" || isNewLogFile == "d")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Нет такого варианта!");
                }
            }

            bool isNewLogFile1 = isNewLogFile == "n";
            string logFilePath = "log.txt";
            if (isNewLogFile1 && File.Exists(logFilePath))
                File.Delete(logFilePath);

            while (true)
            {
                Console.Write
                (
                    "\nВыберите действие:\n" +
                    "1. Просмотр базы данных\n" +
                    "2. Удаление элемента из таблицы\n" +
                    "3. Корректировка элемента в таблице\n" +
                    "4. Добавление элемента в таблицу\n" +
                    "5. Запросы\n" +
                    "6. Выход\n" +
                    "---"
                );
                switch (Console.ReadLine())
                {
                    case "1":
                        databaseController.viewDatabase();
                        break;

                    case "2":
                        Console.WriteLine("Выберите таблицу:\n1. Животные\n2. Покупатели\n3. Продажи");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                databaseController.deleteAnimal();
                                break;
                            case "2":
                                databaseController.deleteCustomer();
                                break;
                            case "3":
                                databaseController.deleteSale();
                                break;
                            default:
                                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                                break;
                        }
                        break;

                    case "3":
                        Console.WriteLine("Выберите таблицу:\n1. Животные\n2. Покупатели\n3. Продажи");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                databaseController.updateAnimal();
                                break;
                            case "2":
                                databaseController.updateCustomer();
                                break;
                            case "3":
                                databaseController.updateSale();
                                break;
                            default:
                                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                                break;
                        }
                        break;

                    case "4":
                        Console.WriteLine("Выберите таблицу:\n1. Животные\n2. Покупатели\n3. Продажи");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                databaseController.addAnimal();
                                break;
                            case "2":
                                databaseController.addCustomer();
                                break;
                            case "3":
                                databaseController.addSale();
                                break;
                            default:
                                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                                break;
                        }
                        break;

                    case "5":
                        Console.WriteLine
                        (
                            "Введите код запроса:\n" +
                            "1. Вывод все животных вида 'Собака'\n" +
                            "2. Получение количества покупок для покупателя с именем 'Amanda Mclaughlin'\n" +
                            "3. Получение видов животных, которых покупали люди в возрасте больше 50\n" +
                            "4. Получение суммы купленных пород 'Ара' людьми из Уфы"
                        );
                        switch (Console.ReadLine())
                        {
                            case "1":
                                databaseController.executeQueries(1);
                                break;
                            case "2":
                                databaseController.executeQueries(2);
                                break;
                            case "3":
                                databaseController.executeQueries(3);
                                break;
                            case "4":
                                databaseController.executeQueries(4);
                                break;
                            default:
                                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                                break;
                        }
                        break;

                    case "6":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
    }
}
