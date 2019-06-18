using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.View;
using CharacterSheetGenerator.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

    private MainWindowViewModel m_DataContext;

    public MainWindow()
    {
      InitializeComponent();
      DataContext = new MainWindowViewModel();

    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        m_DataContext = (MainWindowViewModel)this.DataContext;
      }
      catch
      {

      }

    }

    private void Button3_Click(object sender, RoutedEventArgs re)
    {


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
      //        if(e.Name == "Übersicht")
      //        {
      //            printDialog.PrintVisual(Übersicht, "My Print");
      //        }
      //        if (e.Name == "Fertigkeiten")
      //        {
      //            printDialog.PrintVisual(Fertigkeiten, "My Print");
      //        }
      //        if (e.Name == "Kampf")
      //        {
      //            printDialog.PrintVisual(Kampf, "My Print");
      //        }
      //        if (e.Name == "Zauber")
      //        {
      //            printDialog.PrintVisual(Zauber, "My Print");
      //        }
      //        if (e.Name == "Inventar")
      //        {
      //            printDialog.PrintVisual(Inventar, "My Print");
      //        }


      //        //apply the original transform.
      //        e.LayoutTransform = originalScale;

      //}

      //    }



    }

    private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
      if ((Keyboard.Modifiers & ModifierKeys.Control) > 0)
      {
        m_DataContext.ZoomFactor += 0.1 * (e.Delta > 0 ? 1 : -1);
        e.Handled = true;
      }
    }
  }
}
