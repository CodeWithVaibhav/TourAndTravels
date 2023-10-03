using System.DirectoryServices;

namespace TourAndTravels.Infrastucture.Extensions
{
    public static class SearchResultExtensions
    {
        public static string GetPropertyValue(this SearchResult sr, string propertyName)
        {
            string ret = string.Empty;

            if (sr.Properties[propertyName].Count > 0)
                ret = sr.Properties[propertyName][0].ToString();

            return ret;
        }
    }
}
