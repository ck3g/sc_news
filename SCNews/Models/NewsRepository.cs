using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SCNews.Models;

namespace SCNews.Models
{
    public class NewsRepository
    {
        private NewsDataContext db = new NewsDataContext( ConfigurationManager.ConnectionStrings["StarcraftMDexpress"].ConnectionString );


        #region News Methods
        public IQueryable<News> FindAllNews()
        {
            var query =
                from n in db.News
                join u in db.Users
                on n.author_id equals u.UserId
                where n.type != 2
                select n;
            return query;
        }

        public IQueryable<News> FindNewsOnPage(int page, int pageSize)
        {
            // source.Skip( (PageIndex - 1) * PageSize ).Take( PageSize )
            var query =
                (from n in db.News
                join u in db.Users
                on n.author_id equals u.UserId
                where n.type != 2
                select n).OrderByDescending(n => n.created_at).Skip((page - 1) * pageSize).Take(pageSize);
            return query;
        }

        public Int32 GetNewsCount()
        {
            var query =
                from n in db.News
                where n.type != 2
                select n.id;
            return query.Count();
        }

        public IQueryable<News> FindNewsByTag( String tagName, int page, int pageSize ) 
        {
            
            var query =
                (from n in db.News
                join u in db.Users on n.author_id equals u.UserId
                join nt in db.NewsInTags on n.id equals nt.news_id
                join t in db.Tags on nt.tag_id equals t.id
                where t.name == tagName && n.type != 2
                select n).OrderByDescending(n => n.created_at).Skip((page - 1) * pageSize).Take(pageSize);
            return query;
        }

        public int GetNewsCountByTag(String tagName) 
        {
            var query =
                (from n in db.News
                join u in db.Users on n.author_id equals u.UserId
                join nt in db.NewsInTags on n.id equals nt.news_id
                join t in db.Tags on nt.tag_id equals t.id
                where t.name == tagName && n.type != 2
                select n.id).Count();
            return query;
        }

        public IQueryable<News> FindAllPinnedNews()
        {
            
            var query =
                from n in db.News
                join u in db.Users
                on n.author_id equals u.UserId
                where n.type == 2
                select n;
            return query;
        }

        public Dictionary<Int64, String> GetTags( IQueryable<News> newsList )
        {
            var tags = new Dictionary<Int64, String>();
            foreach (var news in newsList)
            {
                var newsCopy = news;
                var query = from nt in db.NewsInTags
                            join t in db.Tags
                                on nt.tag_id equals t.id
                                where nt.news_id == newsCopy.id
                            select t;
                var tagList = new List<String>();
                foreach (var s in query.Select( tag => tag.name ).ToArray())
                {
                    tagList.Add( "<a href='/Tags/" + s + "'>" + s + "</a>" );
                }
                tags.Add( newsCopy.id, String.Join( ", ", tagList.ToArray() ) );
            }
            return tags;
        }

        public String GetTags( Int64 newsId )
        {
            var query = from nt in db.NewsInTags
                        join t in db.Tags
                            on nt.tag_id equals t.id
                            where nt.news_id == newsId
                        select t;
            var tagList = new List<String>();
            foreach (var s in query.Select( tag => tag.name ).ToArray())
            {
                tagList.Add( "<a href='/Tags/" + s + "'>" + s + "</a>" );
            }
            return String.Join( ", ", tagList.ToArray() );
        }

        public News GetNews( long id ) 
        {
            return db.News.SingleOrDefault( n => n.id == id );
        }

        public News GetLastNewsId()
        {
            return db.News.OrderByDescending( n => n.id ).FirstOrDefault();
        }

        public void IncreaseViewsCount( long id )
        {
            db.News.SingleOrDefault( n => n.id == id ).views++;
            Save();
        }

        public void IncreaseHitsCount( long id )
        {
            db.News.SingleOrDefault( n => n.id == id ).hits++;
            Save();
        }

        public int? VoteForNews( long id, String userName )
        {
            var userId = GetAuthorId( userName );
            var query =
                from v in db.NewsVotes
                where v.news_id == id && v.user_id == userId
                select v;
            if (query.Count() == 0)
            {
                var vote = new NewsVote() { news_id = id, user_id = userId};
                db.NewsVotes.InsertOnSubmit( vote );
                var voteCount = ++db.News.SingleOrDefault( n => n.id == id ).voted_for;
                Save();
                return voteCount;
            }
            return db.News.SingleOrDefault( n => n.id == id).voted_for;
        }

        #endregion

        #region Users Methods
        public Guid GetAuthorId( String authorName )
        {
            return db.Users.Single( u => u.LoweredUserName == authorName.ToLower() ).UserId;
        }
        #endregion

