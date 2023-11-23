namespace ProjectSemantic3WebMVC.Helpers
{
    public static class DataValidationHelper
    {
        public static bool IsValid(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            if (s == "N/A") return false;

            return true;
        }
    }
}