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
using System.ComponentModel;
using System.Windows.Threading;
using Path = System.IO.Path;
using System.Text.RegularExpressions;

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
        
        
        private string folderPath = "C:\\Users\\kl\\source\\repos\\NoteApp\\TextFiles\\";
        private string createFilePath = $@"{ DateTime.Now.Ticks}.xaml";
        private RichTextBox thisRTB;
       

        /*-------------------------- Main ------------------------------------------*/

        public MainWindow()
        {
            InitializeComponent();
            LoadContent();
            
            
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
                //this timer should be reset if the user is still typing
                //to prevent excess calls to methods
                timeSinceAutoSave = DateTime.Now;
                Timer autoSaveTimer = new Timer(2000);
                autoSaveTimer.Elapsed += OnAutoSaveTimer;
                autoSaveTimer.AutoReset = false;
                autoSaveTimer.Enabled = true;
           
            
            

    }

        /*---------------------------------- Databind shit ----------------------------------------------------*/

        //create a method that will databind the fileName associated with the given RTB
        //should return that as a list
        public List<RichTextBox> BindHere()
        {
            List<string> fileList = LoadFilesToList();
            List<RichTextBox> boxList = StyleContent();
            List<RichTextBox> bindedList = new List<RichTextBox>();
            foreach (string name in fileList)
            {
                foreach(RichTextBox box in boxList)
                {
                    string newName = Path.GetFileName(name);
                    string validName = "A" + newName;
                    validName = Regex.Replace(validName, @".", "_");
                    box.Name = validName;
                    this.RegisterName(box.Name, box);
                    bindedList.Add(box);
                }
                
            }
            return bindedList;
        }



        /*---------------------------------- Methods To View Preview Panel Notes ----------------------------------------------------*/

        public void PreviewBoxClicked(object sender, MouseButtonEventArgs e)
        {
            
            var thisBox = sender as RichTextBox;
            
        }


        
        /*-------------------------- Loading Notes To Preview Panel From Directory Path------------------------------------------*/

        
        private List<string> LoadFilesToList()
        {
            List<string> thisList = Directory.EnumerateFiles(folderPath, "*.xaml").ToList();
            List<string> filesAdded = new List<string>();
            foreach(string s in thisList)
            {
                filesAdded.Add(s);   
            } 
            filesAdded.Reverse();
            return filesAdded;
        }


        private List<RichTextBox> LoadXamlToRTB()
        {
            TextRange range;
            FileStream fStream;
            List<string> fileNames = LoadFilesToList();
            List<RichTextBox> textAdded = new List<RichTextBox>();

            foreach(string text in fileNames)
            {
                RichTextBox rtb = new RichTextBox();
                range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                fStream = new FileStream(text, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.XamlPackage);
                textAdded.Add(rtb);
            }
            return textAdded;
        }

        private List<RichTextBox> StyleContent()
        {
            
            List<RichTextBox> addStyle = LoadXamlToRTB();
            List<RichTextBox> styleAdded = new List<RichTextBox>();
            foreach(RichTextBox rtb in addStyle)
            {

                Style style = Application.Current.FindResource("PreviewRTB") as Style;
                rtb.Style = style;
                this.Select(rtb, 0, int.MaxValue, Colors.WhiteSmoke);
                styleAdded.Add(rtb);  
            }
            return styleAdded;
        }
        private void LoadContent()
        {
            if(LoadFilesToList().Count != 0)
            {
                List<RichTextBox> RTBToLoad = BindHere();
                foreach (RichTextBox rtb in RTBToLoad)
                {
                    StackHere.Children.Add(rtb);

                }
            }
        }

        private static TextPointer GetTextPointAt(TextPointer from, int pos)
        {
            TextPointer ret = from;
            int i = 0;

            while ((i < pos) && (ret != null))
            {
                if ((ret.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text) || (ret.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.None))
                    i++;

                if (ret.GetPositionAtOffset(1, LogicalDirection.Forward) == null)
                    return ret;

                ret = ret.GetPositionAtOffset(1, LogicalDirection.Forward);
            }

            return ret;
        }

        internal string Select(RichTextBox rtb, int offset, int length, Color color)
        {
            // Get text selection:
            TextSelection textRange = rtb.Selection;

            // Get text starting point:
            TextPointer start = rtb.Document.ContentStart;

            // Get begin and end requested:
            TextPointer startPos = GetTextPointAt(start, offset);
            TextPointer endPos = GetTextPointAt(start, offset + length);

            // New selection of text:
            textRange.Select(startPos, endPos);
            
            // Apply property to the selection:
            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));

            // Return selection text:
            return rtb.Selection.Text;
        }

        
    }
}
