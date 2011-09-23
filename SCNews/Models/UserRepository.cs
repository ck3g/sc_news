using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SCNews.Models {
    public class UserRepository {
        private readonly NewsDataContext _db = new NewsDataContext( ConfigurationManager.ConnectionStrings["StarcraftMDexpress"].ConnectionString );

        public IQueryable<User> FindAllUsers() {
            var query =
                from users in _db.Users
                select users;
            return query;
        }

        public IQueryable<User> FindUsersOnPage(int page, int pageSize) {
            var query =
                (from users in _db.Users
                select users).OrderBy(n => n.UserName).Skip((page - 1) * pageSize).Take(pageSize);
            return query;
        }

        public int GetUsersCount()
        {
            return
                (from users in _db.Users
                 select users.UserId).Count();
        }

        public IQueryable<User> GetOnlineUsers()
        {
            var query = 
                from users in _db.Users
                where users.LastActivityDate >= DateTime.UtcNow.AddMinutes( -10 )
                select users;
            return query;
        }

        public IQueryable<Role> GetAllRoles() {
            var query =
                from r in _db.Roles
                select r;
            return query;
        }

        public IQueryable<UsersRole> GetUserRoles( Guid userId ) {
            var query =
                from r in _db.Roles
                join ur in _db.UsersRoles
                    on r.RoleId equals ur.RoleId into userRoles
                from roles in userRoles.DefaultIfEmpty()
                where roles.UserId == userId
                select roles;
            return query;
        }

        public Boolean IsInRole( Guid roleId, Guid userId ) {
            var query =
                from ur in _db.UsersRoles
                where ur.UserId == userId && ur.RoleId == roleId
                select ur;
            if (query.Count() == 0)
                return false;
            return true;

        }

        public UsersProfile GetUserProfile( Guid userId )
        {
            return _db.UsersProfiles.SingleOrDefault( p => p.user_id == userId );
        }

        public UsersProfile GetUserProfile( String userName )
        {
            return GetUserProfile( _db.Users.SingleOrDefault( u => u.LoweredUserName == userName.ToLower() ).UserId );
        }

        public void UpdateActivity( String userName )
        {
            var user = _db.Users.SingleOrDefault( u => u.LoweredUserName == userName.ToLower() );
            user.LastActivityDate = DateTime.UtcNow;
            Save();
        }

        #region Insert/Delete methods

        internal void RemoveRole( Guid roleId, Guid userId ) {
            var query =
                from ur in _db.UsersRoles
                where ur.RoleId == roleId && ur.UserId == userId
                select ur;
            _db.UsersRoles.DeleteOnSubmit( query.SingleOrDefault() );
            Save();
        }

        internal void AddRole( Guid roleId, Guid userId ) {
            var userRole = new UsersRole();
            userRole.RoleId = roleId;
            userRole.UserId = userId;
            _db.UsersRoles.InsertOnSubmit( userRole );
            Save();
        }

        public void Delete( User user ) {
            _db.Users.DeleteOnSubmit( user );
        }

        #endregion

        #region Persistence
        public void Save() {
            _db.SubmitChanges();
        }
        #endregion

    }
}
