using CharacterSheetGenerator.Control;
using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using CharacterSheetGenerator.Model.CombatSheet;
using CharacterSheetGenerator.View;
using CharacterSheetGenerator.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;


namespace CharacterSheetGenerator
{
    class CharacterSheetViewModel : ViewModelBase
    {

        #region Properties

        public DataSet Data { get; set; } = new DataSet();

        public ObservableCollection<ModifierModel> BaseModifiers
        {
            get { return Get<ObservableCollection<ModifierModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<ControlModel> Pages
        {
            get { return Get<ObservableCollection<ControlModel>>(); }
            set { Set(value); }
        }

        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }


        #endregion Properties

        public CharacterSheetViewModel()
        {
            Pages = new ObservableCollection<ControlModel>();
            Pages.Add(new ControlModel
            {
                Control = new MainSheet { DataContext = this }
            });

            Pages.Add(new ControlModel
            {
                Control = new SkillSheet { DataContext = this }
            });

            Pages.Add(new ControlModel
            {
                Control = new CombatSheet { DataContext = this }
            });

            Pages.Add(new ControlModel
            {
                Control = new SpellSheet { DataContext = this }
            });

            Pages.Add(new ControlModel
            {
                Control = new InventorySheet { DataContext = this }
            });

            CreateCommands();

            
        }

        #region Framework


        /// <summary>
        /// /Alle Tabellen aus dem DateSet in Listen laden
        /// </summary>
        public void LoadData(DataSet ds)
        {
            Data = ds.Copy();

            LoadAttributes();
            LoadSpecialAttributes();
            LoadStatusValues();
            LoadSkills();
            LoadLanguages();
            LoadCharacterInformation();
            LoadTraits();
            LoadWeapons();
            LoadSelectedWeapons();
            LoadMeleeWeapons();
            LoadRangedWeapons();
            LoadArmor();
            LoadOffHands();
            LoadSpells();
            LoadRituals();
            LoadInventory();
            LoadMoney();


            GenerateBaseModifiers();
            LoadModifiers();


            //Zusammengesetzte Werte direkt neu berechnen.           
            CalculateModifiers();
            CalculateStatusValuesAll();
            CalculateSkillsAll();
            CalculateWeaponAll();
            Inventory_PropertyChanged(null, null);
            CalculateExpirienceAll();

        }

        /// <summary>
        /// Listen wieder in ein DataSet zurückwandeln fürs Speichern
        /// </summary>
        /// <param name="tblAttributeLink"></param>
        public DataSet SaveData(DataTable tblAttributeLink)
        {
            Data.Clear();
            SaveAttributes();
            SaveSpecialAttributes();
            SaveCharacterInformation();
            SaveStatusValues(tblAttributeLink);
            SaveSkills();
            SaveLanguages();
            SaveWeapons();
            SaveArmor();
            SaveOffHand();
            SaveTraits();
            SaveSpells();
            SaveRituals();
            SaveInventory();
            SaveMoney();
            SaveModifier();
            return Data;
        }

        public void GenerateBaseModifiers()
        {
            BaseModifiers = new ObservableCollection<ModifierModel>();
            foreach(AttributeModel atr in Attributes)
            {
                ModifierModel m = new ModifierModel {   NameLink = atr.Name,  };
                m.Types.Add("Standard");
                BaseModifiers.Add(m);
            }
            foreach(SkillModel skill in SkillsLeft)
            {
                ModifierModel m = new ModifierModel { NameLink = skill.Name, };
                m.Types.Add("Standard");
                if (skill.Name == null || skill.Name == "")
                    continue;

                BaseModifiers.Add(m);
            }
            foreach (SkillModel skill in SkillsRight)
            {
                ModifierModel m = new ModifierModel { NameLink = skill.Name, };
                m.Types.Add("Standard");
                if (skill.Name == null || skill.Name == "")
                    continue;

                BaseModifiers.Add(m);
            }
            foreach(StatusValueModel stv in StatusValues)
            {
                ModifierModel m = new ModifierModel { NameLink = stv.Name, };
                m.Types.Add("Standard");
                BaseModifiers.Add(m);
            }
            foreach(WeaponModel weapon in Weapons)
            {
                ModifierModel m = new ModifierModel { NameLink = weapon.Name, };
                m.Types.Add("Angriff");
                m.Types.Add("Parieren");
                BaseModifiers.Add(m);
            }
        }

        #endregion Framework

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Commands

        //Komplette Dummy-Implementierung der Commands, sodass man es einfach erweitern kann
        private void CreateCommands()
        {
            OpenTraitViewCommand = new RelayCommand<string>(OpenTraitViewMethod);

        }

        public ICommand OpenTraitViewCommand { get; private set; }

        //Das ganze Zeug mit den Traits ist noch unnötig wie shit, sollte man mal vereinfachen.
        public void OpenTraitViewMethod(string Category)
        {

            int keycounter = 0;
            TraitViewModel vm = new TraitViewModel();
            vm.Modifiers = new ObservableCollection<TraitModifierModel>(Modifiers);
            vm.BaseModifiers = BaseModifiers;
            vm.Category = Category;
            //ToDo: Vereinfachen/Zusammenfassen
            foreach (TraitCategoryModel category in Traits)
            {
                if (category.Traits.Count == 0)
                    keycounter = 0;
                else
                    keycounter += category.Traits.Max(x => x.Key) + 1;
            }
            foreach (TraitCategoryModel category in CombatTraits)
            {
                if (category.Traits.Count == 0)
                    keycounter = 0;
                else
                    keycounter += category.Traits.Max(x => x.Key) + 1;
            }
            foreach (TraitCategoryModel category in SpellTraits)
            {
                if (category.Traits.Count == 0)
                    keycounter = 0;
                else
                    keycounter += category.Traits.Max(x => x.Key) + 1;
            }
            vm.KeyCounter = keycounter;
            if (Traits.Any(t => t.Name == Category))
            {
                vm.Traits = new ObservableCollection<TraitModel>(Traits.Where(c => c.Name == Category).SelectMany(x => x.Traits));
            }
            else if (CombatTraits.Any(t => t.Name == Category))
            {
                vm.Traits = new ObservableCollection<TraitModel>(CombatTraits.Where(c => c.Name == Category).SelectMany(x => x.Traits));
            }
            else if (SpellTraits.Any(t => t.Name == Category))
            {
                vm.Traits = new ObservableCollection<TraitModel>(SpellTraits.Where(c => c.Name == Category).SelectMany(x => x.Traits));
            }


            TraitWindow traitview = new TraitWindow();
            traitview.DataContext = vm;
            traitview.ShowDialog();
            if (vm.IsSaved)
            {
                TraitCategoryModel catgory = new TraitCategoryModel();
                if (Traits.Any(t => t.Name == vm.Category))
                {
                     catgory = Traits.FirstOrDefault(c => c.Name == vm.Category);
                }
                else if (CombatTraits.Any(t => t.Name == vm.Category))
                {
                    catgory = CombatTraits.FirstOrDefault(c => c.Name == vm.Category);
                }
                else if (SpellTraits.Any(t => t.Name == vm.Category))
                {
                    catgory = SpellTraits.FirstOrDefault(c => c.Name == vm.Category);
                }
                //Die alten Traits einfach mit den neuen Überschreiben
                catgory.Traits = vm.Traits;
                catgory.TraitTexts = "";
                foreach (TraitModel trait in vm.Traits)
                {
                    catgory.TraitTexts += " " + trait.Name + ",";
                }
                Modifiers = vm.Modifiers;
                CalculateModifiers();
            }


        }



        public bool CanExecute()
        {
            return true; //Hier könnte eine Abfrage, ob das Command ausgeführt werden darf, stehen
        }

        #endregion Commands

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Expirience

        #region Properties

        public double Expierience
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double Expttributes
        {
            get { return Get<double>(); }
            set { Set(value); CalculateExpirience(); }
        }

        public double ExpStatusValues
        {
            get { return Get<double>(); }
            set { Set(value); CalculateExpirience(); }
        }

        public double ExpSkills
        {
            get { return Get<double>(); }
            set { Set(value); CalculateExpirience(); }
        }

        public double ExpWeapons
        {
            get { return Get<double>(); }
            set { Set(value); CalculateExpirience(); }
        }

        public double ExpSpells
        {
            get { return Get<double>(); }
            set { Set(value); CalculateExpirience(); }
        }
        #endregion Properties

        #region Calculation

        public void CalculateExpirienceAll()
        {
            CalcExpAttributes();
            CalcSExpStatusValues();
            CalcExpSkills();
            CalcExpWeapon();
            CalcExpSpells();
        }

        public void CalculateExpirience()
        {
            Expierience = Expttributes + ExpStatusValues + ExpSkills + ExpWeapons + ExpSpells;
        }

        public void CalcExpAttributes()
        {
            double xp = 0;
            foreach(AttributeModel attr in Attributes)
            {
                for (int i = 3; i < attr.Value; i++)
                {
                    xp = (xp + (i - 2) * 2);
                }
            }
            xp = xp - 15 * 18;
            Expttributes = xp;
        }

        public void CalcSExpStatusValues()
        {
            double xp = 0;
            foreach (StatusValueModel stv in StatusValues)
            {
                xp = xp + stv.Bonus * 5;
            }
            ExpStatusValues = xp;
        }

        public void CalcExpSkills()
        {
            double xp = 0;
            foreach(SkillModel skill in SkillsLeft)
            {
                xp = xp + skill.Bonus.GetValueOrDefault() * GetExpSkillModifier(skill.Deployability);
            }
            foreach (SkillModel skill in SkillsRight)
            {
                xp = xp + skill.Bonus.GetValueOrDefault() * GetExpSkillModifier(skill.Deployability);
            }
            ExpSkills = xp;
        }

        public double GetExpSkillModifier(string s)
        {
            double mod = 0;
            switch(s)
            {
                case "D":
                    mod = 2;
                    break;
                case "C":
                    mod = 2.5;
                    break;
                case "B":
                    mod = 3;
                    break;
                case "A":
                    mod = 3.5;
                    break;
                default:
                    break;
            }
            return mod;
        }

        public void CalcExpWeapon()
        {
            double xp = 0;
            foreach(WeaponModel weapon in Weapons)
            {
                for (int i = 0; i < weapon.AttackBonus; i++)
                {
                    xp = xp + i;
                }
                xp = xp + weapon.AttackBonus;
                for (int i = 0; i < weapon.BlockBonus; i++)
                {
                    xp = xp + i;
                }
                xp = xp + weapon.BlockBonus;
            }
            ExpWeapons = xp;
        }

        public void CalcExpSpells()
        {
            double xp = 0;
            foreach(SpellModel spell in Spells)
            {
                xp = xp + spell.Value == null ? 0 : (double)spell.Value * 5;
            }
            ExpSpells = xp;
        }

        #endregion Calculation


        #endregion Expierience

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Attributes

        #region Properties

        public ObservableCollection<AttributeModel> Attributes
        {
            get { return Get<ObservableCollection<AttributeModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<AttributeModel> SpecialAttributes
        {
            get { return Get<ObservableCollection<AttributeModel>>(); }
            set { Set(value); }
        }

        #endregion Properties

        #region Loading

        //ToDo: Direkt nach dem Laden die Value berechnen? bzw umbauen, dass Skills ausgewertetet werden, wenn sich bei den attributes die value ändert
        private void LoadAttributes()
        {
            //Attributes = new ObservableCollection<AttributeModel>();
            //foreach (DataRow row in Data.Tables["Attributes"].Select("Type = 'Base'"))
            //{
            //    AttributeModel attribute = new AttributeModel
            //    {
            //        Name = row["Name"].ToString(),
            //        Tag = row["Tag"].ToString(),
            //        Base = double.Parse(row["Value"].ToString()),
            //        Color = new SolidColorBrush(ColorHandler.IntToColor(int.Parse(row["Color"].ToString()))),
            //        Special = false,

            //    };
            //    Attributes.Add(attribute);
            //    attribute.PropertyChanged += Attribute_PropertyChanged;
                Attributes = new ObservableCollection<AttributeModel>(DataTableListConverter.ConvertToObservableCollection<AttributeModel>(Data.Tables["Attributes"], Attribute_PropertyChanged).Where(a => a.Special == false));
                SpecialAttributes = new ObservableCollection<AttributeModel>(DataTableListConverter.ConvertToObservableCollection<AttributeModel>(Data.Tables["Attributes"], Attribute_PropertyChanged).Where(a => a.Special == true));
            //}

        }

        /// <summary>
        /// Spezielle Attribute sind eigentlich StatusValues, die auf einigen Seiten bei den Attributen oben stehen.
        /// </summary>
        private void LoadSpecialAttributes()
        {
            //Aktuell immer schwarz, sonst Kirsten Ritzmann!
            //SpecialAttributes = new ObservableCollection<AttributeModel>();
            //foreach (DataRow row in Data.Tables["Attributes"].Select("Type = 'Special'"))
            //{
            //    AttributeModel attribute = new AttributeModel
            //    {
            //        Name = row["Name"].ToString(),
            //        Tag = row["Tag"].ToString(),
            //        Base = double.Parse(row["Value"].ToString()),
            //        Color = new SolidColorBrush(ColorHandler.IntToColor(int.Parse(row["Color"].ToString()))),
            //        Special = true,

            //    };
            //    SpecialAttributes.Add(attribute);
            //    attribute.PropertyChanged += Attribute_PropertyChanged;
            //}
        }

        #endregion Loading

        #region Saving

        private void SaveAttributes()
        {
            foreach (AttributeModel atr in Attributes)
            {
                Data.Tables["Attributes"].Rows.Add(atr.Name, "Base", atr.Tag, atr.Base, ColorHandler.ColorToInt(atr.Color.Color));
            }
          
        }
        private void SaveSpecialAttributes()
        {
            foreach (AttributeModel atr in SpecialAttributes)
            {
                Data.Tables["Attributes"].Rows.Add(atr.Name, "Special", atr.Tag, atr.Base, ColorHandler.ColorToInt(atr.Color.Color));
            }
        }

        #endregion Saving

        #region Events

        public void Attribute_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            AttributeModel attr = sender as AttributeModel;

            //StatusWerte, die auf diesem Attribut basieren updaten
            foreach (StatusValueModel stv in StatusValues.Where(x => x.AttributeLinks.Any(s => s == attr.Name)))
            {
                ObservableCollection<AttributeModel> attributeList = new ObservableCollection<AttributeModel>();
                foreach (string s in stv.AttributeLinks.ToList())
                {
                    attributeList.Add(Attributes.Where(x => x.Name == s).FirstOrDefault());
                }
                AttributeModel[] attributes = attributeList.ToArray();
                CalculateStatusValues(attributes, stv);
            }

            CalculateCarryWeight();

            //Alle Waffen die auf dieses Attribut basieren updaten
            foreach (WeaponModel weapon in Weapons.Where(x => x.AttributeLink.Contains(attr.Tag)))
            {
                CalculateWeapon(weapon); 
            }
            switch (e.PropertyName)
            {
                case "Value":
                    attr.Base = attr.Value - attr.Modifiers;
                    foreach(SkillModel skill in SkillsLeft.Cast<SkillModel>().Where(s => s.Requirement.ToUpper().Contains(attr.Tag.ToUpper())))
                    {
                        CalculateSkillBase(skill);
                    }
                    foreach (SkillModel skill in SkillsRight.Cast<SkillModel>().Where(s => s.Requirement.ToUpper().Contains(attr.Tag.ToUpper())))
                    {
                        CalculateSkillBase(skill);
                    }
                    break;
                case "Modifiers":
                    attr.Value = attr.Base + attr.Modifiers;
                    break;
                default:
                    break;
            }
            //Exp neu Berechnen
            CalcExpAttributes();

            
        }

        #endregion Events

        #endregion Attributes

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Character Information

        #region Properties

        public ObservableCollection<CharacterInformationModel> CharacterInformation
        {
            get { return Get<ObservableCollection<CharacterInformationModel>>(); }
            set { Set(value); }
        }

        #endregion Properties

        #region Loading

        //Wird schwer in den DataTableListConverter zu packen
        public void LoadCharacterInformation()
        {

            ObservableCollection<CharacterInformationModel> l_CharInfo = new ObservableCollection<CharacterInformationModel>();
            int maxvalue = int.Parse(Data.Tables["CharacterInformation"].Compute("Max(RowPosition)", string.Empty).ToString());

            for (int i = 0; i <= maxvalue; i++)
            {
                string[] names = new string[3];
                string[] values = new string[3];
                int c = 0;
                DataRow[] rows = Data.Tables["CharacterInformation"].Select("RowPosition =" + i);
                foreach (DataRow row in rows)
                {
                    names[c] = row["Name"].ToString();
                    values[c] = row["Value"].ToString();
                    c++;

                }
                CharacterInformationModel charinfo = new CharacterInformationModel
                {
                    FirstElement = names[0],
                    FirstValue = values[0],
                    SecondElement = names[1],
                    SecondValue = values[1],
                    ThirdElement = names[2],
                    ThirdValue = values[2],

                };
                l_CharInfo.Add(charinfo);


            }
            CharacterInformation = l_CharInfo;
        }

        #endregion Loading

        #region Saving

        private void SaveCharacterInformation()
        {
            int i = 0;
            foreach (CharacterInformationModel info in CharacterInformation)
            {
                Data.Tables["CharacterInformation"].Rows.Add(info.FirstElement, info.FirstValue, i);
                if (info.SecondElement != null && info.SecondElement != "")
                {
                    Data.Tables["CharacterInformation"].Rows.Add(info.SecondElement, info.SecondValue, i);
                }
                if (info.SecondElement != null && info.SecondElement != "")
                {
                    Data.Tables["CharacterInformation"].Rows.Add(info.ThirdElement, info.ThirdValue, i);
                }
                i++;
            }
        }

        #endregion Saving

        #region Events

        public void CahracterInformation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        #endregion Events

        #endregion Character Information

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\
        
        #region Status Values

        #region Properties

        public ObservableCollection<StatusValueModel> StatusValues
        {
            get { return Get<ObservableCollection<StatusValueModel>>(); }
            set { Set(value); }
        }

        #endregion Properties

        #region Loading

        public void LoadStatusValues()
        {
            //StatusValues = new ObservableCollection<StatusValueModel>();
            //foreach (DataRow row in Data.Tables["StatusValues"].Rows)
            //{
            //    ObservableCollection<string> attributelinks = new ObservableCollection<string>();
            //    foreach (DataRow rowAttributeLink in Data.Tables["SVAttributeLink"].Select("StatusValues_Id = " + row["StatusValues_Id"]))
            //    {
            //        attributelinks.Add(rowAttributeLink["SVAttributeLink_Text"].ToString());
            //    }

            //    StatusValueModel statusvalue = new StatusValueModel
            //    {
            //        Key = int.Parse(row["StatusValues_Id"].ToString()),
            //        Name = row["Name"].ToString(),
            //        Base = double.Parse(row["Base"].ToString()),
            //        Bonus = double.Parse(row["Bonus"].ToString()),
            //        AttributeLinks = attributelinks,

            //    };

            //    StatusValues.Add(statusvalue);
            //    statusvalue.PropertyChanged += StatusValue_PropertyChanged;
                StatusValues = DataTableListConverter.ConvertToObservableCollection<StatusValueModel>(Data.Tables["StatusValues"], StatusValue_PropertyChanged, Data.Tables["SVAttributeLink"]);
            //}
        }

        #endregion Loading

        #region Saving

        private void SaveStatusValues(DataTable tblAttributeLink)
        {
            foreach (StatusValueModel stv in StatusValues)
            {
                Data.Tables["StatusValues"].Rows.Add(stv.Name, stv.Base, stv.Bonus, stv.Key);
            }

            foreach (DataRow row in tblAttributeLink.Rows)
            {
                Data.Tables["SVAttributeLink"].Rows.Add(row["SVAttributeLink_Text"], row["StatusValues_Id"]);
            }
        }

        #endregion Saving

        #region Events

        public void StatusValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            StatusValueModel stv = sender as StatusValueModel;

            switch (e.PropertyName)
            {
                case "Base":
                    //Wenn sich der Grundwert ändert, ändert sich der Standard und für dessen Neuberechnung sind die Attribute nötig, aus denen er sich errechnet
                    ObservableCollection<AttributeModel> attributeList = new ObservableCollection<AttributeModel>();
                    foreach (string s in stv.AttributeLinks.ToList())
                    {
                        attributeList.Add(Attributes.Where(x => x.Name == s).FirstOrDefault());
                    }
                    AttributeModel[] attributes = attributeList.ToArray();
                    CalculateStatusValues(attributes, stv);
                    break;
                case "Standard":
                    stv.Value = Math.Round(stv.Standard + stv.Modifiers + stv.Bonus, 0);
                    break;
                case "Modifiers":
                    stv.Value = Math.Round(stv.Standard + stv.Modifiers + stv.Bonus, 0);
                    break;
                case "Bonus":
                    stv.Value = Math.Round(stv.Standard + stv.Modifiers + stv.Bonus, 0);
                    break;
                case "Value":
                    //Aus Platzgründen werden alle Statuswerte auf einigen Seiten wie Attribute dargestellt. Dafür gibt es 'spezielle' Attribute, 
                    //die ebenfalls geupdatet werden müssen, wenn sich am StatusValue was ändert.
                    SpecialAttributes.Where(x => x.Name == stv.Name).FirstOrDefault().Value = stv.Value;
                    break;
                default:
                    break;
            }


            //Exp neu Berechnen
            CalcSExpStatusValues();
        }

        #endregion Events

        #region Calculation

        public void CalculateStatusValuesAll()
        {
            foreach (StatusValueModel stv in StatusValues)
            {
                ObservableCollection<AttributeModel> attributeList = new ObservableCollection<AttributeModel>();
                foreach (string s in stv.AttributeLinks.ToList())
                {
                    attributeList.Add(Attributes.Where(x => x.Name == s).FirstOrDefault());
                }
                AttributeModel[] attributes = attributeList.ToArray();
                CalculateStatusValues(attributes, stv);

            }
        }

        public void CalculateStatusValues(AttributeModel[] attributes, StatusValueModel stv)
        {

            switch (stv.Name)
            {
                case "Lebenspunkte":
                    stv.Standard = Math.Round(stv.Base + attributes[0].Value + attributes[1].Value, 0);
                    break;
                case "Ausdauer":
                    stv.Standard = Math.Round(stv.Base + attributes[0].Value * 2, 0);
                    break;
                case "Mana":
                    stv.Standard = Math.Round(stv.Base + attributes[0].Value + attributes[1].Value / 2, 0);
                    break;
                case "Zähigkeit":
                    stv.Standard = Math.Round(stv.Base + (attributes[0].Value * 2 + attributes[1].Value) / 8, 0);
                    break;
                case "Magieresistenz":
                    stv.Standard = Math.Round(stv.Base + (attributes[0].Value + attributes[1].Value) / 3, 0);
                    break;
                case "Agilität":
                    stv.Standard = Math.Round(stv.Base + (attributes[0].Value + attributes[1].Value) / 4, 0);
                    break;
                case "Initiative":
                    stv.Standard = Math.Round(stv.Base + (attributes[0].Value + attributes[1].Value) / 2, 0);
                    break;
                case "Geschwindigkeit":
                    stv.Standard = Math.Round(stv.Base, 0);
                    break;
                case "Wundschwelle":
                    stv.Standard = Math.Round(stv.Base + (attributes[0].Value + attributes[1].Value)/5, 0);
                    break;
                default:
                    break;
            }

        }

        #endregion Calculation

        #endregion Status Values

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Skills 

        #region Properties

        public ListCollectionView SkillsLeft
        {
            get { return Get<ListCollectionView>(); }
            set { Set(value); }
        }

        public ListCollectionView SkillsRight
        {
            get { return Get<ListCollectionView>(); }
            set { Set(value); }
        }


        #endregion Properties

        #region Loading

        private void LoadSkills()
        {
            List<SkillModel> l_skills = DataTableListConverter.ConvertToList<SkillModel>(Data.Tables["Skills"], Skill_PropertyChanged);
            SkillsLeft = new ListCollectionView(l_skills.Where(s => s.Grouping == "Left").ToList());
            SkillsLeft.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            SkillsRight = new ListCollectionView(l_skills.Where(s => s.Grouping == "Right").ToList());
            SkillsRight.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

        }

        #endregion Loading

        #region Saving

        public void SaveSkills()
        {

            foreach (SkillModel skill in SkillsLeft)
            {
                Data.Tables["Skills"].Rows.Add(skill.Name, skill.Requirement, skill.Bonus, skill.Difficulty, skill.Deployability, skill.Comment, skill.Category, skill.Grouping);
            }
            foreach (SkillModel skill in SkillsRight)
            {
                Data.Tables["Skills"].Rows.Add(skill.Name, skill.Requirement, skill.Bonus, skill.Difficulty, skill.Deployability, skill.Comment, skill.Category, skill.Grouping);
            }
        }

        #endregion Saving

        #region Calculation

        public void CalculateSkillsAll()
        {
            foreach(SkillModel skill in SkillsLeft)
            {
                skill.Value = skill.Base + (skill.Bonus ?? 0) + skill.Modifiers;
            }
            foreach (SkillModel skill in SkillsRight)
            {
                skill.Value = skill.Base + (skill.Bonus ?? 0) + skill.Modifiers;
            }
        }

        public void CalculateSkillBase(SkillModel skill)
        {
            double v = 0;
            if (skill.Name != "")
            {
                foreach (string s in skill.Requirement.Split('/'))
                {
                    v += Attributes.Where(a => a.Tag == s.ToUpper()).FirstOrDefault().Value;
                }
                double difficulty = 0;
                switch(skill.Difficulty)
                {
                    case "A":
                        difficulty = 0;
                        break;
                    case "B":
                        difficulty = 1;
                        break;
                    case "C":
                        difficulty = 2;
                        break;
                    case "D":
                        difficulty = 4;
                        break;
                    default:
                        break;
                }
                double? l_base = Math.Round(v / 6, 0) > 0 ? Math.Round(v / 6, 0) : skill.Base;
                skill.Base = l_base - difficulty;
                skill.Mean = l_base != null ? "(" + l_base * 2 + ")" : "";
            }
        }


        #endregion Calculation

        #region Events

        public void Skill_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SkillModel skl = sender as SkillModel;
            switch (e.PropertyName)
            {
                case "Value":
                    if(skl.Value != null)
                    skl.Bonus = skl.Value - skl.Modifiers - skl.Base;
                    break;
                case "Modifiers":
                    skl.Value = skl.Base + skl.Modifiers + skl.Bonus;
                    break;
                case "Base":
                    skl.Value = skl.Base + skl.Modifiers + skl.Bonus;
                    break;
                default:
                    break;
            }

            //Exp neu berechnen
            CalcExpSkills();
        }

