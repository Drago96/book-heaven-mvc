using BookHeaven.Web.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BookHeaven.Web.Infrastructure.Extensions
{
    public static class ViewDataExtensions
    {
        public static void SetReturnUrl(this ViewDataDictionary viewData, string returnUrl)
        {
            viewData[DictionaryKeys.ReturnUrl] = returnUrl;
        }
    }
}