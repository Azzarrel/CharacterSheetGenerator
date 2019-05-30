
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


    class LoadWindowViewModel: NotifyBase
    {
        const string VERSION = "0.2.0.0";

        public DataSet Data { get; set; } = new DataSet();

        public string SaveName
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                CanExecute();
            }
        }

        public string SaveFolder
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                CanExecute();
            }
        }

        public SaveDataModel SelectedItem
        {
            get { return Get<SaveDataModel>(); }
            set
            {
                Set(value);
                SaveName = value.SaveName;
            }
        }

        public bool LoadSucessful
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }

        public string CommandName
        {
            get { return Get<string>(); }
            set { Set(value); }
        }


        public ObservableCollection<SaveDataModel> SaveData
        {
            get { return Get<ObservableCollection<SaveDataModel>>(); }
            set { Set(value); }
        }

        public LoadWindowViewModel()
        {
            LoadSucessful = false;
            CommandName = "Laden";
            DialogCommand = new RelayCommand(LoadMethod, CanExecute);
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

                }
            }

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
            CreateCommands();
        }


        private void CreateCommands()
        {
            DialogCommand = new RelayCommand(LoadMethod, CanExecute);

        }
        public ICommand DialogCommand { get; private set; }

        public void LoadMethod()
        {
            XmlReader xmlData;

            DataSet l_Data = new DataSet();
            string[] files = Directory.GetFiles(SaveFolder + "\\" + SaveName, "*.xml");


            if (files.Count() == 0)
            {

                throw new Exception("Der angegebene Pfad enthält keine Charakterdaten");
            }
            Data = new DataSet();
            foreach (string s in files)
            {
                l_Data = new DataSet();
                xmlData = XmlReader.Create(s, new XmlReaderSettings());
                l_Data.ReadXml(xmlData);
                Data.Merge(l_Data);
            }
            LoadSucessful = true;
        }

        public bool CanExecute()
        {
            return true;

        }

    }
}