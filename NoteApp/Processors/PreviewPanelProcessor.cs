using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace NoteApp
{
    public class PreviewPanelProcessor
    {
        public void RegisterName(string name, object scopedElement);
        private List<string> fileList;
        private FileProcessor fp;
        private PreviewPanelProcessor pre;
        public PreviewPanelProcessor()
        {

            FileProcessor fileProcessor = new FileProcessor();

            //fileList = fp.LoadFilesToList();
        }
        
        public List<RichTextBox> LoadXamlToRTB()
        {
            TextRange range;
            FileStream fStream;
            List<string> fileNames = fileList;
            List<RichTextBox> textAdded = new List<RichTextBox>();

            foreach (string text in fileNames)
            {
                RichTextBox rtb = new RichTextBox();
                range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                fStream = new FileStream(text, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.XamlPackage);
                textAdded.Add(rtb);
            }
            return textAdded;
        }

        public List<RichTextBox> StyleContent()
        {

            List<RichTextBox> addStyle = LoadXamlToRTB();
            List<RichTextBox> styleAdded = new List<RichTextBox>();
            foreach (RichTextBox rtb in addStyle)
            {

                Style style = Application.Current.FindResource("PreviewRTB") as Style;
                rtb.Style = style;
                this.Select(rtb, 0, int.MaxValue, Colors.WhiteSmoke);
                styleAdded.Add(rtb);
            }
            return styleAdded;
        }
        public void LoadContent(StackPanel panel)
        {
            FileProcessor fp = new FileProcessor();
            fileList = fp.LoadFilesToList();
            if (fileList.Count != 0)
            {
                List<RichTextBox> RTBToLoad = BindHere();
                foreach (RichTextBox rtb in RTBToLoad)
                {
                    panel.Children.Add(rtb);
                }
            }
        }

        public List<RichTextBox> BindHere()
        {
            List<string> fileList = FormatName();
            List<RichTextBox> boxList = StyleContent();
            List<RichTextBox> bindedList = new List<RichTextBox>();
            
            for (var i = 0; i < fileList.Count; i++)
            {

                foreach (RichTextBox box in boxList)
                {
                    
                    box.Name = fileList[i];
                    this.RegisterName(box.Name, box);
                    bindedList.Add(box);
                    i++;

                }

            }

            return bindedList;
        }
        public List<string> FormatName()
        {
            List<string> newt = fileList;
            List<string> formattedList = new List<string>();
            foreach (string name in newt)
            {
                string newName = Path.GetFileName(name);
                string validName = "A" + newName;
                validName = validName.Replace(".", "_");
                formattedList.Add(validName);

            }
            return formattedList;
        }


        /* ---------------------Helper Methods for StyleContent-------------------------------*/
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
