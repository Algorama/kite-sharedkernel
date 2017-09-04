using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http.OData.Query;

namespace SharedKernel.Api.Helpers
{
    public static class ODataParse
    {
        public static List<KeyValuePair<string, string>> RetonaQueryStringParts<T>(ODataQueryOptions<T> queryOptions)
        {
            var queryStringParts = new List<KeyValuePair<string, string>>();

            if (queryOptions.Top != null)
                queryStringParts.Add(new KeyValuePair<string, string>("$top", queryOptions.Top.RawValue));

            if (queryOptions.Skip != null)
                queryStringParts.Add(new KeyValuePair<string, string>("$skip", queryOptions.Skip.RawValue));

            queryStringParts.Add(queryOptions.OrderBy != null
                ? new KeyValuePair<string, string>("$orderby", FromCamelCaseToPascal<T>(queryOptions.OrderBy.RawValue))
                : new KeyValuePair<string, string>("$orderby", "Id desc"));

            if (queryOptions.Filter != null)
            {
                var filter = FromCamelCaseToPascal<T>(queryOptions.Filter.RawValue);

                //Separar todos os filtros entre parenteses para funcionar o NHibernateOData
                filter = "(" + filter.Replace(" and ", ") and (") + ")";

                queryStringParts.Add(new KeyValuePair<string, string>("$filter", filter));
            }

            if (queryOptions.InlineCount != null)
                queryStringParts.Add(new KeyValuePair<string, string>("$inlinecount", queryOptions.InlineCount.RawValue));

            return queryStringParts;
        }

        private static string FromCamelCaseToPascal<T>(string query)
        {
            var campos = typeof(T).GetProperties().Select(propriedade => propriedade.Name).ToList();
            foreach (var campo in campos)
                query = Regex.Replace(query, @"\b" + campo + @"\b", campo, RegexOptions.IgnoreCase | RegexOptions.ECMAScript);

            query = CapitalizeAfter(query, new[] { '/' });
            return query;

        }

        private static string CapitalizeAfter(string source, IEnumerable<char> chars)
        {
            var charsHash = new HashSet<char>(chars);
            var sb = new StringBuilder(source);
            for (var i = 0; i < sb.Length - 1; i++)
            {
                if (charsHash.Contains(sb[i]))
                    sb[i + 1] = char.ToUpper(sb[i + 1]);
            }
            return sb.ToString();
        }
    }
}