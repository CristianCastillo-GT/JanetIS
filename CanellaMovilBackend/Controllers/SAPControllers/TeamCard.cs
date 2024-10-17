using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CanellaMovilBackend.Service.SAPService;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.SAPModels.TeamCard;
using ConexionesSQL.Models;
using CanellaMovilBackend.Utils;
using CanellaMovilBackend.Models.CQMModels;
using SAPbobsCOM;
using CanellaMovilBackend.Models.SAPModels.Reports;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador de Tarjetas Equipo
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="sapService"></param>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class TeamCard(ISAPService sapService) : ControllerBase
    {
        /// <summary>
        /// Crea una nueva tarjeta de equipo
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CreateTeamCard(CreateTC CreateTC)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                // Se obtiene el objeto CustomerEquipmentCards de la API de DI
                CustomerEquipmentCards EquipmentCard = (CustomerEquipmentCards)company.GetBusinessObject(BoObjectTypes.oCustomerEquipmentCards);

                //Campos Obligatorios
                EquipmentCard.ManufacturerSerialNum = CreateTC.manufSN;
                EquipmentCard.InternalSerialNum = CreateTC.internalSN;
                EquipmentCard.ItemCode = CreateTC.itemCode;
                EquipmentCard.CustomerCode = CreateTC.customer;
                EquipmentCard.ContactEmployeeCode = CreateTC.contactCod;

                // Validación para insertar los datos en la creación
                if (EquipmentCard.Add() != 0)
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear la tarjeta de equipo: " + company.GetLastErrorDescription() });
                }
                else
                {
                    var docentry = company.GetNewObjectKey();
                    return Ok(new MessageAPI() { Result = "OK", Message = "La tarjeta de equipo fue creada correctamente.", Code = docentry });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear la tarjeta de equipo: " + ex.Message });
            }
        }



        /// <summary>
        /// Actualiza el socio de negocio mediante el código de una tarjeta de equipo
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult ActualizarBP (UpdateBP UpdateBP)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                CustomerEquipmentCards EquipmentCard = (CustomerEquipmentCards)company.GetBusinessObject(BoObjectTypes.oCustomerEquipmentCards);

                if (EquipmentCard.GetByKey(UpdateBP.InsID))
                {
                    BusinessPartners businessPartner = (BusinessPartners)company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                    if(businessPartner.GetByKey(UpdateBP.Customer))
                    {
                        EquipmentCard.CustomerCode = businessPartner.CardCode;
                        EquipmentCard.CustomerName = businessPartner.CardName;
                        EquipmentCard.StatusOfSerialNumber = UpdateBP.StatusOfSerialNumber;

                        if (EquipmentCard.Update() != 0)
                        {
                            company.GetLastError(out int errCode, out string errMsg);
                            return Conflict(new MessageAPI() { Result = "Fail", Message = "Error en SAP: " + errMsg });
                        }
                        else
                        {
                            return Ok(new MessageAPI() { Result = "OK", Message = "Se actulizo la tarjeta de equipo " + (UpdateBP.InsID.ToString() ?? "") });
                        }
                    }
                    else
                    {
                        return NotFound(new MessageAPI() { Result = "Fail", Message = "No existe el cliente en SAP " + UpdateBP.Customer});
                    }
                }
                else
                {
                    return NotFound(new MessageAPI() { Result = "Fail", Message = "No existe la tarjeta de equipo en SAP" });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo editar la tarjeta de equipo - " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una tarjeta de equipo mediante No Serie Chasis
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok Se encontro los datos de la tarjeta de equipo</response>
        /// <response code="404">Not Found No se encontro tarjeta de equipo</response>
        /// <response code="409">Conflict Fallo al buscar la tarjeta de equipo</response>
        [HttpGet]
        [ProducesResponseType(typeof(ListadoTarjetaEquipos), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult SearchTarjetaEquipo(string internalSN)
        {
            try
            {
                var parameters = new List<Parametros>()
                {
                    new("@internalSN", internalSN)
                };

                SelectTC equipmentCard = new();
                var resultado = equipmentCard.OINS_SelectTC(parameters);

                if (resultado.MensajeTipo == 1)
                {
                    List<ListadoTarjetaEquipos> listado = ObjectConvert.CreateListFromDataTable<ListadoTarjetaEquipos>(resultado.Datos);

                    if (listado.Count > 0)
                    {
                        return Ok(listado.First());
                    }
                    else
                    {
                        return NotFound(new MessageAPI() { Result = "Fail", Message = $"No se encontro la tarjeta de equipo con serie {internalSN}" });
                    }
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
