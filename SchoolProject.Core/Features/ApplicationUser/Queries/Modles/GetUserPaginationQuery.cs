using MediatR;
using SchoolProject.Core.Features.ApplicationUser.Queries.Results;
using SchoolProject.Core.Wrappers;

namespace SchoolProject.Core.Features.ApplicationUser.Queries.Modles
{
    public class GetUserPaginationQuery : IRequest<PaginatedResult<GetUserPaginationResponse>>
    {
        public int PageNubmer { get; set; }
        public int PageSize { get; set; }
    }
}
