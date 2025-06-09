using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace NewProject_PL
{
    /// <summary>
    /// Окно для работы библиотекаря с системой учета книг
    /// </summary>
    /// <remarks>
    /// Предоставляет функционал для:
    /// - Просмотра и поиска книг/читателей
    /// - Добавления новых книг в каталог
    /// - Оформления выдачи книг читателям
    /// - Приема возвращенных книг
    /// - Генерации отчетов по выдачам
    /// 
    /// Взаимодействует с MySQL базой данных через DBConnector
    /// </remarks>
    public partial class AddData : Window
    {
        /// <summary>
        /// Инициализирует окно работы библиотекаря
        /// </summary>
        /// <param name="_name">Имя авторизованного библиотекаря</param>
        public AddData(string _name)
        {
            InitializeComponent();
            loadBooksData();
            loadReadersData();
            libPanelText(_name);
            allBooks();
        }


        /// <summary>
        /// Устанавливает текст заголовка панели библиотекаря
        /// </summary>
        /// <param name="_name">Имя библиотекаря для отображения</param>
        public void libPanelText(string _name)
        {
            LibPanel.Content = $"Панель библиотекаря | {_name}";
        }


        /// <summary>
        /// Обрабатывает изменение текста в поле поиска книг
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void searchTextBoxTextChanged(object _sender, TextChangedEventArgs _e)
        {
            string search = SearchTextBox.Text.Trim();
            loadBooksData(search);
        }


        /// <summary>
        /// Загружает данные о книгах из базы данных с возможностью фильтрации
        /// </summary>
        /// <param name="_searchFilter">Строка для фильтрации книг по названию</param>
        private void loadBooksData(string _searchFilter = "")
        {
            DBConnector dbConnector = new DBConnector();
            try
            {
                dbConnector.openConnection();

                string query = "SELECT ID_book, title, author, ISBN, genre, status FROM books";

                if (!string.IsNullOrEmpty(_searchFilter))
                {
                    query += " WHERE title LIKE @SearchText";
                }

                MySqlCommand load_books = new MySqlCommand(query, dbConnector.getConnection());

                if (!string.IsNullOrEmpty(_searchFilter))
                {
                    load_books.Parameters.AddWithValue("@SearchText", $"%{_searchFilter}%");
                }

                DataTable booksTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(load_books);
                adapter.Fill(booksTable);

                BooksDataGrid.ItemsSource = booksTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных о книгах: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                dbConnector.closeConnection();
            }
        }


        /// <summary>
        /// Загружает данные о читателях из базы данных с возможностью фильтрации
        /// </summary>
        /// <param name="_searchFilter">Строка для фильтрации по фамилии/имени</param>
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

                }
            }
        }


        /// <summary>
        /// Генерирует уникальный ISBN для новой книги
        /// </summary>
        /// <param name="_dbConnector">Активное подключение к БД</param>
        /// <returns>Сгенерированный ISBN в виде строки</returns>
        private string generateISBN(DBConnector _dbConnector)
        {
            string isbn;
            Random random = new Random();

            while (true)
            {
                isbn = string.Concat(Enumerable.Range(0, 13).Select(_ => random.Next(0, 10).ToString()));

                MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM books WHERE ISBN = @isbn", _dbConnector.getConnection());
                checkCommand.Parameters.AddWithValue("@isbn", isbn);

                long count = (long)checkCommand.ExecuteScalar();

                if (count == 0)
                {
                    break;
                }
            }

            return isbn;
        }


        /// <summary>
        /// Обрабатывает добавление новой книги в каталог
        /// </summary>
        private void addBookButtonClick(object _sender, RoutedEventArgs _e)
        {
            DBConnector dbConnector = new DBConnector();
            dbConnector.openConnection();

            string title = TitleTextBox.Text.Trim();
            string author = AuthorTextBox.Text.Trim();
            string genre = GenreTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) ||
                string.IsNullOrWhiteSpace(genre) || title == "Название книги" || author == "Автор" || genre == "Жанр")
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(author, @"^[А-Яа-яЁёA-Za-z]+\s[А-Яа-яЁёA-Za-z]+$"))
            {
                MessageBox.Show("Введите имя и фамилию автора(буквы, пробел между именем и фамилией)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(genre, @"^[А-Яа-яЁёA-Za-z\s]+$"))
            {
                MessageBox.Show("Введите корректный жанр(буквы и пробелы, без спецсимволов)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string isbn = generateISBN(dbConnector);

            MySqlCommand create_book = new MySqlCommand(
                "INSERT INTO `books` (`title`, `author`, `ISBN`, `genre`, `status`) VALUES (@title, @author, @isbn, @genre, @status)",
                dbConnector.getConnection()
            );

            create_book.Parameters.AddWithValue("@title", title);
            create_book.Parameters.AddWithValue("@author", author);
            create_book.Parameters.AddWithValue("@isbn", isbn);
            create_book.Parameters.AddWithValue("@genre", genre);
            create_book.Parameters.AddWithValue("@status", "доступна");

            try
            {
                create_book.ExecuteNonQuery();
                MessageBox.Show($"Книга успешно добавлена! ISBN: {isbn}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                loadBooksData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в создании книги: {ex.Message}");
            }

            TitleTextBox.Clear();
            AuthorTextBox.Clear();
            GenreTextBox.Clear();
            ISBNTextBox.Clear();

            dbConnector.closeConnection();
        }


        /// <summary>
        /// Оформляет выдачу книги читателю
        /// </summary>
        private void issueBookButtonClick(object _sender, RoutedEventArgs _e)
        {
            DBConnector dbConnector = new DBConnector();
            dbConnector.openConnection();

            string cardNumber = CardNumberTextBox.Text.Trim();
            string isbn = BookTitleTextBox.Text.Trim();

            string issDate;
            if (DatePicker.SelectedDate.HasValue)
            {
                issDate = DatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string returnDate;
            if (ReturnDate.SelectedDate.HasValue)
            {
                returnDate = ReturnDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(cardNumber) || string.IsNullOrWhiteSpace(isbn) || issDate == null || returnDate == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MySqlCommand reader_id = new MySqlCommand(
                "SELECT ID_reader FROM readers WHERE LibraryCardNumber = @cardNumber",
                dbConnector.getConnection()
            );
            reader_id.Parameters.AddWithValue("@cardNumber", cardNumber);


            var result = reader_id.ExecuteScalar();
            if (result == null)
            {
                MessageBox.Show("Читатель с таким номером карточки не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int readerId = Convert.ToInt32(result);

            MySqlCommand book_id = new MySqlCommand(
                "SELECT ID_book FROM books WHERE ISBN = @isbn",
                dbConnector.getConnection()
            );
            book_id.Parameters.AddWithValue("@isbn", isbn);

            var book_id_temp = book_id.ExecuteScalar();
            if (book_id_temp == null)
            {
                MessageBox.Show("Книга с таким ISBN не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int bookId = Convert.ToInt32(book_id_temp);

            MySqlCommand create_issue = new MySqlCommand(
                "INSERT INTO issuances (ID_reader, ID_book, IssuanceDate, ReturnPeriod, CurrentCondition) " +
                "VALUES (@readerId, @bookId, @issDate, @returnDate, @currentCondition)",
                dbConnector.getConnection()
            );
            create_issue.Parameters.AddWithValue("@readerId", readerId);
            create_issue.Parameters.AddWithValue("@bookId", bookId);
            create_issue.Parameters.AddWithValue("@issDate", issDate);
            create_issue.Parameters.AddWithValue("@returnDate", returnDate);
            create_issue.Parameters.AddWithValue("@currentCondition", "Хорошее");

            try
            {
                create_issue.ExecuteNonQuery();
                MessageBox.Show($"Книга успешно выдана!\nДата возврата: до {ReturnDate} включительно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                CardNumberTextBox.Clear();
                BookTitleTextBox.Clear();
                DatePicker.SelectedDate = null;
                ReturnDate.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            dbConnector.closeConnection();
        }


        /// <summary>
        /// Обрабатывает возврат книги от читателя
        /// </summary>
        private void returnBookButtonClick(object _sender, RoutedEventArgs _e)
        {
            DBConnector dbConnector = new DBConnector();
            dbConnector.openConnection();

            string cardNumber = ReturnCardNumberTextBox.Text.Trim();
            string isbn = ReturnBookTitleTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                MessageBox.Show("Введите номер билета!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(isbn))
            {
                MessageBox.Show("Введите ISBN книги!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ReturnDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите дату возврата!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime returnDate = ReturnDatePicker.SelectedDate.Value;

            string bookCondition = string.Empty;
            if (BookConditionComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                bookCondition = selectedItem.Content.ToString();
            }
            else
            {
                MessageBox.Show("Выберите состояние книги!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MySqlCommand reader_id = new MySqlCommand(
                "SELECT ID_reader FROM readers WHERE LibraryCardNumber = @cardNumber",
                dbConnector.getConnection()
            );
            reader_id.Parameters.AddWithValue("@cardNumber", cardNumber);

            var reader_id_obj = reader_id.ExecuteScalar();
            if (reader_id_obj == null)
            {
                MessageBox.Show("Читатель с таким номером карточки не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int readerId = Convert.ToInt32(reader_id_obj);

            MySqlCommand book_id = new MySqlCommand(
                "SELECT ID_book FROM books WHERE ISBN = @isbn",
                dbConnector.getConnection()
            );
            book_id.Parameters.AddWithValue("@isbn", isbn);

            var book_id_obj = book_id.ExecuteScalar();
            if (book_id_obj == null)
            {
                MessageBox.Show("Книга с таким ISBN не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int bookId = Convert.ToInt32(book_id_obj);

            MySqlCommand issuance_id = new MySqlCommand(
                "SELECT ID_issuance, ReturnPeriod FROM issuances WHERE ID_reader = @readerId AND ID_book = @bookId",
                dbConnector.getConnection()
            );
            issuance_id.Parameters.AddWithValue("@readerId", readerId);
            issuance_id.Parameters.AddWithValue("@bookId", bookId);

            using (var reader = issuance_id.ExecuteReader())
            {
                if (!reader.Read())
                {
                    MessageBox.Show("Выдача для этой книги и читателя не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int issuanceID = reader.GetInt32(0);
                DateTime expectedReturnDate = reader.GetDateTime(1);

                string expiredStatus = returnDate > expectedReturnDate ? "Просрочено" : "Не просрочено";

                reader.Close();

                MySqlCommand create_return = new MySqlCommand(
                    "INSERT INTO returns (ID_issuance, ReturnDate, BookCondition, ExpiredStatus) " +
                    "VALUES (@issuanceID, @returnDate, @bookCondition, @expiredStatus)",
                    dbConnector.getConnection()
                );

                create_return.Parameters.AddWithValue("@issuanceID", issuanceID);
                create_return.Parameters.AddWithValue("@returnDate", returnDate.ToString("yyyy-MM-dd HH:mm:ss"));
                create_return.Parameters.AddWithValue("@bookCondition", bookCondition);
                create_return.Parameters.AddWithValue("@expiredStatus", expiredStatus);

                try
                {
                    create_return.ExecuteNonQuery();
                    allBooks();
                    MessageBox.Show($"Книга успешно возвращена!\nСтатус возврата: {expiredStatus}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    ReturnCardNumberTextBox.Clear();
                    ReturnBookTitleTextBox.Clear();
                    ReturnDatePicker.SelectedDate = null;
                    BookConditionComboBox.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            dbConnector.closeConnection();
        }


        /// <summary>
        /// Генерирует отчет по выдачам книг с возможностью фильтрации
        /// </summary>
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
        /// Обрабатывает клик по кнопке возврата
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
        /// Загружает данные о всех текущих выдачах книг
        /// </summary>
        private void allBooks()
        {
            DBConnector dbConnector = new DBConnector();
            try
            {
                dbConnector.openConnection();

                string query = "SELECT i.ID_issuance, i.ID_reader, i.ID_book, i.IssuanceDate, i.ReturnPeriod, i.CurrentCondition, " +
                               "b.title, b.author, b.ISBN, b.genre " +
                               "FROM issuances i " +
                               "JOIN books b ON i.ID_book = b.ID_book";

                MySqlCommand load_books = new MySqlCommand(query, dbConnector.getConnection());

                DataTable booksTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(load_books);
                adapter.Fill(booksTable);

                AllDataBooks.ItemsSource = booksTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных о книгах: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            dbConnector.closeConnection();
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
        /// Обрабатывает закрытие приложения
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void closeButtonClick(object _sender, RoutedEventArgs _e)
        {
            Application.Current.Shutdown();
        }


        /// <summary>
        /// Обрабатывает выбор книги в таблице
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void booksDataGridSelectionChanged(object _sender, SelectionChangedEventArgs _e)
        {
            if (BooksDataGrid.SelectedItem is DataRowView selected_row)
            {
                if (selected_row["ISBN"] != null)
                {
                    string isbn = selected_row["ISBN"].ToString();
                    BookTitleTextBox.Text = isbn;
                    ReturnBookTitleTextBox.Text = isbn;
                }
            }
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

                    CardNumberTextBox.Text = libraryCardNumber;
                    ReturnCardNumberTextBox.Text = libraryCardNumber;
                    ReportCardNumberTextBox.Text = libraryCardNumber;
                }
            }
        }


        /// <summary>
        /// Обрабатывает изменение выбора в таблице всех книг
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void allDataBooksSelectionChanged(object _sender, SelectionChangedEventArgs _e)
        {

        }
    }
}
