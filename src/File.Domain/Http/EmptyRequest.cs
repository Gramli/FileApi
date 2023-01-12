namespace File.Domain.Http
{
    public sealed class EmptyRequest
    {
        private static EmptyRequest? _instance;
        public static EmptyRequest Instance
        {
            get
            {
                _instance ??= new EmptyRequest();
                return _instance;
            }
        }

        private EmptyRequest()
        {

        }
    }
}
