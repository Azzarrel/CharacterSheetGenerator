using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using CharacterSheetGenerator.Model.CombatSheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;


namespace CharacterSheetGenerator.Helpers
{
   

    public static class DataTableListConverter

    {
        /// <summary>
        /// Wandelt mithilfe von Attributen dynmaisch DataTables zu Listen um
        /// </summary>
        /// <typeparam name="T">Ein Model basierend auf dem TemplateModel</typeparam>
        /// <param name="dt">Umzuwandelnde Datenbank</param>
        /// <param name="eventHandler">Subsriben eines eigenen PropertyChanged-Event aus dem ViewModel</param>
        /// <param name="reference">Liste, die für die Umwandlung von Verweisen zu Models benötigt wird</param>
        /// <returns></returns>
        public static ObservableCollection<T> ConvertToObservableCollection<T>(DataTable dt, PropertyChangedEventHandler eventHandler = null, ObservableCollection<T> reference = null) where T : TemplateModel
        {
            return new ObservableCollection<T>(ConvertToList<T>(dt, eventHandler, reference.ToList()));

        }

        public static List<T> ConvertToList<T>(DataTable dt, PropertyChangedEventHandler eventHandler, List<T> reference) where T : TemplateModel

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
                                    //ToDO: Erweitern und testen
                                    if (property.GetType() == typeof(WeaponSelectModel))
                                    {
                                        //SelectedWeapons erhalten nur einen Verweis auf die Waffen über den Namen, der wieder in ein Model umgewandelt werden muss
                                        List<WeaponModel> weapons = reference as List<WeaponModel>;
                                        property.SetValue(objT, weapons.Where(w => w.Name == row[((ColumnNameAttribute)attribute).Name].ToString()));

                                    }
                                    //else if ()
                                    //{

                                    //}
                                    else
                                    {
                                        //Wenn keiner der Sonderfälle zutrifft, ist es wohl ein Typ, der ohne Probleme auch so umgewandelt werden kann
                                        //ToDo: Testen, ob das mit double?s auch richtig klappt
                                        property.SetValue(objT, row[((ColumnNameAttribute)attribute).Name]);
                                    }

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
