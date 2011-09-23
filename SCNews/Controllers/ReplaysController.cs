using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SCNews.Helpers;
using SCNews.Models;

namespace SCNews.Controllers
{
    [OutputCache( CacheProfile = "Cache1Hour" )]
    public class ReplaysController : Controller
    {
        ReplaysRepository replaysRepository = new ReplaysRepository();

        //
        // GET: /Replays/

        public ActionResult Index( int? page )
        {
            const int pageSize = 25;

            //todo: refactor
            var replays = replaysRepository.GetAll().OrderByDescending( r => r.uploaded_at ).Take(((page ?? 1) - 1) * pageSize).Skip(pageSize);
            var paginatedReplays = new PaginatedList<Replay>( replays, page ?? 1, pageSize, replays.Count() );

            return View( paginatedReplays );
        }

        //
        // GET: /Replays/Details/5

        public ActionResult Details( int id )
        {
            return View();
        }

        //
        // GET: /Replays/Create

        [Authorize( Roles = "Administrators, Replays" )]
        public ActionResult Create()
        {
            var replays = new Replay();
            return View( replays );
        }

        //
        // POST: /Replays/Create

        [HttpPost]
        [Authorize( Roles = "Administrators, Replays" )]
        public ActionResult Create( FormCollection collection )
        {
            try
            {
                var replay = new Replay();
                var info = new StringBuilder();
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase postedFile = Request.Files[file];

                    if (postedFile == null || postedFile.ContentLength == 0)
                        continue;

                    if (postedFile.ContentLength > 8 * 1024 * 1024)
                    {
                        return RedirectToAction( "Create" );
                    }

                    var newFileName = Guid.NewGuid() + "-" + postedFile.FileName;
                    var newFilePath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory + @"Content\Replays\",
                        Path.GetFileName( newFileName )
                    );

                    postedFile.SaveAs( newFilePath );

                    replay.name = Request.Form["name"];
                    replay.file_name = newFileName;
                    replay.file_size = postedFile.ContentLength;
                    replay.description = Request.Form["description"];
                    replay.uploaded_by = replaysRepository.GetAuthorId( User.Identity.Name );
                    replay.uploaded_at = DateTime.UtcNow;
                    replaysRepository.Insert( replay );
                    replaysRepository.Save();
                }
                ViewData["Info"] = "Реплей успешно загружен";
                return RedirectToAction( "Index" );
            }
            catch
            {
                return View( "Error" );
            }
        }

        //
        // GET: /Replays/Edit/5

        public ActionResult Edit( int id )
        {
            return View();
        }

        //
        // POST: /Replays/Edit/5

        [HttpPost]
        public ActionResult Edit( int id, FormCollection collection )
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction( "Index" );
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Replays/Delete/5

        public ActionResult Delete( int id )
        {
            if (User.IsInRole( "Administrators" ) || User.IsInRole( "Replays" ))
            {
                var replay = replaysRepository.GetById( id );
                var filePath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory + @"Content\Replays\",
                        Path.GetFileName( replay.file_name )
                    );

                if (System.IO.File.Exists( filePath ))
                    System.IO.File.Delete( filePath );

                replaysRepository.Delete( replay );
                replaysRepository.Save();
            }

            return Json( new { result = true }, JsonRequestBehavior.AllowGet );
        }

        

    }
}
