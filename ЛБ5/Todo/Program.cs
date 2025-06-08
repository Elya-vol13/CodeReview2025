/*
    TODO FIXME SUMMARY:
        - Между блоками кода должны быть 2 пустые строки;
        - В конце кода должна быть 1 пустая строка;
        - Убрать лишние очевидные комментарии;
        - Строки длиннее 100 символов нужно разделять, выполняя перенос через конкатенацию (+) строк;
*/

using System;
using System.IO;

namespace ConsoleApp1_csharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "LR5-var2.xls";

            var databaseController = new DatabaseController(filePath); //Создание экземпляра

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
                Console.Write("\nВыберите действие:\n1. Просмотр базы данных\n2. Удаление  элемента из таблицы\n3. Корректировка элемента в таблице\n4. Добавление элемента в таблицу\n5. Запросы\n6. Выход\n---");
                switch (Console.ReadLine())
                {
                    //Просмотр базы данных
                    case "1":
                        databaseController.ViewDatabase();
                        break;
                    //Удаление  элемента из таблицы
                    case "2":
                        Console.WriteLine("Выберите таблицу:\n1. Животные\n2. Покупатели\n3. Продажи");
                        switch(Console.ReadLine())
                        {
                            case "1":
                                databaseController.DeleteAnimal();
                                break;
                            case "2":
                                databaseController.DeleteCustomer();
                                break;
                            case "3":
                                databaseController.DeleteSale();
                                break;
                            default:
                                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                                break;
                        }
                        break;
                    //Корректировка элемента в таблице
                    case "3":
                        Console.WriteLine("Выберите таблицу:\n1. Животные\n2. Покупатели\n3. Продажи");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                databaseController.UpdateAnimal();
                                break;
                            case "2":
                                databaseController.UpdateCustomer();
                                break;
                            case "3":
                                databaseController.UpdateSale();
                                break;
                            default:
                                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                                break;
                        }
                        break;
                    //Добавление элемента в таблицу
                    case "4":
                        Console.WriteLine("Выберите таблицу:\n1. Животные\n2. Покупатели\n3. Продажи");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                databaseController.AddAnimal();
                                break;
                            case "2":
                                databaseController.AddCustomer();
                                break;
                            case "3":
                                databaseController.AddSale();
                                break;
                            default:
                                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                                break;
                        }
                        break;
                    //Запросы
                    case "5":
                        Console.WriteLine("Введите код запроса:\n1. Вывод все животных вида 'Собака'\n2. Получение количества покупок для покупателя с именем 'Amanda Mclaughlin'\n3. Получение видов животных, которых покупали люди в возрасте больше 50\n4. Получение суммы купленных пород 'Ара' людьми из Уфы");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                databaseController.ExecuteQueries(1);
                                break;
                            case "2":
                                databaseController.ExecuteQueries(2);
                                break;
                            case "3":
                                databaseController.ExecuteQueries(3);
                                break;
                            case "4":
                                databaseController.ExecuteQueries(4);
                                break;
                            default:
                                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                                break;
                        }
                        break;
                    //Выход из программы
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