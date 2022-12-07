namespace File.Domain.Logging
{
    public class LogEvents
    {
        public static readonly int GeneralError = 1000;

        //Add File
        public static readonly int AddFileGeneralError = 2000;

        public static readonly int AddFileStreamError = 2100;

        public static readonly int AddFileDatabaseError = 2200;
    }
}
