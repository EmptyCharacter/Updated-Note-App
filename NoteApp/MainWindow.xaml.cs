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
using System.Timers;
using Microsoft.Win32;
using System.IO;
using System.Xaml;


namespace NoteApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow : Window
    {

        private static Timer thisTimer = new Timer();
        private bool keyPressed = false;
        private bool thcCalled = false;
        private bool timeElapsed = false;



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


        void SaveXamlPackage(string _fileName)
        {
            TextRange range;
            FileStream fStream;
            range = new TextRange(NotePad.Document.ContentStart, NotePad.Document.ContentEnd);
            fStream = new FileStream(_fileName, FileMode.Create);
            range.Save(fStream, DataFormats.XamlPackage);
            fStream.Close();
        }

        private void HasChanged(Object sender, ElapsedEventArgs args)
        {
            thisTimer.Stop();
            SaveXamlPackage("C:\\Users\\kl\\source\\repos\\NoteApp\\TextFilestest.xaml");

        }

        private void TextHasChanged(object sender, TextChangedEventArgs e)
        {
            thcCalled = true;
            thisTimer.Start();
            
             
        }

        

        //currently works but will repeat after 2 seconds
        //should reset timer if the user is typing
        private void Timer(object sender, RoutedEventArgs e)
        {
            thisTimer.Interval = 2000;
            thisTimer.Elapsed += HasChanged;
            
        }
    }
}
