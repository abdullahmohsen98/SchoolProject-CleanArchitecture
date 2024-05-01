using MediatR;

namespace SchoolProject.Core.Features.Students.Commanads.Models
{
    public class AddStudentCommand : IRequest<Bases.Response<string>>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int DepartmentID { get; set; }
    }
}
