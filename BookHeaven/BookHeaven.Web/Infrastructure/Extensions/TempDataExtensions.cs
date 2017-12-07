using BookHeaven.Web.Infrastructure.Constants;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BookHeaven.Web.Infrastructure.Extensions
{
    public static class TempDataExtensions
    {
        public static void AddErrorMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[DataKeyConstants.ErrorMessage] = message;
        }

        public static void AddSuccessMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[DataKeyConstants.SuccessMessage] = message;
        }
    }
}