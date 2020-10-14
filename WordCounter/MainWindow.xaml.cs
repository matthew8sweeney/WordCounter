using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WordCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StatDisplayUpdater(object sender, TextChangedEventArgs e)
        {
            int wordCount = CountWords(TextEntry.Text);
            int charCount = TextEntry.Text.Length;

            WordCountDisplay.Text = wordCount.ToString();
            CharCountDisplay.Text = charCount.ToString();

            //DebugInfoDisplay.Text = "e:" + e.OriginalSource;
            //DebugInfoDisplay.Text = "sender Type:" + sender.GetType().ToString();
        }

        private int CountWords(string s)
        {
            int count = 0;
            int i = 0;
            while (i < s.Length)
            {
                // ignore whitespace until start of next word
                while (i < s.Length && Char.IsWhiteSpace(s[i]))
                {
                    i++;
                }

                // count this newly reached word
                if (i < s.Length && !Char.IsWhiteSpace(s[i]))
                {
                    count++;
                }

                // scan to the end of this word
                while (i < s.Length && !Char.IsWhiteSpace(s[i]))
                {
                    i++;
                }
            }
            return count;
        }

        private void SaveText(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true)
            {
                File.WriteAllText(sfd.FileName, TextEntry.Text);
            }
        }

        private void ExitApp(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
