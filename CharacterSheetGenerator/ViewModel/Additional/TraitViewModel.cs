using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CharacterSheetGenerator.ViewModel
{
    class TraitViewModel : NotifyBase
    {


        public ObservableCollection<TraitModel> Traits
        {
            get { return Get<ObservableCollection<TraitModel>>(); }
            set
            {
                Set(value);
                SelectedTrait = Traits.FirstOrDefault();
            }
        }

        public int KeyCounter
        {
            get { return Get<int>(); }
            set
            {
                Set(value);
            }
        }

        public string Category
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
            }
        }

        public bool IsSaved
        {
            get { return Get<bool>(); }
            set
            {
                Set(value);
            }
        }

        public TraitModel SelectedTrait
        {
            get { return Get<TraitModel>(); }
            set
            {
                Set(value);
                if(SelectedTrait != null)
                TraitModifiers = new ObservableCollection<TraitModifierModel>(Modifiers.Where(m => m.TraitLink == SelectedTrait.Key));
                InsertModifiersCommand.UpdateCanExecuteState();
                DeleteTraitsCommand.UpdateCanExecuteState();
            }
        }

        public TraitModifierModel SelectedModifier
        {
            get { return Get<TraitModifierModel>(); }
            set {
                Set(value);
                DeleteModifiersCommand.UpdateCanExecuteState();
            }
        }

        public ObservableCollection<TraitModifierModel> TraitModifiers
        {
            get { return Get<ObservableCollection<TraitModifierModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<TraitModifierModel> Modifiers
        {
            get { return Get<ObservableCollection<TraitModifierModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<ModifierModel> BaseModifiers
        {
            get { return Get<ObservableCollection<ModifierModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<string> Types
        {
            get { return Get<ObservableCollection<string>>(); }
            set { Set(value); }
        }

        public TraitViewModel()
        {
            IsSaved = false;
            CreateCommands();
        }


        private void CreateCommands()
        {
            SaveCommand = new RelayCommand(SaveMethod, CanExecute);
            InsertTraitsCommand = new RelayCommand(InsertTraitsMethod, CanExecute);
            DeleteTraitsCommand = new RelayCommand(DeleteTraitsMethod, () => SelectedTrait != null);
            InsertModifiersCommand = new RelayCommand(InsertModifiersMethod, () => SelectedTrait != null);
            DeleteModifiersCommand = new RelayCommand(DeleteModifiersMethod, () => SelectedModifier != null);

        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand InsertTraitsCommand { get; private set; }
        public RelayCommand DeleteTraitsCommand { get; private set; }
        public RelayCommand InsertModifiersCommand { get; private set; }
        public RelayCommand DeleteModifiersCommand { get; private set; }

        public void InsertTraitsMethod()
        {
            TraitModel trait = new TraitModel { Key = KeyCounter, Name="<Neue Eigenschaft>" };
            Traits.Add(trait);
            SelectedTrait = trait;
            KeyCounter++;

        }

        public void DeleteTraitsMethod()
        {

            Traits.Remove(SelectedTrait);
        }

        public void InsertModifiersMethod()
        {
            TraitModifierModel mod = new TraitModifierModel { Modifier = BaseModifiers.FirstOrDefault(), TraitLink = SelectedTrait.Key };
            Modifiers.Add(mod);
            TraitModifiers = new ObservableCollection<TraitModifierModel>(Modifiers.Where(m => m.TraitLink == SelectedTrait.Key));

        }

        public void DeleteModifiersMethod()
        {
            Modifiers.Remove(SelectedModifier);
            TraitModifiers = new ObservableCollection<TraitModifierModel>(Modifiers.Where(m => m.TraitLink == SelectedTrait.Key));

        }

        public void SaveMethod()
        {
            this.IsSaved = true;
            

        }

        public bool CanExecute()
        {
            return true; //Hier könnte eine Abfrage, ob das Command ausgeführt werden darf, stehen
        }



    }
}
