using CanellaMovilBackend.Filters.UserFilter;
using CanellaMovilBackend.Models;
using CanellaMovilBackend.Models.SAPModels.Inventory;
using CanellaMovilBackend.Service.SAPService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;
using static CanellaMovilBackend.Models.SAPModels.Inventory.RequestDataInventory;

namespace CanellaMovilBackend.Controllers.SAPControllers
{
    /// <summary>
    /// Controlador del Inventario
    /// </summary>
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [ServiceFilter(typeof(RoleFilter))]
    [ServiceFilter(typeof(ResultAllFilter))]
    public class InventoryController : ControllerBase
    {
        private readonly ISAPService sapService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sapService"></param>
        public InventoryController(ISAPService sapService)
        {
            this.sapService = sapService;
        }

        /// <summary>
        /// Obtiene los datos de inventario de un articulo
        /// </summary>
        /// <returns>Retorna un JSON con los datos del inventario</returns>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(OITM), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetItemInventory(RequestGetItem request)
        {
            Company company = sapService.SAPB1();
            try
            {
                // Obtener el item de la base de datos de SAP
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery("SELECT  ItemName, SellItem, PrchSeItem, ItmsGrpCod, ItemCode, '' Barcode, AvgPrice, OnHand,'' Transito,'' Price  FROM OITM WITH (NOLOCK) WHERE ItemCode='" + request.ItemCode + "'");
                if (recordset.RecordCount > 0)
                {
                    OITM? item = null;
                    while (!recordset.EoF)
                    {
                        item = new OITM
                        {
                            ItemName = Convert.ToString(recordset.Fields.Item("ItemName").Value),
                            SellItem = Convert.ToString(recordset.Fields.Item("SellItem").Value),
                            PrchSeItem = Convert.ToString(recordset.Fields.Item("PrchSeItem").Value),
                            ItmsGrpCod = Convert.ToString(recordset.Fields.Item("ItmsGrpCod").Value),
                            ItemCode = Convert.ToString(recordset.Fields.Item("ItemCode").Value),
                            Barcode = Convert.ToString(recordset.Fields.Item("Barcode").Value),
                            AvgPrice = Convert.ToString(recordset.Fields.Item("AvgPrice").Value),
                            OnHand = Convert.ToString(recordset.Fields.Item("OnHand").Value),
                            Transito = Convert.ToString(recordset.Fields.Item("Transito").Value),
                            Price = Convert.ToString(recordset.Fields.Item("Price").Value)
                        };

                        Recordset recordsetWhs = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        recordsetWhs.DoQuery("SELECT WhsCode, OnHand FROM OITW WITH (NOLOCK) WHERE OnHand >= 0 and ItemCode  ='" + Convert.ToString(recordset.Fields.Item("ItemCode").Value + "'"));

                        while (!recordsetWhs.EoF)
                        {
                            OITW whsList = new()
                            {
                                WhsCode = Convert.ToString(recordsetWhs.Fields.Item("WhsCode").Value),
                                OnHand = Convert.ToString(recordsetWhs.Fields.Item("OnHand").Value)
                            };
                            item?.WhsList?.Add(whsList);
                            recordsetWhs.MoveNext();
                        }

                        Recordset recordsetPList = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        recordsetPList.DoQuery("SELECT T1.ItemCode, T3.ListName, T2.Price, T1.AvgPrice FROM OITM T1 WITH (NOLOCK) INNER JOIN ITM1 T2 WITH (NOLOCK) ON T1.ItemCode  = T2.ItemCode INNER JOIN OPLN T3 WITH (NOLOCK) ON T2.PriceList = T3.ListNUm WHERE T2.Price >0 and T1.ItemCode  ='" + Convert.ToString(recordset.Fields.Item("ItemCode").Value + "'"));

                        while (!recordsetPList.EoF)
                        {
                            PriceList priceList = new()
                            {
                                ItemCode = Convert.ToString(recordsetPList.Fields.Item("ItemCode").Value),
                                ListName = Convert.ToString(recordsetPList.Fields.Item("ListName").Value),
                                Price = Convert.ToString(recordsetPList.Fields.Item("Price").Value),
                                AvgPrice = Convert.ToString(recordsetPList.Fields.Item("AvgPrice").Value)
                            };
                            item?.PriceList?.Add(priceList);
                            recordsetPList.MoveNext();
                        }
                        recordset.MoveNext();
                    }
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(item);
                }
                else
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar el item con el codigo: " + request.ItemCode });
                }
            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar el item con el codigo: " + request.ItemCode + " - " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene las existencias de todos los productos o de uno en especifico
        /// </summary>
        /// <returns>Retorna un JSON con los datos del inventario</returns>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>5
        [HttpPost]
        [ProducesResponseType(typeof(OITW), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetItemExistence(RequestGetItem request)
        {
            Company company = sapService.SAPB1();
            try
            {
                // Obtener el item de la base de datos de SAP
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery("SELECT T1.ItemCode, T1.WhsCode, T1.OnHand FROM OITW T1 WITH (NOLOCK) WHERE T1.OnHand >= 1 " + (!(request.ItemCode == "*") ? " AND T1.ItemCode = '" + request.ItemCode + "' " : ""));
                if (recordset.RecordCount > 0)
                {
                    List<OITW> ItemList = [];
                    while (!recordset.EoF)
                    {
                        OITW item = new()
                        {
                            ItemCode = Convert.ToString(recordset.Fields.Item("ItemCode").Value),
                            WhsCode = Convert.ToString(recordset.Fields.Item("WhsCode").Value),
                            OnHand = Convert.ToString(recordset.Fields.Item("OnHand").Value)
                        };
                        ItemList.Add(item);
                        recordset.MoveNext();
                    }
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(ItemList);
                }
                else
                {
                    sapService.SAPB1_DISCONNECT(company);
                    return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo encontrar el item con el codigo: " + request.ItemCode });
                }
            }
            catch (Exception ex)
            {
                sapService.SAPB1_DISCONNECT(company);
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo obtener el listado de existencias - Error: " + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene los datos de articulos de productos que fueron creados o modificados en un rango de fechas
        /// </summary>
        /// <returns>Retorna un JSON con los datos del inventario</returns>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<OITM>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetItemInventoryMassive(RequestGetItemMassive request)
        {
            if (request.InitialDate == null || request.FinalDate == null || request.InitialDate == "" || request.FinalDate == "")
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "Debe especificar la fecha inicial y fecha final" });
            }
            Company company = sapService.SAPB1();
            try
            {
                // Obtener el item de la base de datos de SAP
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery("SELECT  ItemName, SellItem, PrchSeItem, ItmsGrpCod, ItemCode, '' Barcode, AvgPrice, OnHand,'' Transito,'' Price  FROM OITM WITH (NOLOCK) WHERE (CreateDate  between '" + request.InitialDate + "' and '" + request.FinalDate + "') or  (UpdateDate  between '" + request.InitialDate + "' and '" + request.FinalDate + "')");
                if (recordset.RecordCount > 0)
                {
                    List<OITM>? itemList = [];
                    while (!recordset.EoF)
                    {
                        OITM item = new()
                        {
                            ItemName = Convert.ToString(recordset.Fields.Item("ItemName").Value),
                            SellItem = Convert.ToString(recordset.Fields.Item("SellItem").Value),
                            PrchSeItem = Convert.ToString(recordset.Fields.Item("PrchSeItem").Value),
                            ItmsGrpCod = Convert.ToString(recordset.Fields.Item("ItmsGrpCod").Value),
                            ItemCode = Convert.ToString(recordset.Fields.Item("ItemCode").Value),
                            Barcode = Convert.ToString(recordset.Fields.Item("Barcode").Value),
                            AvgPrice = Convert.ToString(recordset.Fields.Item("AvgPrice").Value),
                            OnHand = Convert.ToString(recordset.Fields.Item("OnHand").Value),
                            Transito = Convert.ToString(recordset.Fields.Item("Transito").Value),
                            Price = Convert.ToString(recordset.Fields.Item("Price").Value)
                        };

                        Recordset recordsetWhs = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        recordsetWhs.DoQuery("SELECT WhsCode, OnHand FROM OITW WITH (NOLOCK) WHERE OnHand >= 0 and ItemCode  ='" + Convert.ToString(recordset.Fields.Item("ItemCode").Value + "'"));

                        while (!recordsetWhs.EoF)
                        {
                            OITW whsList = new()
                            {
                                WhsCode = Convert.ToString(recordsetWhs.Fields.Item("WhsCode").Value),
                                OnHand = Convert.ToString(recordsetWhs.Fields.Item("OnHand").Value)
                            };
                            item?.WhsList?.Add(whsList);
                            recordsetWhs.MoveNext();
                        }

                        Recordset recordsetPList = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        recordsetPList.DoQuery("SELECT T1.ItemCode, T3.ListName, T2.Price, T1.AvgPrice FROM OITM T1 WITH (NOLOCK) INNER JOIN ITM1 T2 WITH (NOLOCK) ON T1.ItemCode  = T2.ItemCode INNER JOIN OPLN T3 WITH (NOLOCK) ON T2.PriceList = T3.ListNUm WHERE T2.Price >0 and T1.ItemCode  ='" + Convert.ToString(recordset.Fields.Item("ItemCode").Value + "'"));

                        while (!recordsetPList.EoF)
                        {
                            PriceList priceList = new()
                            {
                                ItemCode = Convert.ToString(recordsetPList.Fields.Item("ItemCode").Value),
                                ListName = Convert.ToString(recordsetPList.Fields.Item("ListName").Value),
                                Price = Convert.ToString(recordsetPList.Fields.Item("Price").Value),
                                AvgPrice = Convert.ToString(recordsetPList.Fields.Item("AvgPrice").Value)
                            };
                            item?.PriceList?.Add(priceList);
                            recordsetPList.MoveNext();
                        }
                        itemList?.Add(item ?? new OITM());
                        recordset.MoveNext();
                    }
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(itemList);
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
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo obtener el listado de items - Error" + ex.Message });
            }
        }

        /// <summary>
        /// Obtiene los datos de articulos de productos que fueron creados o modificados en un rango de fechas
        /// </summary>
        /// <returns>Retorna un JSON con los datos del inventario</returns>
        /// <response code="200">Obtención de datos exitoso</response>
        /// <response code="409">Mensaje de error</response>
        [HttpPost]
        [ProducesResponseType(typeof(List<OITM>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageAPI), StatusCodes.Status409Conflict)]
        public ActionResult GetItemInventoryByDivision(RequestGetItemByDivisionMassive request)
        {
            if (request.InitialDate == null || request.FinalDate == null || request.InitialDate == "" || request.FinalDate == "")
            {
                return Conflict(new MessageAPI() { Result = "Fail", Message = "Debe especificar la fecha inicial y fecha final" });
            }
            Company company = sapService.SAPB1();
            try
            {
                // Obtener el item de la base de datos de SAP
                Recordset recordset = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordset.DoQuery("SELECT  ItemName, SellItem, PrchSeItem, ItmsGrpCod, ItemCode, '' Barcode, AvgPrice, OnHand,'' Transito,'' Price  FROM OITM WITH (NOLOCK) WHERE ((CreateDate  between '" + request.InitialDate + "' and '" + request.FinalDate + "') or  (UpdateDate  between '" + request.InitialDate + "' and '" + request.FinalDate + "')) and U_ClasDivision = '"+ request.Division +"'");
                if (recordset.RecordCount > 0)
                {
                    List<OITM> itemList = [];
                    while (!recordset.EoF)
                    {
                        OITM item = new()
                        {
                            ItemName = Convert.ToString(recordset.Fields.Item("ItemName").Value),
                            SellItem = Convert.ToString(recordset.Fields.Item("SellItem").Value),
                            PrchSeItem = Convert.ToString(recordset.Fields.Item("PrchSeItem").Value),
                            ItmsGrpCod = Convert.ToString(recordset.Fields.Item("ItmsGrpCod").Value),
                            ItemCode = Convert.ToString(recordset.Fields.Item("ItemCode").Value),
                            Barcode = Convert.ToString(recordset.Fields.Item("Barcode").Value),
                            AvgPrice = Convert.ToString(recordset.Fields.Item("AvgPrice").Value),
                            OnHand = Convert.ToString(recordset.Fields.Item("OnHand").Value),
                            Transito = Convert.ToString(recordset.Fields.Item("Transito").Value),
                            Price = Convert.ToString(recordset.Fields.Item("Price").Value)
                        };

                        Recordset recordsetWhs = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        recordsetWhs.DoQuery("SELECT WhsCode, OnHand FROM OITW WITH (NOLOCK) WHERE OnHand >= 0 and ItemCode  ='" + Convert.ToString(recordset.Fields.Item("ItemCode").Value + "'"));


                        while (!recordsetWhs.EoF)
                        {
                            OITW whsList = new()
                            {
                                WhsCode = Convert.ToString(recordsetWhs.Fields.Item("WhsCode").Value),
                                OnHand = Convert.ToString(recordsetWhs.Fields.Item("OnHand").Value)
                            };
                            item?.WhsList?.Add(whsList);
                            recordsetWhs.MoveNext();
                        }

                        Recordset recordsetPList = (Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
                        recordsetPList.DoQuery("SELECT T1.ItemCode, T3.ListName, T2.Price, T1.AvgPrice FROM OITM T1 WITH (NOLOCK) INNER JOIN ITM1 T2 WITH (NOLOCK) ON T1.ItemCode  = T2.ItemCode INNER JOIN OPLN T3 WITH (NOLOCK) ON T2.PriceList = T3.ListNUm WHERE T2.Price >0 and T1.ItemCode  ='" + Convert.ToString(recordset.Fields.Item("ItemCode").Value + "'"));

                        while (!recordsetPList.EoF)
                        {
                            PriceList priceList = new()
                            {
                                ItemCode = Convert.ToString(recordsetPList.Fields.Item("ItemCode").Value),
                                ListName = Convert.ToString(recordsetPList.Fields.Item("ListName").Value),
                                Price = Convert.ToString(recordsetPList.Fields.Item("Price").Value),
                                AvgPrice = Convert.ToString(recordsetPList.Fields.Item("AvgPrice").Value)
                            };
                            item?.PriceList?.Add(priceList);
                            recordsetPList.MoveNext();
                        }
                        itemList?.Add(item ?? new OITM());
                        recordset.MoveNext();
                    }
                    sapService.SAPB1_DISCONNECT(company);
                    return Ok(itemList);
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
                return Conflict(new MessageAPI() { Result = "Fail", Message = "No se pudo obtener el listado de items - Error" + ex.Message });
            }
        }
    }
}
