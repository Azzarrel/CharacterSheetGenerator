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
    class SaveWindowViewModel : DialogueWindowViewModel
    {

        public double Exp
        {
            get { return Get<double>(); }
            set
            {
                Set(value);
                CanExecute();
            }
        }

        public SaveWindowViewModel()
        {
            SaveName = "";
            CommandName = "Speichern";
            Exp = 0;

            base.Initializce();
        }


        public override void ProcessCommand()
        {
            //Prüft, ob bereits ein Speicherstand mit diesen Namen existiert
            if (Directory.GetDirectories(SaveFolder, SaveName).Count() != 0)
            {
                if (MessageBox.Show("Trotzdem überschreiben", "Der angegebene Name existiert bereits", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return;
                };


            }

            // Wirft einen Fehler, wenn ein Zeichen vorkommen sollte, was in der Ordnerstruktur von Windows Probleme machen sollte.
            if ((SaveName != "") && !SaveName.Contains('/') && !SaveName.Contains('\\') && !SaveName.Contains('^') && !SaveName.Contains('"') && !SaveName.Contains('§') && !SaveName.Contains('$') && !SaveName.Contains('&')
                                 && !SaveName.Contains('<') && !SaveName.Contains('>') && !SaveName.Contains('|') && !SaveName.Contains('.') && !SaveName.Contains(':') && !SaveName.Contains(',') && !SaveName.Contains('#') && !SaveName.Contains('+')
                                 && !SaveName.Contains('~') && !SaveName.Contains('?') && !SaveName.Contains('='))
            {
                //Erstellt einen neuen Ordner
                DirectoryInfo di = Directory.CreateDirectory(SaveFolder + "\\" + SaveName);

                //Fügt die Speicherdaten des Charakters in die Tabelle ein
                Data.Tables["SaveData"].Rows.Add(VERSION, SaveName, (Data.Tables["CharacterInformation"].Select("Name = 'Name'")[0]["Value"].ToString() + " " + Data.Tables["CharacterInformation"].Select("Name = 'Familienname'")[0]["Value"].ToString()), Exp, DateTime.Now); 


                //Speichert die Daten in xml ab.
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

    }
}
