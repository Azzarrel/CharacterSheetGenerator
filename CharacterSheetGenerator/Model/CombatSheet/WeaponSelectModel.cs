using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator 
{
    public class WeaponSelectModel : INotifyPropertyChanged
    {
        private WeaponModel m_Weapon;
        public WeaponModel Weapon
        {
            get
            {
                return m_Weapon;
            }
            set
            {
                m_Weapon = value;
                OnPropertyChanged("AttackBase");
                OnPropertyChanged("AttackBonus");
                OnPropertyChanged("AttackTotal");
                OnPropertyChanged("BlockBase");
                OnPropertyChanged("BlockBonus");
                OnPropertyChanged("BlockTotal");
                OnPropertyChanged("Weapon");
                OnPropertyChanged("Position");
                OnPropertyChanged("Stamina");
                OnPropertyChanged("Initiative");
                OnPropertyChanged("Damage");
                OnPropertyChanged("Impulse");
                OnPropertyChanged("ArmorPenetration");
                OnPropertyChanged();
            }
        }
        //public List<WeaponModel> Weapons { get; set; }

        public String Name
        {
            get
            {
                return m_Weapon?.Name;
            }
        }

        public double AttackBase
        {
            get
            {
                return m_Weapon?.AttackBase ?? 0;
            }
            set
            {
                m_Weapon.AttackBase = value;
            }
        }

        public double AttackBonus
        {
            get
            {
                return m_Weapon?.AttackBonus ?? 0;
            }
            set
            {
                m_Weapon.AttackBonus = value;
            }
        }

        public double AttackTotal
        {
            get
            {
                return m_Weapon?.AttackTotal ?? 0;
            }
            set
            {
                m_Weapon.AttackTotal = value;
            }
        }

        public double BlockBase
        {
            get
            {
                return m_Weapon?.BlockBase ?? 0;
            }
            set
            {
                m_Weapon.BlockBase = value;
            }
        }
        public double BlockBonus
        {
            get
            {
                return m_Weapon?.BlockBonus ?? 0;
            }
            set
            {
                m_Weapon.BlockBonus = value;
            }
        }
        public double BlockTotal
        {
            get
            {
                return m_Weapon?.BlockTotal ?? 0;
            }
            set
            {
                m_Weapon.BlockTotal = value;
            }
        }

        public string AttributeLink
        {
            get
            {
                return m_Weapon?.AttributeLink ?? "";
            }
            set
            {
                m_Weapon.AttributeLink = value;
            }
        }

        public int Position
        {
            get
            {
                return m_Weapon?.Position ?? 0;
            }
            set
            {
                m_Weapon.Position = value;
            }
        }

        public int Stamina
        {
            get
            {
                return m_Weapon?.Stamina ?? 0;
            }
            set
            {
                m_Weapon.Stamina = value;
            }
        }

        public int Initiative
        {
            get
            {
                return m_Weapon?.Initiative ?? 0;
            }
            set
            {
                m_Weapon.Initiative = value;
            }
        }

        public string Damage
        {
            get
            {
                return m_Weapon?.Damage ?? "";
            }
            set
            {
                m_Weapon.Damage = value;
            }
        }


        public string Impulse
        {
            get
            {
                return m_Weapon?.Impulse ?? "";
            }
            set
            {
                m_Weapon.Impulse = value;
            }
        }

        public string ArmorPenetration
        {
            get
            {
                return m_Weapon?.ArmorPenetration ?? "";
            }
            set
            {
                m_Weapon.ArmorPenetration = value;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
