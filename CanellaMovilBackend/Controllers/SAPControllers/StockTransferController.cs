using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.IncomingPayments;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;
using System.Data.SqlClient;
using System.Data;
using ConexionesSQL.STOD.ENS;
using ConexionesSQL.Models;
using ConexionesSQL.STOD.Dashboard;
using Microsoft.AspNetCore.Authentication;
using System.Text.RegularExpressions;
using CanellaMovilBackend.Models.STODModels.Dashboard;
using CanellaMovilBackend.Utils;
using CanellaMovilBackend.Models.StockTransfer;
using System.Data.SqlTypes;
using System.Xml.Serialization;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    ///  Controlador Transferencias Ensamble
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]

    public class StockTransferController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>

        public StockTransferController(ISAPService sapService)
        {
            this.sapService = sapService;
        }


        /// <summary>
        /// Crea transferencias de Ensamble a SAP
        /// </summary>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creacion exitosa</response>
        /// <response code="409">Creacion exitosa</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<MessageAPI>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]

        public ActionResult BulkCreateTransfer(List<OWTR> OWTRList)
        {
            int nErr = 0;
            string sql = string.Empty;
            string sqlSAP = string.Empty;



            try
            {
                CompanyConnection companyConnection = this.sapService.SAPB1();
                Company company = companyConnection.Company;
                List<MessageAPI> messageApi = [];

                if (company == null || !company.Connected)
                {
                    throw new InvalidOperationException("No se puede establecer conexión con SAP.");
                }


                foreach (OWTR OWTR in OWTRList)
                {

                    StockTransfer? transfer = (StockTransfer)company.GetBusinessObject(BoObjectTypes.oStockTransfer);

                    transfer.DocDate = DateTime.Now;
                    transfer.Lines.ItemCode = OWTR.ItemCode;
                    transfer.Lines.SerialNumbers.ManufacturerSerialNumber = OWTR.Chasis;
                    transfer.FromWarehouse = OWTR.BodegaOrigenSAP;
                    transfer.ToWarehouse = OWTR.BodegaDestinoSAP;
                    transfer.UserFields.Fields.Item("U_DoctoRefNo").Value = OWTR.PedidoID;
                    transfer.UserFields.Fields.Item("U_PedidoID").Value = OWTR.PedidoID;
                    transfer.UserFields.Fields.Item("U_DoctoGenServ").Value = "N";
                    transfer.UserFields.Fields.Item("U_AlmacenDestino").Value = OWTR.BodegaDestinoSAP;
                    transfer.UserFields.Fields.Item("U_SucursalDestino").Value = OWTR.CodSAPBodega;
                    transfer.UserFields.Fields.Item("U_DoctoRef").Value = "00045";
                    transfer.Lines.FromWarehouseCode = OWTR.BodegaOrigenSAP;
                    transfer.Lines.WarehouseCode = OWTR.BodegaDestinoSAP;
                    transfer.Lines.SerialNumbers.BaseLineNumber = transfer.Lines.LineNum;
                    transfer.Lines.SerialNumbers.Quantity = 1;

                    if (transfer.Add() != 0)
                    {
                        company.GetLastError(out int errCode, out string errMsg);
                        Console.WriteLine(errMsg);
                        if (errMsg.Length > 150)
                        {
                            errMsg = errMsg.Substring(0, 148);
                        }

                        setErrorSAP(errMsg, OWTR.ENS_TrasladoSAPID);
                    }
                    else
                    {
                        var docEntry = company.GetNewObjectKey();
                        messageApi.Add(new MessageAPI { Result = "Success", Message = $"Traslado creado con éxito, DocEntry: {docEntry}" });

                        setDocEntry(docEntry, OWTR.ENS_TrasladoSAPID);

                        UpdateEnsambleSAP(OWTR.ENS_TrasladoSAPID);
                    }

                }

                return Ok(new MessageAPI() { Result = "Success", Message = "Traslado creado con éxito" });

            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear el traslado - error: " });
            }

        }


        private void setErrorSAP(string errorSAP, string IdTraslado)
        {
            var parameters = new List<Parametros>()
            {
                new("errMsg", errorSAP),
                new("IdTraslado", IdTraslado)
            };
            ENS ens = new();
            var resultado = ens.API_ENS_SetErrorSAP(parameters);

        }

        private void setDocEntry(string docEntry, string IdTraslado)
        {
            var parameters = new List<Parametros>()
            {
                new("DocEntry", docEntry),
                new("IdTraslado", IdTraslado)
            };
            ENS ens = new();
            var resultado = ens.API_ENS_SetDocEntrySAP(parameters);
        }

        private void UpdateEnsambleSAP(string IdTraslado)
        {
            var parameters = new List<Parametros>()
            {
                new("LogID")
            };
            ENS ens = new();
            var resultado = ens.API_ENS_ActualizarEnsambleSAP(parameters);
        }

    }
}
