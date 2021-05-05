using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

#nullable enable


namespace WordCounter
{
    public partial class MainWindow : Window
    {
        // contains the logic used for loading and saving files

        // (nullable) string storing the filename of the editor's current file, if applicable
        public string? CurEditorFilename { get; private set; } = null;

        /// <summary>
        /// Load content into the RichTextBox editor from a file chosen using an Open-File dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFD = new OpenFileDialog { Filter = FDFilterStr };

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
                textEditor.Document.ContentStart,
                textEditor.Document.ContentEnd
            );

            try
            {
                // load from the file
                debugInfoDisplay.Text = "Load";
                FileStream fStream = new FileStream(filename, FileMode.OpenOrCreate);
                range.Load(fStream, DetectFileFormat(filename));
                fStream.Close();

                // update the editor's current file
                CurEditorFilename = filename;
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
        /// Implementation for the ApplicationCommands.SaveAs command
        /// </summary>
        private void SaveAsCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveToFileWithDialog();
        }

        /// <summary>
        /// Implementation for the ApplicationCommands.Save command
        /// </summary>
        private void SaveCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // check if the editor has any particular file open
            if (CurEditorFilename is string)
            {
                SaveToFile(CurEditorFilename);
            }
            // otherwise, default to save-as functionality
            else
            {
                SaveToFileWithDialog();
            }
        }

        /// <summary>
        /// Save the content of the RichTextBox editor to a file chosen via a Save-File dialog
        /// </summary>
        /// <param name="filename"></param>
        public void SaveToFileWithDialog()
        {
            SaveFileDialog saveFD = new SaveFileDialog { Filter = FDFilterStr };

            // show file dialog, then if a filename is chosen...
            if (saveFD.ShowDialog() == true)
            {
                SaveToFile(saveFD.FileName);
            }
        }

        /// <summary>
        /// Save the content of the RichTextBox editor to a file with the given name
        /// </summary>
        /// <param name="filename"></param>
        public void SaveToFile(string filename)
        {
            TextRange range = new TextRange(
                textEditor.Document.ContentStart,
                textEditor.Document.ContentEnd
            );

            // save the content to a file
            debugInfoDisplay.Text = "Save";
            FileStream fStream = new FileStream(filename, FileMode.Create);
            range.Save(fStream, DetectFileFormat(filename));
            fStream.Close();

            // update the record of the editor's current file
            CurEditorFilename = filename;
        }


        // utility

        /// <summary>
        /// Returns a string to use as the dataFormat arg to Load/Save the given filename
        /// </summary>
        /// <param name="filename">Path to file</param>
        /// <returns type="string">A string that can be used by TextRange.Load or TextRange.Save for its dataFormat</returns>
        /// <exception cref="ArgumentException">If the file extension is not supported</exception>
        private string DetectFileFormat(string filename)
        {
            string ext = Path.GetExtension(filename);
            switch (ext)
            {
                case ".wcxaml":  // just my own name for this app's Xaml Packages
                case ".xaml":
                    // adds debug text onto whatever is already there (from saving/loading methods)
                    debugInfoDisplay.Text += " " + DataFormats.XamlPackage + ": " + filename;
                    return DataFormats.XamlPackage;

                case ".rtf":
                    debugInfoDisplay.Text += " " + DataFormats.Rtf + ": " + filename;
                    return DataFormats.Rtf;

                case ".txt":
                    debugInfoDisplay.Text += " " + DataFormats.Text + ": " + filename;
                    return DataFormats.Text;

                default:  // treat unrecognized file formats as plain 
                    //throw new ArgumentException("filename " + filename + " has unsupported extension");
                    debugInfoDisplay.Text += " " + ext + " as plain text: " + filename;
                    return DataFormats.Text;
            }
        }
    }
}
