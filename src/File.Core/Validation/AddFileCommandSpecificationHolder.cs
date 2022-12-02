using File.Domain.Abstractions;
using File.Domain.Commands;
using Validot;

namespace File.Core.Validation
{
    internal class AddFileCommandSpecificationHolder : ISpecificationHolder<AddFilesCommand>
    {
        public Specification<AddFilesCommand> Specification { get; }

        public AddFileCommandSpecificationHolder()
        {
            Specification<IFile> fileSpecification = f => f
            .Member(m => m.Name, m => m.Rule(GeneralPredicates.isValidString))
            .Member(m => m.FileName, m => m
                    .Rule(GeneralPredicates.isValidString)
                    .Rule(GeneralPredicates.isValidFileName));
            //.Member(m=>m.ContentType,)
            //.Member(m=>m.Length, ;

            //Specification<AddFilesCommand> addFileCommandSpecification = s => s
            //    .Member(m => m.Files, );

            //Specification = addFileCommandSpecification;
            //TODO VALIDATION WITH OPTIONS?
        }
    }
}
