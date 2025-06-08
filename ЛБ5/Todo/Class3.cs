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
    public class Sale
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }

        public Sale(int id, int animalId, int customerId, DateTime date, decimal price)
        {
            Id = id;
            AnimalId = animalId;
            CustomerId = customerId;
            Date = date;
            Price = price;
        }

        public override string ToString()
        {
            return string.Format("{0,-5} {1,-15} {2,-15} {3, -12} {4, -6}", Id, AnimalId, CustomerId, Date.ToShortDateString(), Price); //ID, ID Животного, ID Покупателя, Дата, Цена
        }
    }
}