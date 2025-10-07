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
        public static string SessionToken = "JWTToken";

        public enum ProjectStatus
        {
            Approved,
            Rejected,
            corrected,
            waiting_to_check
        }

        public enum Role
        {
            admin,
            engineer,
            custom
        }

    }
}