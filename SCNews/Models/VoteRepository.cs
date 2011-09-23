using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SCNews.Models;

namespace SCNews.Models
{
    public class VoteRepository
    {
        private VoteDataContext _db = new VoteDataContext( ConfigurationManager.ConnectionStrings["StarcraftMDexpress"].ConnectionString );

        public IQueryable<Vote> FindAllVotes()
        {
            return
                from v in _db.Votes
                select v;
        }

        public Int32 GetCount()
        {
            return
                (from v in _db.Votes
                select v.id).Count();
        }

        public IQueryable<Vote> FindVotesOnPage(Int32 page, Int32 pageSize)
        {
            return
                (from v in _db.Votes
                select v).OrderByDescending(v => v.created_at).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public Vote GetVote( Int64 id )
        {
            return FindAllVotes().SingleOrDefault( v => v.id == id );
        }

        public IQueryable<VoteAnswer> FindVoteAnswers( Int64 voteId )
        {
            var query = 
                from va in _db.VoteAnswers
                where va.vote_id == voteId
                select va;
            return query.OrderBy( v => v.order_n );
        }

        public Vote FindCurrent()
        {
            var query =
                from v in _db.Votes
                where v.is_pinned == true
                select v;
            return query.SingleOrDefault();
        }

        public IQueryable<VoteByUser> FindVotesByUser( Int64 voteId )
        {
            var query =
                from v in _db.VoteByUsers
                where v.vote_id == voteId
                select v;
            return query;
        }

        public Int64 GetLastVoteId()
        {
            return _db.Votes.OrderByDescending( v => v.id ).FirstOrDefault().id;
        }

        public Guid GetUserId( String userName )
        {
            return _db.VoteUsers.SingleOrDefault( u => u.UserName == userName ).UserId;
        }

        public void ResetPin()
        {
            var votes = from v in _db.Votes select v;
            _db.ExecuteCommand( "UPDATE [Votes] SET [is_pinned] = 'false'" );
            Save();
        }

        public void Vote( VoteByUser voteByUser )
        {
            _db.VoteByUsers.InsertOnSubmit( voteByUser );
            Save();
        }

        #region Insert/Delete methods

        public Int64 AddVote( Vote vote )
        {
            _db.Votes.InsertOnSubmit( vote );
            Save();
            return GetLastVoteId();
        }

        public void AddVoteAnswer( VoteAnswer voteAnswer )
        {
            _db.VoteAnswers.InsertOnSubmit( voteAnswer );
            Save();
        }

        public void RemoveAnswers( Int64 voteId )
        {
            _db.VoteAnswers.DeleteAllOnSubmit( FindVoteAnswers( voteId ) );
            Save();
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