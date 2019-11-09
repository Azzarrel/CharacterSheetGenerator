using CharacterSheetGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace CharacterSheetGenerator.Model
{
    public class SkillModel : TemplateModel
    {
        public BitmapImage Icon
        {
            get
            {
                Dictionary<String, String> Images = new Dictionary<String, String>() {
                    { "Klettern", @"Images\Skills\climb.png" },
                    {"Akrobatik", @"Images\Skills\acrobatic.png" },
                    {"Schwimmen", @"Images\Skills\swim.png" },
                    {"Reiten", @"Images\Skills\riding.png" },
                    {"Schleichen", @"Images\Skills\sneak.png" },
                    {"Kraftakt", @"Images\Skills\biceps.png" },
                    {"Körperbeherrschung", @"Images\Skills\body_balance.png" },
                    {"Selbstbeherrschung", @"Images\Skills\button_finger.png" },
                    {"Stimmen immitieren", @"Images\Skills\sound_waves.png" },
                    {"Tanzen", @"Images\Skills\sensuousness.png" },
                    {"Singen", @"Images\Skills\sing.png" },
                    {"Sicht", @"Images\Skills\brass_eye.png" },
                    {"Gehör", @"Images\Skills\ear.png" },
                    {"Gefühl", @"Images\Skills\hand.png" },
                    {"Geruch u. Geschmack", @"Images\Skills\nose_side.png" },
                    {"Zechen", @"Images\Skills\carouse.png" },
                    {"Bekehren u. Überzeugen", @"Images\Skills\prayer.png" },
                    {"Betören", @"Images\Skills\lipstick.png" },
                    {"Einschüchtern", @"Images\Skills\muscle_up.png" },
                    {"Etikette", @"Images\Skills\jeweled_chalice.png" },
                    {"Gassenwissen", @"Images\Skills\potion_of_madness.png" },
                    {"Menschenkenntnis", @"Images\Skills\person.png" },
                    {"Überreden", @"Images\Skills\convince.png" },
                    {"Verkleiden", @"Images\Skills\ceremonial_mask.png" },
                    {"Täuschen", @"Images\Skills\duality_mask.png" },
                    {"Tarnen", @"Images\Skills\domino_mask.png" },
                    {"Taschendiebstahl", @"Images\Skills\snatch.png" },
                    {"Schauspielerei", @"Images\Skills\drama_masks.png" },
                    {"Unterrichten", @"Images\Skills\teacher.png" },
                    {"Tierkunde", @"Images\Skills\cat.png" },
                    {"Plfanzenkunde", @"Images\Skills\coral.png" },
                    {"Fährtensuchen", @"Images\Skills\velociraptor_tracks.png" },
                    {"Orientierung", @"Images\Skills\compass.png" },
                    {"Wettervorhersagen", @"Images\Skills\thermometer_cold.png" },
                    {"Fesseln", @"Images\Skills\manacles.png" },
                    {"Fallenstellen", @"Images\Skills\wolf_trap.png" },
                    {"Fischen u. Angeln", @"Images\Skills\fishing_pole.png" },
                    {"Brett u. Glücksspiel", @"Images\Skills\conway_life_glider.png" },
                    {"Geschichtswissen", @"Images\Skills\book_pile.png" },
                    {"Götter u. Kulte", @"Images\Skills\hand_of_god.png" },
                    {"Sagen u. Legenden", @"Images\Skills\dragon_spiral.png" },
                    {"Rechtskunde", @"Images\Skills\gavel.png" },
                    {"Staatskunst", @"Images\Skills\wax_seal.png" },
                    {"Magiekunde", @"Images\Skills\magic_swirl.png" },
                    {"Kriegskunst", @"Images\Skills\rally_the_troops.png" },
                    {"Rechnen", @"Images\Skills\abacus.png" },
                    {"Sternkunde", @"Images\Skills\solar_system.png" },
                    {"Geografie", @"Images\Skills\globe.png" },
                    {"Geologie", @"Images\Skills\crystal_growth.png" },
                    {"Mechanik", @"Images\Skills\gears.png" },
                    {"Expertise", @"Images\Skills\gift_of_knowledge.png" },
                    {"Anatomie", @"Images\Skills\anatomy.png" },
                    {"Kartografie", @"Images\Skills\treasure_map.png" },
                    {"Kryptografie", @"Images\Skills\abstract_114.png" },
                    {"Alchemie", @"Images\Skills\bottle_vapors.png" },
                    {"Agrarwirtschaft", @"Images\Skills\scythe.png" },
                    {"Brauerei", @"Images\Skills\barrel.png" },
                    {"Bergbau", @"Images\Skills\miner.png" },
                    {"Handel", @"Images\Skills\trade.png" },
                    {"Heilkunde: Körper", @"Images\Skills\healing.png" },
                    {"Heilkunde: Seele", @"Images\Skills\meditation.png" },
                    {"Heilkunde: Wunden", @"Images\Skills\arm_bandage.png" },
                    {"Holzbearbeitung", @"Images\Skills\chisel.png" },
                    {"Kochen", @"Images\Skills\cook.png" },
                    {"Lederbearbeitung", @"Images\Skills\animal_hide.png" },
                    {"Malen u. Zeichnen", @"Images\Skills\mona_lisa.png" },
                    {"Musizieren", @"Images\Skills\ocarina.png" },
                    {"Schmiedekunst", @"Images\Skills\anvil_impact.png" },
                    {"Schlossknacken", @"Images\Skills\lockpicks.png" },
                    {"Seefahrt", @"Images\Skills\ship_wheel.png" },
                    {"Steinmetzkunst", @"Images\Skills\stone_bust.png" },
                    {"Stoffbearbeitung", @"Images\Skills\rolled_cloth.png" },
                    {"Baukunst", @"Images\Skills\stick_frame.png" },
                    {"Büchsenmacherei", @"Images\Skills\blunderbuss.png" },
                    {"Töpfern", @"Images\Skills\painted_pottery.png" },
                    {"Bogenbau", @"Images\Skills\high_shot.png" }
                };
                if (Images.ContainsKey(Name))
                    return new BitmapImage(new Uri(@"pack://application:,,,/"
                        + Assembly.GetExecutingAssembly().GetName().Name
                        + ";component/"
                        + Images[Name], UriKind.Absolute));
                return null;
            }
        }


        [ColumnName("ID")]
        public string Identifier
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [LangColumnName("Name_ger", "Name_ger")]
        public string Name
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public double Modifiers
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        [ColumnName("Key")]
        public int Key
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

        [ColumnName("Requirement")]
        public string Requirement
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public double? Base
        {
            get { return Get<double?>(); }
            set { Set(value); }
        }

        [ColumnName("Value")]
        public double? Level
        {
            get { return Get<double?>(); }
            set
            {
                Set(value);
                SetRoutine(value);
            }
        }
        public double? Value
        {
            get { return Get<double?>(); }
            set
            {
                Set(value);
                SetToolTip(value);
            }
        }

        public string Mean
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Difficulty")]
        public string Difficulty
        {
            get { return Get<string>(); }
            set { Set(value); }
        }


        [ColumnName("Deployability")]
        public string Deployability
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Routine
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Comment")]
        public string Comment
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Category")]
        public string Category
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Grouping")]
        public string Grouping
        {
            get { return Get<string>(); }
            set { Set(value); }
        }
        public string ToolTip
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public void SetRoutine(double? value)
        {
            Routine = "";
            if (value >= 4)
                Routine = "e";

            if (value >= 7)
                Routine = "r";

            if (value >= 10)
                Routine = "g";

            if (value >= 13)
                Routine = "m";

            if (value >= 16)
                Routine = "l";

        }
        public void SetToolTip(double? value)
        {
            ToolTip = "Gesamt: " + Value + "\n" + "Grundwert: " + Base + "\n" + "Modifier: " + Modifiers + "\n" + "Steigerung: " + Level;
        }
    }

}
