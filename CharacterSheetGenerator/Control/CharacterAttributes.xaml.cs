using System;
using System.Collections.Generic;
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
    public partial class CharacterAttributes : UserControl
    {
        private List<AttributeModel> m_AttributeList = new List<AttributeModel>();

        public static readonly DependencyProperty AttributeListProperty =
            DependencyProperty.Register("AttributeList", typeof(List<AttributeModel>), typeof(CharacterAttributes),
            new FrameworkPropertyMetadata(new List<AttributeModel>(), OnAttributeListPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public List<AttributeModel> AttributeList
        {
            get { return (List<AttributeModel>)GetValue(AttributeListProperty); }
            set { SetValue(AttributeListProperty, value); }
        }

        private static void OnAttributeListPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterAttributes UserControl = obj as CharacterAttributes;
            UserControl.OnPropertyChanged("AttributeList");
            UserControl.OnAttributeListPropertyChanged(e);
        }

        private void OnAttributeListPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_AttributeList = AttributeList;

        }

        public CharacterAttributes()
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
