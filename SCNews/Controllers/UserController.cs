using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BattleNetInfo;
using SCNews.Helpers;
using SCNews.Models;

namespace SCNews.Controllers
{
    public class UserController : Controller
    {
        UserRepository userRepository = new UserRepository();
        ProfileRepository profileRepository = new ProfileRepository();

        //
        // GET: /User/
        [Authorize(Roles = "Administrators")]
        public ActionResult Index( int? page )
        {
            const int pageSize = 25;

            var users = userRepository.FindUsersOnPage(page ?? 1, pageSize);
            var usersCount = userRepository.GetUsersCount();
            var paginatedUsers = new PaginatedList<User>(users, page ?? 1, pageSize, usersCount);

            return View( paginatedUsers );

        }

        public ActionResult ManageRoles( String userName ) {
            if (userName == "")
                return View( "UserNotFound" );

            var userId = profileRepository.GetUserId( userName );
            var roles = userRepository.GetAllRoles();

            ViewData["UserName"] = userName;
            ViewData["userId"] = userId;
            return View( roles );
        }

        public String IsInRole( Guid roleId, Guid userId ) {
            var result = userRepository.IsInRole( roleId, userId );
            if (result) {
                return "checked='checked'";
            }
            return "";
        }

        public ActionResult RemoveRole( Guid roleId, Guid userId ) {
            userRepository.RemoveRole( roleId, userId );
            return Json( new { result = "ok" }, JsonRequestBehavior.AllowGet );
        }

        public ActionResult AddRole( Guid roleId, Guid userId ) {
            userRepository.AddRole( roleId, userId );
            return Json( new { result = "ok" }, JsonRequestBehavior.AllowGet );
        }

        public ActionResult ShowOnlineUsers()
        {
            var onlineUsers = userRepository.GetOnlineUsers();
            return View( onlineUsers );
        }

        public void UpdateActivity()
        {
            userRepository.UpdateActivity( User.Identity.Name );
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /User/Create

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
        // GET: /User/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
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
        // GET: /User/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /User/Delete/5

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

        [HttpGet]
        public ActionResult GetBnetUsers()
        {
            var users = userRepository.FindAllUsers().Where( u => u.UsersProfile.profile_url != "" );
            var bnetUsers = new List<String>();
            foreach (var user in users)
            {
                bnetUsers.Add( user.UserName );
            }

            return Json( bnetUsers, JsonRequestBehavior.AllowGet );
        }

        [HttpGet]
        public ActionResult ReloadBnetData( String username )
        {
            var userProfile = userRepository.GetUserProfile( username );
            if (String.IsNullOrEmpty( userProfile.profile_url ))
                return Json( new {result = "profile_url_is_empty"}, JsonRequestBehavior.AllowGet );

            var bnetProfile = new BattleNetProfile( userProfile.profile_url );
            if (!bnetProfile.IsValidUrl())
                return Json( new { result = "profile_url_is_empty" }, JsonRequestBehavior.AllowGet );

            bnetProfile.DownloadData();
            userProfile.race = bnetProfile.GetRace();
            userProfile.bnet_name = bnetProfile.GetPlayerName();
            userProfile.bnet_server = bnetProfile.GetServer();
            userProfile.league = bnetProfile.GetLeague();
            userProfile.avatar_style = bnetProfile.GetPortraitHtmlStyle( @"/Content/img/bnet/" );
            userProfile.achievements = bnetProfile.GetAchievementPoints();
            userProfile.rank = bnetProfile.GetRank();
            bnetProfile.GetStats();
            userProfile.points = bnetProfile.Points;
            userProfile.wins = Convert.ToInt32( bnetProfile.Wins );
            userProfile.loses = Convert.ToInt32( bnetProfile.Loses );
            userProfile.win_rate = bnetProfile.Percent;
            userProfile.synchronized_at = DateTime.UtcNow;
            userRepository.Save();

            return Json( new { result = "ok" }, JsonRequestBehavior.AllowGet );
        }

    }
}
