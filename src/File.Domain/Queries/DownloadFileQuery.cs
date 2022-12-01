namespace File.Domain.Queries
{
    public class DownloadFileQuery
    {
        public int Id {  get; init; }

        public DownloadFileQuery(int id)
        {
            Id = id;
        }
    }
}
