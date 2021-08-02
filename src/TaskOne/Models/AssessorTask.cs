using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOne.Models
{
    class AssessorTask
    {
        public string Login { get; set; }
        public decimal TaskId { get; set; }
        public short Microtasks { get; set; }
        public DateTime AssignedTs { get; set; }
        public DateTime ClosedTs { get; set; }
        public DateTimeRange DateTimeRange 
        {
            get {
                return new DateTimeRange(this.AssignedTs, this.ClosedTs);
            } 
        }
    }
}
