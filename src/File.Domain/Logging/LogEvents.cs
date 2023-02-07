namespace File.Domain.Logging
{
    public static class LogEvents
    {
        public static readonly int GeneralError = 1000;

        //Add File
        public static readonly int AddFileGeneralError = 2000;

        public static readonly int AddFileStreamError = 2100;

        public static readonly int AddFileDatabaseError = 2200;

        //Get File
        public static readonly int GetFileGeneralError = 3000;

        public static readonly int GetFileStreamError = 3100;

        public static readonly int GetFileDatabaseError = 3200;

        public static readonly int GetFileValidationError = 3300;

        //Export File
        public static readonly int ExportFileGeneralError = 4000;

        public static readonly int ExportFileValidationError = 4100;

        //Convert File
        public static readonly int ConvertFileGeneralError = 4000;
    }
}
