using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SCNews.Models
{
    public class ReplaysRepository
    {
        private readonly ReplaysDataContext _db = new ReplaysDataContext( ConfigurationManager.ConnectionStrings["StarcraftMDexpress"].ConnectionString );

        public IQueryable<Replay> GetAll()
        {
            var query =
                from replays in _db.Replays
                join users in _db.ReplayUsers on replays.uploaded_by equals users.UserId
                select replays;
            return query.OrderByDescending( r => r.uploaded_at );
        }

        public Replay GetById( Int32 id )
        {
            var query = 
                from replays in _db.Replays
                where replays.id == id
                select replays;
            return query.SingleOrDefault();
        }

        public Guid GetAuthorId( String authorName )
        {
            return _db.ReplayUsers.Single( u => u.LoweredUserName == authorName.ToLower() ).UserId;
        }


        #region Insert/Delete methods

        public void Insert( Replay replay )
        {
            _db.Replays.InsertOnSubmit( replay );
        }

        public void Delete( Replay replay )
        {
            _db.Replays.DeleteOnSubmit( replay );
        }

        #endregion

        #region Persistence
        public void Save()
        {
            _db.SubmitChanges();
        }
        #endregion
    }
}
