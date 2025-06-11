using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublisherData
{
    public static class SqlFunctions
    {
        [DbFunction("StandardizeUrl", "dbo")]
        public static string StandardizeUrl( string url)
        {
            url = url.ToLower();

            if (!url.StartsWith("http://"))
            {
                url = string.Concat("http://", url);
            }
            return url;
        }
    }
}
