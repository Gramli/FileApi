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
    internal class JsonToXmlFileConverter : IFileConverter
    {
        public Task<Result<IFile>> Convert(byte[] sourceFileData,, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
