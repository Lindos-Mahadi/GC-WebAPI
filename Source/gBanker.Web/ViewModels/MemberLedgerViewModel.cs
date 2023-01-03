﻿using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace gBanker.Web.ViewModels
{
    public class MemberLedgerViewModel
    {
        public Nullable<int> OfficeID { get; set; }
        public string BranchCode { get; set; }
        [Display(Name = "Samity ID")]
        public Nullable<int> CenterID { get; set; }
        public string BranchName { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string ProductCode { get; set; }
        public Nullable<int> MemberID { get; set; }
        public string MemberCode { get; set; }
        public Nullable<int> LoanTerm { get; set; }
        public Nullable<System.DateTime> InstallmentDate { get; set; }
        public Nullable<decimal> PrincipalLoan { get; set; }
        public Nullable<decimal> LoanInstallment { get; set; }
        public Nullable<decimal> LoanDue { get; set; }
        public Nullable<decimal> IntCharge { get; set; }
        public Nullable<decimal> InterestPaid { get; set; }
        public Nullable<decimal> IntDue { get; set; }
        public Nullable<int> TrxType { get; set; }
        public string CenterName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ProductName { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> DisburseDate { get; set; }
        public Nullable<decimal> LoanBalance { get; set; }
        public Nullable<decimal> InterestBalance { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public int InstallmentNo { get; set; }
        
    }
}