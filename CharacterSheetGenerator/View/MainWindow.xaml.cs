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
