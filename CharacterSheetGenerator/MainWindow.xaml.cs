using System;
using System.Collections.Generic;
using System.Data;
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
using System.Xml;

namespace CharacterSheetGenerator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CharacterSheetViewModel m_DataContext;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new CharacterSheetViewModel();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                m_DataContext = (CharacterSheetViewModel)this.DataContext;
            }
            catch
            {

            }

        }


            private void Button3_Click(object sender, RoutedEventArgs e)
        {


            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(Übersicht, "My First Print Job");
            }
        }


    }
}
