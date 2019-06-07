using CharacterSheetGenerator.CombatSheet.Model;
using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using CharacterSheetGenerator.Traits.Model;
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
    class CharacterSheetViewModel : NotifyBase
    {

        #region Properties

        public DataSet Data { get; set; } = new DataSet();

        public ObservableCollection<ModifierModel> BaseModifiers
        {
            get { return Get<ObservableCollection<ModifierModel>>(); }
            set { Set(value); }
        }

        #endregion Properties

        public CharacterSheetViewModel()
        {
            InitializeSettings();
            CreateCommands();
            //Speicher-Ordner anlegen
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Saves"))
            {
                DirectoryInfo di = Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Saves");
                }
        }

        #region Framework

        /// <summary>
        /// Zur Erstinitialisierung der Standarddaten
        /// </summary>
        private void InitializeSettings()
        {
            
            Data = new DataSet();
            XmlReader xmlData;

            DataSet l_Data = new DataSet();
            string[] files = Directory.GetFiles("Settings", "*.xml");


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


            LoadData();


        }

        /// <summary>
        /// /Alle Tabellen aus dem DateSet in Listen laden
        /// </summary>
        public void LoadData()
        {

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
        public void SaveData(DataTable tblAttributeLink)
        {
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
            SaveCommand = new RelayCommand(SaveMethod, CanExecute);
            LoadCommand = new RelayCommand(LoadMethod, CanExecute);

        }

        public ICommand OpenTraitViewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        
        
       

        public void OpenTraitViewMethod(string Category)
        {
            int keycounter = 0;
            TraitViewModel vm = new TraitViewModel();
            vm.Modifiers = new ObservableCollection<TraitModifierModel>(Modifiers);
            vm.BaseModifiers = BaseModifiers;
            vm.Category = Category;
            foreach (TraitCategoryModel category in Traits)
            {
               keycounter += category.Traits.Count();
            }
            vm.KeyCounter = keycounter;
            vm.Traits  =  new ObservableCollection<TraitModel>(Traits.Where(c => c.Name == Category).SelectMany(x => x.Traits));
            

            TraitView traitview = new TraitView();
            traitview.DataContext = vm;
            traitview.ShowDialog();
            if(vm.IsSaved)
            {
                TraitCategoryModel catgory = Traits.Where(c => c.Name == vm.Category).FirstOrDefault();
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

        public void SaveMethod()
        {

            DataTable tblAttributeLink = Data.Tables["SVAttributeLink"].Copy();

            //DataSet vor dem Speichern mit aktuellen Daten füllen
            Data.Clear();
            SaveData(tblAttributeLink);
  
            SaveWindowViewModel vm = new SaveWindowViewModel();
            vm.Data = this.Data;

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

                LoadData();
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
                xp = xp + skill.Value.GetValueOrDefault() * GetExpSkillModifier(skill.Difficulty);
            }
            foreach (SkillModel skill in SkillsLeft)
            {
                xp = xp + skill.Value.GetValueOrDefault() * GetExpSkillModifier(skill.Difficulty);
            }
            ExpSkills = xp;
        }

        public double GetExpSkillModifier(string s)
        {
            double mod = 0;
            switch(s)
            {
                case "A":
                    mod = 0.5;
                    break;
                case "B":
                    mod = 1;
                    break;
                case "C":
                    mod = 1.5;
                    break;
                case "D":
                    mod = 2;
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

        #region Attributea

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

        private void LoadAttributes()
        {
            Attributes = new ObservableCollection<AttributeModel>();
            foreach (DataRow row in Data.Tables["Attributes"].Select("Type = 'Base'"))
            {
                AttributeModel attribute = new AttributeModel
                {
                    Name = row["Name"].ToString(),
                    Tag = row["Tag"].ToString(),
                    Base = double.Parse(row["Value"].ToString()),
                    Color = new SolidColorBrush(ColorHandler.IntToColor(int.Parse(row["Color"].ToString()))),
                    Special = false,

                };
                Attributes.Add(attribute);
                attribute.PropertyChanged += Attribute_PropertyChanged;
            }

        }

        /// <summary>
        /// Spezielle Attribute sind eigentlich StatusValues, die auf einigen Seiten bei den Attributen oben stehen.
        /// </summary>
        private void LoadSpecialAttributes()
        {
            //Aktuell immer schwarz, sonst Kirsten Ritzmann!
            SpecialAttributes = new ObservableCollection<AttributeModel>();
            foreach (DataRow row in Data.Tables["Attributes"].Select("Type = 'Special'"))
            {
                AttributeModel attribute = new AttributeModel
                {
                    Name = row["Name"].ToString(),
                    Tag = row["Tag"].ToString(),
                    Base = double.Parse(row["Value"].ToString()),
                    Color = new SolidColorBrush(ColorHandler.IntToColor(int.Parse(row["Color"].ToString()))),
                    Special = true,

                };
                SpecialAttributes.Add(attribute);
                attribute.PropertyChanged += Attribute_PropertyChanged;
            }
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

        #endregion Attributea

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
            StatusValues = new ObservableCollection<StatusValueModel>();
            foreach (DataRow row in Data.Tables["StatusValues"].Rows)
            {
                ObservableCollection<string> attributelinks = new ObservableCollection<string>();
                foreach (DataRow rowAttributeLink in Data.Tables["SVAttributeLink"].Select("StatusValues_Id = " + row["StatusValues_Id"]))
                {
                    attributelinks.Add(rowAttributeLink["SVAttributeLink_Text"].ToString());
                }

                StatusValueModel statusvalue = new StatusValueModel
                {
                    Key = int.Parse(row["StatusValues_Id"].ToString()),
                    Name = row["Name"].ToString(),
                    Base = double.Parse(row["Base"].ToString()),
                    Bonus = double.Parse(row["Bonus"].ToString()),
                    AttributeLinks = attributelinks,

                };

                StatusValues.Add(statusvalue);
                statusvalue.PropertyChanged += StatusValue_PropertyChanged;
            }
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
                    stv.Standard = Math.Round(stv.Base + attributes[0].Value * 1.5, 0);
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
            List<SkillModel> l_skills = new List<SkillModel>();
            foreach (DataRow row in Data.Tables["Skills"].Rows)
            {
                SkillModel skill = new SkillModel
                {
                    Name = row["Name"].ToString(),
                    Requirement = row["Requirement"].ToString(),
                    Base = Parser.ToNullable<double>(row["Value"].ToString()),
                    Difficulty = row["Difficulty"].ToString(),
                    Comment = row["Comment"].ToString(),
                    Category = row["Category"].ToString(),
                    Grouping = row["Grouping"].ToString(),


                };
                l_skills.Add(skill);
                skill.PropertyChanged += Skill_PropertyChanged;
            }

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
                Data.Tables["Skills"].Rows.Add(skill.Name, skill.Requirement, skill.Base, skill.Difficulty, skill.Comment, skill.Category, skill.Grouping);
            }
            foreach (SkillModel skill in SkillsRight)
            {
                Data.Tables["Skills"].Rows.Add(skill.Name, skill.Requirement, skill.Base, skill.Difficulty, skill.Comment, skill.Category, skill.Grouping);
            }
        }

        #endregion Saving

        #region Calculation

        public void CalculateSkillsAll()
        {
            foreach(SkillModel skill in SkillsLeft)
            {
                skill.Value = skill.Base + skill.Modifiers;
            }
            foreach (SkillModel skill in SkillsRight)
            {
                skill.Value = skill.Base + skill.Modifiers;
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
                    skl.Base = skl.Value - skl.Modifiers;
                    break;
                case "Modifiers":
                    skl.Value = skl.Base + skl.Modifiers;
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
            Languages = new ObservableCollection<LanguageModel>();
            foreach(DataRow row in Data.Tables["Languages"].Rows)
            {
                LanguageModel language = new LanguageModel
                {
                    Name = row["Name"].ToString(),
                };
                Languages.Add(language);
            }

            Writings = new ObservableCollection<LanguageModel>();
            foreach (DataRow row in Data.Tables["Writings"].Rows)
            {
                LanguageModel writing = new LanguageModel
                {
                    Name = row["Name"].ToString(),
                };
                Writings.Add(writing);
            }
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
            ObservableCollection<WeaponModel> l_weapons = new ObservableCollection<WeaponModel>();
            foreach (DataRow row in Data.Tables["Weapons"].Rows)
            {

                WeaponModel weapon = new WeaponModel
                {
                    Name = row["Name"].ToString(),
                    AttributeLink = row["AttributeLink"].ToString(),
                    AttackBonus = double.Parse(row["AttackBonus"].ToString()),
                    BlockBonus = double.Parse(row["BlockBonus"].ToString()),
                    Position = int.Parse(row["Position"].ToString()),
                    Stamina = int.Parse(row["Position"].ToString()),
                    Initiative = int.Parse(row["Position"].ToString()),
                    Damage = row["Damage"].ToString(),
                    Impulse = row["Impulse"].ToString(),
                    ArmorPenetration = row["ArmorPenetration"].ToString(),
                };
                l_weapons.Add(weapon);
                weapon.PropertyChanged += Weapon_PropertyChanged;
            }
            Weapons = l_weapons;
        }

        public void LoadSelectedWeapons()
        {
            SelectedWeapons = new ObservableCollection<WeaponSelectModel>();
            foreach (WeaponModel w in Weapons.Where(x => x.Position != 0))
            {
                WeaponSelectModel ws = new WeaponSelectModel { Weapon = w };
                SelectedWeapons.Add(ws);
                ws.PropertyChanged += SelectedWeapon_PropertyChanged;
            }



        }

        public void LoadMeleeWeapons()
        {
            ObservableCollection<MeleeWeaponModel> l_weapons = new ObservableCollection<MeleeWeaponModel>();
            foreach (DataRow row in Data.Tables["MeleeWeapons"].Rows)
            {

                MeleeWeaponModel weapon = new MeleeWeaponModel();

                weapon.Name = row["Name"].ToString();
                weapon.Weapons = Weapons.Where(x => x.Name == row["Weapons"].ToString()).ToList().FirstOrDefault();
                weapon.AttackBonus = Parser.ToNullable<double>(row["AttackBonus"].ToString());
                weapon.BlockBonus = Parser.ToNullable<double>(row["BlockBonus"].ToString());
                weapon.Damage = row["Damage"].ToString();
                weapon.Impulse = row["Impulse"].ToString();
                weapon.ArmorPenetration = row["ArmorPenetration"].ToString();
                weapon.Ticks = Parser.ToNullable<int>(row["Ticks"].ToString());
                weapon.Break = Parser.ToNullable<int>(row["Break"].ToString());
                weapon.Range = row["Range"].ToString();


                l_weapons.Add(weapon);
            }
            MeleeWeapons = l_weapons;
        }

        public void LoadRangedWeapons()
        {
            ObservableCollection<RangedWeaponModel> l_weapons = new ObservableCollection<RangedWeaponModel>();
            foreach (DataRow row in Data.Tables["RangedWeapons"].Rows)
            {

                RangedWeaponModel weapon = new RangedWeaponModel();

                weapon.Name = row["Name"].ToString();
                weapon.Weapons = Weapons.Where(x => x.Name == row["Weapons"].ToString()).ToList().FirstOrDefault();
                weapon.AttackBonus = Parser.ToNullable<double>(row["AttackBonus"].ToString());
                weapon.BlockBonus = Parser.ToNullable<double>(row["BlockBonus"].ToString());
                weapon.Damage = row["Damage"].ToString();
                weapon.Impulse = row["Impulse"].ToString();
                weapon.ArmorPenetration = row["ArmorPenetration"].ToString();
                weapon.Load = Parser.ToNullable<int>(row["Load"].ToString());
                weapon.Ticks = Parser.ToNullable<int>(row["Ticks"].ToString());
                weapon.Break = Parser.ToNullable<int>(row["Break"].ToString());
                weapon.Range = row["Range"].ToString();


                l_weapons.Add(weapon);
            }
            RangedWeapons = l_weapons;
        }

        #endregion Loading

        #region Saving

        public void SaveWeapons()
        {
            foreach (WeaponModel weapon in Weapons)
            {
                Data.Tables["Weapons"].Rows.Add(weapon.Name, weapon.AttributeLink, weapon.AttackBonus, weapon.BlockBonus, weapon.Stamina, weapon.Initiative, weapon.Damage, weapon.Impulse, weapon.ArmorPenetration, weapon.Position);
            }
            foreach (MeleeWeaponModel weapon in MeleeWeapons)
            {
                Data.Tables["MeleeWeapons"].Rows.Add(weapon.Name, weapon.Weapons?.Name == null ? "" : weapon.Weapons.Name, weapon.Damage, weapon.Impulse, weapon.ArmorPenetration, weapon.Range, weapon.Break, weapon.Ticks, weapon.AttackBonus, weapon.BlockBonus);
            }
            foreach (RangedWeaponModel weapon in RangedWeapons)
            {
                Data.Tables["RangedWeapons"].Rows.Add(weapon.Name, weapon.Weapons?.Name == null ? "" : weapon.Weapons.Name, weapon.Damage, weapon.Impulse, weapon.ArmorPenetration, weapon.Range, weapon.Break, weapon.Load, weapon.Ticks, weapon.AttackBonus, weapon.BlockBonus);
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
                    weapon.BlockBonus = weapon.BlockTotal - weapon.BlockModifier - weapon.BlockStandard;
                    break;
                case "BlockModifier":
                    weapon.BlockTotal = weapon.BlockStandard + weapon.BlockModifier + weapon.BlockBonus;
                    break;
                default:
                    break;
            }
            CalcExpWeapon();
        }

        public void SelectedWeapon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Enumerable.Range(1, SelectedWeapons.Count).Except(SelectedWeapons.Select(x => x.Position)).FirstOrDefault();
            //Exp neu berechnen

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

            weapon.BlockStandard = Math.Round((standardValues / 2 / attributes.Length) + +weapon.BlockBase, 0);
            weapon.BlockTotal = weapon.BlockStandard + weapon.BlockBonus + weapon.BlockModifier;

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


        public void LoadTraits()
        {

            foreach(DataRow rowCategoryType in Data.Tables["TraitCategory"].DefaultView.ToTable(true, "Type").Rows)
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
                if (rowCategoryType["Type"].ToString() == "Combat")
                {
                    CombatTraits = l_TraitList;
                }
                if (rowCategoryType["Type"].ToString() == "Spell")
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
                    Data.Tables["Traits"].Rows.Add(trait.Name, trait.Description, trait.Key, traitcategory.Key);
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
            //Todo: Bin mir noch nicht sicher wie glücklich ich mit diesem Fix sein sollte 
            if(Data.Tables["Modifiers"] == null)
            {
                Data.Tables.Add("Modifiers");
                Data.Tables["Modifiers"].Columns.Add("NameLink");
                Data.Tables["Modifiers"].Columns.Add("TypeLink");
                Data.Tables["Modifiers"].Columns.Add("Value");
                Data.Tables["Modifiers"].Columns.Add("TraitLink");
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
            ObservableCollection<ArmorModel> l_armor = new ObservableCollection<ArmorModel>();
            foreach (DataRow row in Data.Tables["Armor"].Rows)
            {

                ArmorModel armor = new ArmorModel
                {
                    Name = row["Name"].ToString(),
                    Head = Parser.ToNullable<int>(row["Head"].ToString()),
                    Torso = Parser.ToNullable<int>(row["Torso"].ToString()),
                    LeftArm = Parser.ToNullable<int>(row["LeftArm"].ToString()),
                    RightArm = Parser.ToNullable<int>(row["RightArm"].ToString()),
                    LeftLeg = Parser.ToNullable<int>(row["LeftLeg"].ToString()),
                    RightLeg = Parser.ToNullable<int>(row["RightLeg"].ToString()),
                    Toughness = Parser.ToNullable<int>(row["Toughness"].ToString()),
                    Slow = Parser.ToNullable<int>(row["Slow"].ToString()),
                    Restriction = Parser.ToNullable<int>(row["Restriction"].ToString()),
                    Break = Parser.ToNullable<int>(row["Break"].ToString()),

                };


                l_armor.Add(armor);
            }
            Armor = l_armor;
        }

        public void LoadOffHands()
        {
            ObservableCollection<OffHandModel> l_offhands = new ObservableCollection<OffHandModel>();
            foreach (DataRow row in Data.Tables["OffHand"].Rows)
            {

                OffHandModel offhand = new OffHandModel
                {
                    Name = row["Name"].ToString(),
                    AttackBonus = Parser.ToNullable<double>(row["AttackBonus"].ToString()),
                    BlockBonus = Parser.ToNullable<double>(row["BlockBonus"].ToString()),
                    Toughness = Parser.ToNullable<int>(row["Toughness"].ToString()),
                    Strenght = Parser.ToNullable<int>(row["Strenght"].ToString()),
                    Break = Parser.ToNullable<int>(row["Break"].ToString()),

                };


                l_offhands.Add(offhand);
            }
            OffHands = l_offhands;
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
                Data.Tables["OffHand"].Rows.Add(offhand.Name, offhand.Strenght, offhand.Toughness, offhand.Break, offhand.AttackBonus, offhand.BlockBonus);
            }
        }

        #endregion Saving

        #endregion Armor and Offhand

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
            ObservableCollection<SpellModel> l_spells = new ObservableCollection<SpellModel>();
            foreach (DataRow row in Data.Tables["Spells"].Rows)
            {

                SpellModel spell = new SpellModel
                {
                    Name = row["Name"].ToString(),
                    Type = row["Type"].ToString(),
                    Requirement = row["Requirement"].ToString(),
                    Value = Parser.ToNullable<int>(row["Value"].ToString()),
                    Damage = row["Damage"].ToString(),
                    MagicDamage = row["MagicDamage"].ToString(),
                    ArmorPenetration = row["ArmorPenetration"].ToString(),
                    Impulse = row["Impulse"].ToString(),
                    Range = row["Range"].ToString(),
                    Duration = row["Duration"].ToString(),
                    Description = row["Description"].ToString(),
                };


                l_spells.Add(spell);
            }
            Spells = l_spells;
        }

        public void LoadRituals()
        {
            ObservableCollection<RitualModel> l_rituals = new ObservableCollection<RitualModel>();
            foreach (DataRow row in Data.Tables["Rituals"].Rows)
            {

                RitualModel ritual = new RitualModel
                {
                    Name = row["Name"].ToString(),
                    Type = row["Type"].ToString(),
                    Requirement = row["Requirement"].ToString(),
                    Value = Parser.ToNullable<int>(row["Value"].ToString()),
                    Duration = row["Duration"].ToString(),
                    Description = row["Description"].ToString(),

                };


                l_rituals.Add(ritual);
            }
            Rituals = l_rituals;
        }

        #endregion Loading

        #region Saving

        public void SaveSpells()
        {
            foreach (SpellModel spell in Spells)
            {
                Data.Tables["Spells"].Rows.Add(spell.Name, spell.Type, spell.Requirement, spell.Value, spell.Damage, spell.MagicDamage, spell.ArmorPenetration, spell.Impulse, spell.Range, spell.Duration, spell.Description);
            }
        }

        public void SaveRituals()
        {
            foreach (RitualModel ritual in Rituals)
            {
                Data.Tables["Rituals"].Rows.Add(ritual.Name, ritual.Type, ritual.Requirement, ritual.Value, ritual.Duration, ritual.Description);
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
            List<InventoryItemModel> l_inventory = new List<InventoryItemModel>();
            foreach (DataRow row in Data.Tables["Inventory"].Rows)
            {
                InventoryItemModel item = new InventoryItemModel
                {
                    Name = row["Name"].ToString(),
                    Quantity = Parser.ToNullable<double>(row["Quantity"].ToString()),
                    Value = Parser.ToNullable<double>(row["Value"].ToString()),
                    Weight = Parser.ToNullable<double>(row["Weight"].ToString()),
                    Place = row["Place"].ToString(),
                };
                l_inventory.Add(item);
                item.PropertyChanged += Inventory_PropertyChanged;
            }
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
