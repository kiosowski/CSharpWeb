using System;
using System.Collections.Generic;
using System.Linq;
using Torshia.Data;
using Torshia.Models;
using Torshia.Web.Services.Contracts;

namespace Torshia.Web.Services
{
    public class TasksService : ITasksService
    {
        public readonly TorshiaContext context;
        public TasksService(TorshiaContext context)
        {
            this.context = context;
        }
        public IQueryable<Task> All
            => this.context.Tasks.Where(x => x.IsReported == false);

        public void CreateTask(string title, string dueDate, string description, string participants, List<string> selectedSectors)
        {
            DateTime.TryParse(dueDate, out DateTime date);

            var task = new Task
            {
                Title = title,
                Due = date,
                Description = description,
                Participants = participants
            };

            var taskSectors = new List<TaskSector>();
            foreach (var selectedSector in selectedSectors)
            {
                if (selectedSector != null)
                {
                    bool isValidSector = Enum.TryParse(typeof(Models.Enums.Sector), selectedSector.ToString(),
                        out object sectorValue);

                    if (isValidSector)
                    {
                        var sector = new Models.Sector { Name = (Models.Enums.Sector)sectorValue };
                        var taskSector = new TaskSector { Task = task, Sector = sector };
                        taskSectors.Add(taskSector);
                    }
                }
            }
            task.AffectedSectors.AddRange(taskSectors);

            context.Tasks.Add(task);
            context.SaveChanges();
        }
    }
}