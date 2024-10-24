using Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Story
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? Description { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public string? Location { get; set; }
        public string? Picture { get; set; }
        public string? Video { get; set; }
    }
}
