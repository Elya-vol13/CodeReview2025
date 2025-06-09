using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace NewProject_PL
{
    /// <summary>
    /// Окно читателя библиотечной системы
    /// </summary>
    /// <remarks>
    /// Предоставляет функционал для:
    /// - Просмотра доступных книг
    /// - Просмотра взятых книг
    /// - Резервирования новых книг
    /// - Управления своими книгами
    /// </remarks>
    public partial class ReaderPlace : Window
    {
        /// <summary>
        /// Инициализирует окно читателя
        /// </summary>
        /// <param name="_name">Имя читателя</param>
        /// <param name="_library_card">Номер читательского билета</param>
        public ReaderPlace(string _name, string _library_card)
        {
            InitializeComponent();
            readerPanelText(_name);
            loadBooksData();
            LibraryCard_Text(_library_card);
            loadReaderBooks();
        }


        /// <summary>
        /// Устанавливает текст заголовка панели читателя
        /// </summary>
        /// <param name="_name">Имя читателя для отображения</param>
        public void readerPanelText(string _name)
        {
            ReaderPanel.Content = $"Панель читателя | {_name}";
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
        /// Отображает номер читательского билета
        /// </summary>
        /// <param name="_card">Номер читательского билета</param>
        private void LibraryCard_Text(string _card)
        {
            LibraryCardNumberTextBox.Text = $"{_card}";
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
        /// Очищает placeholder при получении фокуса
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void textBoxGotFocus(object _sender, RoutedEventArgs _e)
        {
            TextBox textBox = _sender as TextBox;

            if (textBox != null)
            {
                if (textBox.Text == "ISBN книги" || textBox.Text == "Выбор даты")

                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = Brushes.Black;
                }
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

            }
        }


        /// <summary>
        /// Закрывает приложение
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void closeButtonClick(object _sender, RoutedEventArgs _e)
        {
            Application.Current.Shutdown();
        }


        /// <summary>
        /// Обрабатывает возврат в окно авторизации
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
                    ISBNTextBox.Text = isbn;
                }
            }
        }


        /// <summary>
        /// Загружает данные о доступных книгах
        /// </summary>
        /// <param name="_searchFilter">Фильтр для поиска книг по названию</param>
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
        /// Загружает данные о книгах, взятых текущим читателем
        /// </summary>
        /// <param name="_readerId">ID читателя</param>
        private void loadMyBooksData(int _readerId)
        {
            DBConnector dbConnector = new DBConnector();
            try
            {
                dbConnector.openConnection();

                string query = "SELECT b.ID_book, b.title, b.author, b.ISBN, b.genre, b.Status " +
                               "FROM books b " +
                               "JOIN issuances i ON b.ID_book = i.ID_book ";
                MySqlCommand load_books = new MySqlCommand(query, dbConnector.getConnection());
                load_books.Parameters.AddWithValue("@readerId", _readerId);

                DataTable booksTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(load_books);
                adapter.Fill(booksTable);

                MyBooksDataGrid.ItemsSource = booksTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных о моих книгах: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            dbConnector.closeConnection();
        }


        /// <summary>
        /// Загружает книги текущего читателя
        /// </summary>
        private void loadReaderBooks()
        {
            string libraryCardNumber = LibraryCardNumberTextBox.Text.Trim();

            DBConnector dbConnector = new DBConnector();
            try
            {
                dbConnector.openConnection();

                MySqlCommand get_reader_id = new MySqlCommand(
                    "SELECT ID_reader FROM readers WHERE LibraryCardNumber = @libraryCardNumber",
                    dbConnector.getConnection()
                );
                get_reader_id.Parameters.AddWithValue("@libraryCardNumber", libraryCardNumber);

                object readerIdResult = get_reader_id.ExecuteScalar();
                if (readerIdResult == null)
                {
                    MessageBox.Show("Читатель с таким номером билета не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int readerId = Convert.ToInt32(readerIdResult);

                loadMyBooksData(readerId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных о моих книгах: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            dbConnector.closeConnection();
        }


        /// <summary>
        /// Обрабатывает резервирование книги
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события</param>
        private void reserveBookButtonClick(object _sender, RoutedEventArgs _e)
        {
            DBConnector dbConnector = new DBConnector();
            dbConnector.openConnection();

            string libraryCardNumber = LibraryCardNumberTextBox.Text.Trim();
            string isbn = ISBNTextBox.Text.Trim();
            DateTime currentDate = DateTime.Now;
            DateTime? returnDate = ReturnDatePicker.SelectedDate;

            MySqlCommand get_reader_id = new MySqlCommand(
                "SELECT ID_reader FROM readers WHERE LibraryCardNumber = @libraryCardNumber",
                dbConnector.getConnection()
            );
            get_reader_id.Parameters.AddWithValue("@libraryCardNumber", libraryCardNumber);

            var readerIdResult = get_reader_id.ExecuteScalar();
            if (readerIdResult == null)
            {
                MessageBox.Show("Читатель с таким номером билета не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int readerId = Convert.ToInt32(readerIdResult);

            MySqlCommand get_book_id = new MySqlCommand(
                "SELECT ID_book FROM books WHERE ISBN = @isbn",
                dbConnector.getConnection()
            );
            get_book_id.Parameters.AddWithValue("@isbn", isbn);

            var bookIdResult = get_book_id.ExecuteScalar();
            if (bookIdResult == null)
            {
                MessageBox.Show("Книга с таким ISBN не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int bookId = Convert.ToInt32(bookIdResult);

            if (!returnDate.HasValue || returnDate.Value <= currentDate || returnDate.Value > currentDate.AddDays(14))
            {
                MessageBox.Show("Дата возврата должна быть позже текущей даты, но не более чем через 2 недели!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MySqlCommand check_book_status_command = new MySqlCommand(
                "SELECT Status FROM books WHERE ID_book = @bookId",
                dbConnector.getConnection()
            );
            check_book_status_command.Parameters.AddWithValue("@bookId", bookId);

            var statusResult = check_book_status_command.ExecuteScalar();
            if (statusResult != null && statusResult.ToString() == "выдана")
            {
                MessageBox.Show("Эту книгу уже невозможно получить, она была выдана!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime issuanceDate = currentDate;

            MySqlCommand insert_command = new MySqlCommand(
                "INSERT INTO issuances (ID_reader, ID_book, IssuanceDate, ReturnPeriod, CurrentCondition) " +
                "VALUES (@readerId, @bookId, @issuanceDate, @returnDate, @currentCondition)",
                dbConnector.getConnection()
            );
            insert_command.Parameters.AddWithValue("@readerId", readerId);
            insert_command.Parameters.AddWithValue("@bookId", bookId);
            insert_command.Parameters.AddWithValue("@issuanceDate", issuanceDate.ToString("yyyy-MM-dd HH:mm:ss"));
            insert_command.Parameters.AddWithValue("@returnDate", returnDate.Value.ToString("yyyy-MM-dd"));
            insert_command.Parameters.AddWithValue("@currentCondition", "выдана");

            try
            {
                insert_command.ExecuteNonQuery();

                MySqlCommand update_book_status = new MySqlCommand(
                    "UPDATE books SET Status = 'выдана' WHERE ID_book = @bookId",
                    dbConnector.getConnection()
                );
                update_book_status.Parameters.AddWithValue("@bookId", bookId);
                update_book_status.ExecuteNonQuery();

                loadReaderBooks();
                loadBooksData();

                MessageBox.Show("Книга успешно зарезервирована!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении книги в таблицу: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            dbConnector.closeConnection();
        }
    }
}
