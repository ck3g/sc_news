using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SCNews.Models
{
    public class ChatRepository
    {
        private ChatDataContext _db = new ChatDataContext( ConfigurationManager.ConnectionStrings["StarcraftMDexpress"].ConnectionString );

        public IQueryable<ChatMessage> GetLastMessages() 
        {
            var query =
                from m in _db.ChatMessages
                join u in _db.ChatUsers
                    on m.author_id equals u.UserId into userMessages
                orderby m.texted_at descending 
                select m;
            return query.Where( m => m.is_deleted == false ).Take( 20 );
        }

        public void RemoveMessage( Guid message_id )
        {
            var message = _db.ChatMessages.SingleOrDefault( m => m.id == message_id );
            Delete( message );
        }

        #region Insert/Delete methods
        public void Add( ChatMessage chatMessage )
        {
            _db.ChatMessages.InsertOnSubmit( chatMessage );
            Save();
        }

        public void Delete( ChatMessage chatMessage )
        {
            _db.ChatMessages.DeleteOnSubmit( chatMessage );
            Save();
        }

        #endregion


        public Guid GetAuthorId( String authorName )
        {
            return _db.ChatUsers.Single( u => u.LoweredUserName == authorName.ToLower() ).UserId;
        }

        #region Persistence
        public void Save()
        {
            _db.SubmitChanges();
        }
        #endregion
    }
}
