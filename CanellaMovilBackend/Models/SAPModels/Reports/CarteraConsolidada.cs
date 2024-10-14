namespace CanellaMovilBackend.Models.SAPModels.Reports
{
    /// <summary>
    ///  Modelos de la tabla OSLP SAP 
    /// </summary>
    public class CarteraConsolidada
    {
        /// <summary>
        /// DocEntry - Codigo que se obtiene de sap
        /// </summary>
        public string DocEntry { get; set; } = string.Empty;

        /// <summary>
        /// DocNum - Codigo que se obtiene de sap
        /// </summary>
        public string DocNum { get; set; } = string.Empty;

        /// <summary>
        /// TransI - Codigo que se obtiene de sap
        /// </summary>
        public string TransID { get; set; } = string.Empty;

        /// <summary>
        /// CodVendedor - Codigo de Vendedor
        /// </summary>
        public string CodVendedor { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del Vendedor
        /// </summary>
        public string NomVendedor { get; set; } = string.Empty;

        /// <summary>
        /// FechaFacturacion - Fecha en que se facturó
        /// </summary>

        public string FechaFacturacion { get; set; } = string.Empty;

        /// <summary>
        /// FechaVence - Fecha que vence la factura
        /// </summary>
        public string FechaVence { get; set; } = string.Empty;

        /// <summary>
        /// Numero de pago en cuotas 
        /// </summary>
        public string PagoNumero { get; set; } = string.Empty;

        /// <summary>
        /// FacturaSerie 
        /// </summary>
        public string FacturaSerie { get; set; } = string.Empty;

        /// <summary>
        /// Numero de factura 
        /// </summary>
        public string FacturaNumero { get; set; } = string.Empty;

        /// <summary>
        /// Factura con el numero de pagos a aplicar
        /// </summary>
        public string NumeroDocumento { get; set; } = string.Empty;

        /// <summary>
        /// CentroCosto - Lugar de costo que pertenece el cobro
        /// </summary>
        public string CentroCosto { get; set; } = string.Empty;

        /// <summary>
        /// TipoDocumento - Factura o nota de debito
        /// </summary>
        public string TipoDocumento { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de pago 
        /// </summary>
        public string TipoPago { get; set; } = string.Empty;

        /// <summary>
        /// ClienteCodigo - Id del cliente
        /// </summary>
        public string ClienteCodigo { get; set; } = string.Empty;

        /// <summary>
        /// Cliente nombre
        /// </summary>
        public string ClienteNombre { get; set; } = string.Empty;

        /// <summary>
        /// grupo ciente al que pertenece
        /// </summary>
        public string GrupoCliente { get; set; } = string.Empty;

        /// <summary>
        /// Direccion Fiscal del cliente
        /// </summary>
        public string DireccionFISCAL { get; set; } = string.Empty;

        /// <summary>
        /// Codigo de cobrador asignado
        /// </summary>
        public string CobradorCodigo { get; set; } = string.Empty;

        /// <summary>
        /// TipoDocumento - Factura o nota de debito
        /// </summary>
        public string CobradorNombre { get; set; } = string.Empty;

        /// <summary>
        /// Total acumulativo de facturas
        /// </summary>
        public string TotalFactura { get; set; } = string.Empty;

        /// <summary>
        /// Total de la cuota especificada por factura
        /// </summary>
        public string TotalCuota { get; set; } = string.Empty;

        /// <summary>
        /// Total del monto pagado por el usuario
        /// </summary>
        public string TotalPagado { get; set; } = string.Empty;

        /// <summary>
        /// Total pagado por cuota especificada por factura
        /// </summary>
        public string TotalPagadoxCuota { get; set; } = string.Empty;

        /// <summary>
        /// Saldo pendiente sumado del conujunto en los interbalos de saldos
        /// </summary>
        public string TotalSaldo { get; set; } = string.Empty;

        /// <summary>
        /// Saldo pendiente entre 0 a 30 dias
        /// </summary>
        public string SALDO_0_30 { get; set; } = string.Empty;

        /// <summary>
        /// Saldo pendiente entre 31 a 60 dias
        /// </summary>
        public string SALDO_31_60 { get; set; } = string.Empty;

        /// <summary>
        /// Saldo pendiente entre 61 a 90 dias
        /// </summary>
        public string SALDO_61_90 { get; set; } = string.Empty;

        /// <summary>
        /// Saldo pendiente entre 91 a 120 dias
        /// </summary>
        public string SALDO_91_120 { get; set; } = string.Empty;

        /// <summary>
        /// Saldo pendiente a 120 dias
        /// </summary>
        public string SALDO_120_ { get; set; } = string.Empty;

        /// <summary>
        /// El saldo al dia 
        /// </summary>
        public string AL_DIA { get; set; } = string.Empty;

        /// <summary>
        /// La mora del cliente
        /// </summary>
        public string MORA { get; set; } = string.Empty;

        /// <summary>
        /// Estado de las acciones
        /// </summary>
        public string Estado { get; set; } = string.Empty;

    }
}
