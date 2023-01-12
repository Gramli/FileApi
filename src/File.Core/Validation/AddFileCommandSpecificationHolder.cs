using File.Domain.Commands;
using Validot;

namespace File.Core.Validation
{
    internal sealed class AddFileCommandSpecificationHolder : ISpecificationHolder<AddFilesCommand>
    {
        public Specification<AddFilesCommand> Specification { get; }

        public AddFileCommandSpecificationHolder()
        {
            Specification<AddFilesCommand> addFileCommandSpecification = s => s
                .Member(m => m.Files, m => m.AsCollection(GeneralPredicates.fileSpecification));

            Specification = addFileCommandSpecification;
        }
    }
}
