using System;
using System.Diagnostics;

namespace Crystal.Shared.Decorator
{
    public static class LogDecorator
    {
        public static void LogException(this object classInstance, string methodName, Exception ex, string msg = "")
        {
            // var properties = new Dictionary<string, string> {
            //     { "Class", classInstance.GetType().Name },
            //     { "Method", methodName },
            //     { "Message", msg }
            // };
            Console.WriteLine(ex);
            Debug.WriteLine(ex);
        }
    }
}