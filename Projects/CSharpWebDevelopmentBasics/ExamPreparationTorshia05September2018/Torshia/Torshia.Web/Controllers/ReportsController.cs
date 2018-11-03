using Microsoft.EntityFrameworkCore;
using SIS.Framework.ActionResults;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Torshia.Models;
using Torshia.Models.Enums;
using Torshia.Web.Controllers.Base;
using Torshia.Web.Services.Contracts;

namespace Torshia.Web.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly IReportService service;

        public ReportsController(IReportService service)
        {
            this.service = service;
        }

        public IActionResult Create()
        {
            if (!AdminAuthorization())
            {
                return RedirectToAction("/");
            }

            var id = int.Parse(this.Request.QueryData["id"].ToString());

            var task = db.Tasks.FirstOrDefault(x => x.Id == id);

            if (task == null)
            {
                return RedirectToAction("/");
            }

            int[] status = { 0, 0, 0, 1 };
            int statusIndex = new Random().Next(0, 4);

            var user = db.Users.FirstOrDefault(x => x.Username == this.Identity.Username);

            task.IsReported = true;

            task.Report = new Report
            {
                Reporter = user,
                ReportedOn = DateTime.UtcNow,
                Status = (Status)status[statusIndex]
            };
            db.SaveChanges();
            return this.RedirectToAction("/");

        }
        public IActionResult All()
        {
            if (!AdminAuthorization())
            {
                return RedirectToAction("/");
            }

            var reports = db.Reports.Include(x => x.Task.AffectedSectors).ToArray();

            var sb = new StringBuilder();
            foreach (var report in reports)
            {
                var count = 1;
                sb.AppendLine(@"<tr class=""row"">");
                sb.AppendLine($@"<th class=""col-md-1"">{count}</th>");
                sb.AppendLine($@"<th class=""col-md-5"">{UrlDecode(report.Task.Title)}</th>");
                sb.AppendLine($@"<th class=""col-md-1"">{report.Task.AffectedSectors.Count}</th>");
                sb.AppendLine($@"<th class=""col-md-2"">{report.Status}</th>");
                sb.AppendLine($@"<td class=""col-md-2""></th>");
                sb.AppendLine($@"<div class=""button-holder d-flex justify-content-between"">");
                sb.AppendLine(
                    $@"<a href=""/Reports/Details/?id={report.Id.ToString()}"" class=""btn bg-torshia text-white"">Details</a>");
                sb.AppendLine("<div>");
                sb.AppendLine("</td>");
                sb.AppendLine("</tr>");
                count++;
            }
            this.Model.Data["Reports"] = sb.ToString();

            return this.View();
        }

        public IActionResult Details()
        {
            var id = int.Parse(this.Request.QueryData["id"].ToString());

            var report = db.Reports.Include(x => x.Task)
                                   .Include(x => x.Reporter)
                                   .Include(x => x.Task.AffectedSectors)
                                   .FirstOrDefault(x => x.Id == id);

            var affectedSectors = db.TaskSectors.Where(x => x.TaskId == report.TaskId)
                .Select(x => x.Sector.Name).ToList();

            this.Model.Data["ReportId"] = report.Id.ToString();
            this.Model.Data["Reporter"] = report.Reporter.Username;
            this.Model.Data["Description"] = UrlDecode(report.Task.Description);
            this.Model.Data["Title"] = UrlDecode(report.Task.Title);
            this.Model.Data["Status"] = report.Status.ToString();
            this.Model.Data["Participants"] = UrlDecode(report.Task.Participants);
            this.Model.Data["ReportedOn"] = report.ReportedOn.ToString("mm-MM-yyyy", CultureInfo.InvariantCulture);
            this.Model.Data["DueDate"] = report.Task.Due?.ToString("mm-MM-yyyy", CultureInfo.InvariantCulture);
            this.Model.Data["AffectedSectors"] = string.Join(", ", affectedSectors);
            this.Model.Data["Level"] = report.Task.AffectedSectors.Count;


            return this.View();
        }
    }
}
