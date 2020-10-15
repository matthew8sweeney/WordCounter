using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;


namespace WordCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string XamlExtension = ".xaml";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StatDisplayUpdater(object sender, TextChangedEventArgs e)
        {
            // aquire the text from the RichTextBox
            string text = new TextRange(textEntry.Document.ContentStart, textEntry.Document.ContentEnd).Text;

            // calculate word and char counts
            int wordCount = CountWords(text);
            int charCount = text.Length;

            // update the word and char count displays
            wordCountDisplay.Text = wordCount.ToString();
            charCountDisplay.Text = charCount.ToString();

            //DebugInfoDisplay.Text = "e:" + e.OriginalSource;
            //DebugInfoDisplay.Text = "sender Type:" + sender.GetType().ToString();
        }

        /// <summary>
        /// Count the number of words in a string
        /// </summary>
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

        /// <summary>
        /// Save the content of the RichTextBox to a file
        /// </summary>
        private void SaveText(object sender, RoutedEventArgs e)
        {
            string format;
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Xaml Package Files (*.xaml)|*.xaml"
            };

            // show file dialog, then if a filename is chosen...
            if (sfd.ShowDialog() == true)
            {
                TextRange range = new TextRange(
                    textEntry.Document.ContentStart,
                    textEntry.Document.ContentEnd
                );

                // detect file format
                if (Path.GetExtension(sfd.FileName) == XamlExtension)
                {
                    format = DataFormats.XamlPackage;
                    debugInfoDisplay.Text = "Saved to " + DataFormats.XamlPackage;
                }
                else
                {
                    format = DataFormats.Rtf;
                    debugInfoDisplay.Text = "Saved to " + DataFormats.Rtf;
                }

                // save the content to a file
                FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                range.Save(fs, format);
                fs.Close();
            }
        }

        /// <summary>
        /// Load content into the RichTextBox editor from a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadText(object sender, RoutedEventArgs e)
        {
            TextRange range;
            FileStream fs;
            string format;
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Xaml Package Files (*.xaml)|*.xaml|Rich Text Format Files (*.rtf)|*.rtf"
            };

            // show file dialog, then if a file is chosen...
            if (ofd.ShowDialog() == true)
            {
                range = new TextRange(
                    textEntry.Document.ContentStart,
                    textEntry.Document.ContentEnd
                );

                // detect file format
                if (Path.GetExtension(ofd.FileName) == XamlExtension)
                {
                    format = DataFormats.XamlPackage;
                    debugInfoDisplay.Text = "Loaded " + DataFormats.XamlPackage;
                }
                else
                {
                    format = DataFormats.Rtf;
                    debugInfoDisplay.Text = "Loaded" + DataFormats.Rtf;
                }

                // load from the file
                fs = new FileStream(ofd.FileName, FileMode.OpenOrCreate);
                range.Load(fs, format);
                fs.Close();
            }
        }

        /// <summary>
        /// Exit the application gracefully
        /// </summary>
        private void ExitApp(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
