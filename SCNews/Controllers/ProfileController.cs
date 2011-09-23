using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BattleNetInfo;
using SCNews.Models;

namespace SCNews.Controllers
{
    public class ProfileController : Controller
    {
        ProfileRepository profileRepository = new ProfileRepository();

        // GET: /Profile/
        // GET: /Profile/UserName
        [Authorize]
        public ActionResult Details(String userName)
        {
            UsersProfile profile;
            
            if (userName == null)
                profile = profileRepository.GetProfile( profileRepository.GetUserId( User.Identity.Name ) );
            else
            {
                try
                {
                    profile = profileRepository.GetProfile( profileRepository.GetUserId( userName ) );
                }
                catch(NullReferenceException)
                {
                    return View( "UserNotFound" );
                }
            }

            if (profile == null)
                return View( "UserNotFound" );

            ViewData["Country"] = profileRepository.GetCountryName( profile );
            return View( profile );

        }


        
        //
        // GET: /Profile/Edit/5
 
        [Authorize]
        public ActionResult Edit()
        {
            var profile = profileRepository.GetProfile( profileRepository.GetUserId( User.Identity.Name ) );

            var countries = new List<SelectListItem>
                                {
                                    new SelectListItem() {Text = "", Value = "0"},
                                    new SelectListItem() {Text = "Молдова", Value = "1"},
                                    new SelectListItem() {Text = "Украина", Value = "2"},
                                    new SelectListItem() {Text = "Россия", Value = "3"}
                                };
            ViewData["country_id"] = new SelectList( countries, "Value", "Text", profile.country_id );
            return View( profile );
        }

        //
        // POST: /Profile/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(FormCollection collection)
        {
            try
            {
                var profile = profileRepository.GetProfile( profileRepository.GetUserId( User.Identity.Name ) );
                profile.first_name = Request.Form["first_name"];
                profile.last_name = Request.Form["last_name"];
                profile.details = Request.Form["details"];
                profile.country_id = Convert.ToInt32( Request.Form["country_id"] );
                
                var profileUrl = Request.Form["profile_url"];
                if (!String.IsNullOrEmpty( profileUrl ))
                {
                    var bnetProfile = new BattleNetProfile( profileUrl );
                    if (!bnetProfile.IsValidUrl())
                        return RedirectToAction( "Edit" );

                    bnetProfile.DownloadData();

                    profile.profile_url = profileUrl;
                    profile.bnet_name = bnetProfile.GetPlayerName();
                    profile.bnet_server = bnetProfile.GetServer();
                    profile.league = bnetProfile.GetLeague();
                    profile.race = bnetProfile.GetRace();
                    profile.avatar_style =
                        bnetProfile.GetPortraitHtmlStyle( @"/Content/img/bnet/" );
                    profile.achievements = bnetProfile.GetAchievementPoints();
                    profile.rank = bnetProfile.GetRank();
                    bnetProfile.GetStats();
                    profile.points = bnetProfile.Points;
                    profile.wins = Convert.ToInt32( bnetProfile.Wins );
                    profile.loses = Convert.ToInt32( bnetProfile.Loses );
                    profile.win_rate = bnetProfile.Percent;
                    profile.synchronized_at = DateTime.UtcNow;
                }
                else
                {
                    profile.profile_url = "";
                }


                profileRepository.Save();
 
                return RedirectToAction( "" );
            }
            catch
            {
                return View();
            }
        }

        public String GetAvatarHtml( String username )
        {
            var profile = profileRepository.GetProfile( profileRepository.GetUserId( username ) );
            if (!String.IsNullOrEmpty( profile.avatar_style ))
            {
                return "<div " + profile.avatar_style + "></div>";
            }
            // Default avatar
            return "<img src=\"/Content/img/avatar.gif\" alt=\"\" style=\"height: 90px;\"/>";
        }

        public String RaceToText( String race )
        {
            switch (race)
            {
                case "terran":
                    return "Терраны";
                case "protoss":
                    return "Протосс";
                case "zerg":
                    return "Зерг";
                case "random":
                    return "Любая";
                default:
                    return "";
            }
        }

        public String ServerToText( String server )
        {
            switch (server)
            {
                case "eu":
                    return "Европа";
                case "kr":
                    return "Корея";
                case "us":
                    return "США";
                case "tw":
                    return "Тайвань";
                case "sea":
                    return "Южно-Центральная Азия";
                default:
                    return "";
            }
        }

    }
}
