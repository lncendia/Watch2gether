using Watch2gether.Domain.Abstractions.Interfaces;
using Watch2gether.Domain.Comments;
using Watch2gether.Domain.Comments.Ordering.Visitor;
using Watch2gether.Domain.Comments.Specifications.Visitor;

namespace Watch2gether.Domain.Abstractions.Repositories;

public interface ICommentRepository : IRepository<Comment,Guid,ICommentSpecificationVisitor, ICommentSortingVisitor>
{
}