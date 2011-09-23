using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCNews.Models;

namespace SCNews.Controllers
{
    [OutputCache(CacheProfile = "Cache10Hours")]
    public class MenuController : Controller
    {

        MenuRepository menuRepository = new MenuRepository();

        //
        // GET: /Menu/
        public ActionResult Index()
        {
            var types = menuRepository.GetTypes();
            return View( types );
        }

        [HttpPost]
        public ActionResult Menus()
        {
            var menu = menuRepository.GetMenus( 1 );
            return PartialView( menu );
        }

        //
        // GET: /Menu/Details/5

        public ActionResult Details()
        {
            var menu = menuRepository.GetMenus( 1 );
            return View( menu );
        }

        //
        // GET: /Menu/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Menu/Create

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
        // POST: /Menu/Edit/5
        [HttpPost]
        [Authorize( Roles = "Administrators, Menu" )]
        public ActionResult Edit( String[] menuNames, String[] menuUrls )
        {
            try
            {
                // TODO: Add update logic here
                var type = Int32.Parse( Request.Form["type"] );
                var menus = menuRepository.GetMenus( type );
                foreach (var menu in menus)
                    menuRepository.Delete( menu );

                var orderN = 1;
                for (var i = 0; i < menuNames.Count(); i++)
                {
                    if (String.IsNullOrEmpty( menuNames[i] ) || String.IsNullOrEmpty( menuUrls[i] ))
                        continue;

                    var menu = new Menu {order_n = orderN, name = menuNames[i], url = menuUrls[i], type_id = type};
                    menuRepository.Insert( menu );
                    orderN++;
                }
                return RedirectToAction( "Index" );
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Menu/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Menu/Delete/5

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

    public class MenuOptions
    {
        public Menu Menu { get; set; }
        public MenuType MenuType { get; set; }
    }
}
