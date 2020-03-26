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



    /*-------------------------- Custom Object Partial Class ------------------------------------------*/

    public partial class NotePadObject 
    {
        private RichTextBox RichTextBox { get; set;}
        private string File { get; set; }
        private string Str { get; set; }
        public NotePadObject(RichTextBox rtb, string fileName, string stringToSave)
        {
            RichTextBox = rtb;
            File = fileName;
            Str = stringToSave;
            
        }

        public RichTextBox GetRTB(RichTextBox thisBox)
        {
            return thisBox;
        }

        public string GetStringToSave()
        {
            return this.Str;
        }

        public string GetFileName()
        {
            return this.File;
        }

        
        
    
    }

    



    public partial class MainWindow : Window
    {
        /*-------------------------- Variables ------------------------------------------*/

        private HashSet<NotePadObject> NoteSet = new HashSet<NotePadObject>();
        
        private DateTime timeSinceAutoSave;
        Timer autoSaveTimer = new Timer();
        private bool AlreadyInitialized = false;
        private string tempString;
        private string folderPath = "C:\\Users\\kl\\source\\repos\\NoteApp\\TextFiles\\";
        private string newFileName = $@"{ DateTime.Now.Ticks}.txt";

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

        /*--------------------------Auto Save Feature------------------------------------------*/
        private void SaveFile()
        {

            //So if there is already a file associated with this object then save there, otherwise just save to a new file

            NotePadObject thisNote = InitializeNote();
            bool isEmpty = (NoteSet.Count == 0);
            Console.WriteLine(NoteSet.Count);
            
            //this statement should check an a file associated with the given object is already in the collection
            //if this is true and the set is not empty then it will overwrite the contents of that file,
            //rather than creating a new file

            if (NoteSet.Contains(thisNote) && !isEmpty)
            {
                //retrive existing file path
                
                string path2 = thisNote.GetFileName();
                string ExistingDocPath = System.IO.Path.Combine(folderPath, path2);

                //save to file path
                File.WriteAllText(ExistingDocPath, thisNote.GetStringToSave());
                
                

                return;
            }
            else //writes text to a new text file
            {
                
                string testDocPath = System.IO.Path.Combine(folderPath, newFileName);

                //Writes the contents of string to save to docPath
                File.WriteAllText(testDocPath, thisNote.GetStringToSave());
                
                
                //Create and add note object to hashset  
                NoteSet.Add(thisNote);
               
            }
        }

        private string GetRichTextBoxContent(RichTextBox rtb)
        {

            //RichTextBox myRichTextBox = new RichTextBox();


            // FlowDocument myFlowDoc = new FlowDocument();

            //myRichTextBox.Document = myFlowDoc;

            
            TextRange textRange = new TextRange(
                    rtb.Document.ContentStart,
                    rtb.Document.ContentEnd
                );
            
            return textRange.Text;
        }

        private NotePadObject InitializeNote()
        {
            
            
            if(AlreadyInitialized == false)
            {
                RichTextBox textBox = NotePad;
                string fileName = $@"{ DateTime.Now.Ticks}.txt";
                tempString = fileName;
                string stringToSave = GetRichTextBoxContent(NotePad);

                NotePadObject thisNote = new NotePadObject(textBox, fileName, stringToSave);
                AlreadyInitialized = true;
                return thisNote;
            }
            else
            {
                RichTextBox textBox = NotePad;
                //this should set the string to just be the same name that was first created with the file
                string sameFileName = tempString;
                string stringToSave = GetRichTextBoxContent(NotePad);
                NotePadObject thisNote = new NotePadObject(textBox, sameFileName, stringToSave);
                return thisNote;
            }
            
            

            
        }

        private void OnAutoSaveTimer(object sender, ElapsedEventArgs e)
        {
            double autoSaveInterval = 2;
            
            double test = ((e.SignalTime - timeSinceAutoSave).TotalSeconds);
            
            
            if (test > autoSaveInterval)
            {
                
                //gets the text from richtextbox

                //set time since autosave to the date time now
                Console.WriteLine("it works!");
                
                SaveFile();
                autoSaveTimer.Enabled = false;
                timeSinceAutoSave = DateTime.Now;

            }
        }


        private void TextHasChanged(object sender, TextChangedEventArgs e)
        {

            timeSinceAutoSave = DateTime.Now;
            Timer autoSaveTimer = new Timer(2000);
            autoSaveTimer.Elapsed += OnAutoSaveTimer;
            autoSaveTimer.AutoReset = false;
            autoSaveTimer.Enabled = true;
            

        }

        /*---------------------------------- NotePreview Methods ----------------------------------------------------*/

        private string GetRTBFileName()
        {
            string currentContents = GetRichTextBoxContent(NotePad);
            string fileFound = "";
            Dictionary<string, string> compareThis = FileContentPair();
            foreach(KeyValuePair<string,string> kvp in compareThis)
            {
                if(kvp.Value == currentContents)
                {
                    fileFound = kvp.Key;
                    break;
                }
            }
            return fileFound;

        }
        
        private Dictionary<string, string> FileContentPair()
        {
            List<string> fileList = LoadList();
            List<string> contentList = ExtractContent();
            var dic = fileList.Zip(contentList, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
            return dic;
        }
        
        
        



        /*-------------------------- Load Note Previews Feature------------------------------------------*/

        private List<string> LoadList()
        {
            string dirPath = "C:\\Users\\kl\\source\\repos\\NoteApp\\TextFiles\\";
            List<string> fileList = Directory.EnumerateFiles(dirPath, "*.txt").ToList();
            fileList.Reverse();
            return fileList;
        }

        private List<string> ExtractContent()
        {
            
            string dirPath = "C:\\Users\\kl\\source\\repos\\NoteApp\\TextFiles\\";
            List<string> fileList = LoadList();
            List<string> contentList = new List<string>();
            foreach(string file in fileList)
            {
                string filePath = System.IO.Path.Combine(dirPath, file);
                contentList.Add(File.ReadAllText(filePath));

            }
            return contentList;
        }

        private List<RichTextBox> WriteContent()
        {
            List<string> contentToWrite = ExtractContent();
            List<RichTextBox> RTBList = new List<RichTextBox>();
            foreach(string str in contentToWrite)
            {
                FlowDocument flowDoc = new FlowDocument(new Paragraph(new Run(str)));
                RichTextBox rtb = new RichTextBox(flowDoc);
                RTBList.Add(rtb);
            }
            return RTBList;
        }

        private List<RichTextBox> StyleContent()
        {
            
            List<RichTextBox> NeedStyleList = WriteContent();
            List<RichTextBox> StyledList = new List<RichTextBox>();
            foreach(RichTextBox rtb in NeedStyleList)
            {

                Style style = Application.Current.FindResource("PreviewRTB") as Style;
                rtb.Style = style;
                this.Select(rtb, 0, int.MaxValue, Colors.WhiteSmoke);
                
                
                StyledList.Add(rtb);
            }
            return StyledList;
        }
        private void LoadContent()
        {
            List<RichTextBox> RTBToLoad = StyleContent();
            foreach(RichTextBox rtb in RTBToLoad)
            {
                StackHere.Children.Add(rtb);
                
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

        public void testmethod()
        {
            
        }
         
        
    }
}
