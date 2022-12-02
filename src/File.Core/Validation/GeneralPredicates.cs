namespace File.Core.Validation
{
    internal static class GeneralPredicates
    {
        internal static readonly Predicate<string> isValidString = m => !string.IsNullOrWhiteSpace(m);
        internal static readonly Predicate<string> isValidFileName = m => m.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
    }
}
