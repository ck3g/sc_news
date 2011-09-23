using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCNews.Models;
using SCNews.Helpers;
using System.Text.RegularExpressions;

namespace SCNews.Controllers
{
    public class NewsController : Controller
    {
        NewsRepository newsRepository = new NewsRepository();

        //
        // GET: /News/
        [OutputCache( CacheProfile = "Cache10Minutes" )]
        public ActionResult Index( int? page )
        {
            const int pageSize = 10;

            //var news = newsRepository.FindAllNews().OrderByDescending( n => n.created_at );
            var news = newsRepository.FindNewsOnPage(page ?? 1, pageSize);
            var newsCount = newsRepository.GetNewsCount();
            var paginatedNews = new PaginatedList<News>(news, page ?? 1, pageSize, newsCount);
            ViewData["Tags"] = newsRepository.GetTags( news );

            return View( paginatedNews );
        }

        [OutputCache(CacheProfile = "Cache1HourTags")]
        public ActionResult Tags( String tagName, int? page )
        {
            const int pageSize = 10;

            var news = newsRepository.FindNewsByTag(tagName, page ?? 1, pageSize);
            var news_in_tag_count = newsRepository.GetNewsCountByTag(tagName);
            var paginatedNews = new PaginatedList<News>( news, page ?? 1, pageSize, news_in_tag_count);
            ViewData["Tags"] = newsRepository.GetTags( news );
            ViewData["TagName"] = tagName;

            return View( "Index", paginatedNews );

        }

        [Authorize( Roles = "Administrators, Tags" )]
        [OutputCache( CacheProfile = "Cache1HourTags" )]
        public ActionResult ManageTags()
        {
            var tags = newsRepository.GetAllTags( true );
            return View( tags );
        }

        [Authorize( Roles = "Administrators, Tags" )]
        public void RenameTag( Int32 id, String name )
        {
            newsRepository.RenameTag( id, name );
        }

        [Authorize( Roles = "Administrators, Tags" )]
        public void DeleteTag( Int32 id )
        {
            newsRepository.DeleteTag( id );
        }

        [OutputCache( CacheProfile = "Cache1Hour" )]
        public String TagList()
        {
            var tags = newsRepository.GetTopTags( 15 );
            var output = "<ul class=\"all-tags\">";
            foreach (var tag in tags)
            {
                var count = tag.Value > 0 ? "(" + tag.Value + ")" : "";
                output += "<li><a href='/Tags/" + tag.Key + "'>" + tag.Key + "</a>" + count + " </li>";
            }
            output += "<li><a href='/TagsCloud'>Облако тегов...</a></li></ul> ";
            return output;
        }

        [OutputCache( CacheProfile = "Cache1Hour" )]
        public String TagListAll()
        {
            var tags = newsRepository.GetTagsWithCount();
            var output = "<ul class=\"all-tags\">";
            foreach (var tag in tags)
            {
                var percent = 120 + tag.Value * 3;
                output += "<li style='font-size: " + percent + "%;'><a href='/Tags/" + tag.Key + "'>" + tag.Key + "</a> </li>";
            }
            output += "</ul>";
            return output;
        }

        [OutputCache( CacheProfile = "Cache1Hour" )]
        public ActionResult AllTags()
        {
            return View();
        }

        [OutputCache( CacheProfile = "Cache10Minutes" )]
        public ActionResult PinnedNews()
        {
            var news = newsRepository.FindAllPinnedNews().OrderByDescending( n => n.created_at );
            ViewData["Tags"] = newsRepository.GetTags( news );

            return View( news );
        }

        //
        // GET: /News/Details/5
        [OutputCache( CacheProfile = "Cache10MinutesById" )]
        public ActionResult Details( int id )
        {
            var news = newsRepository.GetNews( id );
            var comments = newsRepository.FindAllComments( news.id );
            var comment = new Comment();

            var newsComments = new NewsComments() {News = news, Comments = comments, Comment = comment};
            var newsId = news.id.ToString();
            ViewData["Tags"] = newsRepository.GetTags( news.id );

            // Increase page views count

            var cookie = Request.Cookies["views"];
            if (cookie == null)
            {
                var newcookie = new HttpCookie( "views" ) {Value = newsId, Expires = DateTime.UtcNow.AddYears( 1 )};
                Response.Cookies.Add( newcookie );
                newsRepository.IncreaseViewsCount( news.id );
            }
            else
            {
                var cookieValues = cookie.Value;
                if (!cookieValues.Contains( newsId ))
                {
                    cookie.Value += "|" + newsId;
                    Response.Cookies.Set( cookie );
                    newsRepository.IncreaseViewsCount( news.id );
                }
            }

            newsRepository.IncreaseHitsCount( news.id );

            if (news == null)
                return View( "NotFound" );
                
            return View( newsComments );
        }

