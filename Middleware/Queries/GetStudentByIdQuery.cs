using MediatR;
using Middleware.Models;

namespace Middleware.Queries
{
    public class GetStudentByIdQuery : IRequest<StudentDetails>
    {
        public int Id { get; set; }
    }
}
