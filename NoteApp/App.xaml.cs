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
    }
}
