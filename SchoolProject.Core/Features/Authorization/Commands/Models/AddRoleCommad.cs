
using MediatR;
using SchoolProject.Core.Bases;

namespace SchoolProject.Core.Features.Authorization.Commands.Models
{
    public class AddRoleCommad : IRequest<Response<string>>
    {
        public string RoleName { get; set; }
    }
}
