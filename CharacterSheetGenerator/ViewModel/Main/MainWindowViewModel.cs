using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using CharacterSheetGenerator.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace CharacterSheetGenerator.ViewModel
{
    class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {

        public DataSet Data { get; set; } = new DataSet();

        private Double m_ZoomFactor = 1;
        public Double ZoomFactor
        {
            get
            {
                return m_ZoomFactor;
            }
            set
            {
                m_ZoomFactor = value;
                if (m_ZoomFactor < 0.1)
                    m_ZoomFactor = 0.1;
                if (m_ZoomFactor > 2)
                    m_ZoomFactor = 2;
                OnPropertyChanged();
            }
        }

        public UserControl PresentationElement
        {
            get { return Get<UserControl>(); }
            set { Set(value); }
        }

        public ObservableCollection<TabModel> Characters
        {
            get { return Get<ObservableCollection<TabModel>>(); }
            set { Set(value); }
        }

        public CharacterSheetViewModel SelectedCharacter
        {
            get { return Get<CharacterSheetViewModel>(); }
            set { Set(value); }
        }

        public MainWindowViewModel()
        {
            Characters = new ObservableCollection<TabModel>();
            InitializeStartUp();
            //Speicherpfad für die Charakterdaten prüfen und neuen Ordner anlegen, wenn noch keiner existiert
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Saves"))
            {
                DirectoryInfo di = Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Saves");
            }
            CreateCommands();
        }


        /// <summary>
        /// Starten mit Default Character vorgeladen
        /// </summary>
        private void InitializeStartUp()
        {

            PresentationElement = new CharacterSheetView();
            SelectedCharacter = new CharacterSheetViewModel();


            Data = new DataSet();
            XmlReader xmlData;

            DataSet l_Data = new DataSet();
            string[] files = Directory.GetFiles("Settings//Character//Default", "*.xml");


            if (files.Count() == 0)
            {

                throw new Exception("Die Charactervorlage ist nicht vorhanden. Ein neuer Character kann nicht erstellt werden");
            }
            Data = new DataSet();
            try
            {
                foreach (string s in files)
                {
                    l_Data = new DataSet();
                    xmlData = XmlReader.Create(s, new XmlReaderSettings());
                    l_Data.ReadXml(xmlData);
                    Data.Merge(l_Data);
                    xmlData.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            SelectedCharacter.LoadData(Data);
            PresentationElement.DataContext = SelectedCharacter;


            TabModel tab = new TabModel
            {
                ViewModel = SelectedCharacter,
                Name = SelectedCharacter.Name,
            };
            Characters.Add(tab);

        }


        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Commands

        //Komplette Dummy-Implementierung der Commands, sodass man es einfach erweitern kann
        private void CreateCommands()
        {
            SaveCommand = new RelayCommand(SaveMethod, CanExecute);
            LoadCommand = new RelayCommand(LoadMethod, CanExecute);
            PrintCommand = new RelayCommand(PrintMethod, CanExecute);

        }

        public ICommand SaveCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand PrintCommand { get; private set; }


        public void SaveMethod()
        {

            DataTable tblAttributeLink = Data.Tables["SVAttributeLink"].Copy();

            //DataSet vor dem Speichern mit aktuellen Daten füllen
            Data.Clear();
            SelectedCharacter.SaveData(tblAttributeLink);

            SaveWindowViewModel vm = new SaveWindowViewModel();
            vm.Data = this.Data;
            vm.Exp = SelectedCharacter.Expierience;

            SaveFileWindow saveWindow = new SaveFileWindow();
            saveWindow.DataContext = vm;
            saveWindow.ShowDialog();


        }


        public void LoadMethod()
        {

            LoadWindowViewModel vm = new LoadWindowViewModel();

            SaveFileWindow saveWindow = new SaveFileWindow();
            saveWindow.DataContext = vm;
            saveWindow.ShowDialog();
            //Wenn das Lade-Fenster geschlossen wurde, ohne, dass ein Ladevorgang ausgeführt wurde, dann keine neuen Daten laden.
            if (vm.LoadSucessful == true)
            {
                this.Data.Clear();
                this.Data = vm.Data;

                SelectedCharacter.LoadData(Data);
            }


        }

        public void PrintMethod()
        {
            if(PresentationElement.GetType() == typeof(CharacterSheetView))
            {
                ((CharacterSheetView)PresentationElement).Print();
            }
        }

        public bool CanExecute()
        {
            return true; //Hier könnte eine Abfrage, ob das Command ausgeführt werden darf, stehen
        }

        #endregion Commands

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
