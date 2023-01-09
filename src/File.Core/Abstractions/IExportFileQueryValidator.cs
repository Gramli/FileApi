using File.Core.Queries;
using File.Domain.Queries;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File.Core.Abstractions
{
    public interface IExportFileQueryValidator
    {
        Result<bool> Validate(ExportFileQuery exportFileQuery);
    }
}
