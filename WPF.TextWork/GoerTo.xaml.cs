﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF.TextWork
{
    /// <summary>
    /// Логика взаимодействия для GoToLine.xaml
    /// </summary>
    public partial class GoerTo : Window
    {
        public int PosID { get; private set; } = -1;
        public int LineID { get; private set; } = -1;
        public GoerTo()
        {
            InitializeComponent();
        }
        private void LineGoer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LineID = Convert.ToInt32(LineValue.Text);
                DialogResult = true;
                Close();
            }
            catch
            {
                MessageBox.Show("Некорректно введено значение строки",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                LineValue.Clear();
                LineValue.Focus();
            }
        }        
        private void PosGoer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PosID = Convert.ToInt32(PosValue.Text);
                DialogResult = true;
                Close();
            }
            catch
            {
                MessageBox.Show("Некорректно введено значение строки",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                PosValue.Clear();
                PosValue.Focus();
            }
        }
        public void Clear()
        {
            PosID = -1;
            LineID = -1;
        }
    }
    /// <summary>
    /// Заготовленный класс для будущей реализации поиска слова в строке
    /// </summary>
    public class FindData
    {
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public string SubWord { get; set; }
        public TextRange MyText { get; set; }
        public FindData() { }
        public FindData(TextRange text)
        {
            MyText = text;
        }
        public FindData(TextRange text, string subWord)
        {
            MyText = text;
            SubWord = subWord;
        }
        public void GiveValues(TextRange text, string subWord)
        {
            MyText = text;
            SubWord = subWord;
        }
        public bool? FindSubWord()
        {
            if (SubWord != "")
            {
                StartIndex = MyText.Text.IndexOf(SubWord);
                if (StartIndex != -1)
                {
                    EndIndex = MyText.Text.Length - (MyText.Text.Length - SubWord.Length) - 1;
                    return true;
                }
            }
            return false;
        }
        public void Clear()
        {
            StartIndex = 0;
            EndIndex = 0;
            SubWord = "";
        }
    }
}