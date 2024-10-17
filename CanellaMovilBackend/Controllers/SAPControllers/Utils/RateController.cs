using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Service.SAPService;
using SAPbobsCOM;
using System.Drawing;
using System.Globalization;
using CanellaMovilBackend.Models.SAPModels;
using ConexionesSQL.Models;
using System.Data;
using ConexionesSQL.SAP;



namespace CanellaMovilBackend.Controllers.SAPControllers.Utils
{
    /// <summary>
    /// Controlador para obtener el tipo de cambio
    /// </summary>
    
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class RateController : ControllerBase
    {

        private readonly ISAPService sapService;

        private readonly HttpClient _httpClient;

        /// <summary>
        /// </summary>
        /// <param name="sapService"></param>
        public RateController(ISAPService sapService)
        {
            this.sapService = sapService;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.banguat.gob.gt")
            };
        }

        /// <summary>
        /// Se conecta a los servicios de BANGUAT para obtener la tasa 
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> GetRate()
        {
            // XML de la solicitud SOAP
            string soapRequest = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <TipoCambioDia xmlns=""http://www.banguat.gob.gt/variables/ws/"" />
              </soap:Body>
            </soap:Envelope>";

            string fecha = null;
            string referencia = null;

            try
            {

                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                // Crear una instancia de HttpContent con el contenido SOAP y el tipo de contenido
                HttpContent content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");

                // Configurar el encabezado SOAPAction
                content.Headers.Add("SOAPAction", "\"http://www.banguat.gob.gt/variables/ws/TipoCambioDia\"");

                // Enviar la solicitud POST
                HttpResponseMessage response = await _httpClient.PostAsync("/variables/ws/TipoCambio.asmx", content);

                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta SOAP
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Analizar la respuesta XML
                    XmlDocument xmlDoc = new();
                    xmlDoc.LoadXml(responseContent);

                    // XmlNamespaceManager maneja espacios de nombres en XML
                    XmlNamespaceManager nsmgr = new(xmlDoc.NameTable);
                    nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                    nsmgr.AddNamespace("ns", "http://www.banguat.gob.gt/variables/ws/");

                    // Buscar el nodo TipoCambioDiaResult
                    XmlNode resultNode = xmlDoc.SelectSingleNode("//ns:TipoCambioDiaResult", nsmgr);

                    if (resultNode != null)
                    {
                        // Extrae fecha y tipo de cambio
                        XmlNode dolarNode = resultNode.SelectSingleNode("ns:CambioDolar/ns:VarDolar", nsmgr);

                        if (dolarNode != null)
                        {
                            fecha = dolarNode.SelectSingleNode("ns:fecha", nsmgr)?.InnerText;
                            referencia = dolarNode.SelectSingleNode("ns:referencia", nsmgr)?.InnerText;

                            var formattedDate = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                            //Valida que ambas variables tengan datos
                            if (fecha != null && referencia != null)
                            {
                       
                                //Convierte al tipo de dato correcto
                                if (Double.TryParse(referencia, NumberStyles.Any, CultureInfo.InvariantCulture, out double referenciaValue))
                                {

                                    //Convierte al tipo de dato correcto
                                    if (DateTime.TryParse(formattedDate, out DateTime fechaValue))
                                    {

                                        SAPbobsCOM.SBObob oSBObob;
                                        oSBObob = (SAPbobsCOM.SBObob)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);

                                        //Hace el insert a SAP tabla ORTT
                                        oSBObob.SetCurrencyRate("USD", fechaValue, referenciaValue, true);


                                        return Ok("Tipo de cambio agregado correctamente. ");


                
                                    }
                                    return BadRequest("Error en el formato de la fecha.");
                                }
                                return BadRequest("Error al obtener el valor de cambio.");
                            }
                            return BadRequest("Una de las 2 variables no devuelve datos.");
                        }
                        return BadRequest("No se pudo encontrar el nodo VarDolar en la respuesta.");
                    }
                    else
                    {
                        return BadRequest("No se encontró el nodo TipoCambioDiaResult en la respuesta.");
                    }
                }
                else
                {
                    return BadRequest("Error al llamar al servicio web. Código de estado: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }


        }


    }
}
