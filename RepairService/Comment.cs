using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairService
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
