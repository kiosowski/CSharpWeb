using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Torshia.Data;
using Torshia.Models;
using Torshia.Web.Services.Contracts;

namespace Torshia.Web.Services
{
    public class ReportService : IReportService
    {
        private readonly TorshiaContext context;
        public ReportService(TorshiaContext context)
        {
            this.context = context;
        }
        public IQueryable<Report> All => context.Reports;
    }
}
