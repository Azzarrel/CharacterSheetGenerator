using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using CharacterSheetGenerator.Model.CombatSheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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

namespace CharacterSheetGenerator.Control
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class SpellSheet : UserControl
    {


        private SolidColorBrush m_CellColor = new SolidColorBrush();

        public static readonly DependencyProperty CellColorProperty =
            DependencyProperty.Register("CellColor", typeof(SolidColorBrush), typeof(SpellSheet),
            new FrameworkPropertyMetadata(new SolidColorBrush(), OnCellColorPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public SolidColorBrush CellColor
        {
            get { return (SolidColorBrush)GetValue(CellColorProperty); }
            set { SetValue(CellColorProperty, value); }
        }

        private static void OnCellColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SpellSheet UserControl = obj as SpellSheet;
            UserControl.OnPropertyChanged("CellColor");
            UserControl.OnCellColorPropertyChanged(e);
        }

        private void OnCellColorPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_CellColor = CellColor;

        }

        private ObservableCollection<WeaponModel> m_Weapons = new ObservableCollection<WeaponModel>();

        public static readonly DependencyProperty WeaponProperty =
            DependencyProperty.Register("Weapons", typeof(ObservableCollection<WeaponModel>), typeof(SpellSheet),
            new FrameworkPropertyMetadata(new ObservableCollection<WeaponModel>(), OnWeaponPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<WeaponModel> Weapons
        {
            get { return (ObservableCollection<WeaponModel>)GetValue(WeaponProperty); }
            set { SetValue(WeaponProperty, value); }
        }

        private static void OnWeaponPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SpellSheet UserControl = obj as SpellSheet;
            UserControl.OnPropertyChanged("Weapons");
            UserControl.OnWeaponPropertyChanged(e);
        }

        private void OnWeaponPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Weapons = Weapons;

        }

        private ObservableCollection<WeaponSelectModel> m_SelectedWeapons = new ObservableCollection<WeaponSelectModel>();

        public static readonly DependencyProperty SelectedWeaponProperty =
            DependencyProperty.Register("SelectedWeapons", typeof(ObservableCollection<WeaponSelectModel>), typeof(SpellSheet),
            new FrameworkPropertyMetadata(new ObservableCollection<WeaponSelectModel>(), OnSelectedWeaponPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<WeaponSelectModel> SelectedWeapons
        {
            get { return (ObservableCollection<WeaponSelectModel>)GetValue(SelectedWeaponProperty); }
            set { SetValue(SelectedWeaponProperty, value); }
        }

        private static void OnSelectedWeaponPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SpellSheet UserControl = obj as SpellSheet;
            UserControl.OnPropertyChanged("SelectedWeapons");
            UserControl.OnSelectedWeaponPropertyChanged(e);
        }

        private void OnSelectedWeaponPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_SelectedWeapons = SelectedWeapons;

        }

        private ObservableCollection<TraitCategoryModel> m_SpellTraits = new ObservableCollection<TraitCategoryModel>();

        public static readonly DependencyProperty TraitProperty =
            DependencyProperty.Register("SpellTraits", typeof(ObservableCollection<TraitCategoryModel>), typeof(SpellSheet),
            new FrameworkPropertyMetadata(new ObservableCollection<TraitCategoryModel>(), OnTraitPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<TraitCategoryModel> SpellTraits
        {
            get { return (ObservableCollection<TraitCategoryModel>)GetValue(TraitProperty); }
            set { SetValue(TraitProperty, value); }
        }

        private static void OnTraitPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SpellSheet UserControl = obj as SpellSheet;
            UserControl.OnPropertyChanged("SpellTraits");
            UserControl.OnTraitPropertyChanged(e);


        }

        private void OnTraitPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_SpellTraits = SpellTraits;

        }

        private ObservableCollection<SpellModel> m_Spells = new ObservableCollection<SpellModel>();

        public static readonly DependencyProperty SpellProperty =
            DependencyProperty.Register("Spells", typeof(ObservableCollection<SpellModel>), typeof(SpellSheet),
            new FrameworkPropertyMetadata(new ObservableCollection<SpellModel>(), OnSpellPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<SpellModel> Spells
        {
            get { return (ObservableCollection<SpellModel>)GetValue(SpellProperty); }
            set { SetValue(SpellProperty, value); }
        }

        private static void OnSpellPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SpellSheet UserControl = obj as SpellSheet;
            UserControl.OnPropertyChanged("Spells");
            UserControl.OnSpellPropertyChanged(e);
        }

        private void OnSpellPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Spells = Spells;

        }

        private ObservableCollection<RitualModel> m_Rituals = new ObservableCollection<RitualModel>();

        public static readonly DependencyProperty RitualProperty =
            DependencyProperty.Register("Rituals", typeof(ObservableCollection<RitualModel>), typeof(SpellSheet),
            new FrameworkPropertyMetadata(new ObservableCollection<RitualModel>(), OnRitualPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<RitualModel> Rituals
        {
            get { return (ObservableCollection<RitualModel>)GetValue(RitualProperty); }
            set { SetValue(RitualProperty, value); }
        }

        private static void OnRitualPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SpellSheet UserControl = obj as SpellSheet;
            UserControl.OnPropertyChanged("Rituals");
            UserControl.OnRitualPropertyChanged(e);
        }

        private void OnRitualPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Rituals = Rituals;

        }

        private ICommand m_TraitClickCommand;

        public static readonly DependencyProperty TraitClickCommandProperty =
            DependencyProperty.Register("TraitClickCommand", typeof(ICommand), typeof(SpellSheet),
            new FrameworkPropertyMetadata(null, OnTraitClickCommandPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ICommand TraitClickCommand
        {
            get { return (ICommand)GetValue(TraitClickCommandProperty); }
            set { SetValue(TraitClickCommandProperty, value); }
        }

        private static void OnTraitClickCommandPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SpellSheet UserControl = obj as SpellSheet;
            UserControl.OnPropertyChanged("TraitClickCommand");
            UserControl.OnTraitClickCommandPropertyChanged(e);


        }

        private void OnTraitClickCommandPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_TraitClickCommand = TraitClickCommand;

        }

        public SpellSheet()
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