        //
        // GET: /News/Create
        [ValidateInput( false )]
        [Authorize( Roles = "Administrators, News" )]
        public ActionResult Create()
        {
            var news = new News();
            ViewData["AllTags"] = newsRepository.GetAllTags();

            return View( news );
        }

        //
        // POST: /News/Create

        [HttpPost]
        [ValidateInput( false )]
        [Authorize( Roles = "Administrators, News" )]
        public ActionResult Create( FormCollection collection )
        {

            var news = new News();
            try
            {
                // TODO: Add insert logic here
                UpdateModel( news );
                news.created_at = DateTime.UtcNow;
                news.ip_address = System.Web.HttpContext.Current.Request.UserHostAddress;
                news.author_id = newsRepository.GetAuthorId( User.Identity.Name );
                news.voted_for = 0;
                news.voted_against = 0;
                news.state = 0;
                news.type = 0;
                news.views = 0;
                news.hits = 0;

                newsRepository.Add( news );
                newsRepository.Save();
                newsRepository.TagsJob( Request.Form["tags"], news.id );


                return RedirectToAction( "Index" );
            }
            catch
            {
                return View( "Error" );
            }
        }

        //
        // GET: /News/Edit/5
        [ValidateInput( false )]
        [Authorize( Roles = "Administrators, News" )]
        public ActionResult Edit( int id )
        {
            var news = newsRepository.GetNews( id );

            var items = new List<SelectListItem>();
            items.Add( new SelectListItem() { Text = "Обычная", Value = "0" } );
            items.Add( new SelectListItem() { Text = "Важная (в разработке)", Value = "1" } );
            items.Add( new SelectListItem() { Text = "Прикрепленная", Value = "2" } );
            ViewData["types"] = new SelectList( items, "Value", "Text", news.type );

            ViewData["AllTags"] = newsRepository.GetAllTags();
            ViewData["NewsTags"] = newsRepository.GetNewsTags( news.id );

            return View( news );
        }

        //
        // POST: /News/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        [Authorize( Roles = "Administrators, News" )]
        public ActionResult Edit( int id, FormCollection collection )
        {
            /**/
            try
            {
             /**/
                // TODO: Add update logic here
                var news = newsRepository.GetNews( id );
                news.title = Request.Form["title"];
                news.body = Request.Form["body"];
                news.ip_address = System.Web.HttpContext.Current.Request.UserHostAddress;  
                news.modified_at = DateTime.UtcNow;
                news.type = int.Parse( Request.Form["types"] );

                // Вместо всего этого можно использовать:
                //UpdateModel( news );

                newsRepository.Save();
                newsRepository.TagsJob( Request.Form["tags"], news.id );

                return RedirectToAction( "Index" );
            /**/
            }
            catch
            {
                return View();
            }
    /**/
        }

        //
        // GET: /News/Delete/5

        [Authorize( Roles = "Administrators, News" )]
        public ActionResult Delete( int id )
        {
            var news = newsRepository.GetNews( id );

            if (news == null)
                return View( "NotFound" );
            else
                return View( news );
        }

        //
        // POST: /News/Delete/5

