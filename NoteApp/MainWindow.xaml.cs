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
        private string guid { get; set; }
        public NoteObject(RichTextBox rtb, string str)
        {
            richTextBox = rtb;
            guid = str;
        }

        
        
    
    }


    public partial class MainWindow : Window
    {
        private HashSet<NoteObject> NoteSet;
        private DateTime timeSinceAutoSave;
        
        

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

            NoteObject thisNote = new NoteObject(NotePad, stringToSave);
            bool isEmpty = (NoteSet.Count == 0);
            
            
            if (NoteSet.Contains(thisNote) && !isEmpty)
            {
                //retrive this file
                //save to this file
                return;
            }
            else //new note
            {
                //create a new unique name for textfile
                var myUniqueFileName = $@"{Guid.NewGuid()}.txt";

                //declare the path to save file
                string docPath = @"C:\Users\kl\source\repos\NoteApp\TextFiles\test.text";

                //write file 
                File.WriteAllText(docPath, stringToSave);
                
                //create new file with guid
                
               
                
                

                //save to this new file

                //add this guid into the hashset
                NoteObject newNote = new NoteObject(NotePad, myUniqueFileName);
                NoteSet.Add(newNote);
                Console.WriteLine(NoteSet.Count());
            }
        }

        private string GetRichTextBoxContent(RichTextBox richTextBox)
        {
            
            RichTextBox myRichTextBox = new RichTextBox();
            myRichTextBox = NotePad;

            FlowDocument myFlowDoc = new FlowDocument();

            myRichTextBox.Document = myFlowDoc;

            TextRange textRange = new TextRange(
                    myRichTextBox.Document.ContentStart,
                    myRichTextBox.Document.ContentEnd
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
                SaveFile(GetRichTextBoxContent(NotePad));
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
