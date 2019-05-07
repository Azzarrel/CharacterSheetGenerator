using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator 
{
    public class WeaponModel : NotifyBase
    {

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public double AttackBase
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double AttackModifier
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double AttackBonus
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double AttackTotal
        {
            get { return Get<double>(); }
            set
            {
                Set(value);
                AttackBonus = AttackTotal - (AttackBase+AttackModifier);
            }
        }

        public double BlockBase
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double BlockModifier
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double BlockBonus
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double BlockTotal
        {
            get { return Get<double>(); }
            set
            {
                Set(value);
                BlockBonus = BlockTotal - (BlockBase+BlockModifier);
            }
        }

        public string AttributeLink
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public int Position
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

        public int Stamina
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

        public int Initiative
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
    }
}
