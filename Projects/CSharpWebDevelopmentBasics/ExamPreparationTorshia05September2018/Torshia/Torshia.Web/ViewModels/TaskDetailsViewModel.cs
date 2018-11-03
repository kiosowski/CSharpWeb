using System;
using System.Collections.Generic;
using System.Text;

namespace Torshia.Web.ViewModels
{
    public class TaskDetailsViewModel
    {
        public string Title { get; set; }
        public string Level { get; set; }
        public string Participants { get; set; }
        public string DueDate { get; set; }
        public string AffectedSectors { get; set; }
        public string Description { get; set; }
    }
}
