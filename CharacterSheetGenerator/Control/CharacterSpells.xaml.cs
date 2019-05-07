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
    public partial class CharacterSpells : UserControl
    {
        private List<WeaponModel> m_Weapons = new List<WeaponModel>();

        public static readonly DependencyProperty WeaponProperty =
            DependencyProperty.Register("Weapons", typeof(List<WeaponModel>), typeof(CharacterSpells),
            new FrameworkPropertyMetadata(new List<WeaponModel>(), OnWeaponPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<WeaponModel> Weapons
        {
            get { return (List<WeaponModel>)GetValue(WeaponProperty); }
            set { SetValue(WeaponProperty, value); }
        }

        private static void OnWeaponPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterSpells UserControl = obj as CharacterSpells;
            UserControl.OnPropertyChanged("Weapons");
            UserControl.OnWeaponPropertyChanged(e);
        }

        private void OnWeaponPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Weapons = Weapons;

        }

        private List<WeaponSelectModel> m_SelectedWeapons = new List<WeaponSelectModel>();

        public static readonly DependencyProperty SelectedWeaponProperty =
            DependencyProperty.Register("SelectedWeapons", typeof(List<WeaponSelectModel>), typeof(CharacterSpells),
            new FrameworkPropertyMetadata(new List<WeaponSelectModel>(), OnSelectedWeaponPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<WeaponSelectModel> SelectedWeapons
        {
            get { return (List<WeaponSelectModel>)GetValue(SelectedWeaponProperty); }
            set { SetValue(SelectedWeaponProperty, value); }
        }

        private static void OnSelectedWeaponPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterSpells UserControl = obj as CharacterSpells;
            UserControl.OnPropertyChanged("SelectedWeapons");
            UserControl.OnSelectedWeaponPropertyChanged(e);
        }

        private void OnSelectedWeaponPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_SelectedWeapons = SelectedWeapons;

        }

        private List<TraitCategoryModel> m_SpellTraits = new List<TraitCategoryModel>();

        public static readonly DependencyProperty TraitProperty =
            DependencyProperty.Register("SpellTraits", typeof(List<TraitCategoryModel>), typeof(CharacterSpells),
            new FrameworkPropertyMetadata(new List<TraitCategoryModel>(), OnTraitPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<TraitCategoryModel> SpellTraits
        {
            get { return (List<TraitCategoryModel>)GetValue(TraitProperty); }
            set { SetValue(TraitProperty, value); }
        }

        private static void OnTraitPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterSpells UserControl = obj as CharacterSpells;
            UserControl.OnPropertyChanged("SpellTraits");
            UserControl.OnTraitPropertyChanged(e);


        }

        private void OnTraitPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_SpellTraits = SpellTraits;

        }

        private List<SpellModel> m_Spells = new List<SpellModel>();

        public static readonly DependencyProperty SpellProperty =
            DependencyProperty.Register("Spells", typeof(List<SpellModel>), typeof(CharacterSpells),
            new FrameworkPropertyMetadata(new List<SpellModel>(), OnSpellPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<SpellModel> Spells
        {
            get { return (List<SpellModel>)GetValue(SpellProperty); }
            set { SetValue(SpellProperty, value); }
        }

        private static void OnSpellPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterSpells UserControl = obj as CharacterSpells;
            UserControl.OnPropertyChanged("Spells");
            UserControl.OnSpellPropertyChanged(e);
        }

        private void OnSpellPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Spells = Spells;

        }

        private List<RitualModel> m_Rituals = new List<RitualModel>();

        public static readonly DependencyProperty RitualProperty =
            DependencyProperty.Register("Rituals", typeof(List<RitualModel>), typeof(CharacterSpells),
            new FrameworkPropertyMetadata(new List<RitualModel>(), OnRitualPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<RitualModel> Rituals
        {
            get { return (List<RitualModel>)GetValue(RitualProperty); }
            set { SetValue(RitualProperty, value); }
        }

        private static void OnRitualPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterSpells UserControl = obj as CharacterSpells;
            UserControl.OnPropertyChanged("Rituals");
            UserControl.OnRitualPropertyChanged(e);
        }

        private void OnRitualPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Rituals = Rituals;

        }

        public CharacterSpells()
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
