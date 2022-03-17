namespace FileData
{/// <summary>
/// Используется для стандартной работы с файлами
/// </summary>
    public class FileInfo
    {
        public string Path { get; protected set; }//Путь к файлу
        public bool IsModified { get; set; }//Признак модификации файла
        public FileInfo() { }
        public FileInfo(string path)
        {
            Path = path;
        }/// <summary>
        /// Очистка пути
        /// </summary>
        public void Clear()
        {
            Path = "";
        }
    }
}
