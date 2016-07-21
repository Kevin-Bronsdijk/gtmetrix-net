using System;

namespace GTmetrix.Logic
{
    internal static class Helper
    {
        public static void ThrowIfNullOrEmpty(this string value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
            if (value == string.Empty)
            {
                throw new ArgumentException("Argument must not be the empty string.", name);
            }
        }

        //public static OptimizeSetWaitResults JsonToSet(string json)
        //{
        //    JObject jsono = JObject.Parse(json);

        //    var optimizeSetWaitResults = new OptimizeSetWaitResults();
        //    optimizeSetWaitResults.Success = true;

        //    foreach (var result in jsono.Children().Children().Children())
        //    {
        //        if (result.Path.StartsWith("results."))
        //        {
        //            foreach (var resultsItem in result.Children())
        //            {
        //                var optimizeSetWaitResult = JsonConvert.DeserializeObject<OptimizeSetWaitResult>(resultsItem.ToString());
        //                optimizeSetWaitResult.Name = result.Path.Replace("results.", string.Empty);
        //                optimizeSetWaitResult.Success = true;
        //                optimizeSetWaitResults.Results.Add(optimizeSetWaitResult);
        //            }
        //        }
        //    }

        //    return optimizeSetWaitResults;
        //}
    }
}