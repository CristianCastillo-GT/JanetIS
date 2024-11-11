using System.ComponentModel.DataAnnotations;

namespace CanellaMovilBackend.Models.ArchivosLaserFiche
{
    /// <summary>
    /// Modelo de consulta para obtener los archivos de SAP
    /// </summary>
    public class DataArchivos
    {
        /// <summary>
        /// DocNum - Codigo de Documento
        /// </summary>
        public string DocNum { get; set; } = string.Empty;
        /// <summary>
        /// ObjType - Tipe de Documento
        /// </summary>
        public string ObjType { get; set; } = string.Empty;
        /// <summary>
        /// FileName - Nombre de Archivo
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        /// <summary>
        /// INPath - Path Origen
        /// </summary>
        public string INPath { get; set; } = string.Empty;
        /// <summary>
        /// OutPath - Path Destino
        /// </summary>
        public string OUTPath { get; set; } = string.Empty;
    }
}
