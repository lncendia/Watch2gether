using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Comments;
using Overoom.Domain.Comments.Ordering.Visitor;
using Overoom.Domain.Comments.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface ICommentRepository : IRepository<Comment,Guid,ICommentSpecificationVisitor, ICommentSortingVisitor>
{
}