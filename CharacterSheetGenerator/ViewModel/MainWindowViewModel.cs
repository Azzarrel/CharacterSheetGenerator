﻿using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using CharacterSheetGenerator.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace CharacterSheetGenerator.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {

        public DataSet Data { get; set; } = new DataSet();

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
            InitializeStartUp();
            //Speicherpfad für die Charakterdaten prüfen und neuen Ordner anlegen, wenn noch keiner existiert
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Saves"))
            {
                DirectoryInfo di = Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Saves");
            }
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
            OpenTraitViewCommand = new RelayCommand<string>(OpenTraitViewMethod);
            SaveCommand = new RelayCommand(SaveMethod, CanExecute);
            LoadCommand = new RelayCommand(LoadMethod, CanExecute);

        }

        public ICommand OpenTraitViewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }




        public void OpenTraitViewMethod(string Category)
        {
            if (SelectedCharacter != null)
            {

                int keycounter = 0;
                TraitViewModel vm = new TraitViewModel();
                vm.Modifiers = new ObservableCollection<TraitModifierModel>(SelectedCharacter.Modifiers);
                vm.BaseModifiers = SelectedCharacter.BaseModifiers;
                vm.Category = Category;
                foreach (TraitCategoryModel category in SelectedCharacter.Traits)
                {
                    keycounter += category.Traits.Count();
                }
                vm.KeyCounter = keycounter;
                vm.Traits = new ObservableCollection<TraitModel>(SelectedCharacter.Traits.Where(c => c.Name == Category).SelectMany(x => x.Traits));


                TraitWindow traitview = new TraitWindow();
                traitview.DataContext = vm;
                traitview.ShowDialog();
                if (vm.IsSaved)
                {
                    TraitCategoryModel catgory = SelectedCharacter.Traits.Where(c => c.Name == vm.Category).FirstOrDefault();
                    //Die alten Traits einfach mit den neuen Überschreiben
                    catgory.Traits = vm.Traits;
                    catgory.TraitTexts = "";
                    foreach (TraitModel trait in vm.Traits)
                    {
                        catgory.TraitTexts += " " + trait.Name + ",";
                    }
                    SelectedCharacter.Modifiers = vm.Modifiers;
                    SelectedCharacter.CalculateModifiers();
                }
            }

        }

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

        public bool CanExecute()
        {
            return true; //Hier könnte eine Abfrage, ob das Command ausgeführt werden darf, stehen
        }

        #endregion Commands
    }
}
