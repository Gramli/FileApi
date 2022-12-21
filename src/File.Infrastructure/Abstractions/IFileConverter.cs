namespace File.Infrastructure.Abstractions
{
    internal interface IFileConverter
    {
        Task<string> Convert(string fileContent, CancellationToken cancellationToken); 
    }
}
