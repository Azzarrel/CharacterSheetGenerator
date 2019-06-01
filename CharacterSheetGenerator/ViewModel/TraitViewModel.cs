using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
                TraitModifiers = SelectedTrait.Modifiers;
            }
        }

        public ObservableCollection<TraitModifierModel> TraitModifiers
        {
            get { return Get<ObservableCollection<TraitModifierModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<string> Names
        {
            get { return Get<ObservableCollection<string>>(); }
            set { Set(value); }
        }

        public ObservableCollection<string> Types
        {
            get { return Get<ObservableCollection<string>>(); }
            set { Set(value); }
        }

        public TraitViewModel()
        {
            

        }

        public void TraitGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            e.NewItem = new TraitModel
            {
                Name = "",
                Modifiers = new ObservableCollection<TraitModifierModel>(),
                Description = "",
            };

        }


    }
}
