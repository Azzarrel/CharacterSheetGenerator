using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator 
{
    public class SkillModel : ModelObject
    {


        public string Requirement
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        //public int Value
        //{
        //    get { return Get<int>(); }
        //    set
        //    {
        //        Set(value);
        //        SetRoutine(value);
        //    }
        //}

        public string Difficulty
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Routine
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Comment
        {
            get { return Get<string>(); }
            set { Set(value); }
        }
        public string Category
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public void SetRoutine(int value)
        {
            Routine = "";
            if (value > 6)
                Routine = "r";

            if (value > 10)
                Routine = "g";

            if (value > 14)
                Routine = "m";
            
        }
    }

}
