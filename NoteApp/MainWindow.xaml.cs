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
        
        private DateTime timeSinceAutoSave;
        private Timer autoSaveTimer;

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

        

        private void OnAutoSaveTimer(object sender, ElapsedEventArgs e)
        {
            double autoSaveInterval = 2;
            double test = ((e.SignalTime - timeSinceAutoSave).TotalSeconds);
            Console.WriteLine(test);
            
            if (test > autoSaveInterval)
            {
                //do autosave
                //set time since autosave to the date time now
                Console.WriteLine("it works!");
                timeSinceAutoSave = DateTime.Now;
            }
        }


        private void TextHasChanged(object sender, TextChangedEventArgs e)
        {
            timeSinceAutoSave = DateTime.Now;
            Timer autoSaveTimer = new Timer(2000);
            autoSaveTimer.Elapsed += OnAutoSaveTimer;
            autoSaveTimer.Enabled = true;

        }

        

        
        
    }
}
