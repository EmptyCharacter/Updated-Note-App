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

    public partial class NoteObject 
    {
        private RichTextBox richTextBox { get; set;}
        private string file { get; set; }
        private string str { get; set; }
        public NoteObject(RichTextBox rtb, string fileName, string stringToSave)
        {
            richTextBox = rtb;
            file = fileName;
            str = stringToSave;
            
        }

        public RichTextBox GetRTB(RichTextBox thisBox)
        {
            return thisBox;
        }

        public string GetStringToSave()
        {
            return this.str;
        }

        public string GetFileName()
        {
            return this.file;
        }

        
        
    
    }


    public partial class MainWindow : Window
    {
        private HashSet<NoteObject> NoteSet = new HashSet<NoteObject>();
        private DateTime timeSinceAutoSave;
        Timer autoSaveTimer = new Timer();
        private bool NoteInitialized = false;



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

        private void SaveFile()
        {
            

            NoteObject tempNote = InitializeNote();
            NoteObject thisNote = new NoteObject(null, null, null);

            if (NoteInitialized == true)
            {
                thisNote = tempNote;
            }
            
            bool isEmpty = (NoteSet.Count == 0);
            Console.WriteLine(NoteSet.Count);
            
            //this statement should check an a file associated with the given object is already in the collection
            //if this is true and the set is not empty then it will overwrite the contents of that file,
            //rather than creating a new file

            if (NoteSet.Contains(thisNote) && !isEmpty)
            {
                //retrive existing file path
                string path1 = "C:\\Users\\kl\\source\\repos\\NoteApp\\TextFiles\\";
                string path2 = thisNote.GetFileName();
                string ExistingDocPath = System.IO.Path.Combine(path1, path2);

                //save to file path
                File.WriteAllText(ExistingDocPath, thisNote.GetStringToSave());

                return;
            }
            else //writes text to a new text file
            {
                
                string path3 = "C:\\Users\\kl\\source\\repos\\NoteApp\\TextFiles\\";
                string path4 = thisNote.GetFileName();

                //Should declare path, but use the myUniqueFileName var as the file to save to...
                
                string testDocPath = System.IO.Path.Combine(path3, path4);

                //Writes the contents of string to save to docPath
                File.WriteAllText(testDocPath, thisNote.GetStringToSave());
                
                
                //Create and add note object to hashset 
                
                NoteSet.Add(thisNote);
                Console.WriteLine(NoteSet.Count());
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

        private NoteObject InitializeNote()
        {
            

            RichTextBox textBox = NotePad;
            string fileName = $@"{ DateTime.Now.Ticks}.txt";
            
            string stringToSave = GetRichTextBoxContent(NotePad);

            NoteObject thisNote = new NoteObject(textBox, fileName, stringToSave);

            NoteInitialized = true;

            return thisNote;
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

        

        
        
    }
}
