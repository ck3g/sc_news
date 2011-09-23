using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BattleNetInfo;
using SCNews.Models;


namespace SCNews.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [Authorize( Roles = "Administrator" )]
        public ActionResult Index()
        {
            /*
            ViewData["Message"] = "Welcome to ASP.NET MVC!";
            /*
            var mpq = new MpqLib.Mpq.CArchive( "D:\\mouz.SC2Replay" );
            ViewData["FileName"] = "";
            
            foreach (var file in mpq.FindFiles( "*" ))
            {
                ViewData["FileName"] += file.FileName + "<br />";
            }

            mpq.ExportFile( "replay.details", "D:\\mouz.txt" );
            /**/
            
            /*
            const string filePath = "D:\\mouz.SC2Replay";
            var fileInfo = new FileInfo( filePath );
            ViewData["Exists"] = fileInfo.Exists;

            if (fileInfo.Exists)
            {
                var gameInfo = ReplayParser.Load( filePath );
                ViewData["GameInfo"] = gameInfo;
                //ViewData["GameSpeed"] = gameInfo.GameSpeed;
                //ViewData["Map"] = gameInfo.Map.Name;
                //ViewData["ImageSize"] = gameInfo.Map.Thumbnail.Height + "x" + gameInfo.Map.Thumbnail.Width;
                ViewData["PlayersCount"] = gameInfo.Players.Count();

                foreach (Player player in gameInfo.Players)
                    ViewData["Players"] +=
                        string.Format( "\tName: {0}\n\r\tRace: {1}\n\r\tHuman: {2}\n\r\tColor: {3}\n\r\tParty: {4}\n\r", player.Name,
                                       player.Race.ToString(), player.IsHuman, player.Color.Name, player.Party );
            }
            /**/
            return View();
        }

        [Authorize( Roles = "Administrator" )]
        public ActionResult Test()
        {
            /**/
            String[] urls = {
                                @"http://eu.battle.net/sc2/ru/profile/267901/1/Zakk/",
                                @"http://eu.battle.net/sc2/ru/profile/355105/1/mapcuahuh/",
                                @"http://eu.battle.net/sc2/ru/profile/336403/1/Plastique/",
                                @"http://google.com",
                                @"http://eu.battle.net/sc2/en/profile/248476/1/MaDFroG/",
                                @"http://eu.battle.net/sc2/en/profile/383164/1/DuckloadRa/"
                            };
            /**/
            string pageContent = "<ul style='list-style-type: none; margin: 0; padding: 0;'>";

            foreach (var url in urls)
            {
                pageContent += "<li style='display: block; float: left; margin: 10px;'>";
                /**/
                var profile = new BattleNetProfile( url );
                if (profile.IsValidUrl())
                {
                    profile.DownloadData();
                    pageContent += "<div " +
                                   profile.GetPortraitHtmlStyle( @"/Content/img/bnet/portraits-" ) +
                                   "></div>";
                    pageContent += "<div>PlayerName:" + profile.GetPlayerName() + "</div>";
                    pageContent += "<div>Server:" + profile.GetServer() + "</div>";
                    pageContent += "<div>Achievements:" + profile.GetAchievementPoints() + "</div>";
                    pageContent += "<div>Race:" + profile.GetRace() + "</div>";
                    pageContent += "<div>League:" + profile.GetLeague() + "</div>";
                    var leagueParts = profile.GetLeague().Split('_');
                                        
                    pageContent += "<div>LeagueImg:<span class=\"badge badge-" + leagueParts[0] + " badge-medium-" + leagueParts[1] + "\"></span></div>";
                    pageContent += "<div>Rank:" + profile.GetRank() + "</div>";
                    profile.GetStats();
                    pageContent += "<div>Points:" + profile.Points + "</div>";
                    pageContent += "<div>Wins:" + profile.Wins + "</div>";

                    //pageContent += "<div>LadderContent: " + profile.LadderContent + "</div>";
                    //pageContent += "<div>ProfileContent: " + profile.ProfileContent + "</div>";
                }
                else
                {
                    pageContent += "<div>Invalid url</div>";
                }

                /**/
                pageContent += "</li>";
            }

            pageContent += "</ul>";
            ViewData["Content"] = pageContent;
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Bnet()
        {
            var url = @"http://eu.battle.net/sc2/ru/profile/267901/1/Zakk/";
            var webClient = new WebClient();
            var content = webClient. DownloadData( url );
            var result = System. Text. Encoding. UTF8. GetString( content );
            if (String. IsNullOrEmpty( result ))
            {
                ViewData["Content"] = "Result is empty";
                return View( "Test" );
            }

            ViewData["Content"] = "Content downloaded sucessfully<br />";
            var ProfileContent = result;

            // Get Achievements
            var re = new Regex( @"<h3>(?<apoints>\d+)</h3>" );
            var result2 = re. Match( ProfileContent ). Groups["apoints"]. Value;
            var points = 0;
            Int32. TryParse( result2, out points );

            var AchievementPoints = points;
            ViewData["Content"] += "<b>Achievements:</b> " + AchievementPoints;



            //ViewData["Content"] = "some text <strong>here</strong>";)
            return View("Test");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Rules()
        {
            return View();
        }

        public ActionResult Jobs()
        {
            return View();
        }

        public ActionResult Http404( string url )
        {
            Response.StatusCode = 404;
            ViewData["url"] = url;
            return View();
        }

        public ActionResult UnknownError( string url )
        {
            ViewData["url"] = url;
            return View();
        }
    }
}
