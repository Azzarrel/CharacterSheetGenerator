using CharacterSheetGenerator.Helpers;
using CharacterSheetGenerator.Model;
using CharacterSheetGenerator.Model.CombatSheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Media;

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
        public static ObservableCollection<T> ConvertToObservableCollection<T>(DataTable dt, PropertyChangedEventHandler eventHandler = null, DataTable reference = null) where T : TemplateModel
        {
            return new ObservableCollection<T>(ConvertToList<T>(dt, eventHandler, reference));

        }

        public static List<T> ConvertToList<T>(DataTable dt, PropertyChangedEventHandler eventHandler, DataTable reference = null) where T : TemplateModel

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
                            if (columnNames.Contains(((ColumnNameAttribute)attribute).Name.ToLower()) &&  row[((ColumnNameAttribute)attribute).Name].GetType() != typeof(DBNull))
                            {
                                try
                                {                               
                                    if (property.PropertyType == typeof(double))
                                    {
                                        property.SetValue(objT, double.Parse(row[((ColumnNameAttribute)attribute).Name].ToString()));
                                    }
                                    else if (property.PropertyType == typeof(double?))
                                    {
                                        property.SetValue(objT, Parser.ToNullable<double>(row[((ColumnNameAttribute)attribute).Name].ToString()));
                                    }
                                    else if (property.PropertyType == typeof(SolidColorBrush))
                                    {
                                        property.SetValue(objT, new SolidColorBrush(ColorHandler.IntToColor(int.Parse(row["Color"].ToString()))));
                                    }                                   
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

                    //Speicalattributes (also StatusValues als Attribute) werden im Moment noch ein wenig kompliziert gehandhabt
                    if (property.Name == "Special")
                    {
                        if (row["Type"].ToString() == "Base")
                            property.SetValue(objT, false);
                        else
                            property.SetValue(objT, true);

                    }
                    //Ebenfalls noch ein wenig awkward eingebunden
                    else if (property.Name == "AttributeLinks")
                    {
                        ObservableCollection<string> attributelinks = new ObservableCollection<string>();
                        foreach (DataRow attribtuelink in reference.Select("StatusValues_Id = " + row["StatusValues_Id"]))
                        {
                            attributelinks.Add(attribtuelink["SVAttributeLink_Text"].ToString());
                        }
                        property.SetValue(objT, attributelinks);
                    }

                }

                objT.PropertyChanged += eventHandler;
                return objT;

            }).ToList();

        }


        /// <summary>
        /// Wandelt mithilfe von Attributen dynmaisch DataTables zu Listen um
        /// </summary>
        /// <typeparam name="T">Ein Model basierend auf dem TemplateModel</typeparam>
        /// <param name="dt">Umzuwandelnde Datenbank</param>
        /// <param name="eventHandler">Subsriben eines eigenen PropertyChanged-Event aus dem ViewModel</param>
        /// <param name="reference1">Liste, die für die Umwandlung von Verweisen zu Models benötigt wird</param>
        /// <returns></returns>
        public static ObservableCollection<T> ConvertToObservableCollection<T, R>(DataTable dt, PropertyChangedEventHandler eventHandler = null, ObservableCollection<R> reference = null) where T : TemplateModel
        {
            return new ObservableCollection<T>(ConvertToList<T, R>(dt, eventHandler, reference.ToList()));

        }

        public static List<T> ConvertToList<T, R>(DataTable dt, PropertyChangedEventHandler eventHandler, List<R> reference) where T : TemplateModel

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
                                    if (property.PropertyType == typeof(WeaponModel))
                                    {
                                        //SelectedWeapons erhalten nur einen Verweis auf die Waffen über den Namen, der wieder in ein Model umgewandelt werden muss
                                        List<WeaponModel> weapons = reference as List<WeaponModel>;
                                        property.SetValue(objT, weapons.Where(w => w.Name == row[((ColumnNameAttribute)attribute).Name].ToString()).FirstOrDefault());

                                    }
                                    else if (property.PropertyType == typeof(double))
                                    {
                                        property.SetValue(objT, double.Parse(row[((ColumnNameAttribute)attribute).Name].ToString()));
                                    }
                                    else if (property.PropertyType == typeof(double?))
                                    {
                                        property.SetValue(objT, Parser.ToNullable<double>(row[((ColumnNameAttribute)attribute).Name].ToString()));
                                    }
                                    else if (property.PropertyType == typeof(SolidColorBrush))
                                    {
                                        property.SetValue(objT, new SolidColorBrush(ColorHandler.IntToColor(int.Parse(row["Color"].ToString()))));
                                    }
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
                        //ToDO: Erweitern und testen
                        if (property.GetType() == typeof(WeaponModel))
                        {
                            //Bei Waffen wird der Name gespeichert, deswegen string
                            dataTable.Columns.Add(new DataColumn(((ColumnNameAttribute)attribute).Name, typeof(string)) );

                        }
                        //else if ()
                        //{

                        //}
                        else
                        {
                            //Wenn keiner der Sonderfälle zutrifft, ist es wohl ein Typ, der ohne Probleme auch so umgewandelt werden kann
                            //ToDo: Testen, ob das mit double?s auch richtig klappt
                            dataTable.Columns.Add(new DataColumn(((ColumnNameAttribute)attribute).Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType));
                        }

                    }

                }

            }



            foreach (T entity in list)

            {

                object[] values = new object[properties.Length];

                for (int i = 0; i < properties.Length; i++)

                {
                    if (properties[i].GetType() == typeof(WeaponModel))
                    {
                        //Bei Waffen wird der Name gespeichert, deswegen string
                        WeaponModel m = properties[i].GetValue(entity) as WeaponModel;
                        values[i] = m.Name;

                    }
                    //else if ()
                    //{

                    //}
                    else
                    {
                        //Wenn keiner der Sonderfälle zutrifft, ist es wohl ein Typ, der ohne Probleme auch so umgewandelt werden kann
                        //ToDo: Testen, ob das mit double?s auch richtig klappt
                        values[i] = properties[i].GetValue(entity);
                    }
                    values[i] = properties[i].GetValue(entity);

                }



                dataTable.Rows.Add(values);

            }



            return dataTable;

        }



    }

}
