using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RSSFeedv2.Models
{
    public class SubscriptionModel
    {
        public int ID { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "RSS Feed URL")]
        [Required]
        public string FeedURL { get; set; }

        public string UserID { get; set; }
    }
}
