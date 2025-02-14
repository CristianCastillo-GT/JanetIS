namespace CanellaMovilBackend.Models.SAPModels.Inventory
{
    public class OITMAF
    {
        public int NoRegistro { get; set; }
        /// <summary>
        /// Nombre del Producto
        /// </summary>
        public string ItemName { get; set; } = string.Empty;
        ///<summary>
        ///Serie Tipo Activo Fijo
        ///</summary>
        public string Series { get; set; } = string.Empty;
        ///<summary>
        ///Nombre extranjero
        ///</summary>
        public string FrgnName { get; set; } = string.Empty;
        ///<summary>
        ///Tipo de articulo 
        ///</summary>
        public string ItemType { get; set; } = string.Empty;
        ///<summary>
        ///Grupo de articulo
        ///</summary>
        public string GrpCod { get; set; } = string.Empty;
        ///<summary>
        ///Clase activo fijo
        ///</summary>
        public string AssetesClass { get; set; } = string.Empty;
        ///<summary>
        ///Grupo de activo fijo
        ///</summary>
        public string AssetsGroup { get; set; } = string.Empty;
        ///<summary>
        ///Serie del producto
        ///</summary>
        public string AssetsNoSer { get; set; } = string.Empty;
        ///<summary>
        ///Empleado
        ///</summary>
        public string Employee { get; set; } = string.Empty;
        ///<summary>
        ///Valido de
        ///</summary>
        public string ValidForm { get; set; } = string.Empty;
        ///<summary>
        ///Centro de costo
        ///</summary>
        public string OcrCode { get; set; } = string.Empty;
        ///<summary>
        ///Contrato
        ///</summary>
        public string U_NoContrato { get; set; } = string.Empty;
        ///<summary>
        ///attriTxt1/ Clase vehiculo
        ///</summary>
        public string AttriTxt1 { get; set; } = string.Empty;
        ///<summary>
        ///attriTxt2/ Marca
        ///</summary>
        public string AttriTxt2 { get; set; } = string.Empty;
        ///<summary>
        ///attriTxt3/ Modelo
        ///</summary>
        public string AttriTxt3 { get; set; } = string.Empty;
        ///<summary>
        ///attriTxt4/ Placa
        ///</summary>
        public string AttriTxt4 { get; set; } = string.Empty;
        ///<summary>
        ///attriTxt5/ Combustible
        ///</summary>
        public string AttriTxt5 { get; set; } = string.Empty;
        ///<summary>
        ///attriTxt6/ Color
        ///</summary>
        public string AttriTxt6 { get; set; } = string.Empty;
        ///<summary>
        ///attriTxt7/ NumeroMotor
        ///</summary>
        public string AttriTxt7 { get; set; } = string.Empty;
        ///<summary>
        ///attriTxt8/ EsatdoActual
        ///</summary>
        public string AttriTxt8 { get; set; } = string.Empty;
        ///<summary>
        ///attriTxt9/ UsoPrincipal
        ///</summary>
        public string AttriTxt9 { get; set; } = string.Empty;
        ///<summary>
        ///attriTxt10/ EstadoSAT
        ///</summary>
        public string AttriTxt10 { get; set; } = string.Empty;
        ///<summary>
        ///attriTxt11/ TipoSeguro
        ///</summary>
        public string AttriTxt11 { get; set; } = string.Empty;
    }
}
