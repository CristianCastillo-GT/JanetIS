﻿using System.Data;
using System.Reflection;

namespace CanellaMovilBackend.Static.Utils
{
    /// <summary>
    /// Clase Generica para Conviertir una lista en un DataTable para SQL Sever
    /// </summary>
    public class DataTableConvert
    {
        /// <summary>
        /// Convierte una lista en un DataTable para SQL Sever
        /// </summary>
        public static DataTable ToListConvertToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null) ?? "";
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
