using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SCNews.Models
{
    public class MenuRepository
    {
        private readonly MenuDataContext _db = new MenuDataContext( ConfigurationManager.ConnectionStrings["StarcraftMDexpress"].ConnectionString );

        public IQueryable<Menu> GetMenus( Int32 typeId )
        {
            var query = 
                from menus in _db.Menus
                where menus.type_id == typeId
                select menus;
            return query;
        }

        public Menu GetMenu( Int32 typeId, Int32 orderN )
        {
            var query =
                from menus in _db.Menus
                where menus.type_id == typeId 
                    && menus.order_n == orderN
                select menus;
            return query.SingleOrDefault();
        }

        public IQueryable<MenuType> GetTypes()
        {
            return
                from types in _db.MenuTypes
                select types;
        }

        #region Insert/Delete methods

        public void Insert( Menu menu )
        {
            _db.Menus.InsertOnSubmit( menu );
            Save();
        }

        public void Delete( Menu menu )
        {
            _db.Menus.DeleteOnSubmit( menu );
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
