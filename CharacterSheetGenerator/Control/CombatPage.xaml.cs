using CharacterSheetGenerator.CombatSheet.Model;
using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
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
    public partial class CombatPage : UserControl
    {
        private ObservableCollection<WeaponModel> m_Weapons = new ObservableCollection<WeaponModel>();

        public static readonly DependencyProperty WeaponProperty =
            DependencyProperty.Register("Weapons", typeof(ObservableCollection<WeaponModel>), typeof(CombatPage),
            new FrameworkPropertyMetadata(new ObservableCollection<WeaponModel>(), OnWeaponPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<WeaponModel> Weapons
        {
            get { return (ObservableCollection<WeaponModel>)GetValue(WeaponProperty); }
            set { SetValue(WeaponProperty, value); }
        }

        private static void OnWeaponPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CombatPage UserControl = obj as CombatPage;
            UserControl.OnPropertyChanged("Weapons");
            UserControl.OnWeaponPropertyChanged(e);
        }

        private void OnWeaponPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Weapons = Weapons;

        }

        private ObservableCollection<WeaponSelectModel> m_SelectedWeapons = new ObservableCollection<WeaponSelectModel>();

        public static readonly DependencyProperty SelectedWeaponProperty =
            DependencyProperty.Register("SelectedWeapons", typeof(ObservableCollection<WeaponSelectModel>), typeof(CombatPage),
            new FrameworkPropertyMetadata(new ObservableCollection<WeaponSelectModel>(), OnSelectedWeaponPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<WeaponSelectModel> SelectedWeapons
        {
            get { return (ObservableCollection<WeaponSelectModel>)GetValue(SelectedWeaponProperty); }
            set { SetValue(SelectedWeaponProperty, value); }
        }

        private static void OnSelectedWeaponPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CombatPage UserControl = obj as CombatPage;
            UserControl.OnPropertyChanged("SelectedWeapons");
            UserControl.OnSelectedWeaponPropertyChanged(e);
        }

        private void OnSelectedWeaponPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_SelectedWeapons = SelectedWeapons;

        }

        private ObservableCollection<TraitCategoryModel> m_CombatTraits = new ObservableCollection<TraitCategoryModel>();

        public static readonly DependencyProperty TraitProperty =
            DependencyProperty.Register("CombatTraits", typeof(ObservableCollection<TraitCategoryModel>), typeof(CombatPage),
            new FrameworkPropertyMetadata(new ObservableCollection<TraitCategoryModel>(), OnTraitPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<TraitCategoryModel> CombatTraits
        {
            get { return (ObservableCollection<TraitCategoryModel>)GetValue(TraitProperty); }
            set { SetValue(TraitProperty, value); }
        }

        private static void OnTraitPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CombatPage UserControl = obj as CombatPage;
            UserControl.OnPropertyChanged("CombatTraits");
            UserControl.OnTraitPropertyChanged(e);


        }

        private void OnTraitPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_CombatTraits = CombatTraits;

        }

        private ObservableCollection<MeleeWeaponModel> m_MeleeWeapons = new ObservableCollection<MeleeWeaponModel>();

        public static readonly DependencyProperty MeleeWeaponProperty =
            DependencyProperty.Register("MeleeWeapons", typeof(ObservableCollection<MeleeWeaponModel>), typeof(CombatPage),
            new FrameworkPropertyMetadata(new ObservableCollection<MeleeWeaponModel>(), OnMeleeWeaponPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<MeleeWeaponModel> MeleeWeapons
        {
            get { return (ObservableCollection<MeleeWeaponModel>)GetValue(MeleeWeaponProperty); }
            set { SetValue(MeleeWeaponProperty, value); }
        }

        private static void OnMeleeWeaponPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CombatPage UserControl = obj as CombatPage;
            UserControl.OnPropertyChanged("MeleeWeapons");
            UserControl.OnMeleeWeaponPropertyChanged(e);
        }

        private void OnMeleeWeaponPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_MeleeWeapons = MeleeWeapons;

        }

        private ObservableCollection<RangedWeaponModel> m_RangedWeapons = new ObservableCollection<RangedWeaponModel>();

        public static readonly DependencyProperty RangedWeaponProperty =
            DependencyProperty.Register("RangedWeapons", typeof(ObservableCollection<RangedWeaponModel>), typeof(CombatPage),
            new FrameworkPropertyMetadata(new ObservableCollection<RangedWeaponModel>(), OnRangedWeaponPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<RangedWeaponModel> RangedWeapons
        {
            get { return (ObservableCollection<RangedWeaponModel>)GetValue(RangedWeaponProperty); }
            set { SetValue(RangedWeaponProperty, value); }
        }

        private static void OnRangedWeaponPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CombatPage UserControl = obj as CombatPage;
            UserControl.OnPropertyChanged("RangedWeapons");
            UserControl.OnRangedWeaponPropertyChanged(e);
        }

        private void OnRangedWeaponPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_RangedWeapons = RangedWeapons;

        }

        private ObservableCollection<ArmorModel> m_Armor = new ObservableCollection<ArmorModel>();

        public static readonly DependencyProperty ArmorProperty =
            DependencyProperty.Register("Armor", typeof(ObservableCollection<ArmorModel>), typeof(CombatPage),
            new FrameworkPropertyMetadata(new ObservableCollection<ArmorModel>(), OnArmorPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<ArmorModel> Armor
        {
            get { return (ObservableCollection<ArmorModel>)GetValue(ArmorProperty); }
            set { SetValue(ArmorProperty, value); }
        }

        private static void OnArmorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CombatPage UserControl = obj as CombatPage;
            UserControl.OnPropertyChanged("Armor");
            UserControl.OnArmorPropertyChanged(e);
        }

        private void OnArmorPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Armor = Armor;

        }

        private ObservableCollection<OffHandModel> m_OffHands = new ObservableCollection<OffHandModel>();

        public static readonly DependencyProperty OffHandProperty =
            DependencyProperty.Register("OffHands", typeof(ObservableCollection<OffHandModel>), typeof(CombatPage),
            new FrameworkPropertyMetadata(new ObservableCollection<OffHandModel>(), OnOffHandPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<OffHandModel> OffHands
        {
            get { return (ObservableCollection<OffHandModel>)GetValue(OffHandProperty); }
            set { SetValue(OffHandProperty, value); }
        }

        private static void OnOffHandPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CombatPage UserControl = obj as CombatPage;
            UserControl.OnPropertyChanged("OffHands");
            UserControl.OnOffHandPropertyChanged(e);
        }

        private void OnOffHandPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_OffHands = OffHands;

        }

        private SolidColorBrush m_CellColor = new SolidColorBrush();

        public static readonly DependencyProperty CellColorProperty =
            DependencyProperty.Register("CellColor", typeof(SolidColorBrush), typeof(CombatPage),
            new FrameworkPropertyMetadata(new SolidColorBrush(), OnCellColorPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public SolidColorBrush CellColor
        {
            get { return (SolidColorBrush)GetValue(CellColorProperty); }
            set { SetValue(CellColorProperty, value); }
        }

        private static void OnCellColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CombatPage UserControl = obj as CombatPage;
            UserControl.OnPropertyChanged("CellColor");
            UserControl.OnCellColorPropertyChanged(e);
        }

        private ICommand m_TraitClickCommand;

        public static readonly DependencyProperty TraitClickCommandProperty =
            DependencyProperty.Register("TraitClickCommand", typeof(ICommand), typeof(CombatPage),
            new FrameworkPropertyMetadata(null, OnTraitClickCommandPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ICommand TraitClickCommand
        {
            get { return (ICommand)GetValue(TraitClickCommandProperty); }
            set { SetValue(TraitClickCommandProperty, value); }
        }

        private static void OnTraitClickCommandPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CombatPage UserControl = obj as CombatPage;
            UserControl.OnPropertyChanged("TraitClickCommand");
            UserControl.OnTraitClickCommandPropertyChanged(e);


        }

        private void OnTraitClickCommandPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_TraitClickCommand = TraitClickCommand;

        }

        private void OnCellColorPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_CellColor = CellColor;

        }


        public CombatPage()
        {
            InitializeComponent();
            CellColor = new SolidColorBrush(ColorHandler.IntToColor(15329769));

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
