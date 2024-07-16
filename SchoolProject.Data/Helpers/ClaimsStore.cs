using System.Security.Claims;

namespace SchoolProject.Data.Helpers
{
    public static class ClaimsStore
    {
        public static List<Claim> claims = new()
        {
            new Claim("Create student","false"),
            new Claim("Edit student","false"),
            new Claim("Delete student","false")
        };
    }
}
