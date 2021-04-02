namespace MyHR_Web.Exts
{
    /// <summary>
    /// int 擴充方法
    /// </summary>
    public static class ExtInteger
    {
        public static int? TryToInt(this string input)
        {
            int result = 0;
            if (int.TryParse(input, out result))
            {
                return result;
            }
            return null;
        }

    }
}
