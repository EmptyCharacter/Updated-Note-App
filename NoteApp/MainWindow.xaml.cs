using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoteApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool textHasChanged = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch(Exception)
            {

            }
        }

        private void closeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void minimizeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private void AutoSave()
        {
            //check if CheckText has been called
            if(textHasChanged == true)
            {
                //if true, then; check if a file for this object already exists
                if()
                {
                    //if file exists, then; overwrite that same file
                }
                else
                {
                    //if DNE, then; create a new file and write there.
                }
            }
            else
            {
                //if false, then; do nothing
            }
        }

        //check if a file already exists
        private void CheckFile()
        {
            
            if()
            {

            }
        }


        //Run method when the text has been updated
        private void CheckText(object sender, TextChangedEventArgs e)
        {
            if (textHasChanged == false)
            {
                textHasChanged = true;
                AutoSave();
            }
            textHasChanged = false;
        }

    }
}
