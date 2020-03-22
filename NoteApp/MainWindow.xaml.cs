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
        private string str { get; set; }
        public NoteObject(RichTextBox rtb, string fileName)
        {
            richTextBox = rtb;
            str = fileName;
        }

        public RichTextBox GetRTB(RichTextBox thisBox)
        {
            return thisBox;
        }

        
        
    
    }


    public partial class MainWindow : Window
    {
        private HashSet<NoteObject> NoteSet = new HashSet<NoteObject>();
        private DateTime timeSinceAutoSave;
        Timer autoSaveTimer = new Timer();



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

        private void SaveFile(string stringToSave)
        {
            
            NoteObject thisNote = new NoteObject(NotePad, "placeholder");
            
            bool isEmpty = (NoteSet.Count == 0);
            Console.WriteLine(NoteSet.Count);
            
            //this statement should check an a file associated with the given object is already in the collection
            //if this is true and the set is not empty then it will overwrite the contents of that file,
            //rather than creating a new file
            if (NoteSet.Contains(thisNote) && !isEmpty)
            {
                //retrive this file
                //save to this file
                return;
            }
            else //writes text to a new text file
            {
                //create a new unique name for textfile
                var myUniqueFileName = $@"{Guid.NewGuid()}.txt";

                //Should declare path, but use the myUniqueFileName var as the file to save to...
                string docPath = @"C:\Users\kl\source\repos\NoteApp\TextFiles\test.text";

                //Writes the contents of string to save to docPath
                File.WriteAllText(docPath, stringToSave);
                
                
                //Create and add note object to hashset 
                NoteObject newNote = new NoteObject(NotePad, myUniqueFileName);
                NoteSet.Add(newNote);
                Console.WriteLine(NoteSet.Count());
            }
        }

        private string GetRichTextBoxContent(RichTextBox richTextBox)
        {
            
            //RichTextBox myRichTextBox = new RichTextBox();
            

           // FlowDocument myFlowDoc = new FlowDocument();

            //myRichTextBox.Document = myFlowDoc;

            TextRange textRange = new TextRange(
                    richTextBox.Document.ContentStart,
                    richTextBox.Document.ContentEnd
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
                
                SaveFile(GetRichTextBoxContent(NotePad));
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
