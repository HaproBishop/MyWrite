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
using Microsoft.Win32;
using Commands;
using FileData;
using TitleChanger;
using WPF.TextWork;

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
            _titleChanger.Clear();
            Title = _titleChanger.Title;
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
                Filter = "Текстовый документ(*.rtf) | *.rtf|Все файлы (*.*) | *.*",
                DefaultExt = "rtf",
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
            CurrentScale.Text = scale.ScaleY.ToString();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_titleChanger.FileName == "" || e.Source == SaveAs || e.Source == Create)
            {
                SaveFileDialog save = new SaveFileDialog
                {
                    Title = "Сохранить как",
                    Filter = "Текстовый документ(*.rtf) | *.rtf|Все файлы (*.*) | *.*",
                    DefaultExt = "rtf",
                };
                if (_fileInfo.IsModified)
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
            CurrentScale.Text = scale.ScaleY.ToString();
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
        double _reservedHeight;
        private void ShowStatus_Click(object sender, RoutedEventArgs e)
        {
            if (ShowStatus.IsChecked)
            {
                Status.Height = _reservedHeight;
            }
            else
            {
                _reservedHeight = Status.Height;
                Status.Height = 0;
            }
        }
        ScaleTransform scale;
        private void DefaultScale_Click(object sender, RoutedEventArgs e)
        {
            CurrentScale.Text = "100";
            scale.ScaleX = scale.ScaleY = 1.0;
        }
        private void ScaleMinus_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentScale.Text != "50")
            {
                CurrentScale.Text = ((scale.ScaleY = scale.ScaleX -= 0.1) * 100).ToString();
            }
        }
        private void ScalePlus_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentScale.Text != "200")
            {
                CurrentScale.Text = ((scale.ScaleY = scale.ScaleX += 0.1) * 100).ToString();
            }
        }
        private void HelpMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("У данной программы существует ряд следующих особенностей:\n" +
                "1) При изменении настроек окна сохранение происходит для текущего окна и его потомков(ctrl+shift+n)\n" +
                "2) По умолчанию используется 100% масштаб\n" +
                "3) Используются стандартные сочетания клавиш для быстрого использования функционала\n" +
                "4) При использовании переходчика для перехода к позиции, стоит помнить о том," +
                "что новый абзац дает еще 4 символа в общее количество", "Справка",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void AboutProgram_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Версия 1.0. Разработчиком является Лопаткин Сергей (Псевдоним: Hapro Bishop) из группы ИСП-31", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        int line,//Offset от текущей строки
            currentChar;//Текущее кол-во символов
        private void MyText_LayoutUpdated(object sender, EventArgs e)
        {
            currentChar = MyText.Selection.Start.DocumentStart.GetOffsetToPosition(MyText.CaretPosition) - 1;
            if (currentChar == -1) CurrentColumn.Text = (currentChar + 2).ToString();
            else if (currentChar == 0) CurrentColumn.Text = (currentChar + 1).ToString();
            else CurrentColumn.Text = currentChar.ToString();
            MyText.CaretPosition.GetLineStartPosition(int.MinValue, out line);
            CurrentRow.Text = (-(line - 1)).ToString();
            if (!MyText.CanUndo) UndoMenu.IsEnabled = false;
            else UndoMenu.IsEnabled = true;
            if (!MyText.CanRedo) RedoMenu.IsEnabled = false;
            else RedoMenu.IsEnabled = true;
            if (MyText.Document.Blocks.Count == 0) SelectAllMenu.IsEnabled = false;
            else SelectAllMenu.IsEnabled = true;            
        }
        private void MyText_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (MyText.Selection.Text.Length == 0)
            {
                CutMenu.IsEnabled = CopyMenu.IsEnabled = DelMenu.IsEnabled = false;
            }
            else CutMenu.IsEnabled = CopyMenu.IsEnabled = DelMenu.IsEnabled = true;
        }
        private void MyText_TextChanged(object sender, TextChangedEventArgs e)
        {
            PrimaryWindow.Title = _titleChanger.FileChanged();
            _fileInfo.IsModified = true;
        }
        private void PrimaryWindow_Loaded(object sender, RoutedEventArgs e)
        {
            scale = new ScaleTransform//Кинуть в Activated
            {
                ScaleX = 1.0,
                ScaleY = 1.0
            };
            MyText.LayoutTransform = scale;
            _fileInfo.Text = MyText;
            _titleChanger.Title = Title;
        }
        private void Goer_Click(object sender, RoutedEventArgs e)
        {
            GoerTo goerToWin = new GoerTo
            {
                Owner = this
            };
            if (goerToWin.ShowDialog() == true)
            {
                if (goerToWin.LineID != -1)
                {
                    try
                    {
                        int lineIndex = goerToWin.LineID - 1;//Задание переходочной строки
                        MyText.CaretPosition = MyText.Selection.Start.GetLineStartPosition(line);
                        MyText.CaretPosition = MyText.Selection.Start.GetLineStartPosition(lineIndex);
                    }
                    catch
                    {
                        MessageBox.Show("Указанной строки не найдено!", "Поиск строки",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    try
                    {
                        int posIndex = goerToWin.PosID - 1;//Задание номера позиции, взятого из окна
                        int charCount = MyText.Selection.Start.DocumentStart.GetOffsetToPosition(MyText.Selection.Start);
                        MyText.CaretPosition = MyText.Selection.Start.GetPositionAtOffset(-charCount);
                        MyText.CaretPosition = MyText.Selection.Start.GetPositionAtOffset(posIndex);                        
                    }
                    catch
                    {
                        MessageBox.Show("Указанной позиции не существует!", "Поиск позиции",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            
        }
    }
}
