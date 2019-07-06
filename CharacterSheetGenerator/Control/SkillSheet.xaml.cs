using CharacterSheetGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CharacterSheetGenerator.Control
{
    /// <summary>
    /// Interaktionslogik für SkillSheet.xaml
    /// </summary>
    public partial class SkillSheet : UserControl
    {
        private SolidColorBrush m_CellColor = new SolidColorBrush();

        public static readonly DependencyProperty CellColorProperty =
            DependencyProperty.Register("CellColor", typeof(SolidColorBrush), typeof(SkillSheet),
            new FrameworkPropertyMetadata(new SolidColorBrush(), OnCellColorPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public SolidColorBrush CellColor
        {
            get { return (SolidColorBrush)GetValue(CellColorProperty); }
            set { SetValue(CellColorProperty, value); }
        }

        private static void OnCellColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            SkillSheet UserControl = obj as SkillSheet;
            UserControl.OnPropertyChanged("CellColor");
            UserControl.OnCellColorPropertyChanged(e);
        }

        private void OnCellColorPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_CellColor = CellColor;

        }

        public SkillSheet()
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
