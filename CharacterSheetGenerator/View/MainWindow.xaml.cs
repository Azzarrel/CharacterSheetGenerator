using CharacterSheetGenerator.View;
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

namespace CharacterSheetGenerator.View
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

        //private void ScrollViewerOnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    var scv = sender as ScrollViewer;
        //    if (scv == null) return;
        //    scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
        //    e.Handled = true;
        //}


        private void Button3_Click(object sender, RoutedEventArgs re)
        {
            //ToAuskommentiert, da es weiterhin das Problem gibt, dass UserControls, die noch nicht geladen wurden keine Daten haben.
            //System.Windows.FrameworkElement[] elements = { Übersicht as System.Windows.FrameworkElement, Fertigkeiten as System.Windows.FrameworkElement, Kampf as System.Windows.FrameworkElement,
            //                                               Zauber as System.Windows.FrameworkElement, Inventar as System.Windows.FrameworkElement,};


            //PrintDialog printDialog = new PrintDialog();
            //if (printDialog.ShowDialog() == true)
            //{
            //    foreach (FrameworkElement e in elements)
            //    {
            //        //store original scale
            //        Transform originalScale = e.LayoutTransform;
            //        //get selected printer capabilities
            //        System.Printing.PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);

            //        //ToDo: Versuchen die hardcodierung weg zu kriegen, ohne die es aktuell 2 Probleme gibt: 
            //        // 1. Grids, die noch nicht geladen wurden haben keine Größe.
            //        // 2. Alle Grids außer dem aktuell offenen werden nicht richtig skaliert
            //        // Eigentlich anstelle der Zahlen wäre  e.ActualWidth und e.ActualHeight      
            //        //get scale of the print wrt to screen of WPF visual
            //        double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / 1281, capabilities.PageImageableArea.ExtentHeight /
            //                       1800);

            //        //Transform the Visual to scale
            //        e.LayoutTransform = new ScaleTransform(scale, scale);

            //        //get the size of the printer page
            //        System.Windows.Size sz = new System.Windows.Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

            //        //update the layout of the visual to the printer page size.
            //        e.Measure(sz);
            //        e.Arrange(new System.Windows.Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

            //        //now print the visual to printer to fit on the one page.
            //        printDialog.PrintVisual(e, "My Print");

            //        //apply the original transform.
            //        e.LayoutTransform = originalScale;

            //    }
            //    //printDialog.PrintVisual(Fertigkeiten, "My First Print Job");
            //    //printDialog.PrintVisual(Kampf, "My First Print Job");
            //    //printDialog.PrintVisual(Zauber, "My First Print Job");
            //    //printDialog.PrintVisual(Inventar, "My First Print Job");
            //}
        }


    }
}
