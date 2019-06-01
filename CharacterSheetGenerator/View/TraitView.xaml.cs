using CharacterSheetGenerator.ViewModel;
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
using System.Windows.Shapes;

namespace CharacterSheetGenerator.View
{
    /// <summary>
    /// Interaktionslogik für TraitView.xaml
    /// </summary>
    public partial class TraitView : Window
    {
        public TraitView()
        {
            InitializeComponent();
        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            ((TraitViewModel)DataContext).TraitGrid_AddingNewItem(sender, e);
        }
    }
}
