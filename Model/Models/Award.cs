using Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Award
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public AwardType AwardType { get; set; }
        public int Score { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? Video { get; set; }
        public string? Image { get; set; }
        public bool Status { get; set; }
    }
}
