using System;
using System. Collections. Generic;
using System.Configuration;
using System. Linq;
using System. Web;

namespace SCNews. Models
{
    public class ContentRepository
    {
        private ContentDataContext db = new ContentDataContext( ConfigurationManager. ConnectionStrings["StarcraftMDexpress"]. ConnectionString );

        public IQueryable<Content> FindAll()
        {
            return
                from c in db.Contents
                select c;
        }

        public Content Find( Int32 id )
        {
            var query =
                from c in db.Contents
                where c.type_id == id
                select c;
            return query.SingleOrDefault();
        }

        public void Save()
        {
            db. SubmitChanges();
        }

    }
}