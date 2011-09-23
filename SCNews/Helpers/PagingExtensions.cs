using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCNews.Helpers
{
    public static class PagingExtensions
    {
        public static string Paging( this HtmlHelper helper, String controller, Int32 pageIndex, Int32 totalPages, Boolean hasPrevPage, Boolean hasNextPage )
        {
            var endPages = totalPages - pageIndex;
            var prev = pageIndex - 1;
            var next = pageIndex + 1;

            var paging = "<div class='pager fl'>";

            if (hasPrevPage)
                paging += "<a href=\"/" + controller + "/Page/" + prev + "\">" + HtmlSpan( "&lt; Пред.", "page-numbers" ) + "</a>";

            if (totalPages > 2)
            {
                if (totalPages < 11)
                {
                    for (var page = 1; page < totalPages; page++)
                    {
                        if (pageIndex == page)
                            paging += HtmlSpan( page, "page-numbers current" );
                        else
                            paging += "<a href=\"/" + controller + "/Page/" + page + "\">" + HtmlSpan( page, "page-numbers" ) + "</a>";
                    }
                }
                else
                {
                    if (pageIndex > 5 && endPages > 5)
                    {
                        for (var page = 1; page <= 3; page++)
                            paging += "<a href=\"/" + controller + "/Page/" + page + "\">" + HtmlSpan( page, "page-numbers" ) + "</a>";

                        paging += HtmlSpan( "...", "page-dots" );
                        
                        paging += "<a href=\"/" + controller + "/Page/" + prev + "\">" + HtmlSpan( prev, "page-numbers" ) + "</a>";
                        paging += HtmlSpan( pageIndex, "page-numbers current" );
                        paging += "<a href=\"/" + controller + "/Page/" + next + "\">" + HtmlSpan( next, "page-numbers" ) + "</a>";
                        paging += HtmlSpan( "...", "page-dots" );

                        for (var page = totalPages - 3; page < totalPages; page++)
                            paging += "<a href=\"/" + controller + "/Page/" + page + "\">" + HtmlSpan( page, "page-numbers" ) + "</a>";
                    }
                    else if (pageIndex <= 5)
                    {
                        for (var page = 1; page <= 6; page++)
                        {
                            if (pageIndex == page)
                                paging += HtmlSpan( page, "page-numbers current" );
                            else
                                paging += "<a href=\"/" + controller + "/Page/" + page + "\">" + HtmlSpan( page, "page-numbers" ) + "</a>";
                        }

                        paging += HtmlSpan( "...", "page-dots" );

                        for (var page = totalPages - 3; page < totalPages; page++)
                            paging += "<a href=\"/" + controller + "/Page/" + page + "\">" + HtmlSpan( page, "page-numbers" ) + "</a>";
                    }
                    else if (endPages <= 5)
                    {
                        for (var page = 1; page <= 3; page++)
                            paging += "<a href=\"/" + controller + "/Page/" + page + "\">" + HtmlSpan( page, "page-numbers" ) + "</a>";

                        paging += HtmlSpan( "...", "page-dots" );

                        for (var page = totalPages - endPages - 1; page < totalPages; page++)
                        {
                            if (pageIndex == page)
                                paging += HtmlSpan( page, "page-numbers current" );
                            else
                                paging += "<a href=\"/" + controller + "/Page/" + page + "\">" + HtmlSpan( page, "page-numbers" ) + "</a>";
                        }


                    }
                }

            }

            if (hasNextPage)
                paging += "<a href=\"/" + controller + "/Page/" + next + "\">" + HtmlSpan( "След. &gt;", "page-numbers" ) + "</a>";

            paging += "</div>";

            return paging;
        }

        private static String HtmlSpan( String value, String classes )
        {
            return "<span class=\"" + classes + "\">" + value + "</span>";
        }

        private static String HtmlSpan( Int32 value, String classes )
        {
            return HtmlSpan( value.ToString(), classes );
        }

    }
}
