using CharacterSheetGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.Model 
{
    public class SpellModel : NotifyBase
    {

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Type
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Requirement
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public int Value
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

        public string Damage
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Impulse
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string ArmorPenetration
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string MagicDamage
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string FlavorText
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Range
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Duration
        {
            get { return Get<string>(); }
            set { Set(value); }
        }
    }
}
