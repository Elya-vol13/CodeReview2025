/*
    TODO FIXME SUMMARY:
        - Убрать лишние using;
        - Между блоками кода должны быть 2 пустые строки;
        - В конце кода должна быть 1 пустая строка;
        - Убрать лишние очевидные комментарии;
        - Переименовать название файла;
        - Переименовать параметры методов;
        - Переименовать поля и сделать их приватными;
*/

using System;

namespace ConsoleApp1_csharp
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }

        public Customer(int id, string name, int age, string address)
        {
            Id = id;
            Name = name;
            Age = age;
            Address = address;
        }

        public override string ToString()
        {
            return string.Format("{0,-5} {1,-30} {2,-9} {3, -30}", Id, Name, Age, Address); //ID, Имя, Возраст, Адрес
        }
    }
}