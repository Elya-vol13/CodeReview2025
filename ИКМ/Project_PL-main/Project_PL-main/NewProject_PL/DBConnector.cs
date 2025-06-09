using MySql.Data.MySqlClient;


namespace NewProject_PL
{
    /// <summary>
    /// Класс для управления подключением к MySQL базе данных
    /// </summary>
    /// <remarks>
    /// Содержит базовые методы для работы с соединением:
    /// - Открытие соединения
    /// - Закрытие соединения 
    /// - Получение текущего соединения
    /// </remarks>
    class DBConnector
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=auth");


        /// <summary>
        /// Открывает соединение с базой данных, если оно закрыто
        /// </summary>
        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }


        /// <summary>
        /// Закрывает соединение с базой данных, если оно открыто
        /// </summary>
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Возвращает текущее соединение с базой данных
        /// </summary>
        /// <returns>Объект MySqlConnection</returns>
        public MySqlConnection getConnection()
        {
            return connection;
        }
    }
}
