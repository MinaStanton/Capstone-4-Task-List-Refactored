using System;
using System.Collections.Generic;

namespace Capstone_Tasklist_Refactored.Models
{
    public partial class Tasks
    {
        public int Id { get; set; }
        public string TaskDescription { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
        public string TaskOwnerId { get; set; }

        public virtual AspNetUsers TaskOwner { get; set; }
    }
}
