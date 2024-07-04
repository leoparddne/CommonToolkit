namespace Common.Toolkit.Helper
{
    public static class GUIDHelper
    {
        public static string NewGuid => Guid.NewGuid().ToString("N");
    }
}
