using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Queries.Results;

namespace SchoolProject.Core.Features.ApplicationUser.Queries.Modles
{
    public class GetUserByIdQuery : IRequest<Response<GetSingleUserResponse>>
    {
        public int Id { get; set; }
        public GetUserByIdQuery(int id)
        {
            Id = id;
        }
    }
}
