using CharacterSheetGenerator.Model;
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

namespace CharacterSheetGenerator.View
{
    /// <summary>
    /// Interaktionslogik für CharacterSheetView.xaml
    /// </summary>
    public partial class CharacterSheetView : UserControl
    {
        public CharacterSheetView()
        {
            InitializeComponent();
        }



        public void Print()
        {


           

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {

                foreach (ControlModel model in Pages.Items)
                {
                    FrameworkElement e = model.Control;
                    //store original scale
                    Transform originalScale = e.LayoutTransform;
                    //get selected printer capabilities
                    System.Printing.PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);


                    //get scale of the print wrt to screen of WPF visual
                    double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / e.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
                                   e.ActualHeight);

                    //Transform the Visual to scale
                    e.LayoutTransform = new ScaleTransform(scale, scale);

                    //get the size of the printer page
                    System.Windows.Size sz = new System.Windows.Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                    //update the layout of the visual to the printer page size.
                    e.Measure(sz);
                    e.Arrange(new System.Windows.Rect(new System.Windows.Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

                    //now print the visual to printer to fit on the one page.
                    printDialog.PrintVisual(e, "My Print");


                    //apply the original transform.
                    e.LayoutTransform = originalScale;

                }

            }



        }
    }
}
