using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File.Infrastructure.Abstractions
{
    internal interface IFileConverterFactory
    {
        IFileConverter Create(string contentType);
    }
}
