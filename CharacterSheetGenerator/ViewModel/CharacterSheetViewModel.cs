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

        public DataSet WeaponData { get; set; } = new DataSet();

        public List<AttributeModel> Attributes
        {
            get { return Get<List<AttributeModel>>(); }
            set { Set(value); }
        }

        public List<ModelObject> ModelObjects
        {
            get { return Get<List<ModelObject>>(); }
            set { Set(value); }
        }

        public List<AttributeModel> SpecialAttributes
        {
            get { return Get<List<AttributeModel>>(); }
            set { Set(value); }
        }

        #region Traits

        public List<TraitCategoryModel> Traits
        {
            get { return Get<List<TraitCategoryModel>>(); }
            set { Set(value); }
        }

        public List<TraitCategoryModel> SpellTraits
        {
            get { return Get<List<TraitCategoryModel>>(); }
            set { Set(value); }
        }

        public List<TraitCategoryModel> CombatTraits
        {
            get { return Get<List<TraitCategoryModel>>(); }
            set { Set(value); }
        }

        #endregion Traits

        public List<StatusValueModel> StatusValues
        {
            get { return Get<List<StatusValueModel>>(); }
            set { Set(value); }
        }

        public List<CharacterInformationModel> CharacterInformation
        {
            get { return Get<List<CharacterInformationModel>>(); }
            set { Set(value); }
        }

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

        #region Combat Sheet

        public List<WeaponModel> Weapons
        {
            get { return Get<List<WeaponModel>>(); }
            set { Set(value); }
        }

        public List<WeaponSelectModel> SelectedWeapons
        {
            get { return Get<List<WeaponSelectModel>>(); }
            set { Set(value); }
        }

        public List<MeleeWeaponModel> MeleeWeapons
        {
            get { return Get<List<MeleeWeaponModel>>(); }
            set { Set(value); }
        }

        public List<RangedWeaponModel> RangedWeapons
        {
            get { return Get<List<RangedWeaponModel>>(); }
            set { Set(value); }
        }

        public List<ArmorModel> Armor
        {
            get { return Get<List<ArmorModel>>(); }
            set { Set(value); }
        }

        public List<OffHandModel> OffHands
        {
            get { return Get<List<OffHandModel>>(); }
            set { Set(value); }
        }

        #endregion Combat Sheet

        public List<SpellModel> Spells
        {
            get { return Get<List<SpellModel>>(); }
            set { Set(value); }
        }

        public List<RitualModel> Rituals
        {
            get { return Get<List<RitualModel>>(); }
            set { Set(value); }
        }

        public List<InventoryItemModel> InventoryLeft
        {
            get { return Get<List<InventoryItemModel>>(); }
            set { Set(value); }
        }

        public List<InventoryItemModel> InventoryRight
        {
            get { return Get<List<InventoryItemModel>>(); }
            set { Set(value); }
        }

        #endregion Properties

        public CharacterSheetViewModel()
        {
            InitializeSettings();

        }

        private void InitializeSettings()
        {
            //Hier wird das ganze Xml-Zeugs aus dem Ordner ins DataSet geladen
            XmlReader xmlData;
            Data = new DataSet();

            DataSet l_Data = new DataSet();
            string[] files = Directory.GetFiles("Settings", "*.xml");

            foreach (string s in files)
            {
                l_Data = new DataSet();
                xmlData = XmlReader.Create(s, new XmlReaderSettings());
                l_Data.ReadXml(xmlData);
                Data.Merge(l_Data);
            }

            ModelObjects = new List<ModelObject>();

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
            CreateCommands();

            foreach (AttributeModel attr in Attributes)
            {
                CalculateModelObject(attr);
            }

            foreach (StatusValueModel stv in StatusValues)
            {
                List<AttributeModel> attributeList = new List<AttributeModel>();
                foreach (string s in stv.AttributeLinks)
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

        }
        public ICommand SaveCommand { get; private set; }
        public void SaveMethod()
        {

            DataTable tblAttributeLink = Data.Tables["SVAttributeLink"].Copy();


            Data.Clear();
            foreach(AttributeModel atr in Attributes)
            {
                Data.Tables["Attributes"].Rows.Add(atr.Name, "Base" , atr.Tag, atr.Base, atr.Color);
            }
            foreach (AttributeModel atr in SpecialAttributes)
            {
                Data.Tables["Attributes"].Rows.Add(atr.Name, "Special", atr.Tag, atr.Base, atr.Color);
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
                Data.Tables["MeleeWeapons"].Rows.Add(weapon.Name, weapon.Weapons, weapon.Damage, weapon.Impulse, weapon.ArmorPenetration, weapon.Range, weapon.Break, weapon.Ticks, weapon.AttackBonus, weapon.BlockBonus);
            }
            foreach(RangedWeaponModel weapon in RangedWeapons)
            {
                Data.Tables["RangedWeapons"].Rows.Add(weapon.Name, weapon.Weapons, weapon.Damage, weapon.Impulse, weapon.ArmorPenetration, weapon.Range, weapon.Break, weapon.Load, weapon.Ticks, weapon.AttackBonus, weapon.BlockBonus);
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
               foreach (TraitModel trait in traitcategory.Traits)
                {
                    Data.Tables["Trait"].Rows.Add(trait.Name, trait.Description, trait.Key, traitcategory.Key);
                    foreach(TraitModifierModel modifier in trait.Modifiers)
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

            string SaveName = "MyDummyCharacter";

            DirectoryInfo di = Directory.CreateDirectory(SaveName);

            foreach (DataTable tbl in Data.Tables)
            {
                XmlTextWriter writer = new XmlTextWriter(SaveName + "/" + tbl.TableName + ".xml", System.Text.Encoding.UTF8);
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


            //XmlTextWriter writer = new XmlTextWriter("Product2.xml", System.Text.Encoding.UTF8);
            //    writer.WriteStartDocument(true);
            //    writer.Formatting = Formatting.Indented;
            //    writer.Indentation = 2;
            //    writer.WriteStartElement("Table");
            //    foreach (DataTable tbl in Data.Tables)
            //    {

            //        foreach (DataRow row in tbl.Rows)
            //        {
            //            writer.WriteStartElement(tbl.TableName);
            //            foreach (DataColumn col in tbl.Columns)
            //            {
            //                writer.WriteStartElement(col.ColumnName);
            //                writer.WriteString(row[col].ToString());
            //                writer.WriteEndElement();
            //            }
            //            writer.WriteEndElement();
            //        }

            //    }
            //    writer.WriteEndDocument();
            //    writer.Close();
            MessageBox.Show("XML File created ! ");
            

        }
        public bool CanExecute()
        {
            return true; //Hier könnte eine Abfrage, ob das Command ausgeführt werden darf, stehen
        }
        #endregion Commands

        #region Initialization

        #region Attributes

        private void CreateAttributes()
        {


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
                ModelObjects.Add(attribute);
                attribute.PropertyChanged += Attribute_PropertyChanged;
            }

            Attributes = ModelObjects.OfType<AttributeModel>().ToList().Where(a => a.Special == false).ToList();

            //    return animals.Where(animal => animal is Cat)
            //          .Select(animal => (Cat)animal);


        }

        /// <summary>
        /// Spezielle Attribute sind eigentlich StatusValues, die auf einigen Seiten bei den Attributen oben stehen.
        /// </summary>
        private void CreateSpecialAttributes()
        {
            //Aktuell immer schwarz, sonst Kirsten Ritzmann!

            foreach (DataRow row in Data.Tables["Attributes"].Select("Type = 'Special'"))
            {
                AttributeModel attribute = new AttributeModel
                {
                    Name = row["Name"].ToString(),
                    Tag = row["Tag"].ToString(),
                    Base = double.Parse(row["Value"].ToString()),
                    Value = double.Parse(row["Value"].ToString()),
                    Color = new SolidColorBrush(ColorHandler.IntToColor(int.Parse(row["Color"].ToString()))),
                    Special = true,

                };
                ModelObjects.Add(attribute);
                attribute.PropertyChanged += Attribute_PropertyChanged;
            }
            SpecialAttributes = ModelObjects.OfType<AttributeModel>().ToList().Where(a => a.Special == true).ToList();
        }

        public void Attribute_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            AttributeModel a = sender as AttributeModel;

            //StatusWerte, die auf diesem Attribut basieren updaten
            foreach (StatusValueModel stv in StatusValues.Where(x => x.AttributeLinks.Any(s => s == a.Name)))
            {
                List<AttributeModel> attributeList = new List<AttributeModel>();
                foreach (string s in stv.AttributeLinks)
                {
                    attributeList.Add(Attributes.Where(x => x.Name == s).FirstOrDefault());
                }
                AttributeModel[] attributes = attributeList.ToArray();
                CalculateStatusValues(attributes, stv);
            }
            switch (e.PropertyName)
            {
                case "Value":
                    a.Base = a.Value - a.Modifiers;
                    break;
                case "Modifiers":
                    a.Value = a.Base + a.Modifiers;
                    break;
                default:
                    break;
            }
        }

        #endregion Attributes

        #region Persönliche Daten

        public void CreateCharacterInformation()
        {

            List<CharacterInformationModel> l_CharInfo = new List<CharacterInformationModel>();
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

        public void CahracterInformation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        #endregion Persönliche Daten

        #region Statuswerte


        public void CreateStatusValues()
        {

            foreach (DataRow row in Data.Tables["StatusValues"].Rows)
            {
                List<string> attributelinks = new List<string>();
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

                ModelObjects.Add(statusvalue);
                statusvalue.PropertyChanged += StatusValue_PropertyChanged;
            }
            StatusValues = ModelObjects.OfType<StatusValueModel>().ToList();
        }

        public void StatusValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            StatusValueModel stv = sender as StatusValueModel;

            switch (e.PropertyName)
            {
                case "Base":
                    List<AttributeModel> attributeList = new List<AttributeModel>();
                    foreach (string s in stv.AttributeLinks)
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
                    CalculateSpecialAttributes(stv, SpecialAttributes.Where(x => x.Name == stv.Name).FirstOrDefault());
                    break;
                default:
                    break;
            }

        }

        #endregion Statuswerte

        #region Fertigkeiten 

        private void CreateSkills()
        {

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
                ModelObjects.Add(skill);
            }

            SkillsLeft = new ListCollectionView(ModelObjects.OfType<SkillModel>().ToList().Where(s => s.Grouping == "Left").ToList());
            SkillsLeft.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            SkillsRight = new ListCollectionView(ModelObjects.OfType<SkillModel>().ToList().Where(s => s.Grouping == "Right").ToList());
            SkillsRight.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

        }

        public void Skill_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CalculateModelObject(sender as SkillModel);
        }

        #endregion Fertigkeiten    

        #region Traits

        public void CreateCharacterTraits()
        {
            List<TraitCategoryModel> l_TraitList = new List<TraitCategoryModel>();
            foreach (DataRow rowCategory in Data.Tables["TraitCategory"].Select("Type = 'Character'"))
            {
                List<TraitModel> traits = new List<TraitModel>();
                string traitTexts = "";
                foreach (DataRow rowTrait in Data.Tables["Trait"].Select("TraitCategory_Id = " + rowCategory["TraitCategory_Id"]))
                {

                    List<TraitModifierModel> modifiers = new List<TraitModifierModel>();
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
            List<TraitCategoryModel> l_TraitList = new List<TraitCategoryModel>();
            foreach (DataRow rowCategory in Data.Tables["TraitCategory"].Select("Type = 'Combat'"))
            {
                List<TraitModel> traits = new List<TraitModel>();
                string traitTexts = "";
                foreach (DataRow rowTrait in Data.Tables["Trait"].Select("TraitCategory_Id = " + rowCategory["TraitCategory_Id"]))
                {

                    List<TraitModifierModel> modifiers = new List<TraitModifierModel>();
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
            List<TraitCategoryModel> l_TraitList = new List<TraitCategoryModel>();
            foreach (DataRow rowCategory in Data.Tables["TraitCategory"].Select("Type = 'Spell'"))
            {
                List<TraitModel> traits = new List<TraitModel>();
                string traitTexts = "";
                foreach (DataRow rowTrait in Data.Tables["Trait"].Select("TraitCategory_Id = " + rowCategory["TraitCategory_Id"]))
                {

                    List<TraitModifierModel> modifiers = new List<TraitModifierModel>();
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


        public void Trait_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        #endregion Traits

        #region Waffen

        public void CreateWeapons()
        {
            List<WeaponModel> l_weapons = new List<WeaponModel>();
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
            List<WeaponSelectModel> l_SelectedWeapons = new List<WeaponSelectModel>();
            foreach (WeaponModel w in Weapons.Where(x => x.Position != 0))
            {
                WeaponSelectModel ws = new WeaponSelectModel { Weapon = w };
                l_SelectedWeapons.Add(ws);
                ws.PropertyChanged += SelectedWeapon_PropertyChanged;
            }
            SelectedWeapons = l_SelectedWeapons.OrderBy(x => x.Position).ToList();


        }

        public void SelectedWeapon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Enumerable.Range(1, SelectedWeapons.Count).Except(SelectedWeapons.Select(x => x.Position)).FirstOrDefault();

        }

        public void CreateMeleeWeapons()
        {
            List<MeleeWeaponModel> l_weapons = new List<MeleeWeaponModel>();
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
            List<RangedWeaponModel> l_weapons = new List<RangedWeaponModel>();
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

        #endregion Waffen

        #region Rüstung und Zweithand

        public void CreateArmor()
        {
            List<ArmorModel> l_armor = new List<ArmorModel>();
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
            List<OffHandModel> l_offhands = new List<OffHandModel>();
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

        #endregion Rüstung und Zweithand

        #region Zauber und Rituale

        public void CreateSpells()
        {
            List<SpellModel> l_spells = new List<SpellModel>();
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
            List<RitualModel> l_rituals = new List<RitualModel>();
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

        #endregion Zauber und Rituale

        #region Inventar

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
            InventoryLeft = l_inventory.GetRange(0, l_inventory.Count / 2);
            InventoryRight = l_inventory.GetRange(l_inventory.Count / 2, l_inventory.Count / 2);
        }

        #endregion Inventar

        #endregion Initialization

        #region Calculation

        #region Pre Init

        public void PreInitializeModelObjects(List<ModelObject> ModelList)
        {
            foreach(ModelObject model in ModelList)
            {
                if(model.GetType() == typeof(StatusValueModel))
                {

                    List<AttributeModel> attributeList = new List<AttributeModel>();
                    foreach (string s in ((StatusValueModel)model).AttributeLinks)
                    {
                        attributeList.Add(Attributes.Where(x => x.Name == s).FirstOrDefault());
                    }
                    CalculateStatusValues(attributeList.ToArray(), (StatusValueModel)model);
                }


            }
        }

        #endregion Pre Init

        #region Post Init

        #endregion Post Init 

        #region Basiswerte

        public void CalculateModelObject(ModelObject model)
        {
            model.Value = model.Base + model.Modifiers;
        }

        /// <summary>
        /// Die Berechnung wird beim  StatusValue gemacht, wir übernehmen nur die Werte
        /// </summary>
        /// <param name="StV">StatusValue</param>
        /// <param name="Att">Attribut zm StatusValue</param>
        public void CalculateSpecialAttributes(StatusValueModel StV, AttributeModel Att)
        {
            Att.Value = StV.Value;
        }

        #endregion Basiswerte

        #region Statuswerte

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

        #endregion Statuswerte

        #region Waffen

        public void CalculateWeaponAll()
        {
            foreach (WeaponModel weapon in Weapons)
            {
                CalculateWeapon(weapon);
            }
        }

        public void CalculateWeapon(WeaponModel weapon)
        {
            string[] attributes = weapon.AttributeLink.Split('/');
            double baseValues = 0;
            foreach (string s in attributes)
            {
                baseValues = Attributes.Where(x => x.Tag == s).FirstOrDefault().Value;
            }
            weapon.AttackBase = weapon.BlockBase = Math.Round(baseValues / 2 / attributes.Length, 0);
            weapon.AttackTotal = weapon.AttackBase + weapon.AttackBonus;
            weapon.BlockTotal = weapon.BlockBase + weapon.BlockBonus;
        }

        #endregion Waffen

        #region Traits
        //Todo: Geht aktuell nur auf Traits, nicht auf Combat und SpellTraits
        public void CalculateTraitModifiers()
        {

            foreach (TraitModifierModel modifier in Traits.SelectMany(c => c.Traits).SelectMany(t => t.Modifiers))
            {
                foreach (ModelObject model in ModelObjects.Where(m => m.Name == modifier.NameLink))
                {
                    model.Modifiers = Math.Round(model.Modifiers + modifier.Value, 0);
                }
                //Todo: Gleiches für alle Weapons machen
            }
        }




        #endregion Traits


        #endregion Calculation

    }
}
