﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("PKSFFundLoan")]
    public class PKSFFundLoan
    {
        [Key]
        public int FundLoanID { get; set; }

        public string FundLoanCode { get; set; }

        public decimal? PrincipalAmount { get; set; }

        public int? LoanSanctionNo { get; set; }

        public int? LoanSanctionTerm { get; set; }

        public DateTime? LoanSanctionApproveDate { get; set; }

        public DateTime? LoanDisbursementDate { get; set; }

        public decimal? DisbursedAmount { get; set; }

        public decimal? InterestRate { get; set; }

        public int? LoanDuration { get; set; }

        public int? GracePeriod { get; set; }

        public int? NoOfInstallment { get; set; }

        public int? TotalInstallmentNo { get; set; }

        public DateTime? InstallmentDate { get; set; }

        public decimal? LoanInstallmentAmount { get; set; }

        public decimal? ServiceCharge { get; set; }
    }
}
