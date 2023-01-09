using File.Core.Queries;
using File.Domain.Commands;
using Validot;

namespace File.Core.Validation
{
    internal class ConvertToQuerySpecificationHolder : ISpecificationHolder<ConvertToQuery>
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
