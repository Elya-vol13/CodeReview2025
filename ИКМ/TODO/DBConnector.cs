﻿/*
    TODO FIXME SUMMARY:
        - Убрать лишние using;
        - Убрать лишние очевидные комментарии или заменить на docstring;
        - Названия переменных, полей, методов и их параметров не соотвествуют код стайлу;
        - Между методами должны быть 2 пустые строк;
        - Убрать лишние пустые строки;
*/

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewProject_PL
{
    class DBConnector
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=auth");
        //соединение

        //открытие соединения
        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        //закрытие
        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        //получение
        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
