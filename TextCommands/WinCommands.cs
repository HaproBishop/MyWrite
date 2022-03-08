using System.Windows.Input;

namespace TextCommands
{
    public class WinCommands
    {
        static WinCommands()
        {
            InputGestureCollection inputNewWindow = new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Shift|ModifierKeys.Alt, "Shift+Alt+N")
            };
            NewWindow = new RoutedCommand("NewWindow", typeof(WinCommands), inputNewWindow);
        }
        public static RoutedCommand NewWindow { get; set; }
    }
}
