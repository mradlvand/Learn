using Model.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Podcast
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? Sound { get; set; }
        public string? Image { get; set; }

        public PodcastCategory PodcastCategory { get; set; }
        [ForeignKey("PodcastCategoryId")]
        public int PodcastCategoryId { get; set; }

    }
}
