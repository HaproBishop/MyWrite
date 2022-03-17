using System;
using System.Windows;
using System.Windows.Documents;

namespace WPF.TextWork
{
    /// <summary>
    /// Логика взаимодействия для GoToLine.xaml
    /// </summary>
    public partial class GoerTo : Window
    {
        public int PosID { get; private set; } = -1;//Задание начальной позиции (вернее, ее отсутствие)
        public int LineID { get; private set; } = -1;
        public GoerTo()
        {
            InitializeComponent();
        }
        private void LineGoer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LineID = Convert.ToInt32(LineValue.Text);
                DialogResult = true;//Результат диалогового окна 
                Close();
            }
            catch
            {
                MessageBox.Show("Некорректно введено значение строки",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                LineValue.Clear();
                LineValue.Focus();
            }
        }        
        private void PosGoer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PosID = Convert.ToInt32(PosValue.Text);
                DialogResult = true;
                Close();
            }
            catch
            {
                MessageBox.Show("Некорректно введено значение строки",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                PosValue.Clear();
                PosValue.Focus();
            }
        }/// <summary>
        /// Очистка позиций при неправльном вводе
        /// </summary>
        public void Clear()
        {
            PosID = -1;
            LineID = -1;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            PosValue.Focus();
        }
    }
}
