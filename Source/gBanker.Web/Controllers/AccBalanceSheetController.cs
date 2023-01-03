﻿using gBanker.Service;
using gBanker.Service.ReportExecutionService;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class AccBalanceSheetController : BaseController
    {
        #region Variables
        private readonly IAccTrxMasterService accTrxMasterService;
        private readonly IAccTrxDetailService accTrxDetailService;
        private readonly IAccChartService accChartService;
        private readonly IAccLastVoucherService accLastVoucherService;
        private readonly IProcessInfoService processInfoService;
        private readonly IAccReportService accReportService;
        private readonly IOfficeService officeService;
        private readonly IArchiveDbMappingService archiveDbMappingService;
        private readonly IUltimateReportService ultimateReportService;
        public AccBalanceSheetController(IAccTrxMasterService accTrxMasterService, IAccTrxDetailService accTrxDetailService, IAccChartService accChartService, IAccLastVoucherService accLastVoucherService, IProcessInfoService processInfoService, IAccReportService accReportService, IOfficeService officeService, IArchiveDbMappingService archiveDbMappingService, IUltimateReportService ultimateReportService)
        {
            this.accTrxMasterService = accTrxMasterService;
            this.accTrxDetailService = accTrxDetailService;
            this.accChartService = accChartService;
            this.accLastVoucherService = accLastVoucherService;
            this.processInfoService = processInfoService;
            this.accReportService = accReportService;
            this.officeService = officeService;
            this.archiveDbMappingService = archiveDbMappingService;
            this.ultimateReportService = ultimateReportService;
        }
        #endregion

        #region Methods
        public JsonResult GetHOList()
        {
            var First_Level = officeService.GetByOfficeOrgID(Convert.ToInt32(SessionHelper.LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 1 && c.FirstLevel == First_Level.FirstLevel);
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetZoneList(string HO_val)
        {
            var ofcId = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 2 && c.FirstLevel == ofcId && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAreaList(string HO_val,string zone_val)
        {
            var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 3 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOfficeList(string HO_val, string zone_val, string area_val)
        {
            var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var area_code = officeService.GetById(Convert.ToInt32(area_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 4 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.ThirdLevel == area_code && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetZoneListByOffice(string HO_val)
        {
            //var ofcId = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 2 && c.OfficeID == LoginUserOfficeID && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOfficeListByOffice(string HO_val, string zone_val, string area_val)
        {
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 4 && c.OfficeID == LoginUserOfficeID && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAreaListByOffice(string HO_val, string zone_val)
        {
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 3 && c.OfficeID == LoginUserOfficeID && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GenerateBalanceSheetReport(string office_id, string to_date, string acc_level, string qType)
        {

            try
            {
                if (qType == "1" || qType == "0")
                {
                    office_id = "1";
                }
                var main_param = new object();
                if (SessionHelper.LoginUserOrganizationID == 12 || SessionHelper.LoginUserOrganizationID == 10)
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = to_date, ToDate = to_date, qtype = (string.IsNullOrEmpty(qType) ? 0 : int.Parse(qType)) };
                else
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = to_date, ToDate = to_date };

                var liabilities_param = new { to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='L'", AllOffice = qType };
                var assets_param = new { to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='A'", AllOffice = qType };
                var allVouchers = accReportService.GetDataIncExpReport(main_param);
                var Liabilities_DB = accReportService.GetNewAccDataForReportGUK(liabilities_param, "Proc_Rpt_Acc_BalanceSheet");
                var Assets_DB = accReportService.GetNewAccDataForReportGUK(assets_param, "Proc_Rpt_Acc_BalanceSheet");
                var subReportDB = new Dictionary<string, DataTable>();
                //subReportDB.Add("rpt_acc_sub_liabilities", Liabilities_DB.Tables!=null && Liabilities_DB.Tables.Count>0? Liabilities_DB.Tables[0]:new DataTable());
                //subReportDB.Add("rpt_acc_sub_asset", Assets_DB.Tables != null && Assets_DB.Tables.Count > 0 ? Assets_DB.Tables[0] : new DataTable());
                
                subReportDB.Add("rpt_acc_sub_liabilities", Liabilities_DB.Tables[0]);
                subReportDB.Add("rpt_acc_sub_asset", Assets_DB.Tables[0]);

                ReportHelper.PrintWithSubReport("rpt_acc_balance_sheet.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_balance_sheet());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateBalanceSheetReportArchive(string office_id, string to_date, string acc_level, string qType)
        {

            try
            {
                if (qType == "1" || qType == "0")
                {
                    office_id = "1";
                }
                var main_param = new object();
                if (SessionHelper.LoginUserOrganizationID == 12 || SessionHelper.LoginUserOrganizationID == 10)
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = to_date, ToDate = to_date, qtype = (string.IsNullOrEmpty(qType) ? 0 : int.Parse(qType)) };
                else
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = to_date, ToDate = to_date };


                //check archive DB

                var archiveMapDb = "";
                var archiveMap = archiveDbMappingService.GetMany(p => p.OrgId == LoggedInOrganizationID).FirstOrDefault();
                if (archiveMap != null)
                    archiveMapDb = archiveMap.ArchiveDbName;                

                var liabilities_param = new { to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='L'", AllOffice = qType, archiveMapDb = archiveMapDb };
                var assets_param = new { to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='A'", AllOffice = qType, archiveMapDb = archiveMapDb };
                var allVouchers = accReportService.GetDataIncExpReport(main_param);
                var Liabilities_DB = accReportService.GetNewAccDataForReportGUK(liabilities_param, "Proc_Rpt_Acc_BalanceSheet_Archive");
                var Assets_DB = accReportService.GetNewAccDataForReportGUK(assets_param, "Proc_Rpt_Acc_BalanceSheet_Archive");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("rpt_acc_sub_liabilities", Liabilities_DB.Tables[0]);
                subReportDB.Add("rpt_acc_sub_asset", Assets_DB.Tables[0]);
                ReportHelper.PrintWithSubReport("rpt_acc_balance_sheet.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_balance_sheet());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        /* 
        public ActionResult GenerateBalanceSheetReport(string office_id, string to_date, string acc_level, string qType)
        {
            DataSet allVouchers;
            DataSet Liabilities_DB;
            DataSet Assets_DB;
            try
            {
                if (qType == "1" || qType == "0")
                {
                    office_id = "1";
            }
                var main_param = new object();
                if (SessionHelper.LoginUserOrganizationID == 12 || SessionHelper.LoginUserOrganizationID == 10)
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = to_date, ToDate = to_date, qtype = (string.IsNullOrEmpty(qType) ? 0 : int.Parse(qType)) };
                else
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = to_date, ToDate = to_date };

                var liabilities_param = new { to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='L'", AllOffice = qType };
                var assets_param = new { to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='A'", AllOffice = qType };


                //check archive DB starts

                var archiveMapDb = "";
                var archiveMap = archiveDbMappingService.GetMany(p => p.OrgId == LoggedInOrganizationID).FirstOrDefault();
                if (archiveMap != null)
                    archiveMapDb = archiveMap.ArchiveDbName;
                var paramCheckArchive = new { office_id = office_id, from_date = "", to_date = to_date, AllOffice = qType, archiveMapDb = archiveMapDb };
                var getData = ultimateReportService.GetDataWithParameter(paramCheckArchive, "CheckAccTrxMasterDataExists");

                if (getData.Tables[0].AsEnumerable().Any())
                {
                    var main_param_archive = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = to_date, ToDate = to_date, archiveMapDb = archiveMapDb };
                    var liabilities_param_archive = new { to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='L'", AllOffice = qType, archiveMapDb = archiveMapDb };
                    var assets_param_archive = new { to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='A'", AllOffice = qType, archiveMapDb = archiveMapDb };
                    
                    Liabilities_DB = accReportService.GetNewAccDataForReportGUK(liabilities_param_archive, "Proc_Rpt_Acc_BalanceSheet_Archive");
                    Assets_DB = accReportService.GetNewAccDataForReportGUK(assets_param_archive, "Proc_Rpt_Acc_BalanceSheet_Archive");
                }
                else
                {                    
                    Liabilities_DB = accReportService.GetNewAccDataForReportGUK(liabilities_param, "Proc_Rpt_Acc_BalanceSheet");
                    Assets_DB = accReportService.GetNewAccDataForReportGUK(assets_param, "Proc_Rpt_Acc_BalanceSheet");
                }
                allVouchers = accReportService.GetDataIncExpReport(main_param);
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("rpt_acc_sub_liabilities", Liabilities_DB.Tables[0]);
                subReportDB.Add("rpt_acc_sub_asset", Assets_DB.Tables[0]);
                ReportHelper.PrintWithSubReport("rpt_acc_balance_sheet.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_balance_sheet());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        */
        #endregion

        #region Events
        // GET: AccBalanceSheet
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;


            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate;
            }
            return View();
        }
        public ActionResult IndexByOffice()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate;
            }
            return View();
        }
        // GET: AccBalanceSheet/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: AccBalanceSheet/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: AccBalanceSheet/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: AccBalanceSheet/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: AccBalanceSheet/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: AccBalanceSheet/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: AccBalanceSheet/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult BalanceSheetArchive()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate;
            }
            return View();
        }
        #endregion
    }
}
