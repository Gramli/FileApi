using File.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File.Infrastructure.FileConversions
{
    internal class FileConverterFactory : IFileConverterFactory
    {
        public IFileConverter Create(string contentType)
        {
            throw new NotImplementedException();
        }
    }
}
