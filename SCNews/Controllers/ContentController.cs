using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCNews.Models;

namespace SCNews.Controllers
{
    
    public class ContentController : Controller
    {
        ContentRepository contentRepository = new ContentRepository();


        //
        // GET: /Content/
        [OutputCache( CacheProfile = "Cache1Hour" )]
        [Authorize( Roles = "Administrators, Content" )]
        public ActionResult Index()
        {
            var content = contentRepository.FindAll();
            return View( content );
        }

        //
        // GET: /Content/Details/5
        [OutputCache( CacheProfile = "Cache1HourById" )]
        public ActionResult Details(int id)
        {
            var content = contentRepository.Find(id);
            return PartialView( content );
        }

        //
        // GET: /Content/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Content/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Content/Edit/5
        [Authorize( Roles = "Administrators, Content" )]
        public ActionResult Edit(int id)
        {
            var content = contentRepository.Find(id);
            return View( content );
        }

        //
        // POST: /Content/Edit/5

        [HttpPost]
        [ValidateInput( false )]
        [Authorize( Roles = "Administrators, Content" )]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var content = contentRepository.Find(id);
                content.text = Request.Form["text"];
                contentRepository.Save();
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Content/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Content/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
