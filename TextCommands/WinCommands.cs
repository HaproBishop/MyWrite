using System.Windows.Input;

namespace TextCommands
{/// <summary>
/// Статический класс оконных команд
/// </summary>
    public class WinCommands
    {
        static WinCommands()
        {
            InputGestureCollection inputNewWindow = new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Alt|ModifierKeys.Shift, "Alt+Shift+N")
            };
            NewWindow = new RoutedCommand("NewWindow", typeof(WinCommands), inputNewWindow);
        }
        public static RoutedCommand NewWindow { get; set; }//Команда для создания экземпляра окна
    }
}
