using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Data;
using System.Linq;
using System.Collections.ObjectModel;
using CharacterSheetGenerator.Model;
using System.Windows.Input;

namespace CharacterSheetGenerator.Control
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainSheet : UserControl
    {


        private double m_Expirience = new double();

        public static readonly DependencyProperty ExpirienceProperty =
            DependencyProperty.Register("Expirience", typeof(double), typeof(MainSheet),
            new FrameworkPropertyMetadata(new double(), OnExpiriencePropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public double Expirience
        {
            get { return (double)GetValue(ExpirienceProperty); }
            set { SetValue(ExpirienceProperty, value); }
        }

        private static void OnExpiriencePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            MainSheet UserControl = obj as MainSheet;
            UserControl.OnPropertyChanged("Expirience");
            UserControl.OnExpiriencePropertyChanged(e);


        }

        private void OnExpiriencePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Expirience = Expirience;

        }

        private ObservableCollection<CharacterInformationModel> m_CharacterInformation = new ObservableCollection<CharacterInformationModel>();

        public static readonly DependencyProperty CharacterInformationProperty =
            DependencyProperty.Register("CharacterInformation", typeof(ObservableCollection<CharacterInformationModel>), typeof(MainSheet),
            new FrameworkPropertyMetadata(new ObservableCollection<CharacterInformationModel>(), OnCharacterInformationPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<CharacterInformationModel> CharacterInformation
        {
            get { return (ObservableCollection<CharacterInformationModel>)GetValue(CharacterInformationProperty); }
            set { SetValue(CharacterInformationProperty, value); }
        }

        private static void OnCharacterInformationPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            MainSheet UserControl = obj as MainSheet;
            UserControl.OnPropertyChanged("CharacterInformation");
            UserControl.OnCharacterInformationPropertyChanged(e);


        }

        private void OnCharacterInformationPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_CharacterInformation = CharacterInformation;

        }

        private ObservableCollection<TraitCategoryModel> m_Traits = new ObservableCollection<TraitCategoryModel>();

        public static readonly DependencyProperty TraitProperty =
            DependencyProperty.Register("Traits", typeof(ObservableCollection<TraitCategoryModel>), typeof(MainSheet),
            new FrameworkPropertyMetadata(new ObservableCollection<TraitCategoryModel>(), OnTraitPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<TraitCategoryModel> Traits
        {
            get { return (ObservableCollection<TraitCategoryModel>)GetValue(TraitProperty); }
            set { SetValue(TraitProperty, value); }
        }

        private static void OnTraitPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            MainSheet UserControl = obj as MainSheet;
            UserControl.OnPropertyChanged("Traits");
            UserControl.OnTraitPropertyChanged(e);


        }

        private void OnTraitPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Traits = Traits;

        }

        private ObservableCollection<StatusValueModel> m_StatusValues = new ObservableCollection<StatusValueModel>();

        public static readonly DependencyProperty StatusValuesProperty =
            DependencyProperty.Register("StatusValues", typeof(ObservableCollection<StatusValueModel>), typeof(MainSheet),
            new FrameworkPropertyMetadata(new ObservableCollection<StatusValueModel>(), OnStatusValuesPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<StatusValueModel> StatusValues
        {
            get { return (ObservableCollection<StatusValueModel>)GetValue(StatusValuesProperty); }
            set { SetValue(StatusValuesProperty, value); }
        }

        private static void OnStatusValuesPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            MainSheet UserControl = obj as MainSheet;
            UserControl.OnPropertyChanged("StatusValues");
            UserControl.OnStatusValuesPropertyChanged(e);


        }

        private void OnStatusValuesPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_StatusValues = StatusValues;

        }

        private ICommand m_TraitClickCommand;

        public static readonly DependencyProperty TraitClickCommandProperty =
            DependencyProperty.Register("TraitClickCommand", typeof(ICommand), typeof(MainSheet),
            new FrameworkPropertyMetadata(null, OnTraitClickCommandPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ICommand TraitClickCommand
        {
            get { return (ICommand)GetValue(TraitClickCommandProperty); }
            set { SetValue(TraitClickCommandProperty, value); }
        }

        private static void OnTraitClickCommandPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            MainSheet UserControl = obj as MainSheet;
            UserControl.OnPropertyChanged("TraitClickCommand");
            UserControl.OnTraitClickCommandPropertyChanged(e);


        }

        private void OnTraitClickCommandPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_TraitClickCommand = TraitClickCommand;

        }


        public MainSheet()
        {
            InitializeComponent();
        }



        private void Charinfo_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBox box = sender as TextBox;
            box.Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void Charinfo_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBox box = sender as TextBox;
            box.Background = new SolidColorBrush(Colors.White);
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
