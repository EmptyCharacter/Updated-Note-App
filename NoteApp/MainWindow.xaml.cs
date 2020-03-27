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


    public partial class Note 
    {
        private string str;
        private RichTextBox rtb;
        private string thisStr;
        public Note(string file, RichTextBox richTextBox)
        {
            str = file;
            rtb = richTextBox;
            
        }

        public RichTextBox GetNoteRTBName()
        {
            return rtb;
        }
        public string GetNoteFileName()
        {
            return str;
            
        }

       

        public void SetRichTextBoxName(RichTextBox thisBox)
        {
            rtb = thisBox;
        }
        public void SetNoteFileName(string thisFile)
        {
            str = thisFile;
        }
    
     
    }

        

    public partial class MainWindow : Window
    {


        /*-------------------------- Variables ------------------------------------------*/

        
        private DateTime timeSinceAutoSave;
        Timer autoSaveTimer = new Timer();
        
        
        private string folderPath = "C:\\Users\\kl\\source\\repos\\NoteApp\\TextFiles\\";
        private string createFilePath = $@"{ DateTime.Now.Ticks}.rtf";
        private string addedText;
        private bool testc;

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

           
            //this statement should check an a file associated with the given object is already in the directory
            //if this is true then it will overwrite the contents of that file,

            if (FileExists(NotePad) == true) 
            {
                //retrive existing file path

                string filePath = GetRTBFileName(NotePad);
                string existingFullPath = System.IO.Path.Combine(folderPath, filePath);

                //save to file path
                File.WriteAllText(existingFullPath, GetRichTextBoxContent(NotePad));

            }
            else //writes text to a new text file
            {

                string newFullPath = System.IO.Path.Combine(folderPath, createFilePath);

                //Writes the contents of string to save to docPath
                File.WriteAllText(newFullPath, GetRichTextBoxContent(NotePad));

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

            //only want to execute this if the preview event has not been fired
            //other wise just use the timer normally
            RichTextBox textBox = sender as RichTextBox;
            if (textBox != null)
            {
                addedText = GetRichTextBoxContent(textBox);
            }


            if (testc == false)
            {
                //this timer should be reset if the user is still typing
                //to prevent excess calls to methods
                timeSinceAutoSave = DateTime.Now;
                Timer autoSaveTimer = new Timer(2000);
                autoSaveTimer.Elapsed += OnAutoSaveTimer;
                autoSaveTimer.AutoReset = false;
                autoSaveTimer.Enabled = true;
            }
            
            


    }

        /*---------------------------------- Methods To View Preview Panel Notes ----------------------------------------------------*/

        public void PreviewBoxClicked(object sender, MouseButtonEventArgs e)
        {
            testc = true;
            var thisBox = sender as RichTextBox;
            //first check if notepad needs to be saved before continuing with swap
            //PreventDataLoss();
            
            MovePreview(thisBox);
            testc = false;
        }

        private void PreventDataLoss()
        {
            if (IsRichTextBoxEmpty(NotePad) == false)
            {
                //saves file then reupdates preview list
                SaveFile();

                //this call is unnessecary and slow, just add this object singly for now
                LoadContent();

            }
        }

        private void MovePreview(RichTextBox rtb)
        {
            NotePad.Document.Blocks.Clear();
            string text = GetRichTextBoxContent(rtb);

            NotePad.Document.Blocks.Add(new Paragraph(new Run(text)));
            this.Select(NotePad, 0, int.MaxValue, Colors.WhiteSmoke);

            if(FileExists(rtb) == true)
            {
                StackHere.Children.Remove(rtb);
            }
            

        }

        public bool IsRichTextBoxEmpty(RichTextBox rtb)
        {
            
            if (rtb.Document.Blocks.Count == 0) return true;
            TextPointer startPointer = rtb.Document.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward);
            TextPointer endPointer = rtb.Document.ContentEnd.GetNextInsertionPosition(LogicalDirection.Backward);
            return startPointer.CompareTo(endPointer) == 0;
        }
        private bool FileExists(RichTextBox rtb)
        {
            bool found = false;  
            string currentText = GetRichTextBoxContent(rtb);
            string temp = currentText;
            temp = temp.Replace("\r\n", string.Empty);
            Dictionary<string, string> compareThis = FileContentPair();
            foreach (KeyValuePair<string, string> kvp in compareThis)
            {
                Console.WriteLine(kvp.Value);
                if (kvp.Value.Replace("\r\n", string.Empty) == temp)
                {
                    
                    found = true;
                    break;
                }
                
            }
            return found;
        }
        

        private string GetRTBFileName(RichTextBox rtb)
        {
            string currentContents = GetRichTextBoxContent(rtb);
            currentContents.Replace("\r\n", string.Empty);
            string fileFound = "";
            Dictionary<string, string> compareThis = FileContentPair();
            foreach(KeyValuePair<string,string> kvp in compareThis)
            {
                if(kvp.Value.Replace("\r\n", string.Empty) == currentContents)
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
        
        
        /*-------------------------- Loading Notes To Preview Panel From Directory Path------------------------------------------*/

        private List<Note> LoadFilesToList()
        {
            List<string> thisList = Directory.EnumerateFiles(folderPath, "*.rtf").ToList();
            List<Note> filesAdded = new List<Note>();
            foreach(string s in thisList)
            {
                Note note = new Note(s, null);
                filesAdded.Add(note);
            }
            filesAdded.Reverse();
            return filesAdded;
        }

        private List<string> ExtractTextFromFiles()
        {
            List<Note> fileList = LoadFilesToList();
            List<string> contentList = new List<string>();
            foreach(Note n in fileList)
            {
                
                string filePath = System.IO.Path.Combine(folderPath, n.GetNoteFileName());
                contentList.Add(File.ReadAllText(filePath));

            }
            return contentList;
        }

        private List<RichTextBox> WriteTextToRTB()
        {
            List<string> contentToWrite = ExtractTextFromFiles();
            List<RichTextBox> textAdded = new List<RichTextBox>();

            foreach(string text in contentToWrite)
            {
                FlowDocument flowDoc = new FlowDocument(new Paragraph(new Run(text)));
                RichTextBox rtb = new RichTextBox(flowDoc);
                textAdded.Add(rtb);
            }
            textAdded.Reverse();
            return textAdded;
        }

        private List<RichTextBox> StyleContent()
        {
            
            List<RichTextBox> addStyle = WriteTextToRTB();
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

        
    }
}
