using System.Windows.Input;

namespace TextCommands
{/// <summary>
///  Статический класс команд по работе с масштабом
/// </summary>
    public class ScaleCommands
    {
            static ScaleCommands()
            {
                InputGestureCollection inputScalePlus = new InputGestureCollection()
            {
                new KeyGesture(Key.OemPlus, ModifierKeys.Alt, "Alt+Плюс(+)")
            };
                CommandScalePlus = new RoutedCommand("CommandScalePlus", typeof(ScaleCommands), inputScalePlus);
                InputGestureCollection inputScaleMinus = new InputGestureCollection()
            {
                new KeyGesture(Key.OemMinus, ModifierKeys.Alt, "Alt+Минус(-)")
            };
                CommandScaleMinus = new RoutedCommand("CommandScaleMinus", typeof(ScaleCommands), inputScaleMinus);
                InputGestureCollection inputScaleDefault = new InputGestureCollection()
            {
                new KeyGesture(Key.D9|Key.D0, ModifierKeys.Alt, "Alt+()")
            };
                CommandScaleDefault = new RoutedCommand("CommandDefaultScale", typeof(ScaleCommands), inputScaleDefault);
            }
            public static RoutedCommand CommandScalePlus { get; private set; }//Команда увеличения масштаба
            public static RoutedCommand CommandScaleMinus { get; private set; }//Команда уменьшения масштаба
            public static RoutedCommand CommandScaleDefault { get; private set; }//Команда установки дефолтного масштаба
    }
}
