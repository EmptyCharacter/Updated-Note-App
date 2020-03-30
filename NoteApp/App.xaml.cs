using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NoteApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void PreviewBoxClicked(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.PreviewBoxClicked(sender, e);
            //throw new NotImplementedException();
        }

        private void BoxHover(object sender, MouseEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.BoxHover(sender, e);
        }

        private void BoxUnhover(object sender, MouseEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.BoxUnhover(sender, e);
        }
    }
}
