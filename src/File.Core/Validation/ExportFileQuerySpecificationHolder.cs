using File.Domain.Queries;
using Validot;

namespace File.Core.Validation
{
    internal sealed class ExportFileQuerySpecificationHolder : ISpecificationHolder<ExportFileQuery>
    {
        public Specification<ExportFileQuery> Specification { get; }

        public ExportFileQuerySpecificationHolder()
        {
            Specification<ExportFileQuery> exportFileQuerySpecification = s => s
                .Member(m => m.Id, m => m.Rule(GeneralPredicates.isValidId));

            Specification = exportFileQuerySpecification;
        }
    }
}
