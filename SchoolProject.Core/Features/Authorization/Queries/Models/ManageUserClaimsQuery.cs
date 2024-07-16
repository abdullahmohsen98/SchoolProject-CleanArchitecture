using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Data.Results;

namespace SchoolProject.Core.Features.Authorization.Queries.Models
{
    public class ManageUserClaimsQuery : IRequest<Response<ManageUserClaimsResult>>
    {
        public int UserId { get; set; }
    }
}
