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

     

        public void PreviewBoxClicked(object sender, MouseButtonEventArgs e)
        {
            
            var thisBox = sender as RichTextBox;
            string name = thisBox.Name;
             
            
            string newName = UnfomatFile(name);
            RichTextBox tempBox = new RichTextBox();
            tempBox = LoadXamlPackage(newName);
            DockHere.Children.Add(tempBox);

        }

        RichTextBox LoadXamlPackage(string _fileName)
        {
            TextRange range;
            FileStream fStream;
            
                RichTextBox box = new RichTextBox();
                range = new TextRange(box.Document.ContentStart, box.Document.ContentEnd);
                fStream = new FileStream(_fileName, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.XamlPackage);
                return box;
                //fStream.Close();
            
        }

        public RichTextBox WriteText(RichTextBox rtb, string file)
        {
            TextRange range;
            FileStream fStream;

            range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            fStream = new FileStream(file, FileMode.OpenOrCreate);
            range.Load(fStream, DataFormats.XamlPackage);
            return rtb;
        }

        public string UnfomatFile(string str)
        {
            str = str.Replace("_", ".");
            str = str.Replace("A", "");
            return str;
        }




    }
}
