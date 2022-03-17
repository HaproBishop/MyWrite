using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FileData
{/// <summary>
/// Потомок FileInfo, который используется для работы с RichTextBox от WPF
/// </summary>
    public class RichTextBoxData:FileInfo
    {
        public RichTextBox Text { get; set; }//Свойство для синхронизации текста
        /// <summary>
        /// Сохранение файла
        /// </summary>        
        /// <returns>true, если успешно сохранен или false, если не успешно</returns>
        public bool? Save()
        {
            try
            {
                using (FileStream fileStream = File.Create(Path))
                {
                    TextRange text = new TextRange(Text.Document.ContentStart, Text.Document.ContentEnd);
                    text.Save(fileStream, DataFormats.Rtf);
                    IsModified = false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }/// <summary>
         /// Сохранение файла
         /// </summary>
         /// <param name="path">Путь к файлу</param>
         /// <returns>true, если успешно сохранен или false, если не успешно</returns>
        public bool? Save(string path)
        {
            Path = path;
            return Save();
        }/// <summary>
        /// Открытие файла
        /// </summary>
        /// <returns>true, если успешно открыт или false, если не успешно</returns>
        public bool? Open()
        {
            try
            {
                using (FileStream fileStream = File.OpenRead(Path))
                {
                    TextRange text = new TextRange(Text.Document.ContentStart, Text.Document.ContentEnd);
                    text.Load(fileStream, DataFormats.Rtf);
                    IsModified = false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }/// <summary>
         /// Открытие файла
         /// </summary>
         /// <param name="path">Путь к файлу</param>
         /// <returns>true, если успешно открыт или false, если не успешно</returns>
        public bool? Open(string path)
        {
            Path = path;
            return Open();
        }
    }
}
