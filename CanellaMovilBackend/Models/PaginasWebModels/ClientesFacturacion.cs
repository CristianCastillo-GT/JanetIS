namespace CanellaMovilBackend.Models.PaginasWebModels
{
    /// <summary>
    /// Modelo de consulta para obtener informaciion de clientes y cantidad facturada SAP canella y maquipos 
    /// </summary>
    public class ClientesFacturacion
    {
        /// <summary>
        /// Empresa - Empresa de SAP
        /// </summary>
        public string Empresa { get; set; } = string.Empty;
        /// <summary>
        /// CardCode - Codigo de cliente
        /// </summary>
        public string CardCode { get; set; } = string.Empty;
        /// <summary>
        /// CardName - Nombre de cliente
        /// </summary>
        public string CardName { get; set; } = string.Empty;
        /// <summary>
        /// CardFName - Nombre comercial de cliente
        /// </summary>
        public string CardFName { get; set; } = string.Empty;
        /// <summary>
        /// addid - Nit del cliente
        /// </summary>
        public string addid { get; set; } = string.Empty;
        /// <summary>
        /// phone1 - Telefono del cliente
        /// </summary>
        public string phone1 { get; set; } = string.Empty;
        /// <summary>
        /// e_mail - Correo del cliente
        /// </summary>
        public string e_mail { get; set; } = string.Empty;
        /// <summary>
        /// street - Direccion del cliente
        /// </summary>
        public string street { get; set; } = string.Empty;
        /// <summary>
        /// county - Departamento
        /// </summary>
        public string county { get; set; } = string.Empty;
        /// <summary>
        /// country - Municipio
        /// </summary>
        public string country { get; set; } = string.Empty;
        /// <summary>
        /// Facturacion - Facturacion del ultimo mes en SAP empresa
        /// </summary>
        public string Facturacion { get; set; } = string.Empty;
    }
}
