
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace CharacterSheetGenerator.ViewModel
{


    class LoadWindowViewModel: DialogueWindowViewModel
    {


        public bool LoadSucessful
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }


        public LoadWindowViewModel()
        {
            LoadSucessful = false;
            CommandName = "Laden";
            base.Initializce();
        }



        public override void ProcessCommand(Window window)
        {
            //Schließt den Dialog
            window.Close();

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



    }
}