﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public  class ProductMainCodeModel
    { 
        public string MainProductCode { get; set; }      
        public string MainItemName { get; set; }
    }
}
