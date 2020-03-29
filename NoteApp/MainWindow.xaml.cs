using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Timers;
using Microsoft.Win32;
using System.IO;
using Path = System.IO.Path;


namespace NoteApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


   

        

    public partial class MainWindow : Window
    {


        /*-------------------------- Variables ------------------------------------------*/
        private FileProcessor fileProcessor;
        private PreviewPanelProcessor previewPanelProcessor;
        
        private DateTime timeSinceAutoSave;
        Timer autoSaveTimer = new Timer();
        
        private string folderPath = "C:\\Users\\kl\\source\\repos\\NoteApp\\TextFiles\\";
        private string createFilePath = $@"{ DateTime.Now.Ticks}.xaml";
        
       

        /*-------------------------- Main ------------------------------------------*/

        public MainWindow()
        {
            InitializeComponent();
            previewPanelProcessor.
            
            
        }

         /*-------------------------- Title Bar Buttons------------------------------------------*/
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

        private void CloseApp(object sender, MouseButtonEventArgs e)
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

        private void MinimizeApp(object sender, MouseButtonEventArgs e)
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
        private void MaximizeApp(object sender, MouseButtonEventArgs e)
        {
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
        }


        //just had idea for the panel click function
        /* so if we have note */
        
        /*--------------------------Auto Save Feature------------------------------------------*/
        private void SaveFile()
        {

            SaveXamlPackage(createFilePath);
            /*if (FindName(yes) == thisRTB)
            {
                SaveXamlPackage(yes, createFilePath);
                
            }
            else
            {
                SaveXamlPackage(thisRTB, thisRTB.Name);
            }*/
            
        }


        private void SaveXamlPackage(string _fileName)
        {
            this.Dispatcher.Invoke(() =>
            {
                string fullPath = Path.Combine(folderPath, _fileName);
                TextRange range;
                FileStream fStream;
                range = new TextRange(yes.Document.ContentStart, yes.Document.ContentEnd);
                fStream = new FileStream(fullPath, FileMode.Create);
                range.Save(fStream, DataFormats.XamlPackage);
                fStream.Close();
            });
           
        }
        

        
        private void OnAutoSaveTimer(object sender, ElapsedEventArgs e)
        {
            
            double autoSaveInterval = 2;
            
            double test = ((e.SignalTime - timeSinceAutoSave).TotalSeconds);
            
            
            if (test > autoSaveInterval)
            {
               
                SaveFile();
                autoSaveTimer.Enabled = false;
                timeSinceAutoSave = DateTime.Now;

            }
        }


        private void TextHasChanged(object sender, TextChangedEventArgs e)
        { 
                thisRTB = sender as RichTextBox;
                timeSinceAutoSave = DateTime.Now;
                Timer autoSaveTimer = new Timer(2000);
                autoSaveTimer.Elapsed += OnAutoSaveTimer;
                autoSaveTimer.AutoReset = false;
                autoSaveTimer.Enabled = true;
           
        }

        


        /*---------------------------------- Methods To View Preview Panel Notes ----------------------------------------------------*/

        public void PreviewBoxClicked(object sender, MouseButtonEventArgs e)
        {
            
            var thisBox = sender as RichTextBox;
            
        }



      
        
    }
}
