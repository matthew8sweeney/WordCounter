using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

#nullable enable


namespace WordCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // filter string given to file dialogs
        private const string FDFilterStr =
              "All Files (*.*)|*.*|"
            + "All Supported Files|*.wcxaml;*.xaml;*.rtf;*.txt|"
            + "WordCounter Xaml Package (*.wcxaml)|*.wcxaml|"
            + "Xaml Package (*.xaml)|*.xaml|"
            + "Rich Text Format (*.rtf)|*.rtf|"
            + "Text Files (*.txt)|*.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StatDisplayUpdater(object sender, TextChangedEventArgs e)
        {
            // aquire the text from the RichTextBox
            string text = new TextRange(textEditor.Document.ContentStart, textEditor.Document.ContentEnd).Text;

            // calculate word, line, and char counts
            (int wordCount, int lineCount) = CountWords(text);
            int charCount = text.Length - 2;

            // update the word, line, and char count displays
            wordCountDisplay.Text = wordCount.ToString();
            lineCountDisplay.Text = lineCount.ToString();
            charCountDisplay.Text = charCount.ToString();

            //DebugInfoDisplay.Text = "e:" + e.OriginalSource;
            //DebugInfoDisplay.Text = "sender Type:" + sender.GetType().ToString();
        }

        /// <summary>
        /// Count the number of words in a string
        /// </summary>
        private (int words, int lines) CountWords(string s)
        {
            int wordCount = 0;
            int lineCount = 0;
            int i = 0;
            while (i < s.Length)
            {
                // ignore whitespace until start of next word
                while (i < s.Length && Char.IsWhiteSpace(s[i]))
                {
                    // check if a newline
                    if (s[i] == '\n')
                    {
                        lineCount++;
                    }
                    i++;
                }

                // count this newly reached word
                if (i < s.Length && !Char.IsWhiteSpace(s[i]))
                {
                    wordCount++;
                }

                // scan to the end of this word
                while (i < s.Length && !Char.IsWhiteSpace(s[i]))
                {
                    i++;
                }
            }
            return (wordCount, lineCount);
        }

        private void MWEditor_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            textEditor.Editor_ContextMenuOpening(sender, e);
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
