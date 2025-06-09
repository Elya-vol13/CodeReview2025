using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace NewProject_PL
{
    /// <summary>
    /// Окно для отображения отчетов по выданным книгам
    /// </summary>
    /// <remarks>
    /// Показывает таблицу с данными о:
    /// - Выданных книгах
    /// - Сроках возврата
    /// - Статусах возврата
    /// </remarks>
    public partial class ReportWindow : Window
    {
        /// <summary>
        /// Инициализирует окно отчета
        /// </summary>
        public ReportWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Обрабатывает выбор элемента в таблице отчетов
        /// </summary>
        /// <param name="_sender">Таблица с отчетами</param>
        /// <param name="_e">Данные события выбора</param>
        private void reportDataGridSelectionChanged(object _sender, SelectionChangedEventArgs _e)
        {

        }


        /// <summary>
        /// Закрывает окно отчета
        /// </summary>
        /// <param name="_sender">Кнопка закрытия</param>
        /// <param name="_e">Данные события</param>
        private void closeButtonClick_Window(object _sender, RoutedEventArgs _e)
        {
            this.Close();
        }


        /// <summary>
        /// Обрабатывает перемещение окна
        /// </summary>
        /// <param name="_sender">Источник события</param>
        /// <param name="_e">Данные события (координаты мыши)</param>
        private void windowMouseDown(object _sender, MouseButtonEventArgs _e)
        {
            if (_e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
