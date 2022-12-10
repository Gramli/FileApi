using File.Domain.Abstractions;
using File.Domain.Commands;
using File.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validot;

namespace File.Core.Validation
{
    internal class DownloadFileQuerySpecificationHolder : ISpecificationHolder<DownloadFileQuery>
    {
        public Specification<DownloadFileQuery> Specification { get; }

        public DownloadFileQuerySpecificationHolder()
        {
            Specification<DownloadFileQuery> addFileCommandSpecification = s => s
                .Member(m => m.Id, m => m.Rule(v=>v > 0));

            Specification = addFileCommandSpecification;
        }
    }
}
