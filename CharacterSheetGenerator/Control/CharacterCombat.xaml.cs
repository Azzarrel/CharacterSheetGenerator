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
    public partial class CharacterCombat : UserControl
    {
        private List<WeaponModel> m_Weapons = new List<WeaponModel>();

        public static readonly DependencyProperty WeaponProperty =
            DependencyProperty.Register("Weapons", typeof(List<WeaponModel>), typeof(CharacterCombat),
            new FrameworkPropertyMetadata(new List<WeaponModel>(), OnWeaponPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<WeaponModel> Weapons
        {
            get { return (List<WeaponModel>)GetValue(WeaponProperty); }
            set { SetValue(WeaponProperty, value); }
        }

        private static void OnWeaponPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterCombat UserControl = obj as CharacterCombat;
            UserControl.OnPropertyChanged("Weapons");
            UserControl.OnWeaponPropertyChanged(e);
        }

        private void OnWeaponPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Weapons = Weapons;

        }

        private List<WeaponSelectModel> m_SelectedWeapons = new List<WeaponSelectModel>();

        public static readonly DependencyProperty SelectedWeaponProperty =
            DependencyProperty.Register("SelectedWeapons", typeof(List<WeaponSelectModel>), typeof(CharacterCombat),
            new FrameworkPropertyMetadata(new List<WeaponSelectModel>(), OnSelectedWeaponPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<WeaponSelectModel> SelectedWeapons
        {
            get { return (List<WeaponSelectModel>)GetValue(SelectedWeaponProperty); }
            set { SetValue(SelectedWeaponProperty, value); }
        }

        private static void OnSelectedWeaponPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterCombat UserControl = obj as CharacterCombat;
            UserControl.OnPropertyChanged("SelectedWeapons");
            UserControl.OnSelectedWeaponPropertyChanged(e);
        }

        private void OnSelectedWeaponPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_SelectedWeapons = SelectedWeapons;

        }

        private List<TraitCategoryModel> m_CombatTraits = new List<TraitCategoryModel>();

        public static readonly DependencyProperty TraitProperty =
            DependencyProperty.Register("CombatTraits", typeof(List<TraitCategoryModel>), typeof(CharacterCombat),
            new FrameworkPropertyMetadata(new List<TraitCategoryModel>(), OnTraitPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<TraitCategoryModel> CombatTraits
        {
            get { return (List<TraitCategoryModel>)GetValue(TraitProperty); }
            set { SetValue(TraitProperty, value); }
        }

        private static void OnTraitPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterCombat UserControl = obj as CharacterCombat;
            UserControl.OnPropertyChanged("CombatTraits");
            UserControl.OnTraitPropertyChanged(e);


        }

        private void OnTraitPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_CombatTraits = CombatTraits;

        }

        private List<MeleeWeaponModel> m_MeleeWeapons = new List<MeleeWeaponModel>();

        public static readonly DependencyProperty MeleeWeaponProperty =
            DependencyProperty.Register("MeleeWeapons", typeof(List<MeleeWeaponModel>), typeof(CharacterCombat),
            new FrameworkPropertyMetadata(new List<MeleeWeaponModel>(), OnMeleeWeaponPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<MeleeWeaponModel> MeleeWeapons
        {
            get { return (List<MeleeWeaponModel>)GetValue(MeleeWeaponProperty); }
            set { SetValue(MeleeWeaponProperty, value); }
        }

        private static void OnMeleeWeaponPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterCombat UserControl = obj as CharacterCombat;
            UserControl.OnPropertyChanged("MeleeWeapons");
            UserControl.OnMeleeWeaponPropertyChanged(e);
        }

        private void OnMeleeWeaponPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_MeleeWeapons = MeleeWeapons;

        }

        private List<RangedWeaponModel> m_RangedWeapons = new List<RangedWeaponModel>();

        public static readonly DependencyProperty RangedWeaponProperty =
            DependencyProperty.Register("RangedWeapons", typeof(List<RangedWeaponModel>), typeof(CharacterCombat),
            new FrameworkPropertyMetadata(new List<RangedWeaponModel>(), OnRangedWeaponPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<RangedWeaponModel> RangedWeapons
        {
            get { return (List<RangedWeaponModel>)GetValue(RangedWeaponProperty); }
            set { SetValue(RangedWeaponProperty, value); }
        }

        private static void OnRangedWeaponPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterCombat UserControl = obj as CharacterCombat;
            UserControl.OnPropertyChanged("RangedWeapons");
            UserControl.OnRangedWeaponPropertyChanged(e);
        }

        private void OnRangedWeaponPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_RangedWeapons = RangedWeapons;

        }

        private List<ArmorModel> m_Armor = new List<ArmorModel>();

        public static readonly DependencyProperty ArmorProperty =
            DependencyProperty.Register("Armor", typeof(List<ArmorModel>), typeof(CharacterCombat),
            new FrameworkPropertyMetadata(new List<ArmorModel>(), OnArmorPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<ArmorModel> Armor
        {
            get { return (List<ArmorModel>)GetValue(ArmorProperty); }
            set { SetValue(ArmorProperty, value); }
        }

        private static void OnArmorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterCombat UserControl = obj as CharacterCombat;
            UserControl.OnPropertyChanged("Armor");
            UserControl.OnArmorPropertyChanged(e);
        }

        private void OnArmorPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Armor = Armor;

        }

        private List<OffHandModel> m_OffHands = new List<OffHandModel>();

        public static readonly DependencyProperty OffHandProperty =
            DependencyProperty.Register("OffHands", typeof(List<OffHandModel>), typeof(CharacterCombat),
            new FrameworkPropertyMetadata(new List<OffHandModel>(), OnOffHandPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<OffHandModel> OffHands
        {
            get { return (List<OffHandModel>)GetValue(OffHandProperty); }
            set { SetValue(OffHandProperty, value); }
        }

        private static void OnOffHandPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterCombat UserControl = obj as CharacterCombat;
            UserControl.OnPropertyChanged("OffHands");
            UserControl.OnOffHandPropertyChanged(e);
        }

        private void OnOffHandPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_OffHands = OffHands;

        }

        private SolidColorBrush m_CellColor = new SolidColorBrush();

        public static readonly DependencyProperty CellColorProperty =
            DependencyProperty.Register("CellColor", typeof(SolidColorBrush), typeof(CharacterCombat),
            new FrameworkPropertyMetadata(new SolidColorBrush(), OnCellColorPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public SolidColorBrush CellColor
        {
            get { return (SolidColorBrush)GetValue(CellColorProperty); }
            set { SetValue(CellColorProperty, value); }
        }

        private static void OnCellColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterCombat UserControl = obj as CharacterCombat;
            UserControl.OnPropertyChanged("CellColor");
            UserControl.OnCellColorPropertyChanged(e);
        }

        private void OnCellColorPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_CellColor = CellColor;

        }


        public CharacterCombat()
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
