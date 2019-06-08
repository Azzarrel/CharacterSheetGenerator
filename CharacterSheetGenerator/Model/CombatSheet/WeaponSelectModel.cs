using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.CombatSheet.Model
{
    public class WeaponSelectModel : TemplateModel
    {
        [ColumnName("Weapon")]
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
                OnPropertyChanged("AttackStandard");
                OnPropertyChanged("AttackBonus");
                OnPropertyChanged("AttackTotal");
                OnPropertyChanged("BlockBase");
                OnPropertyChanged("BlockStandard");
                OnPropertyChanged("BlockBonus");
                OnPropertyChanged("BlockTotal");
                OnPropertyChanged("Weapon");
                OnPropertyChanged("Position");
                OnPropertyChanged("AttributeLink");
                OnPropertyChanged("Stamina");
                OnPropertyChanged("Initiative");
                OnPropertyChanged("Damage");
                OnPropertyChanged("Impulse");
                OnPropertyChanged("ArmorPenetration"); 
                OnPropertyChanged();
            }
        }
        //public ObservableCollection<WeaponModel> Weapons { get; set; }

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

        public double AttackStandard
        {
            get
            {
                return m_Weapon?.AttackStandard ?? 0;
            }
            set
            {
                m_Weapon.AttackStandard = value;
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

        public double BlockStandard
        {
            get
            {
                return m_Weapon?.BlockStandard ?? 0;
            }
            set
            {
                m_Weapon.BlockStandard = value;
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

        [ColumnName("BlockBonus")]
        public double Position
        {
            get { return Get<double>(); }
            set { Set(value); }
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


    }
}
