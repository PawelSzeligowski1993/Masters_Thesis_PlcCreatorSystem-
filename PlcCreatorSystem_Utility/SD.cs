namespace PlcCreatorSystem_Utility
{
    public static class SD
    {
        public enum ApiType
        {
            GET,
            POST, 
            PUT, 
            DELETE
        }

        public enum ProjectStatus
        {
            Approved,
            Rejected,
            corrected,
            waiting_to_check
        }

    }
}