using Films.Domain.Abstractions.Interfaces;
using Films.Domain.Comments.Entities;
using Films.Domain.Comments.Ordering.Visitor;
using Films.Domain.Comments.Specifications.Visitor;

namespace Films.Domain.Abstractions.Repositories;

public interface ICommentRepository : IRepository<Comment,Guid,ICommentSpecificationVisitor, ICommentSortingVisitor>;