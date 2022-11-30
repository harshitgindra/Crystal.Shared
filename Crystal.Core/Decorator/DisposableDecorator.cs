using System;

namespace Crystal.Shared
{
    /// <summary>
    /// Disposable decorator
    /// </summary>
    public static class DisposableDecorator
    {
        /// <summary>
        /// This method will try to dispose the object if it implements IDisposable
        /// </summary>
        /// <param name="obj"></param>
        public static void TryDispose(this object obj)
        {
            try
            {
                var instance = (IDisposable)obj;
                instance?.Dispose();
            }
            catch
            {
                //***
                //*** ignored intentionally
                //***
            }
        }
    }
}
