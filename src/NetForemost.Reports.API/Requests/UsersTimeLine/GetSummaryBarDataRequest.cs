﻿using NetForemost.SharedKernel.Helpers;
using System.ComponentModel.DataAnnotations;

namespace NetForemost.Reports.API.Requests.UsersTimeLine
{
    public class GetSummaryBarDataRequest : DateRangeHelper
    {
        [Required]
        public int CompanyId { get; set; }
        public int[] UserIds { get; set; } = Array.Empty<int>();
        public int[] ProjectIds { get; set; } = Array.Empty<int>();
        [Required]
        public double TimeZone { get; set; }
    }
}