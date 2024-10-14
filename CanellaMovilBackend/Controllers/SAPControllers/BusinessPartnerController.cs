using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.BusinessPartner;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SAPbobsCOM;
using static CanellaMovilBackend.Models.SAPModels.BusinessPartner.RequestDataBusinessPartner;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador del socio de negocios
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class BusinessPartnerController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public BusinessPartnerController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Obtiene los datos de un socio de negocios
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<OCRD>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetBusinessPartner(RequestGetBusinessPartner request)
        {
            if (request.CardCode == null && request.AddId == null || request.CardCode == "" && request.AddId == "")
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "El CardCode y el AddId no pueden ir vacíos" });
            }

            // Validar el parámetro Empresa
            if (request.Empresa < 1 || request.Empresa > 4)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "El valor de Empresa es inválido" });
            }
            Company company = sapService.SAPB1();
            try
            {
                var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appSettings.json")
               .Build();

                // Obtener el objeto BusinessPartners de la API de DI
                Recordset recordsetBP = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                string consulta;

                switch (request.Empresa)
                {
                    case 1:
                        consulta = "SELECT CardCode, CardName, CardFName, Balance, DNotesBal, OrdersBal, ShipToDef, BillToDef, groupCode, groupNum, Phone1, Cellular, E_Mail, AddID, VatIdUnCmp, CreditLine, U_CobradorCode FROM [SBO_CANELLA].[dbo].OCRD WITH (NOLOCK) " +
                            "WHERE " + (request.CardCode != null && request.CardCode != "" ? "CardCode = '" + request.CardCode + "' " : " CardCode != ''") + (request.AddId != null && request.AddId != "" ? " and addid = '" + request.AddId + "' " : "");
                        break;
                    case 2:
                        consulta = $"SELECT CardCode, CardName, CardFName, Balance, DNotesBal, OrdersBal, ShipToDef, BillToDef, groupCode, groupNum, Phone1, Cellular, E_Mail, AddID, VatIdUnCmp, CreditLine, U_CobradorCode FROM [{configuration.GetConnectionString("VESA") ?? ""}].[SBO_VESA].[dbo].[OCRD] WITH (NOLOCK) " +
                            "WHERE " + (request.CardCode != null && request.CardCode != "" ? "CardCode = '" + request.CardCode + "' " : " CardCode != ''") + (request.AddId != null && request.AddId != "" ? " and addid = '" + request.AddId + "' " : "");
                        break;
                    case 3:
                        consulta = $"SELECT CardCode, CardName, CardFName, Balance, DNotesBal, OrdersBal, ShipToDef, BillToDef, groupCode, groupNum, Phone1, Cellular, E_Mail, AddID, VatIdUnCmp, CreditLine, U_CobradorCode FROM [{configuration.GetConnectionString("TALLER") ?? ""}].[TALLER].[dbo].[OCRD] WITH (NOLOCK) " +
                            "WHERE " + (request.CardCode != null && request.CardCode != "" ? "CardCode = '" + request.CardCode + "' " : " CardCode != ''") + (request.AddId != null && request.AddId != "" ? " and addid = '" + request.AddId + "' " : "");
                        break;
                    case 4:
                        consulta = $"SELECT CardCode, CardName, CardFName, Balance, DNotesBal, OrdersBal, ShipToDef, BillToDef, groupCode, groupNum, Phone1, Cellular, E_Mail, AddID, VatIdUnCmp, CreditLine, U_Cobrador AS U_CobradorCode FROM [{configuration.GetConnectionString("MAQUIPOS") ?? ""}].SBO_MAQUIPOS.[dbo].[OCRD] WITH (NOLOCK) " +
                            "WHERE " + (request.CardCode != null && request.CardCode != "" ? "CardCode = '" + request.CardCode + "' " : " CardCode != ''") + (request.AddId != null && request.AddId != "" ? " and addid = '" + request.AddId + "' " : "");
                        break;
                    default:
                        return Conflict(new MessageAPI() { Result = "Fail", Message = "Empresa no válida" });
                }

                // Ejecutar la consulta SQL
                recordsetBP.DoQuery(consulta);

                if (recordsetBP.RecordCount > 0)
                {
                    List<OCRD> ListOCRD = [];

                    while (!recordsetBP.EoF)
                    {
                        OCRD businessPartners = new()
                        {
                            CardCode = (string)recordsetBP.Fields.Item("CardCode").Value,
                            CardName = (string)recordsetBP.Fields.Item("CardName").Value,
                            CardFName = (string)recordsetBP.Fields.Item("CardFName").Value,
                            BillToDef = (string)recordsetBP.Fields.Item("BillToDef").Value,
                            ShipToDef = (string)recordsetBP.Fields.Item("ShipToDef").Value,
                            GroupCode = Convert.ToString(recordsetBP.Fields.Item("groupCode").Value),
                            GroupNum = Convert.ToString(recordsetBP.Fields.Item("groupNum").Value),
                            Balance = Convert.ToString(recordsetBP.Fields.Item("Balance").Value),
                            DNotesBal = Convert.ToString(recordsetBP.Fields.Item("DNotesBal").Value),
                            OrdersBal = Convert.ToString(recordsetBP.Fields.Item("OrdersBal").Value),
                            Phone1 = (string)recordsetBP.Fields.Item("Phone1").Value,
                            Cellular = (string)recordsetBP.Fields.Item("Cellular").Value,
                            E_Mail = (string)recordsetBP.Fields.Item("E_Mail").Value,
                            AddID = (string)recordsetBP.Fields.Item("AddID").Value,
                            VatIdUnCmp = (string)recordsetBP.Fields.Item("VatIdUnCmp").Value,
                            CreditLine = Convert.ToString(recordsetBP.Fields.Item("CreditLine").Value),
                            U_CobradorCode = Convert.ToString(recordsetBP.Fields.Item("U_CobradorCode").Value)
                        };

                        // Obtener las direcciones del socio de negocios
                        Recordset recordsetAD = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        recordsetAD.DoQuery($"SELECT Address, AdresType, Street, County, State, Country FROM CRD1 WITH (NOLOCK) WHERE CardCode = '{businessPartners.CardCode}'");

                        while (!recordsetAD.EoF)
                        {
                            CRD1 address = new()
                            {
                                Address = (string)recordsetAD.Fields.Item("Address").Value,
                                AddressType = (string)recordsetAD.Fields.Item("AdresType").Value,
                                Street = (string)recordsetAD.Fields.Item("Street").Value,
                                County = (string)recordsetAD.Fields.Item("County").Value,
                                State = (string)recordsetAD.Fields.Item("State").Value,
                                Country = (string)recordsetAD.Fields.Item("Country").Value
                            };
                            businessPartners?.Addresses?.Add(address);
                            recordsetAD.MoveNext();
                        }

                        // Obtener lOS contactos del socio de negocios
                        Recordset recordsetCT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        recordsetCT.DoQuery($"SELECT CntctCode, CardCode, Name, FirstName, LastName, Title, Tel1, Tel2, Cellolar, E_MailL FROM OCPR WITH (NOLOCK) WHERE CardCode = '{businessPartners?.CardCode ?? ""}'");

                        while (!recordsetCT.EoF)
                        {
                            OCPR contact = new()
                            {
                                CntctCode = Convert.ToString(recordsetCT.Fields.Item("CntctCode").Value),
                                CardCode = (string)recordsetCT.Fields.Item("CardCode").Value,
                                Name = (string)recordsetCT.Fields.Item("Name").Value,
                                FirstName = (string)recordsetCT.Fields.Item("FirstName").Value,
                                LasName = (string)recordsetCT.Fields.Item("LastName").Value,
                                Title = (string)recordsetCT.Fields.Item("Title").Value,
                                Tel1 = (string)recordsetCT.Fields.Item("Tel1").Value,
                                Tel2 = (string)recordsetCT.Fields.Item("Tel2").Value,
                                Cellolar = (string)recordsetCT.Fields.Item("Cellolar").Value,
                                E_MailL = (string)recordsetCT.Fields.Item("E_MailL").Value
                            };
                            businessPartners?.Contacts?.Add(contact);
                            recordsetCT.MoveNext();
                        }
                        ListOCRD.Add(businessPartners ?? new OCRD());
                        recordsetBP.MoveNext();
                    }
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(ListOCRD);
                }
                else
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar el socio de negocios con los datos: " + JsonConvert.SerializeObject(request) });
                }
            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar el socio de negocios con el CardCode: " + request.CardCode + " y AddId: " + request.AddId + " - " + ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo socio de negocios en SAP
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult CreatePartner(OCRD OCRD)
        {
            Company company = sapService.SAPB1();
            try
            {
                // Crear una consulta SQL para obtener el CardCode más alto actualmente en uso
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                string query = "SELECT \r\n\t\t\t'C'+replicate('0',6-len(cast(cast(substring(max(cardcode),2 ,len(max(cardcode))) as int)+1 as nvarchar) )) \r\n\t\t\t+cast(cast(substring(max(cardcode),2 ,len(max(cardcode))) as int)+1 as nvarchar) as CardCode\r\n\t\tFROM ocrd WITH (NOLOCK)\r\n\t\tWHERE \r\n\t\t\tsubstring(CardCode,1,1) = 'C' \r\n\t\t\tand FrozenFor = 'N'";
                recordset.DoQuery(query);

                // Obtener el CardCode más alto actualmente en uso y aumentarlo en uno para obtener el siguiente disponible
                OCRD.CardCode = recordset.Fields.Item("CardCode").Value.ToString();

                BusinessPartners businessPartner = (BusinessPartners)company.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                //Encabezado
                businessPartner.CardCode = OCRD.CardCode;
                businessPartner.CardType = BoCardTypes.cCustomer;
                businessPartner.CardName = OCRD.CardName;
                businessPartner.CardForeignName = OCRD.CardFName;
                businessPartner.GroupCode = Convert.ToInt32(OCRD.GroupCode);
                businessPartner.FederalTaxID = "000000000000";
                businessPartner.Currency = "##";

                //General
                businessPartner.Phone1 = OCRD.Phone1;
                businessPartner.Cellular = OCRD.Cellular;
                businessPartner.EmailAddress = OCRD.E_Mail;
                businessPartner.AdditionalID = OCRD.AddID;
                businessPartner.UnifiedFederalTaxID = OCRD.VatIdUnCmp ?? businessPartner.UnifiedFederalTaxID;

                //Condiciones de Pago
                businessPartner.PayTermsGrpCode = Convert.ToInt32(OCRD.GroupNum);
                businessPartner.PriceListNum = 24;

                //Campos de usuario 
                businessPartner.UserFields.Fields.Item("U_FComercial").Value = DateTime.Now;

                foreach (var address in OCRD?.Addresses ?? [])
                {
                    // Agregar la dirección
                    businessPartner.Addresses.AddressName = address.Address;
                    businessPartner.Addresses.AddressType = address.AddressType == "B" ? BoAddressType.bo_BillTo : BoAddressType.bo_ShipTo;
                    businessPartner.Addresses.Street = address.Street;
                    businessPartner.Addresses.County = address.County;
                    businessPartner.Addresses.State = address.State;
                    businessPartner.Addresses.Country = address.Country;
                    businessPartner.Addresses.Add();
                }

                // Agregar el socio de negocios a la base de datos
                int result = businessPartner.Add();

                if (result != 0)
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear el socio de negocios: " + company.GetLastErrorDescription() });
                }
                else
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(new MessageAPI() { Result = "OK", Message = "El cliente fue creado correctamente.", Code = OCRD?.CardCode ?? "" });
                }
            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo crear el socio de negocios: " + ex.Message });
            }
        }

        /// <summary>
        /// Actualiza la información del socio de negocios
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpPost]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult UpdatePartner(OCRD OCRD)
        {
            Company company = sapService.SAPB1();

            // Obtener el objeto BusinessPartners de la API de DI
            BusinessPartners businessPartner = (BusinessPartners)company.GetBusinessObject(BoObjectTypes.oBusinessPartners);

            List<string> addressUpdate = [];
            if (businessPartner.GetByKey(OCRD.CardCode) == true)
            {
                try
                {
                    businessPartner.CardName = OCRD.CardName;
                    businessPartner.CardForeignName = OCRD.CardFName ?? businessPartner.CardForeignName;
                    businessPartner.Phone1 = OCRD.Phone1 ?? businessPartner.Phone1;
                    businessPartner.Cellular = OCRD.Cellular ?? businessPartner.Cellular;
                    businessPartner.EmailAddress = OCRD.E_Mail;
                    businessPartner.AdditionalID = OCRD.AddID ?? businessPartner.AdditionalID;
                    businessPartner.UnifiedFederalTaxID = OCRD.VatIdUnCmp ?? businessPartner.UnifiedFederalTaxID;

                    for (int i = 0; i < businessPartner.Addresses.Count; i++)
                    {
                        businessPartner.Addresses.SetCurrentLine(i);
                        CRD1? address = OCRD?.Addresses?.Where(s => s.Address == businessPartner.Addresses.AddressName && (s.AddressType == "B" ? BoAddressType.bo_BillTo : BoAddressType.bo_ShipTo) == businessPartner.Addresses.AddressType).FirstOrDefault();
                        if (address != null)
                        {
                            businessPartner.Addresses.AddressName = address.Address;
                            businessPartner.Addresses.AddressType = address.AddressType == "B" ? BoAddressType.bo_BillTo : BoAddressType.bo_ShipTo;
                            businessPartner.Addresses.Street = address.Street;
                            businessPartner.Addresses.County = address.County;
                            businessPartner.Addresses.State = address.State;
                            businessPartner.Addresses.Country = address.Country;
                            addressUpdate.Add(address.Address);
                        }
                    }

                    if (OCRD?.Addresses?.Count > addressUpdate.Count)
                    {
                        var newAddressList = OCRD.Addresses.Where(a => addressUpdate.Contains(a.Address));
                        foreach (CRD1 address in OCRD.Addresses.Except(newAddressList))
                        {
                            businessPartner.Addresses.Add();
                            businessPartner.Addresses.AddressName = address.Address;
                            businessPartner.Addresses.AddressType = address.AddressType == "B" ? BoAddressType.bo_BillTo : BoAddressType.bo_ShipTo;
                            businessPartner.Addresses.Street = address.Street;
                            businessPartner.Addresses.County = address.County;
                            businessPartner.Addresses.State = address.State;
                            businessPartner.Addresses.Country = address.Country;
                        }
                    }

                    int updateResult = businessPartner.Update();

                    if (updateResult != 0)
                    {
                        sapService.SAPB1_DISCONNECT(company);
                        return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo actualizar el socio de negocios: " + company.GetLastErrorDescription() });
                    }
                }
                catch (Exception ex)
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo editar el socio de negocios - " + ex.Message });
                }
            }
            else
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar el socio de negocios con el CardCode: " + OCRD?.CardCode ?? "" });
            }
            sapService.SAPB1_DISCONNECT(company);
            return Ok(new MessageAPI() { Result = "OK", Message = "El CardCode: " + OCRD?.CardCode ?? "" + " fue actualizado correctamente." });
        }


        /// <summary>
        /// Actualiza el GroupNum de un listado de socios de negocios
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "BusinessPartner")]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult UpdateGroupNumPartner(List<RequestUpdateGroupNumPartner> OCRDList)
        {
            Company company = sapService.SAPB1();

            List<MessageAPI> ListMessage = [];

            foreach (RequestUpdateGroupNumPartner OCRD in OCRDList ?? [])
            {
                // Obtener el objeto BusinessPartners de la API de DI
                BusinessPartners businessPartners = (BusinessPartners)company.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                if (businessPartners.GetByKey(OCRD.CardCode) == true)
                {
                    businessPartners.GroupCode = Convert.ToInt32(OCRD.GroupCode.Trim());
                    int updateResult = businessPartners.Update();

                    if (updateResult != 0)
                    {
                        ListMessage.Add(new MessageAPI() { Result = "Fail", Message = OCRD.CardCode + " - No se pudo actualizar el socio de negocios: " + company.GetLastErrorDescription() });
                    }
                    else
                    {
                        ListMessage.Add(new MessageAPI() { Result = "OK", Message = OCRD.CardCode + " - Cliente: actualizado correctamente." });
                    }
                }
                else
                {
                    ListMessage.Add(new MessageAPI() { Result = "OK", Message = OCRD.CardCode + " - No se pudo encontrar el socio de negocios." });
                }
            }
            sapService.SAPB1_DISCONNECT(company);
            return Ok(ListMessage);
        }

        /// <summary>
        /// Actualiza socios de negocios de forma masiva
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<MessageAPI>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult UpdatePartnerMassive(ListOCRDMassive OCRDList)
        {
            Company company = sapService.SAPB1();

            List<MessageAPI> ListMessage = [];
            
            foreach (OCRDMassive OCRD in OCRDList?.ListOCRD ?? [])
            {
                // Obtener el objeto BusinessPartners de la API de DI
                BusinessPartners businessPartners = (BusinessPartners)company.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                if (businessPartners.GetByKey(OCRD.CardCode) == true)
                {
                    businessPartners.CardName = OCRD.CardName;
                    businessPartners.CardForeignName = OCRD.CardName;
                    businessPartners.AdditionalID = OCRD.AddId;
                    businessPartners.UnifiedFederalTaxID = OCRD.VatIdUnCmp;
                    businessPartners.Phone1 = OCRD.Phone1;
                    businessPartners.Phone2 = OCRD.Phone2;
                    businessPartners.Fax = OCRD.Fax;
                    businessPartners.Cellular = OCRD.Cellular;
                    businessPartners.EmailAddress = OCRD.E_Mail;

                    int updateResult = businessPartners.Update();

                    if (updateResult != 0)
                    {
                        ListMessage.Add(new MessageAPI() { Result = "Fail", Message = OCRD.CardCode + " - No se pudo actualizar el socio de negocios: " + company.GetLastErrorDescription() });
                    }
                    else
                    {
                        ListMessage.Add(new MessageAPI() { Result = "OK", Message = OCRD.CardCode + " - Cliente: actualizado correctamente." });
                    }
                }
                else
                {
                    ListMessage.Add(new MessageAPI() { Result = "OK", Message = OCRD.CardCode + " - No se pudo encontrar el socio de negocios." });
                }
            }
            sapService.SAPB1_DISCONNECT(company);
            return Ok(ListMessage);
        }

        /// <summary>
        /// Obtiene los datos de un socio de negocios que fue creado o actualizado en un rango de fechas
        /// </summary>
        /// <returns>Mensajes de Respuesta</returns>
        /// <response code="200">Ok</response>
        /// <response code="409">Conflict</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<OCRD>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetBusinessPartnerMassive(RequestGetBusinessPartnerMassive request)
        {
            if (request.InitialDate == null || request.FinalDate == null || request.InitialDate == "" || request.FinalDate == "")
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "Debe especificar la fecha inicial y fecha final" });
            }
            Company company = sapService.SAPB1();
            try
            {
                // Obtener el objeto BusinessPartners de la API de DI
                Recordset recordsetBP = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetBP.DoQuery("SELECT CardCode, CardName, CardFName, groupCode, groupNum, Phone1, Cellular, E_Mail, AddID, VatIdUnCmp, CreditLine FROM OCRD WITH (NOLOCK) WHERE (CreateDate  between '" + request.InitialDate + "' and '" + request.FinalDate + "') or  (UpdateDate  between '" + request.InitialDate + "' and ' " + request.FinalDate + "')");
                if (recordsetBP.RecordCount > 0)
                {
                    List<OCRD> ListOCRD = [];

                    while (!recordsetBP.EoF)
                    {
                        OCRD businessPartners = new()
                        {
                            CardCode = (string)recordsetBP.Fields.Item("CardCode").Value,
                            CardName = (string)recordsetBP.Fields.Item("CardName").Value,
                            CardFName = (string)recordsetBP.Fields.Item("CardFName").Value,
                            GroupCode = Convert.ToString(recordsetBP.Fields.Item("groupCode").Value),
                            GroupNum = Convert.ToString(recordsetBP.Fields.Item("groupNum").Value),
                            Phone1 = (string)recordsetBP.Fields.Item("Phone1").Value,
                            Cellular = (string)recordsetBP.Fields.Item("Cellular").Value,
                            E_Mail = (string)recordsetBP.Fields.Item("E_Mail").Value,
                            AddID = (string)recordsetBP.Fields.Item("AddID").Value,
                            VatIdUnCmp = (string)recordsetBP.Fields.Item("VatIdUnCmp").Value,
                            CreditLine = Convert.ToString(recordsetBP.Fields.Item("CreditLine").Value)
                        };

                        // Obtener las direcciones del socio de negocios
                        Recordset recordsetAD = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        recordsetAD.DoQuery($"SELECT Address, AdresType, Street, County, State, Country FROM CRD1 WITH (NOLOCK) WHERE CardCode = '{businessPartners.CardCode}'");

                        while (!recordsetAD.EoF)
                        {
                            CRD1 address = new()
                            {
                                Address = (string)recordsetAD.Fields.Item("Address").Value,
                                AddressType = (string)recordsetAD.Fields.Item("AdresType").Value,
                                Street = (string)recordsetAD.Fields.Item("Street").Value,
                                County = (string)recordsetAD.Fields.Item("County").Value,
                                State = (string)recordsetAD.Fields.Item("State").Value,
                                Country = (string)recordsetAD.Fields.Item("Country").Value
                            };
                            businessPartners?.Addresses?.Add(address);
                            recordsetAD.MoveNext();
                        }

                        // Obtener lOS contactos del socio de negocios
                        Recordset recordsetCT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        recordsetCT.DoQuery($"SELECT CntctCode, CardCode, Name, FirstName, LastName, Title, Tel1, Tel2, Cellolar, E_MailL FROM OCPR WITH (NOLOCK) WHERE CardCode = '{businessPartners?.CardCode ?? ""}'");

                        while (!recordsetCT.EoF)
                        {
                            OCPR contact = new()
                            {
                                CntctCode = Convert.ToString(recordsetCT.Fields.Item("CntctCode").Value),
                                CardCode = (string)recordsetCT.Fields.Item("CardCode").Value,
                                Name = (string)recordsetCT.Fields.Item("Name").Value,
                                FirstName = (string)recordsetCT.Fields.Item("FirstName").Value,
                                LasName = (string)recordsetCT.Fields.Item("LastName").Value,
                                Title = (string)recordsetCT.Fields.Item("Title").Value,
                                Tel1 = (string)recordsetCT.Fields.Item("Tel1").Value,
                                Tel2 = (string)recordsetCT.Fields.Item("Tel2").Value,
                                Cellolar = (string)recordsetCT.Fields.Item("Cellolar").Value,
                                E_MailL = (string)recordsetCT.Fields.Item("E_MailL").Value
                            };
                            businessPartners?.Contacts?.Add(contact);
                            recordsetCT.MoveNext();
                        }
                        ListOCRD.Add(businessPartners ?? new OCRD());
                        recordsetBP.MoveNext();
                    }
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(ListOCRD);
                }
                else
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(new MessageAPI() { Result = "OK", Message = "No se encontro ningun registro en el rango de fechas seleccionado" });
                }
            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar el listado de socio de negocios - " + ex.Message });
            }
        }
    }
}