        [HttpPost]
        [Authorize( Roles = "Administrators, News" )]
        public ActionResult Delete( int id, FormCollection collection )
        {
            var news = newsRepository.GetNews( id );
            try
            {
                // TODO: Add delete logic here
                if (news == null)
                    return View( "NotFound" );

                newsRepository.Delete( news );
                newsRepository.Save();

                return View( "Deleted" );
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult CreateComment()
        {
            var comment = new Comment();
            return View( comment );
        }


        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult CreateComment(FormCollection collection )
        {

            var comment = new Comment();
            try
            {
                // TODO: Add insert logic here
                UpdateModel( comment );

                Int64 newsId = 0;
                Int64.TryParse( Request.Form["newsId"], out newsId );
                comment.parent_id = newsId;
                comment.created_at = DateTime.UtcNow;
                comment.ip_address = System.Web.HttpContext.Current.Request.UserHostAddress;
                comment.author_id = newsRepository.GetAuthorId( User.Identity.Name );
                comment.voted_for = 0;
                comment.voted_against = 0;

                newsRepository.AddComment( comment );
                newsRepository.Save();

                return RedirectToAction( "Details", "News", new { id = newsId } );
            }
            catch
            {
                return View( "NotFound" );
            }
        }

        public ActionResult VoteFor( long id )
        {
            var voteCount = newsRepository.VoteForNews( id, User.Identity.Name );
            return Json( new { voteFor = voteCount }, JsonRequestBehavior.AllowGet );
        }

        public int GetCommentsCount( Int64 newsId )
        {
            return newsRepository.GetCommentsCount( newsId );
        }

        public ActionResult CommentRemove( long id ) {
            Boolean result = false;
            if (User.IsInRole( "Administrators" ) || User.IsInRole( "Power Users" )) {
                result = newsRepository.CommentRemove( id );
            }
            
            return Json( new { result = result }, JsonRequestBehavior.AllowGet );
        }

        public String ParseNewsBody( String body )
        {
            if (String.IsNullOrEmpty( body ))
                return "";

            body = body.Replace( "&lt;zerg&gt;", "<img src='/Content/img/races/zerg.png' alt='Зерг' />" );
            body = body.Replace( "&lt;protoss&gt;", "<img src='/Content/img/races/protoss.png' alt='Протосс' />" );
            body = body.Replace( "&lt;terran&gt;", "<img src='/Content/img/races/terran.png' alt='Терран' />" );
            body = body.Replace("&lt;random&gt;", "<img src='/Content/img/races/random.png' alt='Случайная' />");

            body = body.Replace("&lt;зерг&gt;", "<img src='/Content/img/races/zerg.png' alt='Зерг' />");
            body = body.Replace( "&lt;протосс&gt;", "<img src='/Content/img/races/protoss.png' alt='Протосс' />" );
            body = body.Replace( "&lt;терран&gt;", "<img src='/Content/img/races/terran.png' alt='Терран' />" );
            body = body.Replace( "&lt;случайная&gt;", "<img src='/Content/img/races/random.png' alt='Случайная' />" );

            body = body.Replace( "&lt;zerg1&gt;", "<img src='/Content/img/races/zerg1.png' alt='Зерг SC:BW' />" );
            body = body.Replace( "&lt;protoss1&gt;", "<img src='/Content/img/races/protoss1.png' alt='Протосс SC:BW' />" );
            body = body.Replace( "&lt;terran1&gt;", "<img src='/Content/img/races/terran1.png' alt='Терран SC:BW' />" );

            body = body.Replace( "&lt;зерг1&gt;", "<img src='/Content/img/races/zerg1.png' alt='Зерг SC:BW' />" );
            body = body.Replace( "&lt;протосс1&gt;", "<img src='/Content/img/races/protoss1.png' alt='Протосс SC:BW' />" );
            body = body.Replace( "&lt;терран1&gt;", "<img src='/Content/img/races/terran1.png' alt='Терран SC:BW' />" );

            body = body.Replace( "&lt;gold&gt;", "<img src='/Content/img/medals/gold.gif' alt='' />" );
            body = body.Replace( "&lt;silver&gt;", "<img src='/Content/img/medals/silver.gif' alt='' />" );
            body = body.Replace( "&lt;bronze&gt;", "<img src='/Content/img/medals/bronze.gif' alt='' />" );
            return body;
        }

        [OutputCache( CacheProfile = "Cache1Hour" )]
        public ActionResult LatestComments()
        {
            var comments = newsRepository.GetLatestComments( 5 );
            return View( comments );
        }

    }

    public class NewsComments
    {
        public News News { get; set; }
        public IQueryable<Comment> Comments { get; set; }
        public Comment Comment { get; set; }
    }
}
