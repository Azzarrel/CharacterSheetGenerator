using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Data;
using System.Collections.ObjectModel;

namespace CharacterSheetGenerator.Control
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class CharacterLanguages : UserControl
    {


        private ObservableCollection<LanguageModel> m_Language = new ObservableCollection<LanguageModel>();

        public new static readonly DependencyProperty LanguageProperty =
        DependencyProperty.Register("Language", typeof(ObservableCollection<LanguageModel>), typeof(CharacterLanguages),
        new FrameworkPropertyMetadata(new ObservableCollection<LanguageModel>(), OnLanguagePropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public new ObservableCollection<LanguageModel> Language
        {
            get { return (ObservableCollection<LanguageModel>)GetValue(LanguageProperty); }
            set { SetValue(LanguageProperty, value); }
        }

        private static void OnLanguagePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterLanguages UserControl = obj as CharacterLanguages;
            UserControl.OnPropertyChanged("Language");
            UserControl.OnLanguagePropertyChanged(e);


        }

        private void OnLanguagePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Language = Language;

        }

        private ObservableCollection<LanguageModel> m_Writing = new ObservableCollection<LanguageModel>();

        public static readonly DependencyProperty WritingProperty =
        DependencyProperty.Register("Writing", typeof(ObservableCollection<LanguageModel>), typeof(CharacterLanguages),
        new FrameworkPropertyMetadata(new ObservableCollection<LanguageModel>(), OnWritingPropertyChanged));

        [EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<LanguageModel> Writing
        {
            get { return (ObservableCollection<LanguageModel>)GetValue(WritingProperty); }
            set { SetValue(WritingProperty, value); }
        }

        private static void OnWritingPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            CharacterLanguages UserControl = obj as CharacterLanguages;
            UserControl.OnPropertyChanged("Writing");
            UserControl.OnWritingPropertyChanged(e);


        }

        private void OnWritingPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            m_Writing = Writing;

        }



        public CharacterLanguages()
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
