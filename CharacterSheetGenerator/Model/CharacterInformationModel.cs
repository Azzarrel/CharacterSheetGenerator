using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator
{
    public class CharacterInformationModel : NotifyBase
    {

        public string FirstElement
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                if (value != null && value != "")
                {
                    FirstElementWidth = 120;
                    FirstValueWidth = 480;
                }
            }
        }

        public string FirstValue
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public double FirstElementWidth
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double FirstValueWidth
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public string SecondElement
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                if (value != null && value != "")
                {
                    FirstElementWidth = 120;
                    FirstValueWidth = 180;
                    SecondElementWidth = 100;
                    SecondValueWidth = 195;
                }
            }
        }

        public string SecondValue
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public double SecondElementWidth
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double SecondValueWidth
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public string ThirdElement
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
                if (value != null && value != "")
                {
                    FirstElementWidth = 120;
                    FirstValueWidth = 180;
                    SecondElementWidth = 100;
                    SecondValueWidth = 45;
                    ThirdElementWidth = 100;
                    ThirdValueWidth = 45;
                }
            }
        }

        public string ThirdValue
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public double ThirdElementWidth
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double ThirdValueWidth
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

    }
}
