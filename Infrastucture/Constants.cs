public static class CookieNames
{
    public static string Prefix => "e2open.LeadManagement";

    /// <summary>
    /// Gets the session cookie name
    /// </summary>
    public static string Auth => Prefix + ".auth";
    public static string Antiforgery => Prefix + ".Antiforgery";
}

public static class JwtClaimIdentifiers
{
    public const string Rol = "rol",
                        Id = "id",
                       
                        Username = "username",
                        Firstname = "firstname",
                        Lastname = "lastname",
                        Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",

                        TimeOut = "timeout",
                        Name = "name",
                        GivenName = "given_name",
                        UserPrincipleName = "user_principle_name",
                        DistingushedName = "distingushed_name";
}