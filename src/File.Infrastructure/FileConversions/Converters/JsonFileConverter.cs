using File.Domain.Abstractions;
using File.Infrastructure.Abstractions;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class JsonFileConverter : IFileConverter
    {
        public Task<Result<IFile>> Convert(IFile sourceFile)
        {
            throw new NotImplementedException();
        }
    }
}
