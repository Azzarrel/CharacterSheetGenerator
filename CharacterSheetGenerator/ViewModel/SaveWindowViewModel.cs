using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace CharacterSheetGenerator.ViewModel
{
    class SaveWindowViewModel : NotifyBase
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

        public double Exp
        {
            get { return Get<double>(); }
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

        public SaveWindowViewModel()
        {
            SaveName = "";
            CommandName = "Speichern";
            CreateCommands();
            Exp = 0;

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
        }
        private void CreateCommands()
        {
            DialogCommand = new RelayCommand(SaveMethod, CanExecute);


        }
        public ICommand DialogCommand { get; private set; }

        public void SaveMethod()
        {
            if (Directory.GetDirectories(SaveFolder, SaveName).Count() != 0)
            {
                if (MessageBox.Show("Trotzdem überschreiben", "Der angegebene Name existiert bereits", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return;
                };


            }

            if ((SaveName != "") && !SaveName.Contains('/') && !SaveName.Contains('\\') && !SaveName.Contains('^') && !SaveName.Contains('"') && !SaveName.Contains('§') && !SaveName.Contains('$') && !SaveName.Contains('&')
                                 && !SaveName.Contains('<') && !SaveName.Contains('>') && !SaveName.Contains('|') && !SaveName.Contains('.') && !SaveName.Contains(':') && !SaveName.Contains(',') && !SaveName.Contains('#') && !SaveName.Contains('+')
                                 && !SaveName.Contains('~') && !SaveName.Contains('?') && !SaveName.Contains('='))
            {
                DirectoryInfo di = Directory.CreateDirectory(SaveFolder + "\\" + SaveName);

                Data.Tables["SaveData"].Rows.Add(VERSION, SaveName, (Data.Tables["CharacterInformation"].Select("Name = 'Name'")[0]["Value"].ToString() + " " + Data.Tables["CharacterInformation"].Select("Name = 'Familienname'")[0]["Value"].ToString()), DateTime.Now, Exp);
                foreach (SaveDataModel s in SaveData.Where(s => s.SaveName == SaveName))
                {
                    SaveData.Remove(s);
                }
                SaveDataModel save = new SaveDataModel
                {
                    Version = VERSION,
                    SaveName = SaveName,
                    CharacterName = (Data.Tables["CharacterInformation"].Select("Name = 'Name'")[0]["Value"].ToString() + " " + Data.Tables["CharacterInformation"].Select("Name = 'Familienname'")[0]["Value"].ToString()),
                    Expieriece = Exp,
                    LastModified = DateTime.Now,


                };
                SaveData.Add(save);




                foreach (DataTable tbl in Data.Tables)
                {
                    XmlTextWriter writer = new XmlTextWriter(SaveFolder + "\\" + SaveName + "/" + tbl.TableName + ".xml", System.Text.Encoding.UTF8);
                    writer.WriteStartDocument(true);
                    writer.Formatting = Formatting.Indented;
                    writer.Indentation = 2;
                    writer.WriteStartElement("Table");
                    foreach (DataRow row in tbl.Rows)
                    {
                        writer.WriteStartElement(tbl.TableName);
                        foreach (DataColumn col in tbl.Columns)
                        {
                            writer.WriteStartElement(col.ColumnName);
                            writer.WriteString(row[col].ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndDocument();
                    writer.Close();
                }


                MessageBox.Show("XML File created ! ");
            }
            else
            {
                MessageBox.Show("Der Speicherpfad enthält ungültige Zeichen");
            }

        }

        public bool CanExecute()
        {
            return true;

        }

    }
}
