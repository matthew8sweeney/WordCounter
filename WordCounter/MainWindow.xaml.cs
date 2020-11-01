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
        private const string FDFilter = "WC Xaml Package (*.wcxaml)|*.wcxaml|"
                                      + "Xaml Package (*.xaml)|*.xaml|"
                                      + "Rich Text Format (*.rtf)|*.rtf|"
                                      + "Text Files (*.txt)|*.txt|"
                                      + "All Files (*.*)|*.*";

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
        /// Load content into the RichTextBox editor from a file chosen using an Open-File dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFD = new OpenFileDialog { Filter = FDFilter };

            // show file dialog, then if a file is chosen...
            if (openFD.ShowDialog() == true)
            {
                OpenFile(openFD.FileName);
            }
        }

        /// <summary>
        /// Load content into the RichTextBox editor from a file with the given name
        /// </summary>
        /// <param name="filename">Path to the file to open</param>
        public void OpenFile(string filename)
        {
            TextRange range = new TextRange(
                textEntry.Document.ContentStart,
                textEntry.Document.ContentEnd
            );

            try
            {
                // load from the file
                debugInfoDisplay.Text = "Load";
                FileStream fStream = new FileStream(filename, FileMode.OpenOrCreate);
                range.Load(fStream, DetectFileFormat(filename));
                fStream.Close();
            }
            catch (ArgumentException argErr)
            {
                // invalid/unaccepted filename
                debugInfoDisplay.Text = "Load failed due to invalid filename";
                MessageBox.Show(argErr.ToString());
            }
            catch (Exception err)
            {
                debugInfoDisplay.Text = "Load failed";
                MessageBox.Show(err.ToString());
            }
        }

        /// <summary>
        /// Returns a string to use as the dataFormat arg to Load/Save the given filename
        /// </summary>
        /// <param name="filename">Path to file</param>
        /// <returns type="string">A string that can be used by TextRange.Load or TextRange.Save for its dataFormat</returns>
        /// <exception cref="ArgumentException">If the file extension is not supported</exception>
        private string DetectFileFormat(string filename)
        {
            switch (Path.GetExtension(filename))
            {
                case ".wcxaml":  // just my own name for this app's Xaml Packages
                case ".xaml":
                    // adds debug text onto whatever is already there (from saving cmd)
                    debugInfoDisplay.Text += " " + DataFormats.XamlPackage + " " + filename;
                    return DataFormats.XamlPackage;

                case ".rtf":
                    debugInfoDisplay.Text += " " + DataFormats.Rtf + " " + filename;
                    return DataFormats.Rtf;

                case ".txt":
                    debugInfoDisplay.Text += " " + DataFormats.Text + " " + filename;
                    return DataFormats.Text;

                default:
                    throw new ArgumentException("filename " + filename + " has unsupported extension");
            }
        }

        /// <summary>
        /// Save the content of the RichTextBox to a file
        /// </summary>
        private void SaveAsCmd_Executed(object sender, ExecutedRoutedEventArgs e)
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
                debugInfoDisplay.Text = "Save";
                FileStream fStream = new FileStream(saveFD.FileName, FileMode.Create);
                range.Save(fStream, DetectFileFormat(saveFD.FileName));
                fStream.Close();
            }
        }

        /// <summary>
        /// Provide a Save-File dialog to save the editor content to a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {

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
