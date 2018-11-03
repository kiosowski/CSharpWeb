using Microsoft.EntityFrameworkCore;
using SIS.Framework.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Torshia.Web.Controllers.Base;
using Torshia.Web.Services.Contracts;
using Torshia.Web.ViewModels;

namespace Torshia.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ITasksService tasksService;

        public HomeController(ITasksService tasksService)
        {
            this.tasksService = tasksService;
        }


        public IActionResult Index()
        {
            var userLoggedIn = this.Identity != null;
            if (!userLoggedIn)
            {
                return this.View();
            }

            var tasks = tasksService.All.ToList();
            var wrapperViewModels = new List<TaskViewModelWrapper>();
            wrapperViewModels.Add(new TaskViewModelWrapper());
            var neededContainers = 0;
            var neededContainersList = new List<string>();
            for (int i = 0; i < tasks.Count(); i++)
            {
                if (i % 5 == 0)
                {
                    wrapperViewModels.Add(new TaskViewModelWrapper());
                }

                var sectors = db.TaskSectors.Include(x => x.Sector).Where(x => x.TaskId == tasks[i].Id)
                    .Select(x => x.Sector.Name).ToList();

                var lastAddedWrapper = wrapperViewModels.Last();
                lastAddedWrapper.TaskViewModels.Add(new TaskViewModel
                {
                    Level = sectors.Count(),
                    Title = UrlDecode(tasks[i].Title),
                    Id = tasks[i].Id.ToString()

                });


            }

            this.Model.Data["ReportViewModels"] = wrapperViewModels;


            return this.View();

        }
    }
}

