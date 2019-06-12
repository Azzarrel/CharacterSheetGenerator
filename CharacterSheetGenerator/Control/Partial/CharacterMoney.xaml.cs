using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Data;
using System.Collections.ObjectModel;
using CharacterSheetGenerator.Model;

namespace CharacterSheetGenerator.Control
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class CharacterMoney : UserControl
    {

        private MoneyModel m_Money = new MoneyModel();

        public static readonly DependencyProperty MoneyProperty =
            DependencyProperty.Register("Money", typeof(MoneyModel), typeof(CharacterMoney),
            new FrameworkPropertyMetadata(new MoneyModel(), OnMoneyPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public MoneyModel Money
        {
            get { return (MoneyModel)GetValue(MoneyProperty); }
            set { SetValue(MoneyProperty, value); }
        }

        private static void OnMoneyPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterMoney UserControl = obj as CharacterMoney;
            UserControl.OnPropertyChanged("Money");
            UserControl.OnMoneyPropertyChanged(e);


        }

        private void OnMoneyPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Money = Money;

        }

        private double m_CarryWeight = new double();

        public static readonly DependencyProperty CarryWeightProperty =
            DependencyProperty.Register("CarryWeight", typeof(double), typeof(CharacterMoney),
            new FrameworkPropertyMetadata(new double(), OnCarryWeightPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public double CarryWeight
        {
            get { return (double)GetValue(CarryWeightProperty); }
            set { SetValue(CarryWeightProperty, value); }
        }

        private static void OnCarryWeightPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterMoney UserControl = obj as CharacterMoney;
            UserControl.OnPropertyChanged("CarryWeight");
            UserControl.OnCarryWeightPropertyChanged(e);


        }

        private void OnCarryWeightPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_CarryWeight = CarryWeight;

        }


        public CharacterMoney()
        {
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
