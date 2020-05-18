using System;
using System.Collections.Generic;

namespace Crystal.Core.Shared.Extension
{
    public static class BaseLogger
    {
        public static void LogException(this object classInstance, string methodName, Exception ex, string msg = "")
        {
            var properties = new Dictionary<string, string> {
                { "Class", classInstance.GetType().Name },
                { "Method", methodName },
                { "Message", msg }
              };
            //Crashes.TrackError(ex, properties);
            Console.WriteLine(ex);
        }
    }
}
