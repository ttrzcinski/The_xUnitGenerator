using System;

namespace The_xUnitGenerator.backend
{
    public static class FastValidators
    {
        /// <summary>
        /// Checks, if it is a valid URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool CheckURLValid(this string url) =>
            string.IsNullOrWhiteSpace(url)
                ? false
                : Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeHttps;
    }
}
