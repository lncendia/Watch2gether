using Overoom.Domain.Abstractions.Interfaces;
using Overoom.Domain.Comment.Ordering.Visitor;
using Overoom.Domain.Comment.Specifications.Visitor;

namespace Overoom.Domain.Abstractions.Repositories;

public interface ICommentRepository : IRepository<Comment.Entities.Comment,Guid,ICommentSpecificationVisitor, ICommentSortingVisitor>
{
}