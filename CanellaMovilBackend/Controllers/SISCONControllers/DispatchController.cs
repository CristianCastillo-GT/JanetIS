using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.SISCONModels;
using CanellaMovilBackend.Service.SAPService;
using ConexionesSQL.Models;
using Microsoft.AspNetCore.Authorization;
using SAPbobsCOM;
using ConexionesSQL.SISCON;
using Microsoft.AspNetCore.Mvc;
using CanellaMovilBackend.Utils;
using System.Data;

namespace CanellaMovilBackend.Controllers.SISCONControllers
{
    /// <summary>
    /// Controlador de los depachos del Sistema de Contratos
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class DispatchController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        public DispatchController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Crea el activo fijo y su capitalización
        /// </summary>
        /// <returns>Devuelve las entregas cerradas</returns>
        /// <response code="200">Actaulización correcta</response>
        /// <response code="409">Error al actualizar boleta</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult ActivoFijoEntrega(List<DeliveryNodo> nodos)
        {
            Company company = sapService.SAPB1();
            try
            {
                if (nodos != null && nodos.Count > 0)
                {
                    company.StartTransaction();
                    List<ListDelivery> datos = [];
                    foreach (var listado in nodos.GroupBy(x => x.DocEntry))
                    {
                        string cadenaArticulosConvertidos = string.Empty;

                        foreach (var nodo in listado)
                        {
                            // OITM para obtener datos del articulo
                            Items oItem = (Items)company.GetBusinessObject(BoObjectTypes.oItems);
                            // OITM para crear el activo fijo en SAP
                            Items oItemAsset = (Items)company.GetBusinessObject(BoObjectTypes.oItems);
                            // OITM para buscar el nuevo activo fijo en SAP
                            Items oItemAsset2 = (Items)company.GetBusinessObject(BoObjectTypes.oItems);

                            if (oItem.GetByKey(nodo.CodigoArticulo))
                            {
                                double precioArticuloSAP = nodo.precio;
                                bool articuloDepreciable = precioArticuloSAP >= double.Parse(nodo.MontoMinimoAF);

                                // Creación del activo fijo en SAP
                                oItemAsset.ItemType = ItemTypeEnum.itFixedAssets;
                                oItemAsset.Series = int.Parse(nodo.SerieAF);
                                oItemAsset.ItemName = oItem.ItemName;
                                oItemAsset.AssetClass = articuloDepreciable ? nodo.ClaseAF : nodo.ClaseGastoAF;
                                oItemAsset.AssetSerialNumber = nodo.Serie;
                                oItemAsset.ItemsGroupCode = int.Parse(nodo.GrupoAF);
                                oItemAsset.Employee = int.Parse(nodo.CodigoEmpleadoAF);
                                oItemAsset.PurchaseItem = BoYesNoEnum.tNO;
                                oItemAsset.SalesItem = BoYesNoEnum.tNO;
                                oItemAsset.UserFields.Fields.Item("U_ItemCode").Value = nodo.CodigoArticulo;
                                oItemAsset.UserFields.Fields.Item("U_Condicion").Value = "D";
                                oItemAsset.DistributionRules.DistributionRule = nodo.NormaRepartoAF;
                                oItemAsset.DistributionRules.ValidFrom = DateTime.Now;

                                if (oItemAsset.Add() == 0)
                                {
                                    company.GetNewObjectCode(out string codigoAF);
                                    cadenaArticulosConvertidos += nodo.CodigoArticulo + "=" + codigoAF + ";";

                                    // Crear Capitalización
                                    CompanyService oService = company.GetCompanyService();
                                    AssetDocumentService AssetService = (AssetDocumentService)oService.GetBusinessService(ServiceTypes.AssetCapitalizationService);
                                    AssetDocument AssetDocument = (AssetDocument)AssetService.GetDataInterface(AssetDocumentServiceDataInterfaces.adsAssetDocument);
                                    AssetDocumentAreaJournal journalEn = AssetDocument.AssetDocumentAreaJournalCollection.Add();

                                    AssetDocument.AssetValueDate = DateTime.Now;
                                    AssetDocument.PostingDate = DateTime.Now;
                                    AssetDocument.DocumentDate = DateTime.Now;
                                    AssetDocument.DepreciationArea = nodo.AreaDepreciacion;

                                    AssetDocumentLine line = AssetDocument.AssetDocumentLineCollection.Add();

                                    if (!articuloDepreciable)
                                    {
                                        line.Quantity = 1;
                                    }
                                    line.AssetNumber = codigoAF;
                                    line.TotalLC = articuloDepreciable ? precioArticuloSAP : 0;
                                    int docEntryCapitalizacion = (AssetService.Add(AssetDocument)).Code;
                                    if (oItemAsset2.GetByKey(codigoAF))
                                    {
                                        oItemAsset2.UserFields.Fields.Item("U_CodigoAAF").Value = codigoAF;

                                        if (oItemAsset2.Update() == 0)
                                        {
                                            nodo.NewCodeAF = codigoAF;
                                        }
                                        else
                                        {
                                            throw new Exception($"No se pudo actualizar el activo fijo para el articulo con codigo {nodo.CodigoArticulo}");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception($"No se pudo crear el activo fijo para el articulo con codigo {nodo.CodigoArticulo}");
                                    }
                                }
                                else
                                {
                                    company.GetLastError(out int nErr, out string errMsg);
                                    throw new Exception($"No se pudo crear el activo fijo para el articulo con codigo {nodo.CodigoArticulo}, EXCEPTION: {errMsg}");
                                }
                            }
                            else
                            {
                                throw new Exception($"No se encontro el siguiente codigo de articulo {nodo.CodigoArticulo}");
                            }
                        }

                        datos.Add(new ListDelivery { Entrega = listado.Key, CadenaArticulo = cadenaArticulosConvertidos });
                    }

                    if (company.InTransaction)
                    {
                        SP_DES deli = new();
                        var parameters = new List<Parametros>
                        {
                            new("@DOC_ENTRY_ENTREGAS", DataTableConvert.ToListConvertToDataTable(
                            datos.Select(
                                x => new
                                {
                                    ENTREGA = x.Entrega,
                                    CADENA_ARTICULOS = x.CadenaArticulo
                                }).ToList())
                        ),
                            new("@TBL_ACTIVOS_FIJOS", DataTableConvert.ToListConvertToDataTable(
                            nodos.Select(x => new
                            {
                                DESPACHO_NO = x.DespachoNo,
                                DESPACHO_LINEA = x.DespachoLinea,
                                CODIGO_AF = x.NewCodeAF,
                                NUMERO_SERIE_AF = x.Serie
                            }).ToList())
                        )
                        };

                        var resultado = deli.DES_UPDATE_ACTIVOS_FIJOS(parameters);

                        if (resultado.MensajeDescripcion == "ACTUALIZADO")
                        {
                            company.EndTransaction(BoWfTransOpt.wf_Commit);
                            sapService.SAPB1_DISCONNECT(company);
                            return Ok(new MessageAPI() { Message = "Activos Fijos y Capitalizacion creada." });
                        }
                        else
                        {
                            throw new Exception($"No se pudo actualizar la informacion del activo fijo, Error: {resultado.MensajeDescripcion}");
                        }
                    }
                    else
                    {
                        throw new Exception("No se pudo actualizar la informacion del activo fijo.");
                    }
                }
                else
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Conflict(new MessageAPI() { Message = "No hay registros para procesar." });
                }
            }
            catch (Exception ex)
            {
                if (company.InTransaction)
                    company.EndTransaction(BoWfTransOpt.wf_RollBack);
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Message = ex.Message });
            }
        }
    }
}
