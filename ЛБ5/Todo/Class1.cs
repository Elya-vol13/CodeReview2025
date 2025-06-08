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
    public class Animal
    {
        public int Id { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }

        public Animal(int id, string species, string breed)
        {
            Id = id;
            Species = species;
            Breed = breed;
        }

        public override string ToString()
        {
            return string.Format("{0,-5} {1,-15} {2,-20}", Id, Species, Breed); //ID, Вид, Порода
        }
    }
}