using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
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
        readonly RichTextBoxData _fileInfo;//Вся информация, касающаяся работы с файлом
        readonly AdvancedChanger _titleChanger;//Changer для заголовка
        public MainWindow()
        {
            InitializeComponent();
            _titleChanger = new AdvancedChanger(Title);//Создание экземпляра с текущим наименованием окна
            _fileInfo = new RichTextBoxData();
        }
    private void Create_Click(object sender, RoutedEventArgs e)//--------------------------Нажатие "Создать"
        {
            if (_fileInfo.IsModified)
            {
                bool yesNo = false;//Yes - true, No - false. Используется для метода с вопросом
                if (PreCloseQuestion(ref yesNo) == true)
                {
                    if (yesNo)//Проверка ответа пользователя
                    {
                        Save_Click(sender, e);
                        if (!_fileInfo.IsModified) DefaultData();
                    }
                    else DefaultData();
                }
            }
            else
            {
                DefaultData();
            }
        }/// <summary>
        /// Метод, использующийся для выдачи сообщения сохранения
        /// </summary>
        /// <param name="yesNo">Отвечает за ответ пользователя</param>
        /// <returns>Значение успешности ответа. Если нажата Cancel - возвращает false</returns>
        private bool? PreCloseQuestion(ref bool yesNo)
        {
            MessageBoxResult result = MessageBox.Show("Хотите сохранить текущие изменения?",
                "Сохранение изменений", MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) return yesNo = true;
            if (result == MessageBoxResult.No) return true;
            return false;
        }/// <summary>
        /// Используется для установки значений "по дефолту"
        /// </summary>
        private void DefaultData()
        {
            MyText.Document.Blocks.Clear();
            _fileInfo.IsModified = false;
            _titleChanger.Clear();
            Title = _titleChanger.Title;
        }
        private void NewWindow_Click(object sender, RoutedEventArgs e)//--------------------------Нажатие "Новое окно"
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Show();
        }
        private void Open_Click(object sender, RoutedEventArgs e)//--------------------------Нажатие "Открыть"
        {
            if (_fileInfo.IsModified)//Начальная проверка на "несохраненность" текущего файла
            {
                bool yesNo = false;//Yes - true, No - false. Используется для метода с вопросом
                if (PreCloseQuestion(ref yesNo) == true)
                {
                    if (yesNo)//Проверка ответа пользователя
                    {
                        Save_Click(sender, e);
                    }
                }
                else return;//Происходит при нажатии Cancel
            }
            OpenFileDialog open = new OpenFileDialog
            {
                Title = "Открыть",
                Filter = "Текстовый документ(*.rtf)|*.rtf|Все файлы (*.*) |*.*",
                DefaultExt = "rtf",
            };
            if (open.ShowDialog() == true)
            {                                
                if (_fileInfo.Open(open.FileName) == true)
                {
                    _titleChanger.FileName = open.SafeFileName;
                    Title = _titleChanger.FullTitle;
                }
            }
            CurrentScale.Text = (scale.ScaleY * 100).ToString();//Обновление значения Scale при открытии диалогового окна (появляется баг, пришлось оставить)
        }
        bool _commandProver;//Используется для идентификации команды в условии
        private void Save_Click(object sender, RoutedEventArgs e)//--------------------------Нажатие "Сохранить (как)"
        {
            if (_titleChanger.FileName == "" || e.Source == SaveAs || e.Source == SaveAsTB || e.Source == Create || _commandProver)
            {
                SaveFileDialog save = new SaveFileDialog
                {
                    Title = "Сохранить как",
                    Filter = "Текстовый документ(*.rtf)|*.rtf|Все файлы (*.*) |*.*",
                    DefaultExt = "rtf",
                };
                if (save.ShowDialog() == true)
                {                    
                    if (_fileInfo.Save(save.FileName) == true)
                    {
                        _titleChanger.FileName = save.SafeFileName;//Добавление имени файла
                        Title = _titleChanger.FullTitle;//Задание нового "дефолтного" наименования
                    }
                }
            }
            else
            {
                _fileInfo.Save();
                Title = _titleChanger.FullTitle;
            }
            _commandProver = false;//Обнуление для последующего приминения
            CurrentScale.Text = (scale.ScaleY * 100).ToString();//Аналогично вышеописанному
        }/// <summary>
        /// Используется для команды Alt+Shift+S
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CSaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _commandProver = true;
            Save_Click(sender, e);
        }
        private void PrintText_Click(object sender, RoutedEventArgs e)//--------------------------Нажатие "Печать текста"
        {
            PrintDialog print = new PrintDialog();
            print.ShowDialog();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)//--------------------------Нажатие "Выход"
        {
            Close();
        }
        double _reservedHeight;//Используется для резерва значения высоты при скрытии StatusBar
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
        private void ScalePlus_Click(object sender, RoutedEventArgs e)//--------------------------Нажатие "Увеличить масштаб"
        {
            if (CurrentScale.Text != "200")
            {
                CurrentScale.Text = ((scale.ScaleY = scale.ScaleX += 0.1) * 100).ToString();
            }
        }
        private void ScaleMinus_Click(object sender, RoutedEventArgs e)//--------------------------Нажатие "Уменьшить масштаб"
        {
            if (CurrentScale.Text != "50")
            {
                CurrentScale.Text = ((scale.ScaleY = scale.ScaleX -= 0.1) * 100).ToString();
            }
        }
        private void DefaultScale_Click(object sender, RoutedEventArgs e)//--------------------------Нажатие "Восстановить по умолчанию (100%)"
        {
            CurrentScale.Text = "100";
            scale.ScaleX = scale.ScaleY = 1.0;
        }
        private void HelpMenu_Click(object sender, RoutedEventArgs e)//--------------------------Нажатие "Справка"
        {
            MessageBox.Show("У данной программы существует ряд следующих особенностей:\n" +
                "1) При изменении настроек окна сохранение происходит для текущего окна и его потомков(ctrl+shift+n)\n" +
                "2) По умолчанию используется 100% масштаб\n" +
                "3) Используются стандартные сочетания клавиш для быстрого использования функционала\n" +
                "4) При использовании переходчика для перехода к позиции, стоит помнить о том," +
                "что новый абзац дает еще 4 символа в общее количество\n" +
                "5) Доступна возможность использовать стандартное сочетание (Ctrl+Прокрутка колеса мыши(Вверх/Вниз))" +
                "для изменения масштаба.\n" +
                "6) Ввиду проблемы со стандартными сочетаниями Ctrl+Shift и прочие, связанные из-за RichTextBox,\n" +
                "было принято решение заменить клавиатурные жесты с Ctrl+Shift и Ctrl+плюс на Alt, вместо Ctrl. " +
                "Для стандартизации, было предпринято решение заменить в логических связках все жесты", "Справка",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void AboutProgram_Click(object sender, RoutedEventArgs e)//--------------------------Нажатие "О программе"
        {
            MessageBox.Show("Версия 1.0.0.1. Разработчиком является Лопаткин Сергей (GitHub.Name: Hapro Bishop) из группы ИСП-31", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        int line,//Offset от текущей строки -- используется для нескольких событий
            currentChar;//Текущее кол-во символо
        private void MyText_LayoutUpdated(object sender, EventArgs e)//-------------------------LayoutUpdated!!
        {
            currentChar = MyText.Selection.Start.DocumentStart.GetOffsetToPosition(MyText.CaretPosition) - 1;//Получение начальной позиции в документе
            if (currentChar == -1) CurrentColumn.Text = (currentChar + 2).ToString();//Отображение с условием возможных счетчиков при старте(удаление след. пустого элемента и прочее)
            else if (currentChar == 0) CurrentColumn.Text = (currentChar + 1).ToString();
            else CurrentColumn.Text = currentChar.ToString();
            MyText.CaretPosition.GetLineStartPosition(int.MinValue, out line);//Получение начальной позиции курсора и занесение значения сдвига в line относительно текущего
            CurrentRow.Text = (-(line - 1)).ToString();//Занесение в Status в правильном виде
            if (MyText.Document.Blocks.Count == 0) SelectAllMenu.IsEnabled = false;//Отвечает за Switch у SelectAll, чтобы не было лишней включенной кнопки
            else SelectAllMenu.IsEnabled = true;            
        }
        private void MyText_SelectionChanged(object sender, RoutedEventArgs e)//------------------------SelectionChanged!!!!!!!!!!
        {
            if (MyText.Selection.Text.Length == 0)//Switch для Del, чтобы выполняла свою поставленную задачу(чтобы не была как простой del)
            {
                DelMenu.IsEnabled = false;
            }
            else DelMenu.IsEnabled = true;
        }
        private void MyText_TextChanged(object sender, TextChangedEventArgs e)//---------------------TextChanged!!!!!!!!!!!!
        {
            PrimaryWindow.Title = _titleChanger.FileChanged();//Обновление Title при изменении
            _fileInfo.IsModified = true;
        }
        readonly RoutedCommand _altSaveAs = new RoutedCommand();
        private void PrimaryWindow_Loaded(object sender, RoutedEventArgs e)//------------------------------------------LoadedWin!!!!!!
        {
            scale = new ScaleTransform
            {
                ScaleX = 1.0,
                ScaleY = 1.0
            };
            MyText.LayoutTransform = scale;
            _fileInfo.Text = MyText;
            _titleChanger.Title = Title;
            _altSaveAs.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Alt|ModifierKeys.Shift));//NewWindow command adding
            PrimaryWindow.CommandBindings.Add(new CommandBinding(_altSaveAs, CSaveAs_Executed));
        }

        private void PrimaryWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)//---------------------------CLOUSING_WINDOW!!!
        {
            if (_fileInfo.IsModified)
            {
                bool yesNo = false;//Yes - true
                if (PreCloseQuestion(ref yesNo) == true)
                {
                    if (yesNo)
                    {
                        Save_Click(sender, new RoutedEventArgs());
                    }
                }
                else e.Cancel = true;
            }
        }
        bool _altIsPressed;//Используется для сканирования нажатого Alt, который, в свою очередь, используется в комбинации клавиш Alt+(-/+)
        private void PrimaryWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl) _altIsPressed = true; 
        }

        private void PrimaryWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.System) _altIsPressed = false;
        }

        private void PrimaryWindow_MouseWheel(object sender, MouseWheelEventArgs e)//------------------------------------------МАСШТАБ!!!
        {
            if (e.Delta > 0 && _altIsPressed) ScalePlus_Click(sender, e);
            if (e.Delta < 0 && _altIsPressed) ScaleMinus_Click(sender, e);
        }
        bool _firstTry;
        private void CurrentFont_SelectionChanged(object sender, SelectionChangedEventArgs e)//----------------------------------Задание FontFamily
        {
            if (_firstTry)
            {
                MyText.Selection.Start.Paragraph.FontFamily = new FontFamily(CurrentFont.Text);
            }
            _firstTry = true;
        }

        private void Goer_Click(object sender, RoutedEventArgs e)//-------------------------------------НАЖАТИЕ ПЕРЕХОДЧИКА
        {
            GoerTo goerToWin = new GoerTo
            {
                Owner = this
            };
            if (goerToWin.ShowDialog() == true)
            {
                if (goerToWin.LineID != -1)//Проверка на "невыбранность"
                {
                    try
                    {
                        int lineIndex = goerToWin.LineID - 1;//Задание переходочной строки
                        MyText.Focus();
                        MyText.CaretPosition = MyText.Selection.Start.GetLineStartPosition(line);//Переход к начальной строке
                        MyText.CaretPosition = MyText.Selection.Start.GetLineStartPosition(lineIndex);//Переход к установленной строке
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
                        MyText.Focus();
                        int charCount = MyText.Selection.Start.DocumentStart.GetOffsetToPosition(MyText.Selection.Start);//Нахождение индекса первого элемента в документе
                        MyText.CaretPosition = MyText.Selection.Start.GetPositionAtOffset(-charCount);//Перемещение курсора в начало документа
                        MyText.CaretPosition = MyText.Selection.Start.GetPositionAtOffset(posIndex);//Перемещение курсора на нужную позицию       
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
