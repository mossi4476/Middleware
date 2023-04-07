using MediatR;
using Middleware.Models;

namespace Middleware.Queries
{
    
        public class GetStudentListQuery : IRequest<List<StudentDetails>>
        {
        }
    
}
