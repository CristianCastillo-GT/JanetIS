using CanellaMovilBackend.Filters.UserFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CanellaMovilBackend.Service.SAPService;
using CanellaMovilBackend.Models;
using SAPbobsCOM;
using CanellaMovilBackend.Models.SAPModels.AlertsManagement;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador para las alertas de SAP
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class AlertsManagementController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public AlertsManagementController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Enviar alerta en SAP
        /// </summary>
        /// <param name="alertSAP">Objecto de envió</param>
        /// <returns>Codigos de respuesta</returns>
        /// <response code="200">Creación exitosa</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult SendAlert(AlertSAP alertSAP)
        {
            Company? company = this.sapService.SAPB1();
            try
            {
                CompanyService? oCmpSrv = company.GetCompanyService();
                MessagesService? oMessageService = (MessagesService)oCmpSrv.GetBusinessService(ServiceTypes.MessagesService);
                Message? oMessage = (Message)oMessageService.GetDataInterface(MessagesServiceDataInterfaces.msdiMessage);

                oMessage.Subject = alertSAP.Subject;
                oMessage.Text = alertSAP.Text;

                RecipientCollection oRecipientCollection = oMessage.RecipientCollection;

                for(int i=0; i < (alertSAP?.AlertUsers?.Count ?? 0);  i++)
                {
                    oRecipientCollection.Add();
                    oRecipientCollection.Item(i).SendInternal = BoYesNoEnum.tYES;
                    oRecipientCollection.Item(i).SendEmail = BoYesNoEnum.tNO;
                    oRecipientCollection.Item(i).UserCode = alertSAP?.AlertUsers[i].UserCode;
                }

                if (alertSAP?.AlertDataColumns.Count > 0)
                {
                    MessageDataColumns pMessageDataColumns = oMessage.MessageDataColumns;

                    foreach (AlertDataColumn data in alertSAP.AlertDataColumns)
                    {
                        MessageDataColumn pMessageDataColumn = pMessageDataColumns.Add();
                        pMessageDataColumn.ColumnName = data.ColumnName;
                        pMessageDataColumn.Link = BoYesNoEnum.tYES;

                        MessageDataLines oLines = pMessageDataColumn.MessageDataLines;

                        foreach (AlertDataLine line in data.Lines)
                        {
                            MessageDataLine oLine = oLines.Add();
                            oLine.Value = line.Value;
                            oLine.Object = line.Object;
                            oLine.ObjectKey = line.ObjectKey;
                        }
                    }
                }
                oMessageService.SendMessage(oMessage);
                sapService.SAPB1_DISCONNECT(company);
                return Ok(new MessageAPI() { Result = "OK", Message = "Alerta Enviada." });
            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo enviar la alerta: " + ex.Message });
            }
        }
    }
}
