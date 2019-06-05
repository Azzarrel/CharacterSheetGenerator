using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator 
{
    public class SaveDataModel : NotifyBase
    {


        public string Version
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string SaveName
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string CharacterName
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public double Expieriece
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public DateTime LastModified
        {
            get { return Get<DateTime>(); }
            set { Set(value); }
        }
    }

}
