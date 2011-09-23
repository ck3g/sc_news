using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SCNews.Models {

    public class GalleryRepository {
        private GalleryDataContext db = new GalleryDataContext( ConfigurationManager.ConnectionStrings["StarcraftMDexpress"].ConnectionString );

        public Int32 GetPicturesCount() 
        {
            return
                (from g in db.Galleries
                join u in db.Users on g.upload_by equals u.UserId
                select g.id).Count();
        }

        public IQueryable<Gallery> FindAllPicturesOnPage(int page, int pageSize)
        {
            var query =
                from g in db.Galleries
                join u in db.Users on g.upload_by equals u.UserId
                select g;
            return query.OrderByDescending(g => g.upload_date).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void DeletePicture( int id )
        {
            Delete( db.Galleries.SingleOrDefault( g => g.id == id ) );
            Save();
        }

        #region Insert/Delete methods
        public void Add( Gallery picture )
        {
            db.Galleries.InsertOnSubmit( picture );
        }

        public void Delete( Gallery picture )
        {
            db.Galleries.DeleteOnSubmit( picture );
        }

        #endregion


        public Guid GetAuthorId( String authorName )
        {
            return db.Users.Single( u => u.LoweredUserName == authorName.ToLower() ).UserId;
        }

        #region Persistence
        public void Save()
        {
            db.SubmitChanges();
        }
        #endregion
    }
}
