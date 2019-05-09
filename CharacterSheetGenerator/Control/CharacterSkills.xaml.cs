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
    public partial class CharacterSkills : UserControl
    {

        private ListCollectionView m_GroupedSkills;
        public ListCollectionView GroupedSkills
        {
            get
            {
                return m_GroupedSkills;
            }
            set
            {
                m_GroupedSkills = value;
                OnPropertyChanged("GroupedSkills");
            }
        }


        private SolidColorBrush m_CellColor = new SolidColorBrush();

        public static readonly DependencyProperty CellColorProperty =
            DependencyProperty.Register("CellColor", typeof(SolidColorBrush), typeof(CharacterSkills),
            new FrameworkPropertyMetadata(new SolidColorBrush(), OnCellColorPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public SolidColorBrush CellColor
        {
            get { return (SolidColorBrush)GetValue(CellColorProperty); }
            set { SetValue(CellColorProperty, value); }
        }

        private static void OnCellColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterSkills UserControl = obj as CharacterSkills;
            UserControl.OnPropertyChanged("CellColor");
            UserControl.OnCellColorPropertyChanged(e);
        }

        private void OnCellColorPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_CellColor = CellColor;

        }

        private ListCollectionView m_Skills;

        public static readonly DependencyProperty SkillsProperty =
            DependencyProperty.Register("Skills", typeof(ListCollectionView), typeof(CharacterSkills),
            new FrameworkPropertyMetadata(null, OnSkillsPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ListCollectionView Skills
        {
            get { return (ListCollectionView)GetValue(SkillsProperty); }
            set { SetValue(SkillsProperty, value); }
        }

        private static void OnSkillsPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterSkills UserControl = obj as CharacterSkills;
            UserControl.OnPropertyChanged("Skills");
            UserControl.OnSkillsPropertyChanged(e);

        }

        private void OnSkillsPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Skills = Skills;

        }

        public CharacterSkills()
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
