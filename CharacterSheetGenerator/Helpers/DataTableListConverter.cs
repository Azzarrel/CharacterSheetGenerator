using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;


namespace CharacterSheetGenerator
{
   

    public static class DataTableListConverter

    {

        public static ObservableCollection<T> ConvertToObservableCollection<T>(DataTable dt, PropertyChangedEventHandler eventHandler) where T : TemplateModel
        {
            return new ObservableCollection<T>(ConvertToList<T>(dt, eventHandler));

        }

        public static List<T> ConvertToList<T>(DataTable dt, PropertyChangedEventHandler eventHandler) where T : TemplateModel

        {
            // gibt die Namen aller Columns eines DataTable zurück
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();

            //gibt alle Properties, die T besitzt zurück
            var properties = typeof(T).GetProperties();

            //wandelt rows in objekte i,
            return dt.AsEnumerable().Select(row => {
                //Erstellt ein neues Objekt
                var objT = Activator.CreateInstance<T>();

                foreach (var property in properties)

                {
                    //Schaut per Attribut welche Column mit dem aktuellen Property des Objekts verlinkt werden soll
                    foreach (var attribute in property.GetCustomAttributes(false))

                    {

                        if (attribute.GetType() == typeof(ColumnNameAttribute))

                        {

                            if (columnNames.Contains(((ColumnNameAttribute)attribute).Name.ToLower()))

                            {

                                try

                                {
                                    //ToDo: Switch case, um die einzelnen Datentypen entsprechend umzuwandeln?
                                    property.SetValue(objT, row[((ColumnNameAttribute)attribute).Name]);

                                }

                                catch (Exception e) { }

                            }



                        }

                    }

                }

                objT.PropertyChanged += eventHandler;
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
