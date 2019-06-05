using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using static CharacterSheetGenerator.Helpers.Attributes;

namespace CharacterSheetGenerator
{

    public static class DataTableListConverter

    {

        public static ObservableCollection<T> ConvertToObservableCollection<T>(DataTable dt)
        {
            return new ObservableCollection<T>(ConvertToList<T>(dt));
        }

        public static List<T> ConvertToList<T>(DataTable dt)

        {

            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();

            var properties = typeof(T).GetProperties();

            return dt.AsEnumerable().Select(row => {

                var objT = Activator.CreateInstance<T>();

                foreach (var property in properties)

                {

                    foreach (var attribute in property.GetCustomAttributes(false))

                    {

                        if (attribute.GetType() == typeof(ColumnNameAttribute))

                        {

                            if (columnNames.Contains(((ColumnNameAttribute)attribute).Name.ToLower()))

                            {

                                try

                                {

                                    property.SetValue(objT, row[((ColumnNameAttribute)attribute).Name]);

                                }

                                catch (Exception e) { }

                            }



                        }

                    }

                }

                return objT;

            }).ToList();

        }



        public static DataTable CreateDataTable<T>(IEnumerable<T> list)

        {

            Type type = typeof(T);

            var properties = type.GetProperties();



            DataTable dataTable = new DataTable();

            foreach (var property in properties)

            {

                foreach (var attribute in property.GetCustomAttributes(false))

                {

                    if (attribute.GetType() == typeof(ColumnNameAttribute))

                    {



                        dataTable.Columns.Add(new DataColumn(((ColumnNameAttribute)attribute).Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType));

                    }

                }

            }



            foreach (T entity in list)

            {

                object[] values = new object[properties.Length];

                for (int i = 0; i < properties.Length; i++)

                {

                    values[i] = properties[i].GetValue(entity);

                }



                dataTable.Rows.Add(values);

            }



            return dataTable;

        }



    }

}
