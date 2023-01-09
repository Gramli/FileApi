using File.Domain.Queries;
using Validot;

namespace File.Core.Validation
{
    internal class DownloadFileQuerySpecificationHolder : ISpecificationHolder<DownloadFileQuery>
    {
        public Specification<DownloadFileQuery> Specification { get; }

        public DownloadFileQuerySpecificationHolder()
        {
            Specification<DownloadFileQuery> downloadFileQuerySpecification = s => s
                .Member(m => m.Id, m => m.Rule(GeneralPredicates.isValidId));

            Specification = downloadFileQuerySpecification;
        }
    }
}