        #endregion Events

        #endregion Skills    

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Languages and Writing

        #region Properties

        public ObservableCollection<LanguageModel> Languages
        {
            get { return Get<ObservableCollection<LanguageModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<LanguageModel> Writings
        {
            get { return Get<ObservableCollection<LanguageModel>>(); }
            set { Set(value); }
        }

        #endregion Properties

        #region Loading

        public void LoadLanguages()
        {

            Languages = DataTableListConverter.ConvertToObservableCollection<LanguageModel>(Data.Tables["Languages"]);            
            Writings = DataTableListConverter.ConvertToObservableCollection<LanguageModel>(Data.Tables["Languages"]);
        }

        #endregion Loading

        #region Saving

        public void SaveLanguages()
        {
            foreach(LanguageModel language in Languages)
            {
                Data.Tables["Languages"].Rows.Add(language.Name);
            }
            foreach (LanguageModel writing in Writings)
            {
                Data.Tables["Writings"].Rows.Add(writing.Name);
            }
        }

        #endregion Saving

        #endregion Languages and Writing

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Weapons

        #region Properties

        public ObservableCollection<WeaponModel> Weapons
        {
            get { return Get<ObservableCollection<WeaponModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<WeaponSelectModel> SelectedWeapons
        {
            get { return Get<ObservableCollection<WeaponSelectModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<MeleeWeaponModel> MeleeWeapons
        {
            get { return Get<ObservableCollection<MeleeWeaponModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<RangedWeaponModel> RangedWeapons
        {
            get { return Get<ObservableCollection<RangedWeaponModel>>(); }
            set { Set(value); }
        }

        #endregion Properties

        #region Loading

        public void LoadWeapons()
        {
            Weapons = DataTableListConverter.ConvertToObservableCollection<WeaponModel>(Data.Tables["Weapons"]);
        }

        public void LoadSelectedWeapons()
        {
            SelectedWeapons = DataTableListConverter.ConvertToObservableCollection<WeaponSelectModel, WeaponModel>(Data.Tables["SelectedWeapons"], SelectedWeapon_PropertyChanged, Weapons);
            SelectedWeapons = new ObservableCollection<WeaponSelectModel>(SelectedWeapons.OrderBy(ws => ws.Position));


        }

        public void LoadMeleeWeapons()
        {
            MeleeWeapons = DataTableListConverter.ConvertToObservableCollection<MeleeWeaponModel, WeaponModel > (Data.Tables["MeleeWeapons"], SelectedWeapon_PropertyChanged, Weapons);
        }

        public void LoadRangedWeapons()
        {
            RangedWeapons = DataTableListConverter.ConvertToObservableCollection< RangedWeaponModel, WeaponModel > (Data.Tables["RangedWeapons"], SelectedWeapon_PropertyChanged, Weapons);
        }

        #endregion Loading

        #region Saving

        public void SaveWeapons()
        {
            foreach (WeaponModel weapon in Weapons)
            {
                Data.Tables["Weapons"].Rows.Add(weapon.Name, weapon.AttributeLink, weapon.Mode, weapon.AttackBase, weapon.BlockBase, weapon.AttackBonus, weapon.BlockBonus, weapon.Stamina, weapon.Initiative, weapon.Damage, weapon.Impulse, weapon.ArmorPenetration, weapon.Reload, weapon.Range);
            }
            foreach(WeaponSelectModel ws in SelectedWeapons)
            {
                Data.Tables["SelectedWeapons"].Rows.Add(ws.Weapon.Name, ws.Position);
            }
            foreach (MeleeWeaponModel weapon in MeleeWeapons)
            {
                Data.Tables["MeleeWeapons"].Rows.Add(weapon.Name, weapon.Weapons?.Name ?? "", weapon.Damage, weapon.Impulse, weapon.ArmorPenetration, weapon.Stamina, weapon.Range, weapon.Break, weapon.Ticks, weapon.AttackBonus, weapon.BlockBonus);
            }
            foreach (RangedWeaponModel weapon in RangedWeapons)
            {
                Data.Tables["RangedWeapons"].Rows.Add(weapon.Name, weapon.Weapons?.Name ?? "", weapon.Damage, weapon.Impulse, weapon.ArmorPenetration, weapon.Stamina, weapon.Range, weapon.Break, weapon.Load, weapon.StaminaLoad, weapon.Ticks, weapon.AttackBonus, weapon.BlockBonus);
            }
        }

        #endregion Saving

        #region Events

        public void Weapon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            WeaponModel weapon = sender as WeaponModel;
            switch (e.PropertyName)
            {                
                case "AttackTotal":
                    weapon.AttackBonus = weapon.AttackTotal - weapon.AttackModifier - weapon.AttackStandard;
                    break;
                case "AttackModifier":
                    weapon.AttackTotal = weapon.AttackStandard + weapon.AttackModifier + weapon.AttackBonus;
                    break;
                case "BlockTotal":
                    if (weapon.Mode != "Ranged")
                        weapon.BlockBonus = weapon.BlockTotal - weapon.BlockModifier - weapon.BlockStandard;
                    else if(weapon.BlockTotal != 0)
                        weapon.BlockTotal = 0;
                    break;
                case "BlockModifier":
                    weapon.BlockTotal = weapon.BlockStandard + weapon.BlockModifier + weapon.BlockBonus;
                    break;
                default:
                    break;
            }
            CalcExpWeapon();
        }

        public void MeleeWeapon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MeleeWeaponModel weapon = sender as MeleeWeaponModel;
            switch (e.PropertyName)
            {
                case "Weapons":
                    weapon.AttackBase = weapon.Weapons.AttackTotal;
                     weapon.BlockBase = weapon.Weapons.BlockTotal;
                    break;
                case "AttackBase":
                    weapon.AttackTotal = weapon.AttackBase + (weapon.AttackBonus ?? 0);
                    break;
                case "AttackTotal":
                    weapon.AttackBonus = weapon.AttackTotal - weapon.AttackBase;
                    break;
                case "BlockBase":
                    weapon.BlockTotal = weapon.BlockBase + (weapon.BlockBonus ?? 0);
                    break;
                case "BlockTotal":
                    weapon.BlockBonus = weapon.BlockTotal - weapon.BlockBase;
                    break;
                default:
                    break;
            }

        }

        public void RangedWeapon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RangedWeaponModel weapon = sender as RangedWeaponModel;
            switch (e.PropertyName)
            {
                case "Weapons":
                    weapon.AttackBase = weapon.Weapons.AttackTotal;
                    break;
                case "AttackBase":
                    weapon.AttackTotal = weapon.AttackBase + (weapon.AttackBonus ?? 0);
                    break;
                case "AttackTotal":
                    weapon.AttackBonus = weapon.AttackTotal - weapon.AttackBase;
                    break;
                default:
                    break;
            }

        }

        public void SelectedWeapon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
           
            

        }

        #endregion Events

        #region Calculation

        public void CalculateWeaponAll()
        {
            foreach (WeaponModel weapon in Weapons)
            {
                CalculateWeapon(weapon);
            }
        }
        //ToDo: Probleme mit dem WeaponSelectModel => Mr. Jacobs fragen.
        public void CalculateWeapon(WeaponModel weapon)
        {
            //Findet die Attribte, auf der die Grundwerte der Waffe basieren
            string[] attributes = weapon.AttributeLink.Split('/');
            double standardValues = 0;
            foreach (string s in attributes)
            {
                standardValues += Attributes.Where(x => x.Tag == s).FirstOrDefault().Value;
            }
            weapon.AttackStandard = Math.Round((standardValues / 2 / attributes.Length) + weapon.AttackBase, 0);
            weapon.AttackTotal = weapon.AttackStandard + weapon.AttackBonus + weapon.AttackModifier;

            if (weapon.Mode != "Ranged")
            {
                weapon.BlockStandard = Math.Round((standardValues / 2 / attributes.Length) + +weapon.BlockBase, 0);
                weapon.BlockTotal = weapon.BlockStandard + weapon.BlockBonus + weapon.BlockModifier;
            }
            else
                weapon.BlockTotal = 0;
            

            //ToDo: Die WeaponSelectModels kriegen aktuell nicht mit, wenn sich die Werte der Waffen verändern
            //Um das fürs Erste mal zu bypassen, setzten wir einfach das WeaponModel-Property nochmal neu und lösen so ein PropertyChangedEvent aus
            foreach (WeaponSelectModel selectedweapon in SelectedWeapons.Where(s => s.Weapon == weapon))
            {
                selectedweapon.Weapon = weapon;
            }

        }


        #endregion Calculation

        #endregion Weapons

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Armor and Offhand

        #region Properties

        public ObservableCollection<ArmorModel> Armor
        {
            get { return Get<ObservableCollection<ArmorModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<OffHandModel> OffHands
        {
            get { return Get<ObservableCollection<OffHandModel>>(); }
            set { Set(value); }
        }

        #endregion Properties

        #region Loading

        public void LoadArmor()
        {
            Armor = DataTableListConverter.ConvertToObservableCollection<ArmorModel>(Data.Tables["Armor"]);
        }

        public void LoadOffHands()
        {
            OffHands = DataTableListConverter.ConvertToObservableCollection<OffHandModel>(Data.Tables["OffHand"]);
        }

        #endregion Loading

        #region Saving

        public void SaveArmor()
        {
            foreach (ArmorModel armor in Armor)
            {
                Data.Tables["Armor"].Rows.Add(armor.Name, armor.Head, armor.Torso, armor.LeftArm, armor.RightArm, armor.LeftLeg, armor.RightLeg, armor.Toughness, armor.Slow, armor.Restriction, armor.Break);
            }
        }
        public void SaveOffHand()
        {
            foreach (OffHandModel offhand in OffHands)
            {
                Data.Tables["OffHand"].Rows.Add(offhand.Name, offhand.Strenght, offhand.Toughness, offhand.Break, offhand.BlockBonus, offhand.TickBonus, offhand.StaminaBonus);
            }
        }

        #endregion Saving


        #region Events


        #endregion Events

        #endregion Armor and Offhand

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Traits

        #region Properties

        public ObservableCollection<TraitCategoryModel> Traits
        {
            get { return Get<ObservableCollection<TraitCategoryModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<TraitCategoryModel> SpellTraits
        {
            get { return Get<ObservableCollection<TraitCategoryModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<TraitCategoryModel> CombatTraits
        {
            get { return Get<ObservableCollection<TraitCategoryModel>>(); }
            set { Set(value); }
        }

        #endregion Properties

        #region Loading

        //ToDo: Diesen Clusterfuck auch irgendwie in den DataTableConverter einpflegen
        public void LoadTraits()
        {
            //Handling für den Fall, dass ein Charakter keine Traits hat
            if(Data.Tables["Traits"] == null)
            {
                Data.Tables.Add("Traits");
                Data.Tables["Traits"].Columns.Add("Key", typeof(int));
                Data.Tables["Traits"].Columns.Add("Name", typeof(string));
                Data.Tables["Traits"].Columns.Add("Description", typeof(string));
                Data.Tables["Traits"].Columns.Add("TraitCategory_Id", typeof(int));
            }
            var v = Data.Tables["TraitCategory"].DefaultView.ToTable(true, "Type");
            foreach (DataRow rowCategoryType in Data.Tables["TraitCategory"].DefaultView.ToTable(true, "Type").Rows)
            {
                ObservableCollection<TraitCategoryModel> l_TraitList = new ObservableCollection<TraitCategoryModel>();
                foreach (DataRow rowCategory in Data.Tables["TraitCategory"].Select("Type = '"+ rowCategoryType["Type"].ToString() + "'"))
                {
                    ObservableCollection<TraitModel> traits = new ObservableCollection<TraitModel>();
                    string traitTexts = ""; //Die Auflistung aller Traits in einer Kateogorie per Name

                    foreach (DataRow rowTrait in Data.Tables["Traits"].Select("TraitCategory_Id = " + rowCategory["TraitCategory_Id"]))
                    {
                        TraitModel trait = new TraitModel
                        {
                            Key = rowTrait["Key"].ToString() == "" ? 0 : int.Parse(rowTrait["Key"].ToString()),
                            Name = rowTrait["Name"].ToString(),
                            Description = rowTrait["Description"].ToString(),
                        };
                        traitTexts += rowTrait["Name"].ToString() + ", ";
                        traits.Add(trait);
                    }

                    TraitCategoryModel category = new TraitCategoryModel
                    {
                        Key = int.Parse(rowCategory["TraitCategory_Id"].ToString()),
                        Name = rowCategory["Name"].ToString(),
                        Type = rowCategory["Type"].ToString(),
                        TraitTexts = traitTexts,
                        Traits = traits,
                    };
                    l_TraitList.Add(category);
                }
                
                //Hardcoded je nach Typ die Zuordnung
                if (rowCategoryType["Type"].ToString() == "Character")
                {
                    Traits = l_TraitList;
                }
                else if (rowCategoryType["Type"].ToString() == "Combat")
                {
                    CombatTraits = l_TraitList;
                }
                else if (rowCategoryType["Type"].ToString() == "Spell")
                {
                    SpellTraits = l_TraitList;
                }
            }
            
        }


        #endregion Loading

        #region Saving

        public void SaveTraits()
        {
            foreach (TraitCategoryModel traitcategory in Traits)
            {
                Data.Tables["TraitCategory"].Rows.Add(traitcategory.Name, traitcategory.Type, traitcategory.Key);
                foreach (TraitModel trait in traitcategory.Traits.ToList())
                {
                    Data.Tables["Traits"].Rows.Add(trait.Key, trait.Name, trait.Description, traitcategory.Key);
                }
            }
        }

        #endregion Saving

        #region Events

        public void Trait_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        #endregion Events

        #region Calculation

        public void CalculateTraitModifiers()
        {

            //List<ObservableCollection<TraitCategoryModel>> l_traits = new List<ObservableCollection<TraitCategoryModel>>();
            //l_traits.Add(Traits);
            //l_traits.Add(CombatTraits);
            //l_traits.Add(SpellTraits);

            //foreach (ObservableCollection<TraitCategoryModel> traitcategory in l_traits)
            //{
            //    foreach (TraitModifierModel modifier in traitcategory.SelectMany(c => c.Traits).SelectMany(t => t.Modifiers))
            //    {
            //        foreach (AttributeModel atr in Attributes.Where(m => m.Name == modifier.NameLink))
            //        {
            //            atr.Modifiers = Math.Round(atr.Modifiers + modifier.Value, 0);
            //        }
            //        foreach (SkillModel skl in SkillsLeft.Cast<SkillModel>().Where(m => m.Name == modifier.NameLink))
            //        {
            //            skl.Modifiers = Math.Round(skl.Modifiers + modifier.Value, 0);
            //        }
            //        foreach (SkillModel skl in SkillsRight.Cast<SkillModel>().Where(m => m.Name == modifier.NameLink))
            //        {
            //            skl.Modifiers = Math.Round(skl.Modifiers + modifier.Value, 0);
            //        }
            //        foreach (StatusValueModel stv in StatusValues.Where(m => m.Name == modifier.NameLink))
            //        {
            //            stv.Modifiers = Math.Round(stv.Modifiers + modifier.Value, 0);
            //        }
            //        foreach (WeaponModel weap in Weapons.Where(m => m.Name == modifier.NameLink))
            //        {
            //            if (modifier.TypeLink == "Angriff")
            //            {
            //                weap.AttackModifier = Math.Round(weap.AttackModifier + modifier.Value, 0);
            //            }
            //            if (modifier.TypeLink == "Parieren")
            //            {
            //                weap.BlockModifier = Math.Round(weap.BlockModifier + modifier.Value, 0);
            //            }
            //        }
            //    }
            //}
        }

        #endregion Calculation

        #endregion Traits

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Modifiers

        #region Properties

        public ObservableCollection<TraitModifierModel> Modifiers
        {
            get { return Get<ObservableCollection<TraitModifierModel>>(); }
            set { Set(value); }
        }

        #endregion Properties 

        #region Loading
        public void LoadModifiers()
        {
            //Todo: Bin mir noch nicht sicher wie glücklich ich mit diesem Fix sein sollte (gleiches bei Traits)
            if(Data.Tables["Modifiers"] == null)
            {
                Data.Tables.Add("Modifiers");
                Data.Tables["Modifiers"].Columns.Add("NameLink", typeof(string));
                Data.Tables["Modifiers"].Columns.Add("TypeLink", typeof(string));
                Data.Tables["Modifiers"].Columns.Add("Value", typeof(double));
                Data.Tables["Modifiers"].Columns.Add("TraitLink", typeof(int));
            }

            Modifiers = new ObservableCollection<TraitModifierModel>();
            foreach (DataRow row in Data.Tables["Modifiers"].Rows)
            {
                ModifierModel basemod = BaseModifiers.Where(m => m.NameLink == row["NameLink"].ToString()).FirstOrDefault();



                TraitModifierModel mod = new TraitModifierModel
                {
                    Modifier = basemod,
                    Value = row["Value"].ToString() == "" ? 0 : double.Parse(row["Value"].ToString()),
                    TraitLink = row["TraitLink"].ToString() == "" ? 0 :int.Parse(row["TraitLink"].ToString()),
                };
                mod.Modifier.TypeLink = row["TypeLink"].ToString();
                Modifiers.Add(mod);
            }
        }
        #endregion Loading

        #region Saving

        public void SaveModifier()
        {
            foreach(TraitModifierModel mod in Modifiers)
            {
                Data.Tables["Modifiers"].Rows.Add(mod.NameLink, mod.TypeLink, mod.Value, mod.TraitLink);
            }
        }

        #endregion Saving

        #region Calculation

        public void CalculateModifiers()
        {
            foreach (WeaponModel weapon in Weapons)
            {
                weapon.AttackModifier = 0;
                weapon.BlockModifier = 0;
                foreach (TraitModifierModel mod in Modifiers.Where(m => m.NameLink == weapon.Name))
                {
                    if(mod.TypeLink == "Angriff")
                    {
                        weapon.AttackModifier = weapon.AttackModifier + mod.Value;
                    }
                    if (mod.TypeLink == "Parieren")
                    {
                        weapon.BlockModifier = weapon.BlockModifier + mod.Value;
                    }
                }
            }
            foreach (SkillModel skill in SkillsLeft)
            {
                skill.Modifiers = 0;
                foreach (TraitModifierModel mod in Modifiers.Where(m => m.NameLink == skill.Name))
                {
                    skill.Modifiers = skill.Modifiers + mod.Value;
                }
            }
            foreach (SkillModel skill in SkillsRight)
            {
                skill.Modifiers = 0;
                foreach (TraitModifierModel mod in Modifiers.Where(m => m.NameLink == skill.Name))
                {
                    skill.Modifiers = skill.Modifiers + mod.Value;
                }
            }
            foreach (AttributeModel atr in Attributes)
            {
                atr.Modifiers = 0;
                foreach (TraitModifierModel mod in Modifiers.Where(m => m.NameLink == atr.Name))
                {
                    atr.Modifiers = atr.Modifiers + mod.Value;
                }
            }
            foreach (StatusValueModel stv in StatusValues)
            {
                stv.Modifiers = 0;
                foreach (TraitModifierModel mod in Modifiers.Where(m => m.NameLink == stv.Name))
                {
                    stv.Modifiers = stv.Modifiers + mod.Value;
                }
            }
        }

        #endregion Calculation

        #endregion Modifiers

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Spells and Rituals

        #region Properties


        public ObservableCollection<SpellModel> Spells
        {
            get { return Get<ObservableCollection<SpellModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<RitualModel> Rituals
        {
            get { return Get<ObservableCollection<RitualModel>>(); }
            set { Set(value); }
        }

        #endregion Properties

        #region Loading

        public void LoadSpells()
        {
            Spells = DataTableListConverter.ConvertToObservableCollection<SpellModel, WeaponModel>(Data.Tables["Spells"], null, Weapons);
        }

        public void LoadRituals()
        {
            Rituals = DataTableListConverter.ConvertToObservableCollection<RitualModel>(Data.Tables["Rituals"]);
        }

        #endregion Loading

        #region Saving

        public void SaveSpells()
        {
            foreach (SpellModel spell in Spells)
            {
                Data.Tables["Spells"].Rows.Add(spell.Name, spell.Weapons?.Name ?? "", spell.Requirement, spell.Value, spell.Description, spell.Mana, spell.Ticks);
            }
        }

        public void SaveRituals()
        {
            foreach (RitualModel ritual in Rituals)
            {
                Data.Tables["Rituals"].Rows.Add(ritual.Name, ritual.Time, ritual.Requirement, ritual.Value, ritual.Duration, ritual.Description);
            }
        }

        #endregion Saving

        #endregion Spells and Rituals

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------\\

        #region Inventory

        #region Properties

        public ObservableCollection<InventoryItemModel> InventoryLeft
        {
            get { return Get<ObservableCollection<InventoryItemModel>>(); }
            set { Set(value); }
        }

        public ObservableCollection<InventoryItemModel> InventoryRight
        {
            get { return Get<ObservableCollection<InventoryItemModel>>(); }
            set { Set(value); }
        }

        public double WeightLeft
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double WeightRight
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double CarryWeight
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public MoneyModel Money
        {
            get { return Get<MoneyModel>(); }
            set { Set(value); }
        }

        #endregion Properties

        #region Loading

        public void LoadInventory()
        {
            List<InventoryItemModel>  l_inventory = DataTableListConverter.ConvertToList<InventoryItemModel>(Data.Tables["Inventory"], Inventory_PropertyChanged);

            InventoryLeft = new ObservableCollection<InventoryItemModel>(l_inventory.GetRange(0, l_inventory.Count / 2));
            InventoryRight = new ObservableCollection<InventoryItemModel>(l_inventory.GetRange(l_inventory.Count / 2, l_inventory.Count / 2));
        }

        public void LoadMoney()
        {
            foreach (DataRow row in Data.Tables["Money"].Rows)
            {
                Money = new MoneyModel
                {
                    Gold = Parser.ToNullable<double>(row["Gold"].ToString()),
                    Silver = Parser.ToNullable<double>(row["Silver"].ToString()),
                    Copper = Parser.ToNullable<double>(row["Copper"].ToString()),
                    Iron = Parser.ToNullable<double>(row["Iron"].ToString()),
                    Gems = row["Gems"].ToString(),
                    Artifacts = row["Artifacts"].ToString(),
                    Rest = row["Rest"].ToString(),
                };
               
            }
        }

        #endregion Loading

        #region Saving

        public void SaveInventory()
        {
            foreach (InventoryItemModel item in InventoryLeft)
            {
                Data.Tables["Inventory"].Rows.Add(item.Name, item.Quantity, item.Value, item.Weight, item.Place);
            }
            foreach (InventoryItemModel item in InventoryRight)
            {
                Data.Tables["Inventory"].Rows.Add(item.Name, item.Quantity, item.Value, item.Weight, item.Place);
            }
        }

        public void SaveMoney()
        {
            Data.Tables["Money"].Rows.Add(Money.Gold, Money.Silver, Money.Copper, Money.Iron, Money.Gems, Money.Artifacts, Money.Rest);
        }
        #endregion Saving

        #region Calculation

        public void CalculateCarryWeight()
        {
            CarryWeight = Math.Round(6 + Attributes.Where(atr => atr.Name == "Stärke").FirstOrDefault().Value * 2);
        }

        #endregion Calculation


        #region Events

        public void Inventory_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            WeightLeft = 0;
            foreach(InventoryItemModel item in InventoryLeft)
            {
                WeightLeft += (item.Weight == null) ? 0 : double.Parse(item.Weight.ToString());
            }

            WeightRight = 0;
            foreach (InventoryItemModel item in InventoryRight)
            {
                WeightRight += (item.Weight == null) ? 0 : double.Parse(item.Weight.ToString());
            }
        }

        #endregion Events

        #endregion Inventory


    }
}
