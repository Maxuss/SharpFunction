namespace SharpFunction.Universal
{
    /// <summary>
    ///     Helper for null checks
    /// </summary>
    public static class NullChecker
    {
        public static bool IsNull(object v)
        {
            return v == null;
        }

        public static bool IsEmpty(string v)
        {
            return v == string.Empty || v == "";
        }
    }
}