        #region Comments methods
        public IQueryable<Comment> FindAllComments( Int64 newsId )
        {
            var query =
                from c in db.Comments
                join u in db.Users
                on c.author_id equals u.UserId
                where c.parent_id == newsId
                select c;
            return query;
        }

        public int GetCommentsCount( long id ) {
            var query =
                from c in db.Comments
                where c.parent_id == id
                select c;
            return query.Count();
        }

        public bool CommentRemove( long id ) {
            DeleteComment( db.Comments.Single( c => c.id == id ) );
            Save();
            return true;
        }

        public IQueryable<Comment> GetLatestComments( int commentCount )
        {
            var query = from c in db.Comments
                        select c;
            return query.OrderByDescending( c => c.created_at ).Take( commentCount );
        }

        public Tag GetTagIdByName( String tagName )
        {
            var query = from t in db.Tags
                        where t.name == tagName
                        select t;
            return query.SingleOrDefault();
        }

        public Dictionary<String, Int32> GetTagsWithCount()
        {
            var allTags = from t in db.Tags
                          select new { t.name, NewsCount = t.NewsInTags.Count };
            return allTags.ToDictionary( tag => tag.name, tag => tag.NewsCount );
        }

        public Dictionary<String, Int32> GetTopTags( Int32 count )
        {
            var allTags = from t in db.Tags
                          select new { t.name, NewsCount = t.NewsInTags.Count };
            return allTags.OrderByDescending( tag => tag.NewsCount ).Take( count ).ToDictionary( tag => tag.name, tag => tag.NewsCount );
        }

        public String GetAllTags()
        {
            var allTags = from t in db.Tags
                          select t;
            return "['" + String.Join( "', '", allTags.Select( tag => tag.name ).ToArray() ) + "']";
        }

        public IQueryable<Tag> GetAllTags( Boolean raw )
        {
            return from t in db.Tags
                          select t; 
        }

        public String GetNewsTags( Int64 newsId )
        {
            var newsTags  = from nt in db.NewsInTags
                        join t in db.Tags
                            on nt.tag_id equals t.id
                        where nt.news_id == newsId
                        select t;

            var tagNames = newsTags.Select( newsTag => newsTag.name ).ToArray();
            return String.Join( ", ", tagNames );

        }

        public void RenameTag( Int32 id, String name )
        {
            var tag = db.Tags.Where( t => t.id == id ).First();
            tag.name = name;
            Save();
        }

        #endregion


        #region Insert/Delete methods
        public void Add( News news ) 
        {
            db.News.InsertOnSubmit( news );
        }

        public void Delete(News news) 
        {
            db.News.DeleteOnSubmit( news );
        }

        public void AddComment( Comment comment )
        {
            db.Comments.InsertOnSubmit( comment );
        }

        public void DeleteComment( Comment comment ) {
            db.Comments.DeleteOnSubmit( comment );
        }

        public Tag AddTag( String tagName )
        {
            var tag = GetTagIdByName( tagName );
            if (tag != null)
                return tag;

            var newTag = new Tag {name = tagName};
            db.Tags.InsertOnSubmit( newTag );
            db.SubmitChanges();
            return newTag;
        }

        public void BindTagToNews( Int32 tagId, Int64 newsId )
        {
            var query = from nt in db.NewsInTags
                        where nt.tag_id == tagId && nt.news_id == newsId
                        select nt;

            if (query.Count() != 0) return;
            db.NewsInTags.InsertOnSubmit( new NewsInTags {tag_id = tagId, news_id = newsId} );
            db.SubmitChanges();
        }

        public void ClearTags( Int64 newsId )
        {
            var tags = from nt in db.NewsInTags
                        where nt.news_id == newsId
                        select nt;

            foreach (var tag in tags)
                db.NewsInTags.DeleteOnSubmit( tag );
            
            db.SubmitChanges();
        }

        public void DeleteTag( Int32 id )
        {
            var tag = from t in db.Tags
                      where t.id == id
                      select t;
            db.Tags.DeleteOnSubmit( tag.SingleOrDefault() );
            Save();
        }

        #endregion

        #region Persistence
        public void Save() 
        {
            db.SubmitChanges();
        }
        #endregion

        public void TagsJob( String tagsAsString, Int64 newsId )
        {
            ClearTags( newsId );
            if (!String.IsNullOrEmpty( tagsAsString ))
            {
                var tags = tagsAsString.Split( ',' );
                foreach (var tagName in tags)
                {
                    var clearTagName = tagName.Trim();
                    if (String.IsNullOrEmpty( clearTagName )) continue;
                    var tag = AddTag( tagName.Trim() );
                    BindTagToNews( tag.id, newsId );
                }
            }

        }
    }
}