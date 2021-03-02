using System;

namespace Crystal.Shared
{
    public static class DisposableDecorator
    {
        public static void TryDispose(this object obj)
        {
            try
            {
                var instance = (IDisposable)obj;
                instance?.Dispose();
            }
            catch
            {

            }
        }
    }
}
