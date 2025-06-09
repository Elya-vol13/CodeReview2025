using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NewProject_PL
{
    /// <summary>
    /// Окно администратора системы управления библиотекой
    /// </summary>
    /// <remarks>
    /// Предоставляет функционал для:
    /// - Управления учетными записями пользователей (добавление/удаление)
    /// - Просмотра и поиска читателей
    /// - Генерации отчетов по активности пользователей
    /// - Назначения ролей (Читатель/Библиотекарь)
    /// </remarks>
    public partial class AdminWindow : Window
    {
        /// <summary>
        /// Инициализирует окно администратора
        /// </summary>
        /// <param name="name">Имя авторизованного администратора</param>
        public AdminWindow(string name)
        {
            InitializeComponent();
            loadReadersData();
            adminLabelPanel(name);
            loadReadersData();
        }


        /// <summary>
        /// Устанавливает текст заголовка панели администратора
        /// </summary>
        /// <param name="_name">Имя администратора для отображения</param>
        public void adminLabelPanel(string _name)
        {
            AdminLabel.Content = $"Панель администратора | {_name}";
        }


        /// <summary>
        /// Обрабатывает выбор читателя в таблице
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void readersDataGridSelectionChanged(object _sender, SelectionChangedEventArgs _e)
        {
            if (ReadersDataGrid.SelectedItem is DataRowView selectedRow)
            {
                if (selectedRow["LibraryCardNumber"] != null)
                {
                    string libraryCardNumber = selectedRow["LibraryCardNumber"].ToString();

                    LibraryCardNumberDeleteTextBox.Text = libraryCardNumber;
                    ReportCardNumberTextBox.Text = libraryCardNumber;
                    ReportCardNumberTextBox.Text = libraryCardNumber;
                }
            }
        }


        /// <summary>
        /// Обрабатывает изменение текста в поле поиска читателей
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void searchReaderTextBoxTextChanged(object _sender, TextChangedEventArgs _e)
        {
            string searchText = SearchReaderTextBox.Text.Trim();
            loadReadersData(searchText);
        }


        /// <summary>
        /// Обрабатывает получение фокуса текстовым полем
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void textBoxGotFocus(object _sender, RoutedEventArgs _e)
        {
            TextBox textBox = _sender as TextBox;

            if (textBox != null)
            {

            }
        }


        /// <summary>
        /// Обрабатывает нажатие на кнопку возврата
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void imageMouseLeftButtonUp(object _sender, MouseButtonEventArgs _e)
        {
            MainWindow main_window = new MainWindow();
            main_window.Show();

            this.Close();
        }


        /// <summary>
        /// Обрабатывает потерю фокуса текстовым полем
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void textBoxLostFocus(object _sender, RoutedEventArgs _e)
        {
            TextBox textBox = _sender as TextBox;

            if (textBox != null)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (textBox.Name == "LastNameTextBox") textBox.Text = "Фамилия";
                    if (textBox.Name == "FirstNameTextBox") textBox.Text = "Имя";
                    if (textBox.Name == "BirthDatePicker") textBox.Text = "Выбор даты";
                    if (textBox.Name == "LibraryCardNumberTextBox") textBox.Text = "Номер билета";
                    if (textBox.Name == "ContactsTextBox") textBox.Text = "Контакты (телефон)";
                    if (textBox.Name == "LibraryCardNumberDeleteTextBox") textBox.Text = "Номер билета";

                    textBox.Foreground = Brushes.Gray;
                }
            }
        }


        /// <summary>
        /// Загружает данные о читателях с возможностью фильтрации
        /// </summary>
        /// <param name="_searchFilter">Строка для фильтрации (по фамилии или имени)</param>
        private void loadReadersData(string _searchFilter = "")
        {
            DBConnector dbConnector = new DBConnector();
            try
            {
                dbConnector.openConnection();

                string query = "SELECT ID_reader, LastName, FirstName, BirthDate, LibraryCardNumber, Contacts FROM readers";

                if (!string.IsNullOrEmpty(_searchFilter))
                {
                    query += " WHERE LastName LIKE @SearchText OR FirstName LIKE @SearchText";
                }

                MySqlCommand load_readers = new MySqlCommand(query, dbConnector.getConnection());

                if (!string.IsNullOrEmpty(_searchFilter))
                {
                    load_readers.Parameters.AddWithValue("@SearchText", $"%{_searchFilter}%");
                }

                DataTable readersTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(load_readers);
                adapter.Fill(readersTable);

                ReadersDataGrid.ItemsSource = readersTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных о читателях: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                dbConnector.closeConnection();
            }
        }


        /// <summary>
        /// Обрабатывает закрытие приложения
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void closeButtonClick(object _sender, RoutedEventArgs _e)
        {
            Application.Current.Shutdown();
        }


        /// <summary>
        /// Обрабатывает перемещение окна
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void windowMouseDown(object _sender, MouseButtonEventArgs _e)
        {
            if (_e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


        /// <summary>
        /// Загружает полный список читателей
        /// </summary>
        private void loadReadersData()
        {
            DBConnector dbConnector = new DBConnector();
            try
            {
                dbConnector.openConnection();

                string query = "SELECT ID_reader, LastName, FirstName, BirthDate, LibraryCardNumber, Contacts, Role FROM readers";
                MySqlCommand load_readers = new MySqlCommand(query, dbConnector.getConnection());

                DataTable readers_table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(load_readers);
                adapter.Fill(readers_table);

                ReadersDataGrid.ItemsSource = readers_table.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            dbConnector.closeConnection();
        }


        /// <summary>
        /// Генерирует уникальный номер читательского билета
        /// </summary>
        /// <param name="_dbConnector">Активное подключение к БД</param>
        /// <returns>Сгенерированный номер билета</returns>
        private string generateLCN(DBConnector _dbConnector)
        {
            string lcn;
            Random random = new Random();

            while (true)
            {
                lcn = string.Concat(Enumerable.Range(0, 20).Select(_ => random.Next(0, 10).ToString()));

                MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM readers WHERE LibraryCardNumber = @lcn", _dbConnector.getConnection());
                checkCommand.Parameters.AddWithValue("@lcn", lcn);

                long count = (long)checkCommand.ExecuteScalar();

                if (count == 0)
                {
                    break;
                }
            }

            return lcn;
        }


        /// <summary>
        /// Обрабатывает добавление нового пользователя
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void addReaderButtonClick(object _sender, RoutedEventArgs _e)
        {
            DBConnector dbConnector = new DBConnector();
            dbConnector.openConnection();

            string lastName = LastNameTextBox.Text.Trim();
            string firstName = FirstNameTextBox.Text.Trim();
            string birthDateString = BirthDatePicker.Text.Trim();
            string libraryCardNumberString = LibraryCardNumberTextBox.Text.Trim();
            string contactsString = ContactsTextBox.Text.Trim();
            string role = RoleComboBox.Text.Trim();
            string registrationDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(birthDateString) || string.IsNullOrWhiteSpace(libraryCardNumberString) ||
                string.IsNullOrWhiteSpace(contactsString) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(lastName, @"^[А-Яа-яЁёA-Za-z]+$"))
            {
                MessageBox.Show("Введите корректную фамилию(только буквы)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(firstName, @"^[А-Яа-яЁёA-Za-z]+$"))
            {
                MessageBox.Show("Введите корректное имя(только буквы)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!DateTime.TryParse(birthDateString, out DateTime birthDate))
            {
                MessageBox.Show("Введите корректную дату рождения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(contactsString, @"^\d{11,}$"))
            {
                MessageBox.Show("Введите корректный номер телефона(только цифры, минимум 11 символов)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (role != "Читатель" && role != "Библиотекарь")
            {
                MessageBox.Show("Выберите корректную роль(читатель или библиотекарь)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string lcn = generateLCN(dbConnector);

            MySqlCommand create_user = new MySqlCommand(
                "INSERT INTO `readers` (`LastName`, `FirstName`, `BirthDate`, `LibraryCardNumber`, `Contacts`, `RegistrationDate`, `Role`) " +
                "VALUES (@lastName, @firstName, @birthDate, @libraryCardNumber, @contacts, @registrationDate, @role)",
                dbConnector.getConnection()
            );

            create_user.Parameters.AddWithValue("@lastName", lastName);
            create_user.Parameters.AddWithValue("@firstName", firstName);
            create_user.Parameters.AddWithValue("@birthDate", birthDate.ToString("yyyy-MM-dd"));
            create_user.Parameters.AddWithValue("@libraryCardNumber", lcn);
            create_user.Parameters.AddWithValue("@contacts", contactsString);
            create_user.Parameters.AddWithValue("@registrationDate", registrationDate);
            create_user.Parameters.AddWithValue("@role", role);

            try
            {
                create_user.ExecuteNonQuery();
                MessageBox.Show("Пользователь успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                loadReadersData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в создании пользователя: {ex.Message}");
            }

            LastNameTextBox.Clear();
            FirstNameTextBox.Clear();
            BirthDatePicker.SelectedDate = null;
            LibraryCardNumberTextBox.Clear();
            ContactsTextBox.Clear();

            dbConnector.closeConnection();
        }


        /// <summary>
        /// Генерирует отчет по активности пользователей
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void generateReportButtonClick(object _sender, RoutedEventArgs _e)
        {
            DBConnector dbConnector = new DBConnector();
            dbConnector.openConnection();

            string cardNumber = ReportCardNumberTextBox.Text.Trim();
            DateTime? startDate = ReportStartDatePicker.SelectedDate;
            DateTime? endDate = ReportEndDatePicker.SelectedDate;

            List<Report> reportItems = new List<Report>();

            string query = "SELECT r.LastName, r.FirstName, b.ISBN, i.IssuanceDate, re.ReturnDate, re.ExpiredStatus " +
                           "FROM returns re " +
                           "JOIN issuances i ON re.ID_issuance = i.ID_issuance " +
                           "JOIN readers r ON i.ID_reader = r.ID_reader " +
                           "JOIN books b ON i.ID_book = b.ID_book ";


            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (!string.IsNullOrWhiteSpace(cardNumber))
            {
                query += "WHERE r.LibraryCardNumber = @cardNumber ";
                parameters.Add(new MySqlParameter("@cardNumber", cardNumber));
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                if (query.Contains("WHERE"))
                {
                    query += "AND re.ReturnDate BETWEEN @startDate AND @endDate ";
                }
                else
                {
                    query += "WHERE re.ReturnDate BETWEEN @startDate AND @endDate ";
                }

                parameters.Add(new MySqlParameter("@startDate", startDate.Value.ToString("yyyy-MM-dd")));
                parameters.Add(new MySqlParameter("@endDate", endDate.Value.ToString("yyyy-MM-dd")));
            }

            query += "ORDER BY re.ReturnDate DESC;";

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, dbConnector.getConnection());

                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                fillReportData(cmd, reportItems);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ReportWindow reportWindow = new ReportWindow();
            reportWindow.ReportDataGrid.ItemsSource = reportItems;

            reportWindow.Show();

            dbConnector.closeConnection();
        }


        /// <summary>
        /// Заполняет список данных для отчета
        /// </summary>
        /// <param name="_cmd">SQL-команда для получения данных</param>
        /// <param name="_reportItems">Список для заполнения данными</param>
        private void fillReportData(MySqlCommand _cmd, List<Report> _reportItems)
        {
            try
            {
                MySqlDataReader reader = _cmd.ExecuteReader();

                while (reader.Read())
                {
                    Report item = new Report
                    {
                        LastName = reader.GetString("LastName"),
                        FirstName = reader.GetString("FirstName"),
                        ISBN = reader.GetString("ISBN"),
                        IssuanceDate = reader.GetDateTime("IssuanceDate"),
                        ReturnDate = reader.GetDateTime("ReturnDate"),
                        ExpiredStatus = reader.GetString("ExpiredStatus")
                    };

                    _reportItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Обрабатывает удаление пользователя
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void deleteReaderButtonClick(object _sender, RoutedEventArgs _e)
        {
            DBConnector dbConnector = new DBConnector();
            dbConnector.openConnection();

            string libraryCardNumberString = LibraryCardNumberDeleteTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(libraryCardNumberString))
            {
                MessageBox.Show("Введите номер читательского билета!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(libraryCardNumberString, @"^\d+$"))
            {
                MessageBox.Show("Введите корректный номер читательского билета(только цифры)!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                MySqlCommand readerChecker = new MySqlCommand(
                    "SELECT Role FROM readers WHERE LibraryCardNumber = @libraryCardNumber",
                    dbConnector.getConnection()
                );
                readerChecker.Parameters.AddWithValue("@libraryCardNumber", libraryCardNumberString);

                var role_object = readerChecker.ExecuteScalar();

                if (role_object == null)
                {
                    MessageBox.Show("Пользователь с таким номером читательского билета не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string role = role_object.ToString();

                if (role == "Администратор")
                {
                    MessageBox.Show("Вы не можете удалить Администратора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MySqlCommand deleteReader = new MySqlCommand(
                    "DELETE FROM readers WHERE LibraryCardNumber = @libraryCardNumber",
                    dbConnector.getConnection()
                );
                deleteReader.Parameters.AddWithValue("@libraryCardNumber", libraryCardNumberString);

                deleteReader.ExecuteNonQuery();
                MessageBox.Show("Пользователь успешно удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                loadReadersData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                dbConnector.closeConnection();
                LibraryCardNumberDeleteTextBox.Clear();
            }
        }
    }
}
