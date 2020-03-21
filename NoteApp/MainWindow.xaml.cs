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
        private Guid guid { get; set; }
        public NoteObject(RichTextBox rtb, Guid g)
        {
            richTextBox = rtb;
            guid = g;
        }

        
        
    
    }


    public partial class MainWindow : Window
    {
        private HashSet<NoteObject> NoteSet;
        private DateTime timeSinceAutoSave;
        private object Note;
        

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

        private void SaveFile(string str)
        {
            NoteObject thisNote = new NoteObject(NotePad, g);
            bool isEmpty = (NoteSet.Count == 0);
            
            
            
            if (NoteSet.Contains(thisNote) && !isEmpty)
            {
                //retrive this file
                //save to this file
            }
            else //new note
            {
                //create new file with guid
                var myUniqueFileName = $@"{Guid.NewGuid()}.txt";
                NoteObject newNote = new NoteObject(NotePad, Guid.NewGuid());
                

                //save to this new file

                //add this guid into the hashset
                NoteSet.Add(newNote);
            }
        }

        private string GetRichTextBoxContent(RichTextBox richTextBox)
        {
            RichTextBox myRichTextBox = new RichTextBox(NotePad.Document);

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
