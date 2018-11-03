using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Torshia.Models;

namespace Torshia.Web.Services.Contracts
{
    public interface IReportService
    {
        IQueryable<Report> All { get; }
    }
}
