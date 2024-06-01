namespace SchoolProject.Core.Features.ApplicationUser.Queries.Results
{
    public class GetUserPaginationResponse
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
    }
}
