using System;
using Newtonsoft.Json;

namespace MobileJO.Data.Models.TFSIntegration
{
    public class Fields
    {
        [JsonProperty("System.AreaPath")]
        public string AreaPath { get; set; }

        [JsonProperty("System.TeamProject")]
        public string TeamProject { get; set; }

        [JsonProperty("System.IterationPath")]
        public string IterationPath { get; set; }

        [JsonProperty("System.WorkItemType")]
        public string WorkItemType { get; set; }

        [JsonProperty("System.State")]
        public string State { get; set; }

        [JsonProperty("System.Reason")]
        public string Reason { get; set; }

        [JsonProperty("System.AssignedTo")]
        public string AssignedTo { get; set; }

        [JsonProperty("System.CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("System.CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("System.ChangedDate")]
        public DateTime changeDate { get; set; }

        [JsonProperty("System.ChangedBy")]
        public string ChangedBy { get; set; }

        [JsonProperty("System.Title")]
        public string Title { get; set; }

        [JsonProperty("System.BoardColumn")]
        public string BoardColumn { get; set; }

        [JsonProperty("System.BoardColumnDone")]
        public Boolean BoardColumnDone { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.Priority")]
        public int Priority { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.BacklogPriority")]
        public double BacklogPriority { get; set; }

        [JsonProperty("Microsoft.VSTS.Scheduling.Effort")]
        public double Effort { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.ValueArea")]
        public string ValueArea { get; set; }

        [JsonProperty("WEF_57345BCC70C34E528C1C7C15390858F1_Kanban.Column")]
        public string Column { get; set; }

        [JsonProperty("WEF_57345BCC70C34E528C1C7C15390858F1_Kanban.Column.Done")]
        public Boolean ColumnDone { get; set; }

        [JsonProperty("Alliance.Groundup.Workitem.ShortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("Alliance.Groundup.Workitem.AccountName")]
        public string AccountName { get; set; }
    }
}
