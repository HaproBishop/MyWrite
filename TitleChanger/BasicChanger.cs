namespace TitleChanger
{/// <summary>
/// Класс, использующийся для обновления имени окна относительно имени файла
/// </summary>
    public class BasicChanger
    {
        /// <summary>
        /// Имя окна
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Полное имя (имя файла + имя окна)
        /// </summary>
        public string FullTitle { get; protected set; }
        /// <summary>
        /// Представляет собой разделитель между FileName и Title.
        /// По умолчанию: " - "
        /// </summary>
        public string Divider { get; set; } = " - ";
        public BasicChanger() { }
        public BasicChanger(string title)
        {
            Title = title;
        }
        public BasicChanger(string title, string fileName)
        {
            Title = title;
            FileName = fileName;
        }
        /// <summary>
        /// Метод для одновременного обновления значений Title и FileName
        /// </summary>
        /// <param name="title">Имя окна</param>
        /// <param name="fileName">Имя файла</param>
        public void GiveValues(string title, string fileName)
        {
            Title = title;
            FileName = fileName;
        }
        /// <summary>
        /// Используется для создания полного наименования заголовка
        /// Возвращает FullTitle = FileName + Divider + Title
        /// Пример:"MyText.txt - MyProgram"
        /// </summary>
        /// <returns>_fullTitle - сформированное полное имя окна</returns>
        public string CreateFullTitle()
        {
            return FullTitle = FileName + Divider + Title;
        }
        /// <summary>
        /// Представляет собой индикацию внесенных изменений в файл.
        /// Возвращает полное наименование окна с "*".
        /// Пример:"*MyText.txt - MyProgram"
        /// </summary>
        /// <returns>"*" + FullTitle</returns>
        public string FileChanged()
        {
            return "*" + FullTitle;
        }
        /// <summary>
        /// Очищает имя файла
        /// </summary>
        public void Clear()
        {            
            FileName = "";
        }
    }
}
