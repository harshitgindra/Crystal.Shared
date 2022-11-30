namespace Crystal.Shared
{
    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringDecorator
    {
        /// <summary>
        /// Returns true if string is null, empty or empty space
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullEmptyWhiteSpace(this string str)
        {
            //***
            //*** Check if string is null, empty or white space
            //***
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }
    }
}
