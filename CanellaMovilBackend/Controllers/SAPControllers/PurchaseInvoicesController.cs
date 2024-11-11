using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.PurchaseInvoices;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;

namespace CanellaMovilBackend.Controllers.SAPControllers
{

    /// <summary>
    /// Controlador de Pagos Recibidos
    /// </summary>
    //[Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    //[ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class PurchaseInvoicesController : ControllerBase
    {

        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public PurchaseInvoicesController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Crea una Factura para Proveedores
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<MessageAPI>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CreatePurchaseInvoice(OPCH OPCH)
        {
            try
            {

                CompanyConnection companyConnection = this.sapService.SAPB1();
                Company company = companyConnection.Company;

                List<MessageAPI> messageApi = new List<MessageAPI>();

                // Obtiene el objeto de Entrada de Mercancías
                Documents oGoodsReceipt = (Documents)company.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);

                // Verificar si la Entrada de Mercancías existe
                if (!oGoodsReceipt.GetByKey(OPCH.ReceiptEntry))
                {
                    return Conflict(new { Result = "Fail", Message = "La Entrada de Mercancías no existe." });
                }

                // Crea una copia de la Entrada de Mercancías como Factura de Proveedores
                Documents oPurchaseInvoice = (Documents)company.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);

                // Configura propiedades de la Factura de Proveedores
                oPurchaseInvoice.DocObjectCode = BoObjectTypes.oPurchaseInvoices;
                oPurchaseInvoice.CardCode = oGoodsReceipt.CardCode;
                oPurchaseInvoice.DocDate = DateTime.Now;
                
                oPurchaseInvoice.DocDueDate = DateTime.Now.AddDays(30); //Vencimiento a los 30 días
                oPurchaseInvoice.TaxDate = DateTime.Now;
                oPurchaseInvoice.Series = 7;

                // Calcular el total de la factura manualmente sumando el monto de cada línea
                double totalFactura = 0.0;


                oPurchaseInvoice.UserFields.Fields.Item("U_DoctoRefNo").Value = oGoodsReceipt.UserFields.Fields.Item("U_DoctoRefNo").Value;
                oPurchaseInvoice.UserFields.Fields.Item("U_TipoCompra").Value = "L";
                oPurchaseInvoice.UserFields.Fields.Item("U_DoctoSerie").Value = OPCH.DocSerie;
                oPurchaseInvoice.UserFields.Fields.Item("U_DoctoNo").Value = OPCH.DocNum;


                oPurchaseInvoice.UserFields.Fields.Item("U_TipoPedido").Value = oGoodsReceipt.UserFields.Fields.Item("U_TipoPedido").Value;

                //Variables para manejar la condicionante para el NumAtCard
                string doctono = oPurchaseInvoice.UserFields.Fields.Item("U_DoctoNo").Value;
                string doctoserie = oPurchaseInvoice.UserFields.Fields.Item("U_DoctoSerie").Value;


                // Se asigna el NumAtCard dependiendo si existe un dato en DoctoSerie
                oPurchaseInvoice.NumAtCard = !string.IsNullOrEmpty(doctoserie) ? doctoserie + "-" + doctono : doctono;


                string numcard = oPurchaseInvoice.NumAtCard;


                double totalImpuestos = 0.0;
                double totalDocumentoFinal = 0.0;

                // Copiar cada línea de la Entrada de Mercancías
                for (int i = 0; i < oGoodsReceipt.Lines.Count; i++)
                {
                    oGoodsReceipt.Lines.SetCurrentLine(i);
                    oPurchaseInvoice.Lines.BaseType = (int)BoObjectTypes.oPurchaseDeliveryNotes;
                    oPurchaseInvoice.Lines.BaseEntry = OPCH.ReceiptEntry;
                    oPurchaseInvoice.Lines.BaseLine = oGoodsReceipt.Lines.LineNum;

                    // Iguala propiedades básicas
                    oPurchaseInvoice.Lines.ItemCode = oGoodsReceipt.Lines.ItemCode;
                    oPurchaseInvoice.Lines.Quantity = oGoodsReceipt.Lines.Quantity;
                    oPurchaseInvoice.Lines.Price = oGoodsReceipt.Lines.Price;
                    oPurchaseInvoice.Lines.WarehouseCode = oGoodsReceipt.Lines.WarehouseCode;
                    oPurchaseInvoice.Lines.TaxCode = oGoodsReceipt.Lines.TaxCode;

                    oPurchaseInvoice.Lines.VatGroup = oGoodsReceipt.Lines.VatGroup;
                    oPurchaseInvoice.Lines.UserFields.Fields.Item("U_Tipo").Value = oGoodsReceipt.Lines.UserFields.Fields.Item("U_Tipo").Value;


                    // Calcula el monto para cada articulo sin impuestos
                    double PrecioArticulo = Math.Round(oPurchaseInvoice.Lines.Price * oPurchaseInvoice.Lines.Quantity, 2);
                    //Total Sin impuestos
                    totalFactura += PrecioArticulo;

                    // Calcula los impuestos de cada articulo
                    double tasaImpuesto = GetTaxRateForCode(company, oPurchaseInvoice.Lines.TaxCode);
                    double impuestoLinea = Math.Round(PrecioArticulo * (tasaImpuesto / 100), 2);
                    //Total de impuestos
                    totalImpuestos += impuestoLinea;

                    oPurchaseInvoice.Lines.Add();
                }

                //Se agrega automaticamente
                //oPurchaseInvoice.UserFields.Fields.Item("U_DoctoTotal").Value = totalFactura;

                double totalRetencion = 0.0;
                // Obtener el objeto del proveedor
                BusinessPartners oBusinessPartner = (BusinessPartners)company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                List<string> withholdingTaxCodes = new List<string>();

                if (oBusinessPartner.GetByKey(oPurchaseInvoice.CardCode))
                {

                    // Itera sobre las líneas de retención configuradas en el proveedor
                    for (int i = 0; i < oBusinessPartner.BPWithholdingTax.Count; i++)
                    {
                        oBusinessPartner.BPWithholdingTax.SetCurrentLine(i);
                        if(oBusinessPartner.BPWithholdingTax.WTCode == "")
                        {
                            //No hace nada
                        }
                        else
                        {
                            withholdingTaxCodes.Add(oBusinessPartner.BPWithholdingTax.WTCode);

                        }
                    }
                    
                    // Agrega los códigos de retención a la factura
                    for (int i = 0; i < withholdingTaxCodes.Count; i++)
                    {
                        if (i >= oPurchaseInvoice.WithholdingTaxData.Count)
                        {
                            oPurchaseInvoice.WithholdingTaxData.Add();
                        }

                        oPurchaseInvoice.WithholdingTaxData.SetCurrentLine(i);
                        oPurchaseInvoice.WithholdingTaxData.WTCode = withholdingTaxCodes[i];
                    }

                }

                //Se usan para luego establecer el escenario de retención
                string Verificador_IVA = "";
                string Verificador_ISR = "";

                        //Recorre y valida los códigos de retención para realizar el cálculo correcto que tenga la factura del proveedor
                        for (int i = 0; i < oPurchaseInvoice.WithholdingTaxData.Count; i++)
                        {
                            oPurchaseInvoice.WithholdingTaxData.SetCurrentLine(i);
                            // Aplicar las reglas para WTAmount
                            if (oPurchaseInvoice.WithholdingTaxData.WTCode == "IVA" && totalFactura > 2500.00)
                            {
                                oPurchaseInvoice.WithholdingTaxData.WTAmount = Math.Min(Math.Round(totalFactura * 0.12 * 0.15, 2), totalFactura);
                                //Se queda en S cuando se aplica iva en todos los casos
                                oPurchaseInvoice.UserFields.Fields.Item("U_DoctoVerificado").Value = "S";
                                Verificador_IVA = "IVA";
                            }
                            else if (oPurchaseInvoice.WithholdingTaxData.WTCode == "IVA5" && totalFactura > 2500.00)
                            {
                                oPurchaseInvoice.WithholdingTaxData.WTAmount = Math.Min(Math.Round(totalFactura * 0.05, 2), totalFactura);
                                //Se queda en S cuando se aplica iva en todos los casos
                                oPurchaseInvoice.UserFields.Fields.Item("U_DoctoVerificado").Value = "S";
                                Verificador_IVA = "IVA5";
                            }
                            else if (oPurchaseInvoice.WithholdingTaxData.WTCode == "ISR5" && totalFactura > 2500.00 && totalFactura <= 30000.00)
                            {
                                oPurchaseInvoice.WithholdingTaxData.WTAmount = Math.Min(Math.Round(totalFactura * 0.05, 2), totalFactura);
                                Verificador_ISR = "ISR5";
                            }
                            else if (oPurchaseInvoice.WithholdingTaxData.WTCode == "6ISR" && totalFactura > 30000.00)
                            {
                                oPurchaseInvoice.WithholdingTaxData.WTAmount = Math.Min(Math.Round(totalFactura * 0.06, 2), totalFactura);
                                Verificador_ISR = "6ISR";
                            }
                            else if ( oPurchaseInvoice.WithholdingTaxData.WTCode == "7ISR" && totalFactura > 30000.00)
                            {
                                oPurchaseInvoice.WithholdingTaxData.WTAmount = Math.Min(Math.Round(totalFactura * 0.07, 2), totalFactura);
                                Verificador_ISR = "7ISR";
                            }
                            else if (oPurchaseInvoice.WithholdingTaxData.WTCode == "ISR5" && totalFactura > 30000.00)
                            {
                                oPurchaseInvoice.WithholdingTaxData.WTAmount = Math.Min(Math.Round(totalFactura * 0.05, 2), totalFactura);
                                Verificador_ISR = "ISR5";
                            }//Verificar
                            else if ((oPurchaseInvoice.WithholdingTaxData.WTCode == "IVA" || oPurchaseInvoice.WithholdingTaxData.WTCode == "IVA5") && totalFactura < 2500.00)
                            {
                                oPurchaseInvoice.WithholdingTaxData.WTAmount = 0.00;
                            }
                            
                            totalRetencion += oPurchaseInvoice.WithholdingTaxData.WTAmount;
                        }


                if (Verificador_IVA == "IVA" && Verificador_ISR == "7ISR")
                {
                    //Retencion ISR 7% IVA 15%
                    oPurchaseInvoice.UserFields.Fields.Item("U_EscenarioRet").Value = "009";

                }
                else if (Verificador_IVA == "IVA" && Verificador_ISR == "6ISR")
                {
                    //Retencion ISR 6% IVA 15%
                    oPurchaseInvoice.UserFields.Fields.Item("U_EscenarioRet").Value = "007";

                }
                else if(Verificador_IVA == "IVA" && Verificador_ISR == "ISR5")
                {
                    //Retencion ISR 5% IVA 15%
                    oPurchaseInvoice.UserFields.Fields.Item("U_EscenarioRet").Value = "001";
                }
                else if (Verificador_ISR == "6ISR")
                {
                    //Retencion ISR 6%
                    oPurchaseInvoice.UserFields.Fields.Item("U_EscenarioRet").Value = "006";
                }
                else if (Verificador_ISR == "ISR5")
                {
                    //Retencion ISR 5%
                    oPurchaseInvoice.UserFields.Fields.Item("U_EscenarioRet").Value = "003";
                }
                else if (Verificador_IVA == "IVA")
                {
                    //Retencion IVA 15%
                    oPurchaseInvoice.UserFields.Fields.Item("U_EscenarioRet").Value = "002";
                }
                else if (Verificador_IVA == "IVA5")
                {
                    //Retencion IVA 5%
                    oPurchaseInvoice.UserFields.Fields.Item("U_EscenarioRet").Value = "005";
                }
                else
                {
                    //Ninguna retención
                    oPurchaseInvoice.UserFields.Fields.Item("U_EscenarioRet").Value = "004";
                }


                //Para verificar total final con los cálculos respectivos
                totalDocumentoFinal = totalFactura + totalImpuestos - totalRetencion;
                oPurchaseInvoice.UserFields.Fields.Item("U_DoctoRef").Value = "00005";

                // Se inserta la factura a SAP
                if (oPurchaseInvoice.Add() == 0)
                {
                    var docEntry = company.GetNewObjectKey();
                    Documents FacturaProveedorCreada = (Documents)company.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);

                    // Convierte DocEntry a entero
                    if (int.TryParse(docEntry, out int docentry2))
                    {
                        // Verifica si obtiene la factura creada mediante DocEntry
                        if (FacturaProveedorCreada.GetByKey(docentry2))
                        {
                            return Ok(new
                            {
                                Result = "OK",
                                Message = "Factura de proveedor creada correctamente",
                                DocEntry = docEntry, 
                                DocNum = FacturaProveedorCreada.DocNum.ToString()
                            });
                        }
                        else
                        {
                            // Si no se encuentra el documento
                            return Conflict(new
                            {
                                Result = "Fail",
                                Message = "No se pudo recuperar la factura con el DocEntry proporcionado."
                            });
                        }
                    }
                    else
                    {
                        // Si la conversión a entero falla
                        return Conflict(new
                        {
                            Result = "Fail",
                            Message = "El DocEntry no es un número válido."
                        });
                    }


                }
                else
                {
                    // Manejo de errores
                    company.GetLastError(out int errCode, out string errMsg);
                    return Conflict(new { Result = "Fail", Message = $"Error al crear la factura: {errMsg}" });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new { Result = "Fail", Message = $"Excepción al crear la factura: {ex.Message}" });
            }

        }

        // Método auxiliar para obtener la tasa de retención de un código
        double GetTaxRateForCode(Company company, string taxCode)
        {
            SalesTaxCodes oTaxCode = (SalesTaxCodes)company.GetBusinessObject(BoObjectTypes.oSalesTaxCodes);
            return oTaxCode.GetByKey(taxCode) ? oTaxCode.Rate : 0.0;
        }




    }
}
