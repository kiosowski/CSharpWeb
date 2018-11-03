using System;
using System.Collections.Generic;
using System.Text;

namespace Torshia.Web.ViewModels
{
    public class ReportViewModelWrapper
    {
        public ReportViewModelWrapper()
        {
            this.ReportViewModels = new List<ReportViewModel>();
        }
        public ICollection<ReportViewModel> ReportViewModels { get; set; }
    }
}
