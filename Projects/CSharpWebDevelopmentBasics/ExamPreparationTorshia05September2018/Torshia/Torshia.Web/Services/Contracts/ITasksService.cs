using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Torshia.Models;

namespace Torshia.Web.Services.Contracts
{
    public interface ITasksService
    {
        IQueryable<Task> All { get; }

        void CreateTask(string title, string dateTime, string description, string participants, List<string> affectedSectors);
    }
}
