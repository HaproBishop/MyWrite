namespace TitleChanger
{/// <summary>
/// Класс, представляющий собой BasicChanger с автоматизированным обновлением FullTitle, 
/// появляющееся после изменения одного из свойств
/// </summary>
    public class AdvancedChanger:BasicChanger
    {/// <summary>
    /// Имя окна. Автоматизирован FullTitle.
    /// </summary>
        public new string Title
        {
            get => base.Title;
            set
            {
                base.Title = value;
                CreateFullTitle();
            }
        }
        /// <summary>
        /// Имя файла. Автоматизирован FullTitle.
        /// </summary>
        public new string FileName
        {
            get => base.FileName;
            set
            {
                base.FileName = value;
                CreateFullTitle();
            }
        }     
        /// <summary>
        /// Наследование конструкторов
        /// </summary>
        public AdvancedChanger() : base() { }
        public AdvancedChanger(string title) : base(title) { }
        public AdvancedChanger(string title, string fileName) : base(title, fileName) { }
    }
}
