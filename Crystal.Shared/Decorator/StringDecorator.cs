namespace Crystal.Shared
{
    public static class StringDecorator
    {
        public static bool IsNullEmptyWhiteSpace(this string str)
        {
            bool returnValue = false;

            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                returnValue = true;
            }

            return returnValue;
        }
    }
}
