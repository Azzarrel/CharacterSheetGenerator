using CharacterSheetGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CharacterSheetGenerator.Model    
{
    public class SaveDataModel : TemplateModel
    {

        [ColumnName("Version")]
        public string Version
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("SaveName")]
        public string SaveName
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("CharacterName")]
        public string CharacterName
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        [ColumnName("Exp")]
        public double Expieriece
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        [ColumnName("LastModified")]
        public DateTime LastModified
        {
            get { return Get<DateTime>(); }
            set { Set(value); }
        }
    }

}
