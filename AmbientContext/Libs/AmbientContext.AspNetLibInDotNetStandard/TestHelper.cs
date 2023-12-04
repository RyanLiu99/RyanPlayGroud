using System;
using System.Linq;
using AmbientContext.Shared.DotNetStandardLib.Models;
using Microsoft.AspNetCore.Http;

namespace AmbientContext.AspNetCoreLibInDotNetStandard
{
    public class TestHelper
    {
        public static RequestData GetDataFromQueryString(HttpRequest request)
        {
            var userNameQuery = request.Query["userName"];

            if (!userNameQuery.Any() || string.IsNullOrEmpty(userNameQuery[0]))
                throw new Exception("Please supply userName in query string");

            var studyIdQuery = request.Query["studyId"];

            if (!studyIdQuery.Any() || !long.TryParse(studyIdQuery[0], out var studyId))
            {
                throw new Exception("Please supply valid studyId in query string");
            }

            return new RequestData(
                userNameQuery[0],
                studyId
            );
        }
    }
}
