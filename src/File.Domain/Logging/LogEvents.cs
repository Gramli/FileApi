namespace File.Domain.Logging
{
    public class LogEvents
    {
        public static readonly int GeneralError = 1000;

        //Add File
        public static readonly int AddFileGeneralError = 2000;

        public static readonly int AddFileStreamError = 2100;

        public static readonly int AddFileDatabaseError = 2200;

        //Get File
        public static readonly int GetFileGeneralError = 3000;

        public static readonly int GetFileStreamError = 3100;

        public static readonly int GetFileValidationError = 3200;
    }
}
