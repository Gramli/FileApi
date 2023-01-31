using File.Domain.Abstractions;
using Validot;

namespace File.Core.Validation
{
    internal static class GeneralPredicates
    {
        internal static readonly Predicate<int> isValidId = m => m > 0 && m < int.MaxValue;
        internal static readonly Predicate<string> isValidFileName = m => m.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        internal static readonly Specification<IFile> fileSpecification = f => 
            f.Member(m => m.FileName, m => m
                .NotEmpty()
                .And()
                .NotWhiteSpace()
                .And()
                .Rule(isValidFileName));
    }
}
