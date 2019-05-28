using Microsoft.Win32;
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

        private void InitializeSettings()
        {
            //Hier wird das ganze Xml-Zeugs aus dem Ordner ins DataSet geladen

            Data = new DataSet();

            LoadData("Settings");



        }

        public void LoadData(string Path)
        {

            XmlReader xmlData;

            DataSet l_Data = new DataSet();
            string[] files = Directory.GetFiles(Path, "*.xml");


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

            //Alle Tabellen aus dem DateSet in Listen umwandeln
            CreateAttributes();
            CreateSpecialAttributes();
            CreateStatusValues();
            CreateSkills();
            CreateCharacterInformation();
            CreateCharacterTraits();
            CreateCombatTraits();
            CreateSpellTraits();
            CreateWeapons();
            CreateSelectedWeapons();
            CreateMeleeWeapons();
            CreateRangedWeapons();
            CreateArmor();
            CreateOffHands();
            CreateSpells();
            CreateRituals();
            CreateInventory();

            CalculateTraitModifiers();



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

            CalculateWeaponAll();

        }

        #region Commands

        //Komplette Dummy-Implementierung der Commands, sodass man es einfach erweitern kann
        private void CreateCommands()
        {
            SaveCommand = new RelayCommand(SaveMethod, CanExecute);
            LoadCommand = new RelayCommand(LoadMethod, CanExecute);

        }
        public ICommand SaveCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public void SaveMethod()
        {

            DataTable tblAttributeLink = Data.Tables["SVAttributeLink"].Copy();


            Data.Clear();
            foreach(AttributeModel atr in Attributes)
            {
                Data.Tables["Attributes"].Rows.Add(atr.Name, "Base" , atr.Tag, atr.Base, ColorHandler.ColorToInt(atr.Color.Color));
            }
            foreach (AttributeModel atr in SpecialAttributes)
            {
                Data.Tables["Attributes"].Rows.Add(atr.Name, "Special", atr.Tag, atr.Base, ColorHandler.ColorToInt(atr.Color.Color));
            }

            int i = 0;
            foreach (CharacterInformationModel info in CharacterInformation)
            {
                Data.Tables["CharacterInformation"].Rows.Add(info.FirstElement, info.FirstValue, i);
                if(info.SecondElement != null && info.SecondElement != "")
                {
                    Data.Tables["CharacterInformation"].Rows.Add(info.SecondElement, info.SecondValue, i);
                }
                if (info.SecondElement != null && info.SecondElement != "")
                {
                    Data.Tables["CharacterInformation"].Rows.Add(info.ThirdElement, info.ThirdValue, i);
                }
                i++;
            }

            foreach(StatusValueModel stv in StatusValues)
            {
                Data.Tables["StatusValues"].Rows.Add(stv.Name, stv.Base, stv.Bonus, stv.Key);
            }

            foreach (DataRow row in tblAttributeLink.Rows)
            {
                Data.Tables["SVAttributeLink"].Rows.Add(row["SVAttributeLink_Text"], row["StatusValues_Id"]);
            }

            foreach (SkillModel skill in SkillsLeft)
            {
                Data.Tables["Skills"].Rows.Add(skill.Name, skill.Requirement, skill.Base, skill.Difficulty, skill.Comment, skill.Category, skill.Grouping);
            }
            foreach (SkillModel skill in SkillsRight)
            {
                Data.Tables["Skills"].Rows.Add(skill.Name, skill.Requirement, skill.Base, skill.Difficulty, skill.Comment, skill.Category, skill.Grouping);
            }

            foreach(WeaponModel weapon in Weapons)
            {
                Data.Tables["Weapons"].Rows.Add(weapon.Name, weapon.AttributeLink, weapon.AttackBonus, weapon.BlockBonus, weapon.Stamina, weapon.Initiative, weapon.Damage, weapon.Impulse, weapon.ArmorPenetration, weapon.Position);
            }
            foreach(MeleeWeaponModel weapon in MeleeWeapons)
            {
                Data.Tables["MeleeWeapons"].Rows.Add(weapon.Name, weapon.Weapons?.Name == null ? "" : weapon.Weapons.Name, weapon.Damage, weapon.Impulse, weapon.ArmorPenetration, weapon.Range, weapon.Break, weapon.Ticks, weapon.AttackBonus, weapon.BlockBonus);
            }
            foreach(RangedWeaponModel weapon in RangedWeapons)
            {
                Data.Tables["RangedWeapons"].Rows.Add(weapon.Name, weapon.Weapons?.Name == null ? "" : weapon.Weapons.Name, weapon.Damage, weapon.Impulse, weapon.ArmorPenetration, weapon.Range, weapon.Break, weapon.Load, weapon.Ticks, weapon.AttackBonus, weapon.BlockBonus);
            }
            foreach(ArmorModel armor in Armor)
            {
                Data.Tables["Armor"].Rows.Add(armor.Name, armor.Head, armor.Torso, armor.LeftArm, armor.RightArm, armor.LeftLeg, armor.RightLeg, armor.Toughness, armor.Slow, armor.Restriction, armor.Break);
            }
            foreach(OffHandModel offhand in OffHands)
            {
                Data.Tables["OffHand"].Rows.Add(offhand.Name, offhand.Strenght, offhand.Toughness, offhand.Break, offhand.AttackBonus, offhand.BlockBonus);
            }
          
            foreach(TraitCategoryModel traitcategory in Traits)
            {
                Data.Tables["TraitCategory"].Rows.Add(traitcategory.Name, traitcategory.Type, traitcategory.Key);
               foreach (TraitModel trait in traitcategory.Traits.ToList())
                {
                    Data.Tables["Trait"].Rows.Add(trait.Name, trait.Description, trait.Key, traitcategory.Key);
                    foreach(TraitModifierModel modifier in trait.Modifiers.ToList())
                    {
                        Data.Tables["TraitModifier"].Rows.Add(modifier.NameLink, modifier.Value, trait.Key);
                    }
                }
            }
            foreach(SpellModel spell in Spells)
            {
                Data.Tables["Spells"].Rows.Add(spell.Name, spell.Type, spell.Requirement, spell.Value, spell.Damage, spell.MagicDamage, spell.ArmorPenetration, spell.Impulse, spell.Range, spell.Duration, spell.FlavorText);
            }
            foreach(RitualModel ritual in Rituals)
            {
                Data.Tables["Rituals"].Rows.Add(ritual.Name, ritual.Type, ritual.Requirement, ritual.Value, ritual.Duration, ritual.FlavorText);
            }
            foreach(InventoryItemModel item in InventoryLeft)
            {
                Data.Tables["Inventory"].Rows.Add(item.Name, item.Quantity, item.Value, item.Weight, item.Place);
            }
            foreach (InventoryItemModel item in InventoryRight)
            {
                Data.Tables["Inventory"].Rows.Add(item.Name, item.Quantity, item.Value, item.Weight, item.Place);
            }





            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Saves";
            if (CharacterInformation.Where(c => c.FirstElement == "Name").FirstOrDefault().FirstValue != "" &&
                CharacterInformation.Where(c => c.FirstElement == "Name").FirstOrDefault().FirstValue != null)
            {
                if (Directory.Exists(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Saves\\" + CharacterInformation.Where(c => c.FirstElement == "Name").FirstOrDefault().FirstValue))
                    folderBrowserDialog.SelectedPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Saves\\" + CharacterInformation.Where(c => c.FirstElement == "Name").FirstOrDefault().FirstValue;
            }

            //saveFileDialog.FileName = CharacterInformation.Where(c => c.FirstElement == "Name").FirstOrDefault().FirstValue;
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string SavePath = folderBrowserDialog.SelectedPath;
                



                DirectoryInfo di = Directory.CreateDirectory(SavePath);


                foreach (DataTable tbl in Data.Tables)
                {
                    XmlTextWriter writer = new XmlTextWriter(SavePath + "/" + tbl.TableName + ".xml", System.Text.Encoding.UTF8);
                    writer.WriteStartDocument(true);
                    writer.Formatting = Formatting.Indented;
                    writer.Indentation = 2;
                    writer.WriteStartElement("Table");
                    foreach (DataRow row in tbl.Rows)
                    {
                        writer.WriteStartElement(tbl.TableName);
                        foreach (DataColumn col in tbl.Columns)
                        {
                            writer.WriteStartElement(col.ColumnName);
                            writer.WriteString(row[col].ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndDocument();
                    writer.Close();
                }


                MessageBox.Show("XML File created ! ");
            }
            

        }

        public void LoadMethod()
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Saves";
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    LoadData(folderBrowserDialog.SelectedPath);
                    MessageBox.Show("Charakter erfolgreich geladen");
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            
        }

        public bool CanExecute()
        {
            return true; //Hier könnte eine Abfrage, ob das Command ausgeführt werden darf, stehen
        }
        #endregion Commands

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

        #region Initialization

        private void CreateAttributes()
        {
            Attributes = new ObservableCollection<AttributeModel>();
            foreach (DataRow row in Data.Tables["Attributes"].Select("Type = 'Base'"))
            {
                AttributeModel attribute = new AttributeModel
                {
                    Name = row["Name"].ToString(),
                    Tag = row["Tag"].ToString(),
                    Base = double.Parse(row["Value"].ToString()),
                    Value = double.Parse(row["Value"].ToString()),
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
        private void CreateSpecialAttributes()
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

        #endregion Initialization

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
        }

        #endregion Events

        #endregion Attributes

        #region Persönliche Daten

        #region Properties

        public ObservableCollection<CharacterInformationModel> CharacterInformation
        {
            get { return Get<ObservableCollection<CharacterInformationModel>>(); }
            set { Set(value); }
        }

        #endregion Properties

        #region Initialization

        public void CreateCharacterInformation()
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

        #endregion Initialization

        #region Events

        public void CahracterInformation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        #endregion Events

        #endregion Persönliche Daten

        #region Statuswerte

        #region Properties

        public ObservableCollection<StatusValueModel> StatusValues
        {
            get { return Get<ObservableCollection<StatusValueModel>>(); }
            set { Set(value); }
        }

        #endregion Properties

        #region Initialization

        public void CreateStatusValues()
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

        #endregion Initialization

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
        }

        #endregion Events

        #region Calculation

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
                default:
                    break;
            }

        }

        #endregion Calculation

        #endregion Statuswerte

        #region Fertigkeiten 

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

        #region Initialization

        private void CreateSkills()
        {
            List<SkillModel> l_skills = new List<SkillModel>();
            foreach (DataRow row in Data.Tables["Skills"].Rows)
            {
                SkillModel skill = new SkillModel
                {
                    Name = row["Name"].ToString(),
                    Requirement = row["Requirement"].ToString(),
                    Value = int.Parse(row["Value"].ToString()),
                    Difficulty = row["Difficulty"].ToString(),
                    Comment = row["Comment"].ToString(),
                    Category = row["Category"].ToString(),
                    Grouping = row["Grouping"].ToString(),


                };
                l_skills.Add(skill);
            }

            SkillsLeft = new ListCollectionView(l_skills.Where(s => s.Grouping == "Left").ToList());
            SkillsLeft.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            SkillsRight = new ListCollectionView(l_skills.Where(s => s.Grouping == "Right").ToList());
            SkillsRight.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

        }

        #endregion Initialization

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
        }

        #endregion Events

        #endregion Fertigkeiten   

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

        #region Initialization

        //ToDo: Rework
        public void CreateCharacterTraits()
        {
            ObservableCollection<TraitCategoryModel> l_TraitList = new ObservableCollection<TraitCategoryModel>();
            foreach (DataRow rowCategory in Data.Tables["TraitCategory"].Select("Type = 'Character'"))
            {
                ObservableCollection<TraitModel> traits = new ObservableCollection<TraitModel>();
                string traitTexts = "";
                foreach (DataRow rowTrait in Data.Tables["Trait"].Select("TraitCategory_Id = " + rowCategory["TraitCategory_Id"]))
                {

                    ObservableCollection<TraitModifierModel> modifiers = new ObservableCollection<TraitModifierModel>();
                    foreach (DataRow rowModifier in Data.Tables["TraitModifier"].Select("Trait_Id = " + rowTrait["Trait_Id"]))
                    {
                        TraitModifierModel modifier = new TraitModifierModel
                        {
                            NameLink = rowModifier["NameLink"].ToString(),
                            Value = int.Parse(rowModifier["Value"].ToString()),
                        };
                        modifiers.Add(modifier);
                    }
                    TraitModel trait = new TraitModel
                    {
                        Key = int.Parse(rowTrait["Trait_Id"].ToString()),
                        Name = rowTrait["Name"].ToString(),
                        Description = rowTrait["Description"].ToString(),
                        Modifiers = modifiers,
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
            Traits = l_TraitList;
        }

        public void CreateCombatTraits()
        {
            ObservableCollection<TraitCategoryModel> l_TraitList = new ObservableCollection<TraitCategoryModel>();
            foreach (DataRow rowCategory in Data.Tables["TraitCategory"].Select("Type = 'Combat'"))
            {
                ObservableCollection<TraitModel> traits = new ObservableCollection<TraitModel>();
                string traitTexts = "";
                foreach (DataRow rowTrait in Data.Tables["Trait"].Select("TraitCategory_Id = " + rowCategory["TraitCategory_Id"]))
                {

                    ObservableCollection<TraitModifierModel> modifiers = new ObservableCollection<TraitModifierModel>();
                    foreach (DataRow rowModifier in Data.Tables["TraitModifier"].Select("Trait_Id = " + rowTrait["Trait_Id"]))
                    {
                        TraitModifierModel modifier = new TraitModifierModel
                        {
                            NameLink = rowModifier["NameLink"].ToString(),
                            Value = int.Parse(rowModifier["Value"].ToString()),
                        };
                        modifiers.Add(modifier);
                    }
                    TraitModel trait = new TraitModel
                    {
                        Name = rowTrait["Name"].ToString(),
                        Description = rowTrait["Description"].ToString(),
                        Modifiers = modifiers,
                    };
                    traitTexts += rowTrait["Name"].ToString() + ", ";
                    traits.Add(trait);
                }
                TraitCategoryModel category = new TraitCategoryModel
                {
                    Name = rowCategory["Name"].ToString(),
                    TraitTexts = traitTexts,
                    Traits = traits,
                };
                l_TraitList.Add(category);
            }
            CombatTraits = l_TraitList;
        }

        public void CreateSpellTraits()
        {
            ObservableCollection<TraitCategoryModel> l_TraitList = new ObservableCollection<TraitCategoryModel>();
            foreach (DataRow rowCategory in Data.Tables["TraitCategory"].Select("Type = 'Spell'"))
            {
                ObservableCollection<TraitModel> traits = new ObservableCollection<TraitModel>();
                string traitTexts = "";
                foreach (DataRow rowTrait in Data.Tables["Trait"].Select("TraitCategory_Id = " + rowCategory["TraitCategory_Id"]))
                {

                    ObservableCollection<TraitModifierModel> modifiers = new ObservableCollection<TraitModifierModel>();
                    foreach (DataRow rowModifier in Data.Tables["TraitModifier"].Select("Trait_Id = " + rowTrait["Trait_Id"]))
                    {
                        TraitModifierModel modifier = new TraitModifierModel
                        {
                            NameLink = rowModifier["NameLink"].ToString(),
                            Value = int.Parse(rowModifier["Value"].ToString()),
                        };
                        modifiers.Add(modifier);
                    }
                    TraitModel trait = new TraitModel
                    {
                        Name = rowTrait["Name"].ToString(),
                        Description = rowTrait["Description"].ToString(),
                        Modifiers = modifiers,
                    };
                    traitTexts += rowTrait["Name"].ToString() + ", ";
                    traits.Add(trait);
                }
                TraitCategoryModel category = new TraitCategoryModel
                {
                    Name = rowCategory["Name"].ToString(),
                    TraitTexts = traitTexts,
                    Traits = traits,
                };
                l_TraitList.Add(category);
            }
            SpellTraits = l_TraitList;
        }

        #endregion Initialization

        #region Events

        public void Trait_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        #endregion Events

        #region Calculation
        //Todo: Geht aktuell nur auf Traits, nicht auf Combat und SpellTraits
        public void CalculateTraitModifiers()
        {

            foreach (TraitModifierModel modifier in Traits.SelectMany(c => c.Traits).SelectMany(t => t.Modifiers))
            {
                foreach (AttributeModel atr in Attributes.Where(m => m.Name == modifier.NameLink))
                {
                    atr.Modifiers = Math.Round(atr.Modifiers + modifier.Value, 0);
                }
                //ToDo: Weg finden das auch auf Skills anzuwneden
                //foreach (SkillModel atr in SkillsLeft.Where(m => m.Name == modifier.NameLink))
                //{
                //    atr.Modifiers = Math.Round(atr.Modifiers + modifier.Value, 0);
                //}
                //foreach (SkillModel atr in SkillsRight.Where(m => m.Name == modifier.NameLink))
                //{
                //    atr.Modifiers = Math.Round(atr.Modifiers + modifier.Value, 0);
                //}
                foreach (StatusValueModel stv in StatusValues.Where(m => m.Name == modifier.NameLink))
                {
                    stv.Modifiers = Math.Round(stv.Modifiers + modifier.Value, 0);
                }
                //Auch hier gibt es noch Probleme, um die ich mich noch kümmern muss
                //foreach (WeaponModel atr in Weapons.Where(m => m.Name == modifier.NameLink))
                //{
                //    atr.Modifiers = Math.Round(atr.Modifiers + modifier.Value, 0);
                //}
            }
            foreach (TraitModifierModel modifier in CombatTraits.SelectMany(c => c.Traits).SelectMany(t => t.Modifiers))
            {

            }
            foreach (TraitModifierModel modifier in SpellTraits.SelectMany(c => c.Traits).SelectMany(t => t.Modifiers))
            {

            }
        }

        #endregion Calculation

        #endregion Traits

        #region Waffen

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

        #region Initialization

        public void CreateWeapons()
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
            }
            Weapons = l_weapons;
        }

        public void CreateSelectedWeapons()
        {
            SelectedWeapons = new ObservableCollection<WeaponSelectModel>();
            foreach (WeaponModel w in Weapons.Where(x => x.Position != 0))
            {
                WeaponSelectModel ws = new WeaponSelectModel { Weapon = w };
                SelectedWeapons.Add(ws);
                ws.PropertyChanged += SelectedWeapon_PropertyChanged;
            }



        }

        public void CreateMeleeWeapons()
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

        public void CreateRangedWeapons()
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

        #endregion Initialization

        #region Events

        public void SelectedWeapon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Enumerable.Range(1, SelectedWeapons.Count).Except(SelectedWeapons.Select(x => x.Position)).FirstOrDefault();

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
            foreach(WeaponSelectModel selectedweapon in SelectedWeapons.Where(s => s.Weapon == weapon))
            {
                selectedweapon.Weapon = weapon;
            }

        }


        #endregion Calculation

        #endregion Waffen

        #region Rüstung und Zweithand

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

        #region Initialization

        public void CreateArmor()
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

        public void CreateOffHands()
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

        #endregion Initialization

        #endregion Rüstung und Zweithand

        #region Zauber und Rituale

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

        #region Initialization

        public void CreateSpells()
        {
            ObservableCollection<SpellModel> l_spells = new ObservableCollection<SpellModel>();
            foreach (DataRow row in Data.Tables["Spells"].Rows)
            {

                SpellModel spell = new SpellModel
                {
                    Name = row["Name"].ToString(),
                    Type = row["Type"].ToString(),
                    Requirement = row["Requirement"].ToString(),
                    Value = int.Parse(row["Value"].ToString()),
                    Damage = row["Damage"].ToString(),
                    MagicDamage = row["MagicDamage"].ToString(),
                    ArmorPenetration = row["ArmorPenetration"].ToString(),
                    Impulse = row["Impulse"].ToString(),
                    Range = row["Range"].ToString(),
                    Duration = row["Duration"].ToString(),
                    FlavorText = row["FlavorText"].ToString(),
                };


                l_spells.Add(spell);
            }
            Spells = l_spells;
        }

        public void CreateRituals()
        {
            ObservableCollection<RitualModel> l_rituals = new ObservableCollection<RitualModel>();
            foreach (DataRow row in Data.Tables["Rituals"].Rows)
            {

                RitualModel ritual = new RitualModel
                {
                    Name = row["Name"].ToString(),
                    Type = row["Type"].ToString(),
                    Requirement = row["Requirement"].ToString(),
                    Value = int.Parse(row["Value"].ToString()),
                    Duration = row["Duration"].ToString(),
                    FlavorText = row["FlavorText"].ToString(),

                };


                l_rituals.Add(ritual);
            }
            Rituals = l_rituals;
        }

        #endregion Initialization

        #endregion Zauber und Rituale

        #region Inventar

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

        #endregion Properties

        #region Initialization

        public void CreateInventory()
        {
            List<InventoryItemModel> l_inventory = new List<InventoryItemModel>();
            foreach (DataRow row in Data.Tables["Inventory"].Rows)
            {
                InventoryItemModel item = new InventoryItemModel
                {
                    Name = row["Name"].ToString(),
                    Quantity = double.Parse(row["Quantity"].ToString()),
                    Value = double.Parse(row["Value"].ToString()),
                    Weight = double.Parse(row["Weight"].ToString()),
                    Place = row["Place"].ToString(),
                };
                l_inventory.Add(item);
            }
            InventoryLeft = new ObservableCollection<InventoryItemModel>(l_inventory.GetRange(0, l_inventory.Count / 2));
            InventoryRight = new ObservableCollection<InventoryItemModel>(l_inventory.GetRange(l_inventory.Count / 2, l_inventory.Count / 2));
        }

        #endregion Initialization

        #endregion Inventar


    }
}
