using System;
using System.Net;
using System.Text.RegularExpressions;

namespace BattleNetInfo
{
    public class BattleNetProfile
    {
        public string ProfileUrl { get; set; }

        public string ProfileContent { get; set; }

        public string LadderContent { get; set; }

        public Int32 AchievementPoints { get; set; }

        public Int32 Points { get; set; }

        public Double Wins { get; set; }

        public Double Loses { get; set; }

        public String Percent { get; set; }

        public BattleNetProfile( String url )
        {
            ProfileContent = "";
            LadderContent = "";
            ProfileUrl = url;
        }

        public Boolean DownloadData()
        {
            var profileResult = DownloadProfileContent();
            var ladderResult = DownloadLadderContent();
            return profileResult && ladderResult;
        }

        public Boolean IsValidUrl()
        {
            return IsValidUrl( ProfileUrl );
        }

        public Boolean IsValidUrl( String url )
        {
            var re = new Regex( @"http://\w+\.battle.net/sc2/\w+\/profile/\d+\/\d\/\w+\/", RegexOptions.IgnoreCase );
            return re.IsMatch( url );
        }

        public String GetServer()
        {
            var re = new Regex( @"http://(?<server>\w+)" );
            return re.Match( ProfileUrl ).Groups["server"].Value;
        }

        public String GetPlayerName()
        {
            var re = new Regex( @"profile/\d+\/\d\/(?<name>\w+)" );
            return re.Match( ProfileUrl ).Groups["name"].Value;
        }

        public Boolean DownloadProfileContent()
        {
            if (!IsValidUrl())
                return false;

            var webClient = new WebClient();
            var content = webClient.DownloadData( ProfileUrl );
            var result = System.Text.Encoding.UTF8.GetString( content );
            if (String.IsNullOrEmpty( result ))
                return false;

            ProfileContent = result;
            return true;
        }

        public Boolean DownloadLadderContent()
        {
            var ladderUrl = ProfileUrl + "ladder/leagues";
            if (!IsValidUrl( ladderUrl ))
                return false;

            var webClient = new WebClient();
            var content = webClient.DownloadData( ladderUrl );
            var result = System.Text.Encoding.UTF8.GetString( content );
            if (String.IsNullOrEmpty( result ))
                return false;

            LadderContent = result;
            return true;
        }

        public Int32 GetAchievementPoints()
        {
            var re = new Regex( @"<h3>(?<apoints>\d+)</h3>" );
            var result = re.Match( ProfileContent ).Groups["apoints"].Value;
            var points = 0;
            Int32.TryParse( result, out points );

            AchievementPoints = points;
            return points;
        }

        public String GetRace()
        {
            var re = new Regex( @"class=\Wrace-(?<race>\w+)\W" );
            return re.Match( ProfileContent ).Groups["race"].Value;
        }

        public void GetStats()
        {
            var ladderLeaguePattern = @"<div\s+class=\Wtooltip-title\W>" + GetPlayerName() + @"</div>[\n\t\s]+<strong>[\w\s]+\W</strong>\s*\d+<br\s*/>[\t\n\s]+<strong>[\w\s]+\W\s*</strong>\s*\w+\s*</div>[\t\n\s]+</td>[\t\n\s]+<td class=\Walign-center\W>(?<points>\d+)</td>[\t\n\s]+<td class=\Walign-center\W>(?<wins>\d+)</td>";
            var re = new Regex( ladderLeaguePattern, RegexOptions.Multiline | RegexOptions.IgnoreCase );
            var result = re.Match( LadderContent );

            var points = 0;
            Int32.TryParse( result.Groups["points"].Value, out points );
            Points = points;

            var wins = 0.0;
            Double.TryParse( result.Groups["wins"].Value, out wins );
            Wins = wins;            

        }

        public Int32 GetRank()
        {
            var re = new Regex( @"<td\sclass=\Walign-center\W\sstyle=\Wwidth:\s40px\W>(?<rank>\d+)\W*\w+</td>[\n\t\s]+<td>[\n\t\s]+<a href=\W/sc2/\w+/profile/\d+/\d/" + GetPlayerName() + @"/", RegexOptions.IgnoreCase | RegexOptions.Multiline );
            var result = re.Match( LadderContent ).Groups["rank"].Value;
            var rank = 0;
            Int32.TryParse( result, out rank );
            return rank;
        }

        public String GetLeague()
        {
            var re = new Regex( @"badge-(?<league>\w+)\sbadge-medium-(?<top>\d+)\W>\s+</span>\s+</a>\s+<div\sid=\Wbest-team-1", RegexOptions.Multiline | RegexOptions.IgnoreCase );
            return re.Match( ProfileContent ).Groups["league"].Value + "_" + re.Match( ProfileContent ).Groups["top"].Value;
        }

        public String GetPortraitHtmlStyle( String newPath )
        {
            var re = new Regex( @"<span class=\Wicon-frame\s+\W[\s\t\n]+(?<PictureStyle>\w+\=.+portraits.+)>", RegexOptions.Multiline | RegexOptions.IgnoreCase );
            var result = re.Match( ProfileContent ).Groups["PictureStyle"].Value;
            return result.Replace( "/sc2/static/local-common/images/sc2/portraits/", newPath );

        }
    }
}
