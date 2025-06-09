using System;


namespace NewProject_PL
{
    /// <summary>
    /// Класс для хранения данных отчета о выданных книгах
    /// </summary>
    /// <remarks>
    /// Содержит информацию о:
    /// - Читателе (ФИО)
    /// - Книге (ISBN)
    /// - Датах выдачи и возврата
    /// - Статусе возврата
    /// </remarks>
    class Report
    {
        private string lastName;
        private string firstName;
        private string isbn;
        private DateTime issuanceDate;
        private DateTime returnDate;
        private string expiredStatus;

        /// <summary>
        /// Фамилия читателя
        /// </summary>
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        /// <summary>
        /// Имя читателя
        /// </summary>
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        /// <summary>
        /// ISBN книги
        /// </summary>
        public string ISBN
        {
            get { return isbn; }
            set { isbn = value; }
        }

        /// <summary>
        /// Дата выдачи книги
        /// </summary>
        public DateTime IssuanceDate
        {
            get { return issuanceDate; }
            set { issuanceDate = value; }
        }

        /// <summary>
        /// Дата возврата книги
        /// </summary>
        public DateTime ReturnDate
        {
            get { return returnDate; }
            set { returnDate = value; }
        }

        /// <summary>
        /// Статус возврата (Просрочено/Не просрочено)
        /// </summary>
        public string ExpiredStatus
        {
            get { return expiredStatus; }
            set { expiredStatus = value; }
        }
    }
}
