﻿using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Web.Core.Extensions;
using gBanker.Data.CodeFirstMigration;
using System.Configuration;
using gBanker.Web.Helpers;
using Microsoft.AspNet.Identity;
using System.Data;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using gBanker.Service.ReportServies;

namespace gBanker.Web.Controllers
{
    public class SpecialLoanCollectionController : BaseController
    {

        private readonly ICenterService centerService;
        private readonly ISpecialLoanCollectionService specialLoanCollectionService;
        private readonly IBranchService branchService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;
        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        private readonly ILoanSummaryService LoanSummaryService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IAccChartService accChartService;
        private readonly IApplicationSettingsService applicationSettingsService;
        private readonly IPortalLoanSummaryService portalLoanSummaryService;
        private readonly ILoanAccRescheduleService loanAccRescheduleService;

        public SpecialLoanCollectionController(ISpecialLoanCollectionService specialLoanCollectionService,
            IUltimateReportService ultimateReportService, ILoanSummaryService LoanSummaryService,
            ICenterService centerService, IProductService productService, IMemberCategoryService membercategoryService,
            IOfficeService officeService, IPurposeService purposeService, IMemberService memberService,
            IAccChartService accChartService, IApplicationSettingsService applicationSettingsService, 
            IPortalLoanSummaryService portalLoanSummaryService,
            ILoanAccRescheduleService loanAccRescheduleService)
        {
            this.specialLoanCollectionService = specialLoanCollectionService;
            this.centerService = centerService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.officeService = officeService;
            this.purposeService = purposeService;
            this.memberService = memberService;
            this.LoanSummaryService = LoanSummaryService;
            this.ultimateReportService = ultimateReportService;
            this.accChartService = accChartService;
            this.applicationSettingsService = applicationSettingsService;
            this.portalLoanSummaryService = portalLoanSummaryService;
            this.loanAccRescheduleService = loanAccRescheduleService;
        }
        // GET: SpecialLoanCollection
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetSpecialCollection(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                string vday=string.Empty;
                 if (IsDayInitiated)
                     vday=TransactionDay;
                var specialLoandetail = specialLoanCollectionService.GetSpecialLoanCollectionDetailEmpWise(LoggedInOrganizationID,LoginUserOfficeID, vday, filterColumn, filterValue,Convert.ToInt16(LoggedInEmployeeID));
                var detail = specialLoandetail.ToList();
                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<proc_get_SpecialLoanCollection_Result>, IEnumerable<SpecialLoanCollectionViewModel>>(entities);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        private void MapDropDownList(SpecialLoanCollectionViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var Transtype = new List<SelectListItem>();
            if (LoggedInOrganizationID == 3 || LoggedInOrganizationID == 10 ||
                LoggedInOrganizationID == 12 || LoggedInOrganizationID == 82
                || LoggedInOrganizationID == 95)
            {
                Transtype.Add(new SelectListItem() { Text = "Cash", Value = "20", Selected = true });
                Transtype.Add(new SelectListItem() { Text = "Bank", Value = "22" });
                Transtype.Add(new SelectListItem() { Text = "Transfer", Value = "21" });
            }
            else
            {
                Transtype.Add(new SelectListItem() { Text = "Cash", Value = "20", Selected = true });
                Transtype.Add(new SelectListItem() { Text = "Bank", Value = "22" });
            }
            


            model.cashListItems = Transtype.AsEnumerable();
            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));
            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });
            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;
            var allpurpose = purposeService.SearchPurpose(Convert.ToInt32(LoggedInOrganizationID));
            var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });
            model.purposeListItems = viewPurpose;
            string vCoday = TransactionDay;

            var param1 = new { @EmpID = LoggedInEmployeeID };
            var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);

            IEnumerable<Center> allcenter;
            if (LoanInstallMent != null)
            {
                var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
                if (empType == "FO")
                {
                    allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt16(LoggedInEmployeeID));
                }
                else
                    allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));
            }
            else
                allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));


            //var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt16(LoggedInEmployeeID));
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });
            model.centerListItems = viewCenter;
            var alloffice = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.OrgID==LoggedInOrganizationID);
            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });
            model.officeListItems = viewOffice;




            var allSearchProd = productService.SearchProduct(0, Convert.ToInt32(LoggedInOrganizationID),"L");
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });

            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.productListItems = proditems;

            //model.MemberProductItemsSelected = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(allproduct);

            var allmembercategory = membercategoryService.GetAll().Where(a=>a.OrgID==LoggedInOrganizationID);

            var viewmembercategory = allmembercategory.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCategoryID, m.CategoryName), Value = m.MemberCategoryID.ToString() });

            model.membercategoryListItems = viewmembercategory;

            IEnumerable<AccChart> allAccountCode = new List<AccChart>();
            IEnumerable<SelectListItem> viewAccountCode = new List<SelectListItem>();

            var BankCode = accChartService.GetByAccCode(applicationSettingsService.GetAll().Where(c => c.OfficeID == LoginUserOfficeID).FirstOrDefault().BankAccount);
                

            if (allAccountCode != null && BankCode!=null)
            {
                allAccountCode = accChartService.GetAll()
                                   .Where(a => a.SecondLevel == BankCode.SecondLevel && a.ModuleID == 1)
                                   .ToList();
                viewAccountCode = allAccountCode.Select(x => new SelectListItem
                {
                    Value = x.AccCode.ToString(),
                    Text = x.AccCode.ToString() + " " + x.AccName.ToString()
                });
            }            
            var Acc_items = new List<SelectListItem>();
            Acc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            Acc_items.AddRange(viewAccountCode);
            model.GetAccountCodeList = Acc_items;


        }
        public JsonResult GetProductList(string Member_id, string center_id)
        {
            List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
            var param = new { OfficeId = LoginUserOfficeID, MemberId = Member_id, CenterId = center_id };
            var div_items = ultimateReportService.GetProductList(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new ProductViewModel
            {
                ProductID = row.Field<Int16>("ProductID"),
                ProductCode = row.Field<string>("ProductCode"),
                ProductName = row.Field<string>("ProductName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductCode.ToString()+" "+x.ProductName.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMemberList(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInstallment(string officeId, string centerId, string MemId, int productid, int loanTerm, int trxType)
        {
            decimal vLoanInstallment = 0;
            decimal vInterestInstallment = 0;
            decimal vTotalIns = 0;
            string vInterestCalcMethod = string.Empty, vPaymentFreq = string.Empty;
            decimal vLoanDue = 0;
            decimal vInterestDue = 0;
            decimal vPrincipalLoan = 0;
            decimal vLoanRepaid = 0;
            Int64 vDailyLoanTrxID = 0;
            int vproductID = 0;
            decimal vcumIntcharge = 0;
            decimal vcumIntPaid = 0;
            decimal vPrePaid = 0;
            decimal vIntCharge = 0;
            decimal vCumLoanDue = 0;
            decimal vCumIntDue = 0;
            int vDUration = 0;
            decimal durationOverLoanDue = 0;
            decimal duartionOverIntDue = 0;
            decimal Fine = 0;
            Int16 NoOfInstallment = 0;
            int orgID = 0;
            int DOC = 0;
            int OrgID = 0;
            if (trxType == 0)
            {
                return GetErrorMessageResult("Please select Transaction Type");
            }


            var proid = productService.GetById(productid);
            var paramSLC = new { @orgID = SessionHelper.LoginUserOrganizationID, @officeID = SessionHelper.LoginUserOfficeID, @CenterID = Convert.ToInt32(centerId), @ProductID = productid, @MemberID = Convert.ToInt64(MemId), @LoanTerm = loanTerm, @CollectionDate = TransactionDate, @TransType = trxType };

            DataSet LoanInstallMent;

            //if (proid.PaymentFrequency == "M")
            //{
            //    ultimateReportService.GetKerPressValueForSpecialLoanMonthly(paramSLC);

            //}
            //else
            //{
            //    ultimateReportService.GetKerPressValueForSpecialLoanWeekly(paramSLC);
            //}



            //var param1 = new { @qType = 2, @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Convert.ToInt32(centerId), @MemberID = Convert.ToInt64(MemId), @ProductID = productid, @LoanTerm = loanTerm, @TrxType = trxType };

            //LoanInstallMent = ultimateReportService.GetDailyLoanTrxlist(param1);


            ////////For On Fly Table 
            if (proid.PaymentFrequency == "M")
            {
                LoanInstallMent = ultimateReportService.GetKerPressValueForSpecialLoanMonthlyFlyTable(paramSLC);

            }
            else
            {
                LoanInstallMent = ultimateReportService.GetKerPressValueForSpecialLoanWeeklyFlyTable(paramSLC);
            }


            if (LoanInstallMent.Tables[0].Rows.Count > 0)
            {

                vLoanInstallment = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["LoanPaid"].ToString());
                vInterestInstallment = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["IntPaid"].ToString());
                vTotalIns = vLoanInstallment + vInterestInstallment;
                var prod = productService.GetById(productid);
                vInterestCalcMethod = prod.InterestCalculationMethod;
                vPaymentFreq = prod.PaymentFrequency;


                //vLoanDue = LoanInstallMent.LoanDue;
                vLoanDue = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["LoanDue"].ToString());

                //vInterestDue = LoanInstallMent.IntDue;

                vInterestDue = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["IntDue"].ToString());

                //vPrincipalLoan = LoanInstallMent.PrincipalLoan;
                vPrincipalLoan = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["PrincipalLoan"].ToString());

                //vLoanRepaid = LoanInstallMent.LoanRepaid;
                vLoanRepaid = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["LoanRepaid"].ToString());
                //vDailyLoanTrxID = LoanInstallMent.DailyLoanTrxID;
                vDailyLoanTrxID = Convert.ToInt64(LoanInstallMent.Tables[0].Rows[0]["DailyLoanTrxID"].ToString());


                //vproductID = Convert.ToInt32(LoanInstallMent.ProductID);
                vproductID = Convert.ToInt16(LoanInstallMent.Tables[0].Rows[0]["ProductID"].ToString());

                //vcumIntcharge = LoanInstallMent.CumIntCharge;
                vcumIntcharge = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["CumIntCharge"].ToString());

                //vcumIntPaid = LoanInstallMent.DueRecovery;
                vcumIntPaid = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["DueRecovery"].ToString());

                //vDUration = LoanInstallMent.Duration;
                vDUration = Convert.ToInt16(LoanInstallMent.Tables[0].Rows[0]["Duration"].ToString());

                //durationOverLoanDue = LoanInstallMent.DurationOverLoanDue;
                durationOverLoanDue = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["DurationOverLoanDue"].ToString());


                //duartionOverIntDue = LoanInstallMent.DurationOverIntDue;
                duartionOverIntDue = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["DurationOverIntDue"].ToString());
                //NoOfInstallment = LoanInstallMent.InstallmentNo;
                NoOfInstallment = Convert.ToInt16(LoanInstallMent.Tables[0].Rows[0]["InstallmentNo"].ToString());
                orgID = Convert.ToInt16(LoanInstallMent.Tables[0].Rows[0]["OrgID"].ToString());
                DOC = Convert.ToInt16(LoanInstallMent.Tables[0].Rows[0]["DurationOverCollection"].ToString());
                vIntCharge = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["IntCharge"].ToString());
                Fine = 0;
                if (orgID == 54)
                {
                    vCumLoanDue = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["CumLoanDue"].ToString());
                    vCumIntDue = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["CumIntDue"].ToString());
                    Fine = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["Fine"].ToString());
                }
                else
                {
                    vCumLoanDue = 0;
                    vCumIntDue = 0;
                    Fine = 0;
                }

            }
            else
            {
                vLoanInstallment = 0;
                vInterestInstallment = 0;
                vTotalIns = 0;
                var prod = productService.GetById(productid);
                vInterestCalcMethod = prod.InterestCalculationMethod;
                vPaymentFreq = prod.PaymentFrequency;
            }
            //var result;
            //var result = new { loan = vLoanInstallment.ToString(), interest = vInterestInstallment.ToString(), interestCalcMethod = vInterestCalcMethod, PaymentFreq = vPaymentFreq, LoanDue = vLoanInstallment, InterestDue = vInterestInstallment, PrincipalLoan = vPrincipalLoan, LoanRepaid = vLoanRepaid, DailyLoanTrxID = vDailyLoanTrxID, ProductID = vproductID, cumIntcharge = vcumIntcharge, DueRecovery = vcumIntPaid, duration = vDUration.ToString(), durationOverLoanDue = durationOverLoanDue, duartionOverIntDue = duartionOverIntDue, NoOfInstallment=NoOfInstallment.ToString(), total = vTotalIns.ToString(), orgID=orgID,DOC=DOC };
            if (LoggedInOrganizationID == 54)
            {
                var result1 = new { loan = vLoanInstallment.ToString(), interest = vInterestInstallment.ToString(), interestCalcMethod = vInterestCalcMethod, PaymentFreq = vPaymentFreq, LoanDue = vLoanDue, InterestDue = vInterestDue, PrincipalLoan = vPrincipalLoan, LoanRepaid = vLoanRepaid, DailyLoanTrxID = vDailyLoanTrxID, ProductID = vproductID, cumIntcharge = vcumIntcharge, DueRecovery = vcumIntPaid, duration = vDUration.ToString(), durationOverLoanDue = durationOverLoanDue, duartionOverIntDue = duartionOverIntDue, NoOfInstallment = NoOfInstallment.ToString(), total = vTotalIns.ToString(), orgID = orgID, DOC = DOC, vIntCharge = vIntCharge, vCumLoanDue = vCumLoanDue, vCumIntDue = vCumIntDue, Fine = Fine };
                return Json(result1, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = new { loan = vLoanInstallment.ToString(), interest = vInterestInstallment.ToString(), interestCalcMethod = vInterestCalcMethod, PaymentFreq = vPaymentFreq, LoanDue = vLoanInstallment, InterestDue = vInterestInstallment, PrincipalLoan = vPrincipalLoan, LoanRepaid = vLoanRepaid, DailyLoanTrxID = vDailyLoanTrxID, ProductID = vproductID, cumIntcharge = vcumIntcharge, DueRecovery = vcumIntPaid, duration = vDUration.ToString(), durationOverLoanDue = durationOverLoanDue, duartionOverIntDue = duartionOverIntDue, NoOfInstallment = NoOfInstallment.ToString(), total = vTotalIns.ToString(), orgID = orgID, DOC = DOC, vIntCharge = vIntCharge, vCumLoanDue = vCumLoanDue, vCumIntDue = vCumIntDue, Fine = Fine };
                return Json(result, JsonRequestBehavior.AllowGet);
            }



        }
        public JsonResult GetMaxLoanTerm(string officeId, string centerId, string MemId, string ProdId)
        {
            int vLoanTerm;
            var model = new SpecialLoanCollectionViewModel();
            model.OfficeID = Convert.ToInt16(officeId);
            model.CenterID = Convert.ToInt32(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);


            var entity = Mapper.Map<SpecialLoanCollectionViewModel, LoanSummary>(model);
            entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
           // var mlt = specialLoanCollectionService.getMaxLoanterm(entity);
                var mlt = specialLoanCollectionService.Get_MaxLoanTerm(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID),entity.CenterID,entity.MemberID,entity.ProductID).FirstOrDefault();
            //Session[ProductSessionKey] = pbr;
            vLoanTerm = Convert.ToInt16(mlt.LoanTerm);

            var result = new { LoanTerm = vLoanTerm.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        public Member GetMember(long memberid)
        {
            var mbr = memberService.GetByIdLong(memberid);
            return mbr;
        }
        public Center GetEmployee(int employeeid)
        {
            var mbr = centerService.GetById(employeeid);
            return mbr;
        }
        public Product GetProduct(int productid)
        {
            var mbr = productService.GetById(productid);
            return mbr;
        }
        public ActionResult Create()
        {
            var model = new SpecialLoanCollectionViewModel();
            //if (IsDayInitiated)
            //model.TrxDate = TransactionDate;
            model.TrxDate = DateTime.UtcNow;
            MapDropDownList(model);
            specialLoanCollectionService.delVoucher(LoginUserOfficeID, model.TrxDate, LoggedInOrganizationID);
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(SpecialLoanCollectionViewModel Model)
        {
            try
            {

                long vlOansummaryID=0;
                long vDailyTrxID;
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                //var paramG = new { @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Convert.ToInt32(Model.CenterID), @MemberID = Convert.ToInt64(Model.MemberID), @ProductID = Model.ProductID, @LoanTerm = Model.LoanTerm, @TrxType = Model.TrxType };
                //var model = ultimateReportService.GetDailyLoanTrxID(paramG);
                //if (model.Tables[0].Rows.Count > 0)
                //{
                //    vlOansummaryID = Convert.ToInt64(model.Tables[0].Rows[0]["LoanSummaryID"]);
                //    vDailyTrxID = Convert.ToInt64(model.Tables[0].Rows[0]["DailyLoanTrxID"]); ;
                //}
                //else
                //    vDailyTrxID = 0;

                //var loanser = specialLoanCollectionService.GetByIdLong(vDailyTrxID);
                //var entity = Mapper.Map<SpecialLoanCollectionViewModel, DailyLoanTrx>(Model);

                //if (ModelState.IsValid)
                //{

                //    var errors = specialLoanCollectionService.IsValidLoan(loanser);

                //    if (errors.ToList().Count == 0)
                //    {

                //        loanser.LoanPaid = Model.LoanPaid;
                //        loanser.IntPaid = Model.IntPaid;
                //        loanser.TotalPaid = Model.TotalPaid;
                //        loanser.TrxType = Model.TrxType;
                //        specialLoanCollectionService.Update(loanser);
                //        if (Model.TrxType == 0)
                //        {
                //            return GetErrorMessageResult("Please select Transaction Type");
                //        }
                //        var param = new { @DailyLoanTrxID = loanser.DailyLoanTrxID, @OfficeId = SessionHelper.LoginUserOfficeID, @CenterId = loanser.CenterID, @MemberID = loanser.MemberID, @ProductID = loanser.ProductID, @LOanterm = loanser.LoanTerm, @LoanPaid = Model.LoanPaid, @IntPaid = Model.IntPaid, @TotalPaid = Model.TotalPaid, @TrxType = Model.TrxType, @orgID = SessionHelper.LoginUserOrganizationID };
                //        ultimateReportService.SetSpecialLoanCollection(param);
                //        return GetSuccessMessageResult();
                //    }
                //    else
                //        return GetErrorMessageResult(errors);
                //}
                ///////////////////////////For Fly Table--------------------

                var paramG = new { @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Convert.ToInt32(Model.CenterID), @MemberID = Convert.ToInt64(Model.MemberID), @ProductID = Model.ProductID, @LoanTerm = Model.LoanTerm, @TrxType = Model.TrxType };
                var spmodel = ultimateReportService.GetLoanSummaryID(paramG);
                if (spmodel.Tables[0].Rows.Count > 0)
                {
                    vlOansummaryID = Convert.ToInt64(spmodel.Tables[0].Rows[0]["LoanSummaryID"]);
                    vDailyTrxID = Convert.ToInt64(spmodel.Tables[0].Rows[0]["DailyLoanTrxID"]); 
                }
                else
                    vDailyTrxID = 0;

                var loanser = LoanSummaryService.GetByIdLong(vlOansummaryID);
                var entity = Mapper.Map<SpecialLoanCollectionViewModel, DailyLoanTrx>(Model);

                if (ModelState.IsValid)
                {

                    if (Model.PrincipalLoan == 0)
                    {
                        return GetErrorMessageResult("Focus on loanterm please.........");
                    }
                    if (Model.TrxType == 0)
                    {
                        return GetErrorMessageResult("Please select Transaction Type");
                    }
                    if (entity.TrxType == 22)
                    {
                        if (entity.BankName == "0")
                        {
                            return GetErrorMessageResult("Please put the BankName");
                        }
                        if (entity.ChequeNo == "NA")
                        {
                            return GetErrorMessageResult("Please put the ChequeNo");
                        }

                    }
                    if (Model.TrxType == 0)
                    {
                        return GetErrorMessageResult("Please select Transaction Type");
                    }
                    if (entity.BankName == null)
                    {
                        entity.BankName = "NA";
                    }
                    if (entity.ChequeNo == null)
                    {
                        entity.ChequeNo = "NA";
                    }
                    var paramD = new {
                        @LoansummaryID = vlOansummaryID,
                        @LoanPaid = Model.LoanPaid, 
                        @IntPaid = Model.IntPaid,
                        @TrxType = Model.TrxType, 
                        //@lcl_BusinessDate = TransactionDate,
                        @lcl_BusinessDate = DateTime.UtcNow,
                        @lcl_OfficeID = LoginUserOfficeID,
                        @LoanDue = Model.LoanDue, 
                        @CumIntCharge = Model.CumIntCharge,
                        @intCharge = Model.IntCharge,
                        @intdue = Model.IntDue, 
                        @InstallmentNo = Model.InstallmentNo,
                        @CreateUser = LoggedInEmployeeID,
                        @BankName =entity.BankName,
                        @CheckNo=entity.ChequeNo };
                    var loanser1 = ultimateReportService.DataInsertintoDailyLoanTrx(paramD);

                    var param = new { 
                        @DailyLoanTrxID = 0, 
                        @OfficeId = SessionHelper.LoginUserOfficeID,
                        @CenterId = Model.CenterID, 
                        @MemberID = Model.MemberID, 
                        @ProductID = Model.ProductID,
                        @LOanterm = entity.LoanTerm, 
                        @LoanPaid = Model.LoanPaid,
                        @IntPaid = Model.IntPaid,
                        @TotalPaid = Model.TotalPaid, 
                        @TrxType = Model.TrxType, 
                        @orgID = SessionHelper.LoginUserOrganizationID };
                    ultimateReportService.SetSpecialLoanCollection(param);
                    //return RedirectToAction("Create", new { name = Model.DueRecovery, desc = Model.DueRecovery });
                    return GetSuccessMessageResult();
                }
                else
                    //return GetErrorMessageResult();
                /////////////////////End For Fly Table--------------------
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        // GET: PortalLoanAccReshedule
        public ActionResult PortalLoanAccResheduleIndex()
        {
            return View();
        }

        //[HttpPost]
        public JsonResult PortalLoanAccResheduleInfo(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                //List<LoanAccReschedule> loAccReshedule = new List<LoanAccReschedule>();
                var loanAccReshedule = loanAccRescheduleService.GetAll();
                if (loanAccReshedule != null)
                {
                    var mapLoanAccReshedule = Mapper.Map<IEnumerable<LoanAccReschedule>, List<LoanAccRescheduleViewModel>>(loanAccReshedule);
                    var loanAccResheduleDetail = mapLoanAccReshedule.Skip(jtStartIndex).Take(jtPageSize).ToList();
                    return Json(new { Result = "OK", Records = mapLoanAccReshedule, TotalRecordCount = loanAccReshedule.Count() });
                }

                return Json(new { Result = "OK", TotalCountRecord = loanAccReshedule.Count() });

            }
            catch (Exception ex)
            {

                return Json(new { Result = "Error", Message = ex.Message });
            }

        }
        //[HttpPost]
        public ActionResult CreatePortalLoanAccReshedule(int id) 
        {
            try
            {
                var getLoanSummaryId = portalLoanSummaryService.GetById(id);
                var model = Mapper.Map<PortalLoanSummary, SpecialLoanCollectionViewModel>(getLoanSummaryId);
                model.TrxDate = DateTime.UtcNow;
                MapDropDownList(model);
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, model.TrxDate, LoggedInOrganizationID);
                return View(model);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult Edit(long id)
        {
            var loanser = specialLoanCollectionService.GetByIdLong(id);
            var member = GetMember(Convert.ToInt64(loanser.MemberID));
            var product = GetProduct(Convert.ToInt16(loanser.ProductID));
            var entity = Mapper.Map<DailyLoanTrx, SpecialLoanCollectionViewModel>(loanser);
            ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName);
            ViewBag.productname = string.Format("{0} - {1}", product.ProductCode, product.ProductName);
//            entity.InterestCalculationMethod = loanser.in
            MapDropDownList(entity);
            return View(entity);
        }
        // POST: SpecialLoanCollection/Edit/5
        [HttpPost]
        public ActionResult Edit(SpecialLoanCollectionViewModel Model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);

                var loanser = specialLoanCollectionService.GetByIdLong(Convert.ToInt64(Model.DailyLoanTrxID));
                var entity = Mapper.Map<SpecialLoanCollectionViewModel, DailyLoanTrx>(Model);
                if (ModelState.IsValid)
                {

                    var errors = specialLoanCollectionService.IsValidLoan(loanser);

                    if (errors.ToList().Count == 0)
                    {
                        if (Model.TrxType == 0)
                        {

                            return GetErrorMessageResult("Please select Transaction Type");
                        }
                        //loanser.LoanPaid = Model.LoanPaid;
                        //loanser.IntPaid = Model.IntPaid;
                        //loanser.TotalPaid = Model.TotalPaid;
                        //specialLoanCollectionService.Update(loanser);
                        //var param = new { @DailyLoanTrxID = loanser.DailyLoanTrxID, @OfficeId = loanser.OfficeID, @CenterId = loanser.CenterID, @MemberID = loanser.MemberID, @ProductID = loanser.ProductID, @LOanterm = loanser.LoanTerm, @LoanPaid = Model.LoanPaid, @IntPaid = Model.IntPaid, @TotalPaid = Model.TotalPaid, @TrxType = Model.TrxType, @orgID = loanser.OrgID };
                        //ultimateReportService.SetSpecialLoanCollection(param);
                        //specialLoanCollectionService.UpdateSpecialLOan(loanser.DailyLoanTrxID, loanser.OfficeID, loanser.CenterID, loanser.MemberID, loanser.ProductID, loanser.LoanTerm, Model.LoanPaid, Model.IntPaid, Model.TotalPaid,Model.TrxType, loanser.OrgID);

                        var param = new { @DailyLoanTrxID = loanser.DailyLoanTrxID, @OfficeId = SessionHelper.LoginUserOfficeID, @CenterId = loanser.CenterID, @MemberID = loanser.MemberID, @ProductID = loanser.ProductID, @LOanterm = loanser.LoanTerm, @LoanPaid = Model.LoanPaid, @IntPaid = Model.IntPaid, @TotalPaid = Model.TotalPaid, @TrxType = Model.TrxType, @orgID = SessionHelper.LoginUserOrganizationID };
                        ultimateReportService.SetSpecialLoanCollection(param);
                     
                        return GetSuccessMessageResult();
                    }
                    else
                        return GetErrorMessageResult(errors);
                }
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        // GET: SpecialLoanCollection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        public JsonResult DeleteDisburse(long LoanSummaryID)
        {
            try
            {
                //disburseservice.Delete(LoanSummaryID);
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        // POST: SpecialLoanCollection/Delete/5
        [HttpPost]
        public ActionResult Delete(int DailyLoanTrxID, SpecialLoanCollectionViewModel model)
        {
            try
            {
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);
                var param1 = new { @Qtype=1,@DailyLoanTrxID = DailyLoanTrxID, @OrgID= LoggedInOrganizationID };
                var LoanInstallMent = ultimateReportService.DelSpecialLoanCollection(param1);

                //var sp = specialLoanCollectionService.GetByIdLong(DailyLoanTrxID);
                //var entity = Mapper.Map<DailyLoanTrx, SpecialLoanCollectionViewModel>(sp);
                //specialLoanCollectionService.Delete(DailyLoanTrxID);


                // TODO: Add delete logic here
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetBalance(decimal loanRepaid, decimal intPaid)
        {
            decimal vBalance = (loanRepaid + intPaid);
            var result = new { balance = vBalance.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
