using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;


namespace NewProject_PL
{
    /// <summary>
    /// Основное окно авторизации в системе библиотеки
    /// </summary>
    /// <remarks>
    /// Обеспечивает:
    /// - Ввод учетных данных (имя и номер читательского билета)
    /// - Проверку роли пользователя
    /// - Перенаправление на соответствующее рабочее окно
    /// </remarks>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Инициализирует окно авторизации
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Обрабатывает получение фокуса текстовым полем (очищает placeholder)
        /// </summary>
        /// <param name="_sender">Текстовое поле, получившее фокус</param>
        /// <param name="_e">Данные события</param>
        private void textBoxGotFocus1(object _sender, RoutedEventArgs _e)
        {
            TextBox textBox = _sender as TextBox;

            if (textBox.Text == "Введите имя" || textBox.Text == "Введите карту читателя")
            {
                textBox.Text = string.Empty;
            }
        }


        /// <summary>
        /// Обрабатывает потерю фокуса текстовым полем (восстанавливает placeholder)
        /// </summary>
        /// <param name="_sender">Текстовое поле, потерявшее фокус</param>
        /// <param name="_e">Данные события</param>
        private void textBoxLostFocus(object _sender, RoutedEventArgs _e)
        {
            TextBox textBox = _sender as TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "LoginTextBox")
                {
                    textBox.Text = "Введите имя";
                }
                else if (textBox.Name == "PasswordTextBox")
                {
                    textBox.Text = "Введите карту читателя";
                }
            }
        }


        /// <summary>
        /// Обрабатывает нажатие кнопки входа в систему
        /// </summary>
        /// <param name="_sender">Кнопка входа</param>
        /// <param name="_e">Данные события</param>
        /// <remarks>
        /// Проверяет учетные данные и перенаправляет пользователя 
        /// на соответствующее его роли рабочее окно
        /// </remarks>
        private void buttonClick (object _sender, RoutedEventArgs _e)
        {
            DBConnector dbConnector = new DBConnector();

            dbConnector.openConnection();

            String loginUser = LoginTextBox.Text;
            String readerCard = PasswordTextBox.Text;


            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand(
                "SELECT Role " +
                "FROM readers " +
                "WHERE FirstName = @uL AND LibraryCardNumber = @uP " +
                "ORDER BY Role DESC " +
                "LIMIT 1",
                dbConnector.getConnection()
            );

            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginUser;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = readerCard;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                string userRole = table.Rows[0]["Role"].ToString();

                switch (userRole)
                {
                    case "Библиотекарь":
                        MessageBox.Show("Вы вошли как Библиотекарь");
                        this.Hide();

                        AddData librarian = new AddData(loginUser);
                        librarian.Show();
                        break;

                    case "Читатель":
                        MessageBox.Show("Вы вошли как Читатель");
                        this.Hide();

                        ReaderPlace reader_form = new ReaderPlace(loginUser, readerCard);
                        reader_form.Show();
                        break;

                    case "Администратор":
                        MessageBox.Show("Вы вошли как Администратор");
                        this.Hide();

                        AdminWindow administration_form = new AdminWindow(loginUser);
                        administration_form.Show();
                        break;

                    default:
                        MessageBox.Show("Системная ошибка. Обратитесь к администратору");
                        break;
                }


            }
            else
            {
                MessageBox.Show("Проверьте корректность вводимых данных. Имя/карточку читателя");
            }

            dbConnector.closeConnection();
        }


        /// <summary>
        /// Обрабатывает изменение текста в поле пароля
        /// </summary>
        /// <param name="_sender">Поле ввода пароля</param>
        /// <param name="_e">Данные события</param>
        private void passwordTextBoxTextChanged(object _sender, TextChangedEventArgs _e)
        {

        }
    }
}
