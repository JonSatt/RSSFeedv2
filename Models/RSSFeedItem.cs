using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RSSFeedv2.Models
{
    public class RSSFeedItem
    {
        [DataType(DataType.ImageUrl)]
        public Uri ImageSrc { get; set; }

        [DataType(DataType.Url)]
        public Uri ItemSrc { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }

    public class RSSFeedItems : List<RSSFeedItem>
    {
    }
}
