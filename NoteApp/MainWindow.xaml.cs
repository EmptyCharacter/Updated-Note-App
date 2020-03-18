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

        private static Timer thisTimer;
        private bool TwoSecondsElapsed = false;
        



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
    
        
         
        void testc()
        {
            Console.WriteLine("sdfgsdf");
        }


        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            
            thisTimer.Stop();
            TwoSecondsElapsed = true;
        }
        private void Timer()
        {
            thisTimer = new System.Timers.Timer();
            thisTimer.Interval = 2000;
            thisTimer.AutoReset = true;
            thisTimer.Enabled = true;
            if (TwoSecondsElapsed == true)
            {
                testc();
            }
        }
       

        private void TextHasChanged(object sender, TextChangedEventArgs e)
        {
            Timer();
    
                        
        }
    }

    

}
