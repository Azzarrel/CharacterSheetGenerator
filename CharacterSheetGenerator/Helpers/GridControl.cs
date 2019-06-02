using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CharacterSheetGenerator.Helpers
{
    class GridControl : DataGrid
    {
        public GridControl()
        {
            KeyDown += DeleteKeydown;
        }
        public void DeleteKeydown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
               if(SelectedCells.Count != 0)
                {
                    foreach(DataGridCellInfo cell in SelectedCells)
                    {
                        DataGridColumn column = cell.Column;
                        string propertyName = ((Binding)column.ClipboardContentBinding).Path.Path;

                        PropertyInfo pi = cell.Item.GetType().GetProperty(propertyName);
                        if (pi != null)
                        {
                            pi.SetValue(cell.Item, null, null);
                        }
                    }
                }
            }
        }
        
    }
}
