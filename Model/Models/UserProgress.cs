using Model.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class UserProgress
    {
        public int Id { get; set; }

        public User User { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public Level Level { get; set; }
        [ForeignKey("LevelId")]
        public int LevelId { get; set; }

        public int ProgressPercent { get; set; }
    }
}
