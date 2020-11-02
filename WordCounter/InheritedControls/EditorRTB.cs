using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace WordCounter.InheritedControls
{
    public class EditorRTB : RichTextBox
    {
        public EditorRTB() : base()
        {
            // specify event handler for creating the EditoRTB's ContextMenu
            //ContextMenuOpening += Editor_ContextMenuOpening;  // not actually work
        }

        /// <summary>
        /// Programatically generate the context menu of the EditorRTB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Editor_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // start with the static MenuItems
            ContextMenu cm = GetInitialContextMenu();

            // add on other MenuItems as appropriate (like the spellcheck stuff)
            AddConditionalMenuItems(cm);

            ContextMenu = cm;
        }

        /// <summary>
        /// Create a new ContextMenu containing all static MenuItems
        /// </summary>
        /// <returns>A ContextMenu containing the MenuItems that are always present</returns>
        private ContextMenu GetInitialContextMenu()
        {
            // start with a new ContextMenu
            ContextMenu cm = new ContextMenu();

            // initialize the "static" MenuItems that are always present on the menu
            var cut   = new MenuItem { Command = ApplicationCommands.Cut };
            var copy  = new MenuItem { Command = ApplicationCommands.Copy };
            var paste = new MenuItem { Command = ApplicationCommands.Paste };

            //// I think this may be worse style than adding each item on its own line
            //foreach (MenuItem mi in new MenuItem[] { cut, copy, paste })
            //    ContextMenu.Items.Add(mi);

            cm.Items.Add(cut);
            cm.Items.Add(copy);
            cm.Items.Add(paste);

            return cm;
        }

        private void AddConditionalMenuItems(ContextMenu cm)
        {
            // check if there is a spelling error at the caret/cursor's position
            if (GetSpellingError(CaretPosition) is SpellingError spError)
            {
                int miIndex = 0;
                foreach (string str in spError.Suggestions.Take(5))
                {
                    // create a new MenuItem for each spelling suggestion
                    MenuItem mi = new MenuItem()
                    {
                        Header = str,
                        FontWeight = FontWeights.Bold,
                        Command = EditingCommands.CorrectSpellingError,
                        CommandTarget = this,
                        CommandParameter = str
                    };
                    cm.Items.Insert(miIndex++, mi);
                }
                // add a separator, then "Ignore All", then another separator
                cm.Items.Insert(miIndex++, new Separator());

                cm.Items.Insert(miIndex++, new MenuItem
                {
                    Header = "Ignore All",
                    Command = EditingCommands.IgnoreSpellingError,
                    CommandTarget = this
                });

                cm.Items.Insert(miIndex++, new Separator());
            }
        }
    }
}
