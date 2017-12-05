using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BookHeaven.Web.Infrastructure.Extensions
{
    using static WebConstants;

    public static class TempDataExtensions
    {
        public static void AddErrorMessage(this ITempDataDictionary tempData,string message)
        {
            tempData[ErrorMessageKey] = message;
        }

        public static void AddSuccessMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[SuccessMessageKey] = message;
        }
    }
}
