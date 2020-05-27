using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RSSFeedv2.Models;
using RSSFeedv2.Data;
using System.Xml;
using System.ServiceModel.Syndication;

namespace RSSFeedv2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Feed));
            }
            return View();
        }

        public IActionResult Feed(string sortBy)
        {
            var feeds = from subs in _context.Subscription
                        join user in _context.Users on subs.UserID equals user.Id
                        where user.UserName == User.Identity.Name
                        select subs.FeedURL;

            var RSSItems = GetRSSFeedItems(feeds.ToList());

            //Sorting 
            ViewBag.SortingDate = string.IsNullOrEmpty(sortBy) ? "date" : "";
            ViewBag.SortingTitle = sortBy == "Title" ? "title_desc" : "Title";
            ViewBag.SortingDesc = sortBy == "Description" ? "description_desc" : "Description";
            var items = from item in RSSItems select item;
            switch (sortBy)
            {
                case "date":
                    items = items.OrderByDescending(item => item.PublishDate);
                    break;
                case "Title":
                    items = items.OrderBy(item => item.Title);
                    break;
                case "title_desc":
                    items = items.OrderByDescending(item => item.Title);
                    break;
                case "Description":
                    items = items.OrderBy(item => item.Description);
                    break;
                case "description_desc":
                    items = items.OrderByDescending(item => item.Description);
                    break;
                default:
                    items = items.OrderByDescending(item => item.PublishDate);
                    break;
            }

            return View(items.ToList());
        }

        public IActionResult Subscription()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscription([Bind("ID,FeedURL,UserID")] SubscriptionModel subscription)
        {
            if(ModelState.IsValid)
            {
                var id = (from user in _context.Users
                         where user.UserName == User.Identity.Name
                         select user.Id).First();

                var sub = new SubscriptionModel { FeedURL = subscription.FeedURL, UserID = id };
                _context.Add(sub);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Feed));
        }

        private List<RSSFeedItem> GetRSSFeedItems(List<string> feedList)
        {
            var RSSItems = new List<RSSFeedItem>();
            foreach (var url in feedList)
            {
                XmlReader reader = XmlReader.Create(url);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                reader.Close();
                foreach (var item in feed.Items)
                {
                    var RSSItem = new RSSFeedItem();
                    RSSItem.ImageSrc = feed.ImageUrl;
                    RSSItem.ItemSrc = item.Links.FirstOrDefault().Uri;
                    RSSItem.PublishDate = item.PublishDate.DateTime;
                    RSSItem.Title = item.Title.Text;
                    RSSItem.Description = item.Summary.Text;

                    RSSItems.Add(RSSItem);
                }
            }
            return RSSItems;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
