using System.Windows.Input;

namespace TextCommands
{
    public class ScaleCommands
    {
            static ScaleCommands()
            {
                InputGestureCollection inputScalePlus = new InputGestureCollection()
            {
                new KeyGesture(Key.OemPlus, ModifierKeys.Control, "Ctrl+Плюс(+)")
            };
                CommandScalePlus = new RoutedCommand("CommandScalePlus", typeof(ScaleCommands), inputScalePlus);
                InputGestureCollection inputScaleMinus = new InputGestureCollection()
            {
                new KeyGesture(Key.OemMinus, ModifierKeys.Control, "Ctrl+Минус(-)")
            };
                CommandScaleMinus = new RoutedCommand("CommandScaleMinus", typeof(ScaleCommands), inputScaleMinus);
                InputGestureCollection inputScaleDefault = new InputGestureCollection()
            {
                new KeyGesture(Key.D9|Key.D0, ModifierKeys.Control, "Ctrl+()")
            };
                CommandScaleDefault = new RoutedCommand("CommandDefaultScale", typeof(ScaleCommands), inputScaleDefault);
            }
            public static RoutedCommand CommandScalePlus { get; private set; }
            public static RoutedCommand CommandScaleMinus { get; private set; }
            public static RoutedCommand CommandScaleDefault { get; private set; }
    }
}
