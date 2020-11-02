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
              "WordCounter Xaml Package (*.wcxaml)|*.wcxaml|"
            + "Xaml Package (*.xaml)|*.xaml|"
            + "Rich Text Format (*.rtf)|*.rtf|"
            + "Text Files (*.txt)|*.txt|"
            + "All Supported Files|*.wcxaml;*.xaml;*.rtf;*.txt|"
            + "All Files (*.*)|*.*";

        public MainWindow()
        {
            InitializeComponent();
            //AugmentEditorContextMenu();
        }

        /// <summary>
        /// Add extra (non-default) MenuItems to the editor's context menu
        /// </summary>
        private void AugmentEditorContextMenu()
        {
            MenuItem mi = new MenuItem();
            mi.Header = "Add to Dictionary";
            //mi.Click = null;

            textEditor.ContextMenu.Items.Add(mi);
        }

        private void StatDisplayUpdater(object sender, TextChangedEventArgs e)
        {
            // aquire the text from the RichTextBox
            string text = new TextRange(textEditor.Document.ContentStart, textEditor.Document.ContentEnd).Text;

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
