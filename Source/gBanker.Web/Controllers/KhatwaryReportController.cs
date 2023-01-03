﻿using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gBanker.Service.ReportExecutionService;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace gBanker.Web.Controllers
{
    public class KhatwaryReportController : BaseController
    {
        #region Variables
        private readonly IWeeklyReportService weeklyReportService;
        private readonly IDailyReportService dailyReportService;
        
        public KhatwaryReportController(IWeeklyReportService weeklyReportService, IDailyReportService dailyReportService)
        {
            this.weeklyReportService = weeklyReportService;
            this.dailyReportService = dailyReportService;
            
 
        }
        #endregion

        #region Methods
        public JsonResult GetMainProductList()
        {
            try
            {
                List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
                var productList = weeklyReportService.GetMainProductList();

                List_ProductViewModel = productList.Tables[0].AsEnumerable()
                .Select(row => new ProductViewModel
                {
                    MainProductCode = row.Field<string>("MainProductCode"),
                    MainItemName = row.Field<string>("MainItemName")

                }).ToList();

                return Json(List_ProductViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetMainSavingProductList()
        {
            try
            {
                List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
                var productList = weeklyReportService.GetSavingMainProductList();

                List_ProductViewModel = productList.Tables[0].AsEnumerable()
                .Select(row => new ProductViewModel
                {
                    MainProductCode = row.Field<string>("MainProductCode"),
                    MainItemName = row.Field<string>("MainItemName")

                }).ToList();

                return Json(List_ProductViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetKhatWaryReport(string MainProductCode, string DateFrom, string DateTo, string Qtype)
        {
            try
            {

                var param = new { Qtype = Qtype, OfficeID = SessionHelper.LoginUserOfficeID, MainProductCode = MainProductCode.Trim(), Date_From = DateFrom, Date_To = DateTo };
                var Weeklys = weeklyReportService.GetKhatWaryReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                          
                ReportHelper.PrintReport("KhatWaryReport.rpt", Weeklys.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GetKhatWaryReportExport(string MainProductCode, string DateFrom, string DateTo, string Qtype)
        {
            try
            {

                var param = new { Qtype = Qtype, OfficeID = SessionHelper.LoginUserOfficeID, MainProductCode = MainProductCode.Trim(), Date_From = DateFrom, Date_To = DateTo };
                var allRepaymentSchedule = weeklyReportService.GetKhatWaryReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.ExportExcelReport("KhatWaryReport.rpt", allRepaymentSchedule.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GetRebateReport(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Date1 = DateFrom, Date2 = DateTo };
                var Weeklys = weeklyReportService.GetRebateReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("RebateInfo.rpt", Weeklys.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GetDualLoanReport(string DateFrom)
        {
            try
            {


                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, TrxDate = DateFrom };
                var Weeklys = weeklyReportService.GetDualLoanReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
               

                ReportHelper.PrintReport("rptDual_Loan_Info.rpt", Weeklys.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GetNagtiveLedgerBalance(string DateFrom, string DateTo)
        {
            try
            {
           
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var Weeklys = weeklyReportService.GetNegativeLoanLedgerBalance(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                             
                ReportHelper.PrintReport("NegativeLedgerBalance.rpt", Weeklys.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GetStaffWiseSpecialSavingReport(string DateFrom, string DateTo)
        {
            try
            {
               
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Date_From = DateFrom, Date_To = DateTo };
                var Weeklys = weeklyReportService.GetStaffWiseSpecialSavingReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                              
                ReportHelper.PrintReport("StaffWiseSpecialSavingReport.rpt", Weeklys.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GetMemberTransfer(string DateFrom, string DateTo, int Qtype)
        {
            try
            {

                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, };
                    var Weeklys = weeklyReportService.GetMemberTransfer(param);

                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);


                    ReportHelper.PrintReport("rptmembertransfer.rpt", Weeklys.Tables[0], reportParam);

                    
               

               
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GetDailyRecoverableAndRecoveryRegisterCenterDateWise(string DateFrom, string DateTo, int Qtype, int CenterID)
        {
            try
            {
                    
                               
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype, CenterID= CenterID };
                DataSet Weeklys;
                if (LoggedInOrganizationID==113)
                {
                     Weeklys = weeklyReportService.DailyRecoverableAndRecoveryRegisterCenterDateWise(param);
                }
                else
                 Weeklys = weeklyReportService.DailyRecoverableAndRecoveryRegisterCenterDateWise(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                if (LoggedInOrganizationID==113)
                {
                    if (Qtype == 1)
                    {
                        ReportHelper.PrintReport("DailyRecovarableAndRecoveryRegisterCenterDateWise_Adra.rpt", Weeklys.Tables[0], reportParam);
                    }
                    if (Qtype == 2)
                    {
                        ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);
                    }
                    if (Qtype == 5)
                    {
                        ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);
                    }
                }
                else
                {
                    if (Qtype == 1)
                    {
                        ReportHelper.PrintReport("DailyRecovarableAndRecoveryRegisterCenterDateWise.rpt", Weeklys.Tables[0], reportParam);
                    }
                    if (Qtype == 2)
                    {
                        ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);
                    }
                    if (Qtype == 5)
                    {
                        ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);
                    }
                }
                
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GetRecoverableAndRecoveryRegisterDSKFormat(string DateTo, int Qtype)
        {
            try
            {


                var paramValues = new List<ParameterValue>();

                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = Convert.ToString(Qtype) });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = Convert.ToString(LoggedInOrganizationID) });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = Convert.ToString(LoginUserOfficeID) });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = DateTo});
                PrintSSRSReport("/gBanker_Reports/Recoverable_Register", paramValues.ToArray(), "gBankerDbContext");
                return Content(string.Empty);


                //var param = new { Qtype= Qtype, Org=LoggedInOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                //var Weeklys = weeklyReportService.RecoverableAndRecoveryRegisterDSKFormat(param);

                //var reportParam = new Dictionary<string, object>();
                //reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                //reportParam.Add("DateTo", DateTo);

                //if (Qtype == 1)
                //{
                //    ReportHelper.PrintReport("DailyRecovarableAndRecoveryRegisterCenterDateWise.rpt", Weeklys.Tables[0], reportParam);


                //}
                //if (Qtype == 2)
                //{


                //    ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);


                //}


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        // GET: KhatwaryReport
        public ActionResult Index()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["MainProductView"] = items;
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult RebateInfo()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult DualLoanInfo()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult NegativeLedgerBalance()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult StaffWiseSpecialSavingReport()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        //public ActionResult DailyRecoverableAndRecoveryRegisterCenterDateWise()
        // {
        //     DateTime VDate;
        //     VDate = System.DateTime.Now;
        //     if (IsDayInitiated)
        //     {
        //         ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
        //     }
        //     else
        //     {
        //         ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
        //     }
        //     //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
        //     return View();
        // }

        public ActionResult DailyRecoverableAndRecoveryRegisterCenterDateWise()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var model = new MemberViewModel();
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var blnk_items = new List<SelectListItem>();
            model.CenterList = blnk_items;
            return View(model);
        }

        //private void MapDropDownList(MemberViewModel model)
        //{
        //    List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
        //    var param = new { SearchByCode = "", SearchBy = "", SearchType = "dis" };
        //    var disList = ultimateReportService.GetLocationList(param);

        //    List_MemberViewModel = disList.Tables[0].AsEnumerable()
        //    .Select(row => new MemberViewModel
        //    {
        //        DistrictCode = row.Field<string>("DistrictCode"),
        //        DistrictName = row.Field<string>("DistrictName")
        //    }).ToList();
        //    var viewDist = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.DistrictCode.ToString(),
        //        Text = x.DistrictName.ToString()
        //    });
        //    var pob_items = new List<SelectListItem>();
        //    pob_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
        //    pob_items.AddRange(viewDist);
        //    model.PlaceOfBirthList = pob_items;
        //}



            public ActionResult MemberTransferReport()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult RecoverableAndRecoveryRegisterDSK()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        #endregion

        #region Events
        // GET: KhatwaryReport/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KhatwaryReport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KhatwaryReport/Create
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

        // GET: KhatwaryReport/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: KhatwaryReport/Edit/5
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

        // GET: KhatwaryReport/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KhatwaryReport/Delete/5
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

        #endregion

    }
}
