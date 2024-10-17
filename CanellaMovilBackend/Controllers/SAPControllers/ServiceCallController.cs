using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CanellaMovilBackend.Service.SAPService;
using CanellaMovilBackend.Models;
using SAPbobsCOM;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.ServiceCall;
using ConexionesSQL.SAP;
using ConexionesSQL.Models;
using CanellaMovilBackend.Utils;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador para las llamadas de servicio
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class ServiceCallController(ISAPService sapService) : ControllerBase
    {

        /// <summary>
        /// Crea una llamada de servicio
        /// </summary>
        /// <param name="serviceCall">Objeto de envio</param>
        /// <returns>Retorna el docentry de la llamada de servicio</returns>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult<MessageAPI> CreateServiceCall(ServiceCall serviceCall)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                ServiceCalls service = (ServiceCalls)company.GetBusinessObject(BoObjectTypes.oServiceCalls);

                service.Series = serviceCall.Series;
                service.CustomerCode = serviceCall.CustomerCode;
                service.ItemCode = serviceCall.ItemCode;
                service.InternalSerialNum = serviceCall.InternalSerialNum;
                service.ManufacturerSerialNum = serviceCall.ManufacturerSerialNum;
                service.Subject = serviceCall.Subject;
                service.ProblemType = serviceCall.ProblemType;
                service.CallType = serviceCall.CallType;
                service.AssigneeCode = serviceCall.AssigneeCode;
                service.Origin = serviceCall.Origin;
                service.Priority = serviceCall.Priority;

                // User Fields
                service.UserFields.Fields.Item("U_AutorizaSup").Value = serviceCall.U_AutorizaSup;
                service.UserFields.Fields.Item("U_TipoLLamada").Value = serviceCall.U_TipoLLamada;
                service.UserFields.Fields.Item("U_Facturable").Value = serviceCall.U_Facturable;
                service.UserFields.Fields.Item("U_LugarAtencion").Value = serviceCall.U_LugarAtencion;
                service.UserFields.Fields.Item("U_CECO").Value = serviceCall.U_CECO;

                if (service.Add() == 0)
                {
                    var docentry = company.GetNewObjectKey();
                    return Ok(new MessageAPI() { Result = "OK", Message = "Creada la llamada de servicio exitosamente", Code = docentry });
                }
                else
                {
                    company.GetLastError(out int errCode, out string errMsg);
                    return Conflict(new MessageAPI() { Result = "Fail", Message = errMsg, Code = string.Empty });
                }
            }
            catch(Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = ex.Message });
            }
        }

        /// <summary>
        /// Relaciona una orden de venta con una llamada de servicio
        /// </summary>
        /// <param name="relationServiceCall">Objeto de envio</param>
        /// <returns>Retorna un mensaje de exito</returns>
        /// <response code="200">Actualización de su relación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult<MessageAPI> UpdateRelationServiceCall(RelationServiceCall relationServiceCall)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                ServiceCalls serviceCall = (ServiceCalls)company.GetBusinessObject(BoObjectTypes.oServiceCalls);
                Documents orderSale = (Documents)company.GetBusinessObject(BoObjectTypes.oOrders);

                orderSale.GetByKey(Convert.ToInt32(relationServiceCall.DocEntrySaleOrder));
                serviceCall.GetByKey(Convert.ToInt32(relationServiceCall.CallID));

                if (orderSale.CardCode == serviceCall.CustomerCode)
                {
                    if (serviceCall.Expenses.DocEntry > 0)
                        serviceCall.Expenses.Add();
                    serviceCall.Expenses.DocEntry = orderSale.DocEntry;
                    serviceCall.Expenses.DocumentType = BoSvcEpxDocTypes.edt_Order;
                    if (serviceCall.Update() == 0)
                    {
                        return Ok(new MessageAPI() { Result = "OK", Message = "Relacion entre la orden de venta y la llamada de servicio exitosa" });
                    }
                    else
                    {
                        company.GetLastError(out int errCode, out string errMsg);
                        return Conflict(new MessageAPI() { Result = "Fail", Message = errMsg, Code = string.Empty });
                    }
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "El cliente de la orden es diferente al de la llamada de servicio"});
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = ex.Message });
            }
        }

        /// <summary>
        /// Obtener las llamadas de servicio por cliente y serie
        /// </summary>
        /// <param name="CardCode">Código del cliente</param>
        /// <param name="Serie">Serie del artículo</param>
        /// <returns>Retorna un listado de las llamadas de servicio abiertas</returns>
        /// <response code="200">obtención de datos exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceCallOne), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult SearchServiceCall(string CardCode, string Serie)
        {
            try
            {
                var parameters = new List<Parametros>()
                {
                    new("@CardCode", CardCode),
                    new("@Serie", Serie)
                };

                SP_OSCL serviceCall = new();
                var resultado = serviceCall.OSCL_SearchServiceCall(parameters);

                if (resultado.MensajeTipo == 1)
                {
                    return Ok(ObjectConvert.CreateListFromDataTable<ServiceCallOne>(resultado.Datos));
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = resultado.MensajeDescripcion });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = ex.Message });
            }
        }
    }
}
