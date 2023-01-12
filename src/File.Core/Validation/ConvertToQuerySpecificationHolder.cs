using File.Core.Queries;
using Validot;

namespace File.Core.Validation
{
    internal sealed class ConvertToQuerySpecificationHolder : ISpecificationHolder<ConvertToQuery>
    {
        public Specification<ConvertToQuery> Specification { get; }

        public ConvertToQuerySpecificationHolder()
        {
            Specification<ConvertToQuery> convertToQuerySpecification = s => s
                .Member(m => m.File, GeneralPredicates.fileSpecification);

            Specification = convertToQuerySpecification;
        }
    }
}
