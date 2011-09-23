using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SCNews.Models;

namespace SCNews.Models
{
    public class ProfileRepository
    {
        private NewsDataContext db = new NewsDataContext( ConfigurationManager.ConnectionStrings["StarcraftMDexpress"].ConnectionString );


        #region Profile Methods

        public void CreateProfile( Guid userId )
        {
            var profile = new UsersProfile();
            profile.user_id = userId;
            profile.country_id = 0;
            profile.bnet_name = "";
            profile.experience = 0;
            profile.first_name = "";
            profile.last_name = "";

            db.UsersProfiles.InsertOnSubmit( profile );
            db.SubmitChanges();
        }

        public UsersProfile GetProfile( Guid id )
        {
            var query =
                from p in db.UsersProfiles
                join m in db.UsersMemberships
                    on p.user_id equals m.UserId
                join u in db.Users
                    on p.user_id equals u.UserId
                select p;
            return query.SingleOrDefault( p => p.user_id == id );
        }

        #endregion

        public String GetCountryName( UsersProfile profile )
        {
            switch (profile.country_id)
            {
                case 1:
                    return "Молдова";
                case 2:
                    return "Украина";
                case 3:
                    return "Россия";
                default:
                    return "";
            }
        }

        #region Users Methods
        public Guid GetUserId( String userName )
        {
            return db.Users.SingleOrDefault( u => u.LoweredUserName == userName.ToLower() ).UserId;
        }
        #endregion


        #region Persistence
        public void Save()
        {
            db.SubmitChanges();
        }
        #endregion

    }
}