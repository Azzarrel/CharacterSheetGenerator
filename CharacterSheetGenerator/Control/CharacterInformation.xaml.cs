using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Data;
using System.Linq;

namespace CharacterSheetGenerator.Control
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class CharacterOverview : UserControl
    {

        private List<CharacterInformationModel> m_CharacterInformation = new List<CharacterInformationModel>();

        public static readonly DependencyProperty CharacterInformationProperty =
            DependencyProperty.Register("CharacterInformation", typeof(List<CharacterInformationModel>), typeof(CharacterOverview),
            new FrameworkPropertyMetadata(new List<CharacterInformationModel>(), OnCharacterInformationPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<CharacterInformationModel> CharacterInformation
        {
            get { return (List<CharacterInformationModel>)GetValue(CharacterInformationProperty); }
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

        private List<TraitCategoryModel> m_Traits = new List<TraitCategoryModel>();

        public static readonly DependencyProperty TraitProperty =
            DependencyProperty.Register("Traits", typeof(List<TraitCategoryModel>), typeof(CharacterOverview),
            new FrameworkPropertyMetadata(new List<TraitCategoryModel>(), OnTraitPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<TraitCategoryModel> Traits
        {
            get { return (List<TraitCategoryModel>)GetValue(TraitProperty); }
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

        private List<StatusValueModel> m_StatusValues = new List<StatusValueModel>();

        public static readonly DependencyProperty StatusValuesProperty =
            DependencyProperty.Register("StatusValues", typeof(List<StatusValueModel>), typeof(CharacterOverview),
            new FrameworkPropertyMetadata(new List<StatusValueModel>(), OnStatusValuesPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<StatusValueModel> StatusValues
        {
            get { return (List<StatusValueModel>)GetValue(StatusValuesProperty); }
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
