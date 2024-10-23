using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class UserAward
    {
        public int Id { get; set; }

        public User User { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public Award Award { get; set; }
        [ForeignKey("AwardId")]
        public int AwardId { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
