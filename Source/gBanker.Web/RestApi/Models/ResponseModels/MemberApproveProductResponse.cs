﻿using gBanker.Web.RestApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.ResponseModels
{
    public class MemberApproveProductResponse : ApiResponse
    {
        public List<GetApproveProduct> products { get; set; }
    }
}