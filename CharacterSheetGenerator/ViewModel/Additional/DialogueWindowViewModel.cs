using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Xml;

namespace CharacterSheetGenerator.ViewModel
{
    class DialogueWindowViewModel : NotifyBase
    {
       
        protected const string VERSION = "0.9.0.0";


        public DataSet Data { get; set; } = new DataSet();


        /// <summary>
        /// Anzeigename auf dem Button
        /// </summary>
        public string CommandName
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        /// <summary>
        /// Name des Ordners, in dem die Spielerdaten abgelegt werden
        /// </summary>
        public string SaveName
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        /// <summary>
        /// Ordner, in dem sich alle Saves befinden
        /// </summary>
        public string SaveFolder
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        /// <summary>
        /// Container mit Anzeigedaten für alle Speicherstände
        /// </summary>
        public ObservableCollection<SaveDataModel> SaveData
        {
            get { return Get<ObservableCollection<SaveDataModel>>(); }
            set { Set(value); }
        }

        /// <summary>
        /// Ausgewähltes Item, in der Listbox
        /// </summary>
        public SaveDataModel SelectedItem
        {
            get { return Get<SaveDataModel>(); }
            set
            {
                Set(value);
                SaveName = value.SaveName;
            }
        }

        public virtual void Initializce()
        {
            XmlReader xmlData;
            DataSet l_Data = new DataSet();
            SaveData = new ObservableCollection<SaveDataModel>();
            SaveFolder = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Saves";
            string[] dirs = Directory.GetDirectories(SaveFolder);
            foreach (string dir in dirs)
            {
                if (Directory.GetFiles(dir, "SaveData.xml").Count() >= 0)
                {
                    xmlData = XmlReader.Create(dir + "\\SaveData.xml", new XmlReaderSettings());
                    l_Data.ReadXml(xmlData);
                    xmlData.Close();

                }
            }


            if (l_Data.Tables["SaveData"] != null)
            {
                foreach (DataRow row in l_Data.Tables["SaveData"].Rows)
                {
                    SaveDataModel save = new SaveDataModel
                    {
                        Version = row["Version"].ToString(),
                        SaveName = row["SaveName"].ToString(),
                        CharacterName = row["CharacterName"].ToString(),
                        Expieriece = double.Parse(row["Exp"].ToString()),
                        LastModified = DateTime.Parse(row["LastModified"].ToString()),

                    };
                    SaveData.Add(save);
                }
            }
                      
            CreateCommands();
        }

        public void CreateCommands()
        {
            DialogCommand = new RelayCommand(ProcessCommand, CanExecute);

        }
        public ICommand DialogCommand { get; private set; }

        public virtual void ProcessCommand()
        {

        }

        public virtual bool CanExecute()
        {
            return true;

        }
    }
}