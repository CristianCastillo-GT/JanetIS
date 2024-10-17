using CanellaMovilBackend.Filters;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.CQMModels;
using CanellaMovilBackend.Models.SAPModels.PageCanon;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;
using static CanellaMovilBackend.Models.SAPModels.PageCanon.CCRequestData;
using static CanellaMovilBackend.Models.SAPModels.PageCanon.DBRequestData;
using static CanellaMovilBackend.Models.SAPModels.PageCanon.SORequestData;
using static CanellaMovilBackend.Models.SAPModels.PageCanon.SWRequestData;
using static CanellaMovilBackend.Models.SAPModels.PageCanon.INRequestData;
using static CanellaMovilBackend.Models.SAPModels.PageCanon.IDRequestData;

namespace CanellaMovilBackend.Controllers.CanonControllers
{
    /// <summary>
    /// Controlador de canon
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(SAPConnectionFilter))]
    public class CanonController : Controller
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        /// 
        public CanonController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Obtiene los datos de los clientes para realizar login y mi cuenta en pagina canon
        /// </summary>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<CardCode>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetCardCode(RequestGetCardCode request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                request.AddId ??= "";

                // Obtener el objeto categoria de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WCAN_SELECT_CARD_CODE '" + request.AddId + "'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<CardCode> List_CardCode = [];
                    while (!recordsetUT.EoF)
                    {
                        CardCode? code = new()
                        {
                            CardCod = (string)recordsetUT.Fields.Item("CARDCODE").Value,
                            CardName = (string)recordsetUT.Fields.Item("CARDNAME").Value,
                            Phone1 = (string)recordsetUT.Fields.Item("PHONE1").Value,
                            Cellular = (string)recordsetUT.Fields.Item("Cellular").Value,
                            E_Mail = (string)recordsetUT.Fields.Item("E_Mail").Value,
                            AvailableCredit = (string)recordsetUT.Fields.Item("AvailableCredit").Value,
                            AddID = (string)recordsetUT.Fields.Item("Addid").Value,
                            InvoiceOpen = (string)recordsetUT.Fields.Item("InvoiceOpen").Value
                        };
                        List_CardCode.Add(code);
                        recordsetUT.MoveNext();
                    }
                    return Ok(List_CardCode);
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la lista con informacion del cliente" });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo consultar lista de categorias: " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene los datos de los clientes para la creacion del dashboard en pagina canon
        /// </summary>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<CardCode>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetDashBoard(RequestGetDashBoard request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                request.AddId ??= "";

                // Obtener el objeto categoria de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WCAN_SELECT_DASH_BOARD '" + request.AddId + "'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<DashBoard> List_DashBoard = [];
                    while (!recordsetUT.EoF)
                    {
                        DashBoard code = new()
                        {
                            CardName = (string)recordsetUT.Fields.Item("CardName").Value,
                            CardCode = (string)recordsetUT.Fields.Item("CardCode").Value,
                            InvoiceOpen = (string)recordsetUT.Fields.Item("InvoiceOpen").Value,
                            OrdersPendingDelivery = (string)recordsetUT.Fields.Item("OrdersPendingDelivery").Value,
                            AvailableCredit = (string)recordsetUT.Fields.Item("AvailableCredit").Value
                        };

                        List_DashBoard.Add(code);
                        recordsetUT.MoveNext();
                    }
                    return Ok(List_DashBoard);
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la lista con informacion del DashBoard" });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo consultar lista de categorias: " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene los datos de las ordenes por medio del nit del cliente
        /// </summary>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<CardCode>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetSalesOrder(RequestGetSalesOrder request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                request.AddId ??= "";

                // Obtener el objeto categoria de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WCAN_SELECT_SALES_ORDER '" + request.AddId + "'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<SalesOrder> List_SalesOrder = [];
                    while (!recordsetUT.EoF)
                    {
                        SalesOrder code = new()
                        {
                            DocNum = (string)recordsetUT.Fields.Item("DocNum").Value,
                            CardCode = (string)recordsetUT.Fields.Item("CardCode").Value,
                            CardName = (string)recordsetUT.Fields.Item("CardName").Value,
                            DocEstatus = (string)recordsetUT.Fields.Item("DocEstatus").Value,
                            DocTotal = (string)recordsetUT.Fields.Item("DocTotal").Value,
                            DocDate = (string)recordsetUT.Fields.Item("DocDate").Value
                        };

                        List_SalesOrder.Add(code);
                        recordsetUT.MoveNext();
                    }
                    return Ok(List_SalesOrder);
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la lista con informacion de los documentos solicitados" });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo consultar lista de documentos: " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene las existencias por bodega segun articulo y lista de precio que tiene asociada el cliente
        /// </summary>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<CardCode>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetStockWhareHouse(RequestGetStockWhareHouse request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                request.AddId ??= "";

                // Obtener el objeto categoria de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WCAN_SELECT_STOCK_BY_WHAREHOUSE '" + request.AddId + "','" + request.ItemCode + "'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<StockWhareHouse> List_Stock = [];
                    while (!recordsetUT.EoF)
                    {
                        StockWhareHouse? code = new()
                        {
                            ItemCode = (string)recordsetUT.Fields.Item("ItemCode").Value,
                            ItemName = (string)recordsetUT.Fields.Item("ItemName").Value,
                            WhsCode = (string)recordsetUT.Fields.Item("WhsCode").Value,
                            WhsName = (string)recordsetUT.Fields.Item("WhsName").Value,
                            PriceList = (string)recordsetUT.Fields.Item("PriceList").Value,
                            Stock = (string)recordsetUT.Fields.Item("Stock").Value
                        };

                        List_Stock.Add(code);
                        recordsetUT.MoveNext();
                    }
                    return Ok(List_Stock);
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la lista con informacion de inventario por bodega" });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo consultar lista de inventario por bodega: " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene inventario lista de precio que tiene asociada el cliente
        /// </summary>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<CardCode>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetInventory(RequestGetInventory request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                request.AddId ??= "";

                // Obtener el objeto categoria de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WCAN_SELECT_INVENTORY '" + request.AddId + "'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<Inventory> List_Inventory = [];
                    while (!recordsetUT.EoF)
                    {
                        Inventory code = new()
                        {
                            ItemCode = (string)recordsetUT.Fields.Item("ItemCode").Value,
                            ItemName = (string)recordsetUT.Fields.Item("ItemName").Value,
                            Fotografia = (string)recordsetUT.Fields.Item("Fotografia").Value,
                            Stock = (string)recordsetUT.Fields.Item("Stock").Value,
                            Price = (string)recordsetUT.Fields.Item("Price").Value,
                            CodeDivision = (string)recordsetUT.Fields.Item("CodeDivision").Value,
                            NameDivision = (string)recordsetUT.Fields.Item("NameDivision").Value,
                            CodeCategoria = (string)recordsetUT.Fields.Item("CodeCategoria").Value,
                            NameCategoria = (string)recordsetUT.Fields.Item("NameCategoria").Value,
                            CodeTipo = (string)recordsetUT.Fields.Item("CodeTipo").Value,
                            NameTipo = (string)recordsetUT.Fields.Item("NameTipo").Value
                        };

                        List_Inventory.Add(code);
                        recordsetUT.MoveNext();
                    }
                    return Ok(List_Inventory);
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la lista con informacion de inventario" });
                }

            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo consultar lista de inventario: " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene inventario de un articulo segun lista de precio que tiene asociada el cliente
        /// </summary>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<CardCode>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetItemCodeDetail(RequestGetInventoryDetail request)
        {
            try
            {
                CompanyConnection companyConnection = sapService.SAPB1();
                Company company = companyConnection.Company;

                request.AddId ??= "";

                // Obtener el objeto categoria de la API de DI
                Recordset recordsetUT = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetUT.DoQuery("EXEC WCAN_SELECT_ITEM_DETAIL '" + request.AddId + "','" + request.ItemCode + "'");

                if (recordsetUT.RecordCount > 0)
                {
                    List<Inventory> List_Inventory = [];
                    while (!recordsetUT.EoF)
                    {
                        Inventory? code = new()
                        {
                            ItemCode = (string)recordsetUT.Fields.Item("ItemCode").Value,
                            ItemName = (string)recordsetUT.Fields.Item("ItemName").Value,
                            Fotografia = (string)recordsetUT.Fields.Item("Fotografia").Value,
                            Stock = (string)recordsetUT.Fields.Item("Stock").Value,
                            Price = (string)recordsetUT.Fields.Item("Price").Value,
                            CodeDivision = (string)recordsetUT.Fields.Item("CodeDivision").Value,
                            NameDivision = (string)recordsetUT.Fields.Item("NameDivision").Value,
                            CodeCategoria = (string)recordsetUT.Fields.Item("CodeCategoria").Value,
                            NameCategoria = (string)recordsetUT.Fields.Item("NameCategoria").Value,
                            CodeTipo = (string)recordsetUT.Fields.Item("CodeTipo").Value,
                            NameTipo = (string)recordsetUT.Fields.Item("NameTipo").Value
                        };

                        List_Inventory.Add(code);
                        recordsetUT.MoveNext();
                    }
                    return Ok(List_Inventory);
                }
                else
                {
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar la lista con informacion de inventario" });
                }
            }
            catch (Exception ex)
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo consultar lista de inventario: " + ex.Message });
            }
        }
    }
}
