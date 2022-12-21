namespace File.Infrastructure.Abstractions
{
    internal interface IFileConverterFactory
    {
        IFileConverter Create(string sourceFormat, string destinationExtension);
    }
}
