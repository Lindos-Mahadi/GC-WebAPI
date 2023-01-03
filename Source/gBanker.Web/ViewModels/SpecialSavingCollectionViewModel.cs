﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class SpecialSavingCollectionViewModel : BaseModel
    {
        public long DailySavingTrxID { get; set; }
        public long SavingSummaryID { get; set; }
         [Required(ErrorMessage = "Office is required")]
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
         [Required(ErrorMessage = "Member is required")]
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
         [Required(ErrorMessage = "Product is required")]
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        [Display(Name = "Samity ID")]
         [Required(ErrorMessage = "Samity is required")]
        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
         [Required(ErrorMessage = "Account No. is required")]
        [Range(1, 100000)]
        public int NoOfAccount { get; set; }

        [Required(ErrorMessage = "Transaction Date is required")]
        public DateTime TransactionDate { get; set; }
        [Display(Name = "SavingInstallment(Scheme)")]
        public decimal DueSavingInstallment { get; set; }
        public decimal SavingInstallment { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdrawal { get; set; }
        public decimal Balance { get; set; }
        public decimal Penalty { get; set; }
        public byte TransType { get; set; }
        public decimal MonthlyInterest { get; set; }
        public bool PresenceInd { get; set; }
        public decimal TransferDeposit { get; set; }
        public decimal TransferWithdrawal { get; set; }
        public long IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public Nullable<byte> MemberCategoryID { get; set; }
        public short EmployeeID { get; set; }
        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> cashListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> membercategoryListItems { get; set; }
        public IEnumerable<SelectListItem> purposeListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public string memName { get; set; }
        public string vMaxLoanTerm { get; set; }
        public IEnumerable<ProductViewModel> MemberProductItemsSelected { get; set; }
        public IEnumerable<SelectListItem> GetAccountCodeList { get; set; }
        public int return_value { get; set; }
        public string return_msg { get; set; }
    }
}