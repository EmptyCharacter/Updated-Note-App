using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Timers;

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
        private DateTime timeSinceAutoSave;
        Timer autoSaveTimer = new Timer();
        
        private List<string> duhList;
        
        private string folderPath = "C:\\Users\\kl\\source\\repos\\NoteApp\\TextFiles\\";
        private string createFilePath = $@"{ DateTime.Now.Ticks}.xaml";
        private string newtTemp;
       

        /*-------------------------- Main ------------------------------------------*/

        public MainWindow()
        {
            InitializeComponent();
            PreviewPanelProcessor pre = new PreviewPanelProcessor();
            pre.LoadContent(StackHere);
            FileProcessor fp = new FileProcessor();
            duhList = fp.LoadFilesToList();
            
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


       
        
        /*--------------------------Auto Save Feature------------------------------------------*/
        private void SaveFile()
        {

            if(File.Exists(newtTemp))
            {
                //will save to specified file
                SaveXamlPackage(newtTemp);
            }
            else
            {
                //other wise will save to a new file
                SaveXamlPackage(createFilePath);
            }
           
            
        }

        //Helper method for SaveFile()
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
        
        //Handler for TextHasChangedEvent
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

        /*---------------------------------- Events ----------------------------------------------------*/
        private void TextHasChanged(object sender, TextChangedEventArgs e)
        { 
                //thisRTB = sender as RichTextBox;
                timeSinceAutoSave = DateTime.Now;
                Timer autoSaveTimer = new Timer(2000);
                autoSaveTimer.Elapsed += OnAutoSaveTimer;
                autoSaveTimer.AutoReset = false;
                autoSaveTimer.Enabled = true;
           
        }

     
        //Should display clicked text box into the main notepad view for editing
        public void PreviewBoxClicked(object sender, MouseButtonEventArgs e)
        {
            //Get the name property of this richtextbox
            var thisBox = sender as RichTextBox;
            string name = thisBox.Name;

            //Format this string and pass it through loadxaml method to load into main notepad
            string newName = UnfomatFile(name);
            string newt = Path.Combine(folderPath, newName);
            string time = File.GetLastWriteTime(newt).ToString();
            newtTemp = newt;
            double print = CalculateEdit(newName);
            LoadXamlPackage(newt); 
           

        }

        public double CalculateEdit(string path)
        {
            DateTime now = DateTime.Now;
            DateTime lastEdit = File.GetLastWriteTime(path);
            var temp = now - lastEdit;
            var newTemp = temp.TotalDays;
            return newTemp;
        }

        /*-------------------Helper Methods for PreviewBoxClicked Event -------------------------*/
        void LoadXamlPackage(string _fileName)
        {
            TextRange ranges;
            FileStream fStream;
            
                RichTextBox box = new RichTextBox();
                ranges = new TextRange(yes.Document.ContentStart, yes.Document.ContentEnd);
                fStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                ranges.Load(fStream, DataFormats.XamlPackage);
                
                fStream.Close();
            
        }


        public string UnfomatFile(string str)
        {
            str = str.Replace("_", ".");
            str = str.Replace("A", "");
            return str;
        }




    }
}
