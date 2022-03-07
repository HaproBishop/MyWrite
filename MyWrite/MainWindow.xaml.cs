using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileData;
using TitleChanger;
using Commands;
using Microsoft.Win32;

namespace MyWrite
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RichTextBoxData _fileInfo;
        AdvancedChanger _titleChanger;
        public MainWindow()
        {
            InitializeComponent();
            _titleChanger = new AdvancedChanger(Title);
            _fileInfo = new RichTextBoxData();
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (_fileInfo.IsModified)
            {
                if (PreCloseQuestion() == true)
                {
                    Save_Click(sender, e);
                }
            }
            else
            {
                DefaultData();
            }
        }
        private bool? PreCloseQuestion()
        {
            MessageBoxResult result = MessageBox.Show("Хотите сохранить текущие изменения?",
                "Сохранение изменений", MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) return true;
            return false;
        }
        private void DefaultData()
        {
            MyText.Document.Blocks.Clear();
            Title = _titleChanger.Title;
            _titleChanger.Clear();
        }
        private void NewWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Show();
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                Title = "Открыть",
                Filter = "Текстовый документ(.txt) | *.txt",
                DefaultExt = ".txt",
            };
            if (open.ShowDialog() == true)
            {
                if (_fileInfo.Open(open.FileName) == true)
                {
                    MyText = _fileInfo.Text;
                    _titleChanger.FileName = open.SafeFileName;
                    Title = _titleChanger.FullTitle;
                }
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_titleChanger.FileName == "" || e.Source == SaveAs)
            {
                SaveFileDialog save = new SaveFileDialog
                {
                    Title = "Сохранить как",
                    Filter = "Текстовый документ(*.rtf) | *.rtf|Все файлы (*.*) | *.*",
                    DefaultExt = "rtf",
                };
                if(_fileInfo.IsModified)
                if (save.ShowDialog() == true)
                {
                    if (_fileInfo.Save(save.FileName) == true)
                    {
                        _fileInfo.Text = MyText;
                        _titleChanger.FileName = save.SafeFileName;
                        Title = _titleChanger.FullTitle;
                    }                                        
                }
            }
            else
            {
                _fileInfo.Save();
                Title = _titleChanger.FullTitle;
            }            
        }
        private void PrintText_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog print = new PrintDialog();
            print.ShowDialog();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void DelMenu_Click(object sender, RoutedEventArgs e)
        {
            MyText.Selection.Text.Replace(MyText.Selection.Text, "");
        }
        int _scale = 100;
        private void DefaultScale_Click(object sender, RoutedEventArgs e)
        {
            CurrentScale.Text = (_scale = 100).ToString();
            MyText.LayoutTransform.Value.Scale(1.0, 1.0);
        }
        private void ScaleMinus_Click(object sender, RoutedEventArgs e)
        {
            if (_scale != 10)
            {
                CurrentScale.Text = (_scale-=10).ToString();
                MyText.LayoutTransform.Value.Scale(_scale / 100.0, _scale / 100.0);
            }
        }
        private void ScalePlus_Click(object sender, RoutedEventArgs e)
        {
            if (_scale != 500)
            {
                CurrentScale.Text = (_scale += 10).ToString();
                MyText.LayoutTransform.Value.Scale(_scale/100.0, _scale / 100.0);
            }
        }
        private void HelpMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("У данной программы существует ряд следующих особенностей:\n" +
                "1) При изменении настроек окна сохранение происходит для текущего окна и его потомков(ctrl+shift+n)\n" +
                "2) По умолчанию используется 100% масштаб\n" +
                "3) Используются стандартные сочетания клавиш для быстрого использования функционала", "Справка",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void AboutProgram_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Версия 1.0. Разработчиком является Лопаткин Сергей (Псевдоним: Hapro Bishop) из группы ИСП-31", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void MyText_LayoutUpdated(object sender, EventArgs e)
        {
            if (!MyText.CanUndo) UndoMenu.IsEnabled = false;
            else UndoMenu.IsEnabled = true;
            if (!MyText.CanRedo) RedoMenu.IsEnabled = false;
            else RedoMenu.IsEnabled = true;
            if (MyText.Document.Blocks.Count == 0) SelectAllMenu.IsEnabled = false;
            else SelectAllMenu.IsEnabled = true;
        }
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            CurrentColumn.Text = MyText.Selection.Start.Paragraph.ContentStart.GetOffsetToPosition(MyText.CaretPosition).ToString();            
            MyText.CaretPosition.GetLineStartPosition(int.MinValue, out int line);
            CurrentRow.Text = (-(line-1)).ToString();
            MyText.CaretPosition = MyText.CaretPosition.GetLineStartPosition(1);
        }
    }
}
