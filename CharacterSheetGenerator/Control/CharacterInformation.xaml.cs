using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Data;
using System.Linq;
using System.Collections.ObjectModel;

namespace CharacterSheetGenerator.Control
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class CharacterOverview : UserControl
    {

        private ObservableCollection<CharacterInformationModel> m_CharacterInformation = new ObservableCollection<CharacterInformationModel>();

        public static readonly DependencyProperty CharacterInformationProperty =
            DependencyProperty.Register("CharacterInformation", typeof(ObservableCollection<CharacterInformationModel>), typeof(CharacterOverview),
            new FrameworkPropertyMetadata(new ObservableCollection<CharacterInformationModel>(), OnCharacterInformationPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<CharacterInformationModel> CharacterInformation
        {
            get { return (ObservableCollection<CharacterInformationModel>)GetValue(CharacterInformationProperty); }
            set { SetValue(CharacterInformationProperty, value); }
        }

        private static void OnCharacterInformationPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterOverview UserControl = obj as CharacterOverview;
            UserControl.OnPropertyChanged("CharacterInformation");
            UserControl.OnCharacterInformationPropertyChanged(e);


        }

        private void OnCharacterInformationPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_CharacterInformation = CharacterInformation;

        }

        private ObservableCollection<TraitCategoryModel> m_Traits = new ObservableCollection<TraitCategoryModel>();

        public static readonly DependencyProperty TraitProperty =
            DependencyProperty.Register("Traits", typeof(ObservableCollection<TraitCategoryModel>), typeof(CharacterOverview),
            new FrameworkPropertyMetadata(new ObservableCollection<TraitCategoryModel>(), OnTraitPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<TraitCategoryModel> Traits
        {
            get { return (ObservableCollection<TraitCategoryModel>)GetValue(TraitProperty); }
            set { SetValue(TraitProperty, value); }
        }

        private static void OnTraitPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterOverview UserControl = obj as CharacterOverview;
            UserControl.OnPropertyChanged("Traits");
            UserControl.OnTraitPropertyChanged(e);


        }

        private void OnTraitPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Traits = Traits;

        }

        private ObservableCollection<StatusValueModel> m_StatusValues = new ObservableCollection<StatusValueModel>();

        public static readonly DependencyProperty StatusValuesProperty =
            DependencyProperty.Register("StatusValues", typeof(ObservableCollection<StatusValueModel>), typeof(CharacterOverview),
            new FrameworkPropertyMetadata(new ObservableCollection<StatusValueModel>(), OnStatusValuesPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<StatusValueModel> StatusValues
        {
            get { return (ObservableCollection<StatusValueModel>)GetValue(StatusValuesProperty); }
            set { SetValue(StatusValuesProperty, value); }
        }

        private static void OnStatusValuesPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterOverview UserControl = obj as CharacterOverview;
            UserControl.OnPropertyChanged("StatusValues");
            UserControl.OnStatusValuesPropertyChanged(e);


        }

        private void OnStatusValuesPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_StatusValues = StatusValues;

        }

        public CharacterOverview()
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
    }
}
