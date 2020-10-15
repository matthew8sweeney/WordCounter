using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;


namespace WordCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string XamlExtension = ".xaml";
        private const string RtfExtension = ".rtf";
        private const string FDFilter = "Xaml Package (*.xaml)|*.xaml|Rich Text Format (*.rtf)|*.rtf";

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
        private void SaveCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFD = new SaveFileDialog { Filter = FDFilter };

            // show file dialog, then if a filename is chosen...
            if (saveFD.ShowDialog() == true)
            {
                TextRange range = new TextRange(
                    textEntry.Document.ContentStart,
                    textEntry.Document.ContentEnd
                );

                // save the content to a file
                FileStream fStream = new FileStream(saveFD.FileName, FileMode.Create);
                range.Save(fStream, DetectFileFormat(saveFD.FileName));
                fStream.Close();
            }
        }

        /// <summary>
        /// Load content into the RichTextBox editor from a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TextRange range;
            FileStream fStream;
            OpenFileDialog openFD = new OpenFileDialog { Filter = FDFilter };

            // show file dialog, then if a file is chosen...
            if (openFD.ShowDialog() == true)
            {
                range = new TextRange(
                    textEntry.Document.ContentStart,
                    textEntry.Document.ContentEnd
                );

                // load from the file
                fStream = new FileStream(openFD.FileName, FileMode.OpenOrCreate);
                range.Load(fStream, DetectFileFormat(openFD.FileName));
                fStream.Close();
            }
        }

        /// <summary>
        /// Returns a string to use as the dataFormat arg to Load/Save the given filename
        /// </summary>
        /// <param name="filename">name of the file to be </param>
        /// <returns type="string">A string that can be used by TextRange.Load or TextRange.Save for its dataFormat</returns>
        /// <exception cref="ArgumentException"></exception>
        private string DetectFileFormat(string filename)
        {
            switch (Path.GetExtension(filename))
            {
                case XamlExtension:
                    debugInfoDisplay.Text = "Loaded " + DataFormats.XamlPackage;
                    return DataFormats.XamlPackage;

                case RtfExtension:
                    debugInfoDisplay.Text = "Loaded" + DataFormats.Rtf;
                    return DataFormats.Rtf;

                default:
                    throw new ArgumentException("filename " + filename + " has unsupported extension");
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
