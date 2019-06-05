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

        public TraitModel SelectedTrait
        {
            get { return Get<TraitModel>(); }
            set
            {
                Set(value);
                if(SelectedTrait != null)
                TraitModifiers = new ObservableCollection<TraitModifierModel>(Modifiers.Where(m => m.TraitLink == SelectedTrait.Key));
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
            CreateCommands();
        }


        private void CreateCommands()
        {
            InsertTraitsCommand = new RelayCommand(InsertTraitsMethod, CanExecute);
            DeleteTraitsCommand = new RelayCommand(DeleteTraitsMethod, CanExecute);
            InsertModifiersCommand = new RelayCommand(InsertModifiersMethod, CanExecute);
            DeleteModifiersCommand = new RelayCommand(DeleteModifiersMethod, CanExecute);

        }


        public ICommand InsertTraitsCommand { get; private set; }
        public ICommand DeleteTraitsCommand { get; private set; }
        public ICommand InsertModifiersCommand { get; private set; }
        public ICommand DeleteModifiersCommand { get; private set; }

        public void InsertTraitsMethod()
        {
            TraitModel trait = new TraitModel { Key = 5 };
            Traits.Add(trait);
            SelectedTrait = trait;

        }

        public void DeleteTraitsMethod()
        {


        }

        public void InsertModifiersMethod()
        {
            TraitModifierModel mod = new TraitModifierModel { Modifier = BaseModifiers.FirstOrDefault(), TraitLink = SelectedTrait.Key };
            Modifiers.Add(mod);
            TraitModifiers = new ObservableCollection<TraitModifierModel>(Modifiers.Where(m => m.TraitLink == SelectedTrait.Key));

        }

        public void DeleteModifiersMethod()
        {


        }


        public bool CanExecute()
        {
            return true; //Hier könnte eine Abfrage, ob das Command ausgeführt werden darf, stehen
        }




    }
}
