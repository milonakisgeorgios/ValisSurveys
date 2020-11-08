namespace ValisServer.Runtime
{
    public enum PageType : byte
    {
        /// <summary>
        /// Η πρώτη σελίδα του survey.
        /// </summary>
        FirstPage = 0,
        /// <summary>
        /// Ενδιάμεση σελίδα του survey
        /// </summary>
        NormalPage = 1,
        /// <summary>
        /// Τελευταία σελίδα του survey
        /// </summary>
        LastPage = 2,
        Welcome = 3,
        Goodbye = 4,
        Disqualification = 5,
        EndSurvey = 6
    }
}