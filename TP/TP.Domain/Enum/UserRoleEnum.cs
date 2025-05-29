namespace TP.Domain.Enum
{
    public enum UserRoleEnum
    {
        USER = 1,
        ADMINISTRATOR = 2
    }

    public static class  EnumExtensions
    {
        public static string ToFriendlyString(this UserRoleEnum userRoleEnum)
        {
            switch (userRoleEnum)
            {
                case UserRoleEnum.USER:
                    return "User";
                case UserRoleEnum.ADMINISTRATOR:
                    return "Administrator";
                default:
                    return "Unknown Role";
            }
        }
    }
}
