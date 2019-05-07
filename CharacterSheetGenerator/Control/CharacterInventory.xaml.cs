using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Data;

namespace CharacterSheetGenerator.Control
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class CharacterInventory : UserControl
    {

        private SolidColorBrush m_CellColor = new SolidColorBrush();

        public static readonly DependencyProperty CellColorProperty =
            DependencyProperty.Register("CellColor", typeof(SolidColorBrush), typeof(CharacterInventory),
            new FrameworkPropertyMetadata(new SolidColorBrush(), OnCellColorPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public SolidColorBrush CellColor
        {
            get { return (SolidColorBrush)GetValue(CellColorProperty); }
            set { SetValue(CellColorProperty, value); }
        }

        private static void OnCellColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterInventory UserControl = obj as CharacterInventory;
            UserControl.OnPropertyChanged("CellColor");
            UserControl.OnCellColorPropertyChanged(e);
        }

        private void OnCellColorPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_CellColor = CellColor;

        }

        private List<InventoryItemModel> m_Inventory = new List<InventoryItemModel>();

        public static readonly DependencyProperty InventoryProperty =
            DependencyProperty.Register("Inventory", typeof(List<InventoryItemModel>), typeof(CharacterInventory),
            new FrameworkPropertyMetadata(new List<InventoryItemModel>(), OnInventoryPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<InventoryItemModel> Inventory
        {
            get { return (List<InventoryItemModel>)GetValue(InventoryProperty); }
            set { SetValue(InventoryProperty, value); }
        }

        private static void OnInventoryPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterInventory UserControl = obj as CharacterInventory;
            UserControl.OnPropertyChanged("Inventory");
            UserControl.OnInventoryPropertyChanged(e);


        }

        private void OnInventoryPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Inventory = Inventory;

        }


        public CharacterInventory()
        {
            CellColor = new SolidColorBrush(ColorHandler.IntToColor(15329769));
            InitializeComponent();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
