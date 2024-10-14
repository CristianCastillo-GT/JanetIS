using System.Data;
using System.Reflection;

namespace CanellaMovilBackend.Utils
{
    /// <summary>
    /// Convierte un datatable en una clase
    /// </summary>
    public class ObjectConvert
    {
        /// <summary>
        /// Obtiene el datatable y lo convierte en un listado del objecto
        /// </summary>
        public static List<T> CreateListFromDataTable<T>(DataTable tbl) where T : new()
        {
            List<T> lst = [];

            foreach (DataRow r in tbl.Rows)
            {
                lst.Add(CreateItemFromRow<T>(r));
            }
            return lst;
        }
        /// <summary>
        /// Obtiene el datarow y lo inserta
        /// </summary>
        private static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            T item = new();
            SetItemFromRow(item, row);
            return item;
        }

        /// <summary>
        /// Obtiene el item y lo ingresa al listado
        /// </summary>
        private static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            foreach (DataColumn c in row.Table.Columns)
            {
                PropertyInfo? p = item?.GetType().GetProperty(c.ColumnName);
                if (p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, row[c], null);
                }
            }
        }
    }
}
