namespace Valis.Core
{
    public static class BuiltinProfiles
    {

        public static readonly VLClientProfile UTESTFree = new VLClientProfile { ProfileId = 1, Name = "UTESTFree", UseCredits = false };
        public static readonly VLClientProfile UTESTPaid = new VLClientProfile { ProfileId = 2, Name = "UTESTPaid", UseCredits = true };

        public static readonly VLClientProfile Basic = new VLClientProfile { ProfileId = 6, Name = "Demo", UseCredits = false };
        public static readonly VLClientProfile Default = new VLClientProfile { ProfileId = 7, Name = "Default", UseCredits = true };
    }
}
