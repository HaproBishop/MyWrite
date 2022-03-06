using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FileData
{
    public class RichTextBoxData:FileInfo
    {
        RichTextBox _text;//Текст в RichTextBox
        public RichTextBox Text
        {
            get => _text;
            set
            {
                _text = value;
                IsModified = true;
            }
        }
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
        }
        public bool? Save(string path)
        {
            Path = path;
            return Save();
        }
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
        }
        public bool? Open(string path)
        {
            Path = path;
            return Open();
        }
    }
}
