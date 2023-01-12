namespace File.Domain.Extensions
{
    public static class FileNameExtensions
    {
        public static string GetFileExtension(this string fileName)
        {
            return Path.GetExtension(fileName).Substring(1);
        }
    }
}
