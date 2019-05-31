using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Data;
using System.Collections.ObjectModel;

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


        private double m_Total = new double();

        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(double), typeof(CharacterInventory),
            new FrameworkPropertyMetadata(new double(), OnTotalPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public double Total
        {
            get { return (double)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        private static void OnTotalPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterInventory UserControl = obj as CharacterInventory;
            UserControl.OnPropertyChanged("Total");
            UserControl.OnTotalPropertyChanged(e);


        }

        private void OnTotalPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Total = Total;

        }

        private ObservableCollection<InventoryItemModel> m_Inventory = new ObservableCollection<InventoryItemModel>();

        public static readonly DependencyProperty InventoryProperty =
            DependencyProperty.Register("Inventory", typeof(ObservableCollection<InventoryItemModel>), typeof(CharacterInventory),
            new FrameworkPropertyMetadata(new ObservableCollection<InventoryItemModel>(), OnInventoryPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<InventoryItemModel> Inventory
        {
            get { return (ObservableCollection<InventoryItemModel>)GetValue(InventoryProperty); }
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
