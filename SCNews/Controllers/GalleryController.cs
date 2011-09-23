using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SCNews.Helpers;
using SCNews.Models;

namespace SCNews.Controllers
{
    [OutputCache(CacheProfile = "Cache1Hour")]
    public class GalleryController : Controller
    {
        GalleryRepository galleryRepository = new GalleryRepository();

        //
        // GET: /Gallery/

        [Authorize(Roles = "Administrators, Gallery")]
        public ActionResult Index( int? page )
        {
            const int pageSize = 30;
            var picturesCount = galleryRepository.GetPicturesCount();
            var pictures = galleryRepository.FindAllPicturesOnPage(page ?? 1, pageSize);
            var paginagedPictures = new PaginatedList<Gallery>( pictures, page ?? 1, pageSize, picturesCount );

            return View( paginagedPictures );
        }

        //
        // GET: /Gallery/Details/5
        [Authorize(Roles = "Administrators, Gallery")]
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Gallery/Create
        [Authorize( Roles = "Administrators, Gallery" )]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Gallery/Create

        [HttpPost]
        [Authorize( Roles = "Administrators, Gallery" )]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var picture = new Gallery();
                // TODO: Add insert logic here
                var info = new StringBuilder();
                foreach (string file in Request.Files) 
                {
                    HttpPostedFileBase postedFile = Request.Files[file];
                    if (postedFile.ContentLength == 0)
                        continue;
                    var newFileName = Guid.NewGuid() + "-" + postedFile.FileName;
                    string newFilePath = Path.Combine( 
                        AppDomain.CurrentDomain.BaseDirectory + @"Content\Gallery\",
                        Path.GetFileName( newFileName )
                    );
                    postedFile.SaveAs( newFilePath );

                    picture.name = Request.Form["name"];
                    picture.filename = newFileName;
                    picture.upload_by = galleryRepository.GetAuthorId( User.Identity.Name );
                    picture.upload_date = DateTime.UtcNow;
                    galleryRepository.Add( picture );
                    galleryRepository.Save();
                }


                return RedirectToAction("Index");
            }
            catch
            {
                return View( "Error" );
            }
        }
        
        //
        // GET: /Gallery/Edit/5

        [Authorize( Roles = "Administrators, Gallery" )]
        public ActionResult Edit(int id)
        {
            
            return View();
        }

        //
        // POST: /Gallery/Edit/5

        [HttpPost]
        [Authorize( Roles = "Administrators, Gallery" )]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Gallery/Delete/5

        [Authorize( Roles = "Administrators" )]
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Gallery/Delete/5

        [HttpPost]
        [Authorize( Roles = "Administrators" )]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                galleryRepository.DeletePicture( id );
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
