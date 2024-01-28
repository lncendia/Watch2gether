using Microsoft.EntityFrameworkCore;
using Films.Domain.Abstractions.Repositories;
using Films.Domain.Comments.Entities;
using Films.Domain.Comments.Ordering.Visitor;
using Films.Domain.Comments.Specifications.Visitor;
using Films.Domain.Ordering.Abstractions;
using Films.Domain.Specifications.Abstractions;
using Films.Infrastructure.Storage.Context;
using Films.Infrastructure.Storage.Mappers.Abstractions;
using Films.Infrastructure.Storage.Models.Comment;
using Films.Infrastructure.Storage.Visitors.Sorting;
using Films.Infrastructure.Storage.Visitors.Specifications;

namespace Films.Infrastructure.Storage.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IAggregateMapperUnit<Comment, CommentModel> _aggregateMapper;
    private readonly IModelMapperUnit<CommentModel, Comment> _modelMapper;

    public CommentRepository(ApplicationDbContext context, IAggregateMapperUnit<Comment, CommentModel> aggregateMapper,
        IModelMapperUnit<CommentModel, Comment> modelMapper)
    {
        _context = context;
        _aggregateMapper = aggregateMapper;
        _modelMapper = modelMapper;
    }

    public async Task AddAsync(Comment entity)
    {
        _context.Notifications.AddRange(entity.DomainEvents);
        var comment = await _modelMapper.MapAsync(entity);
        await _context.AddAsync(comment);
    }

    public async Task UpdateAsync(Comment entity)
    {
        _context.Notifications.AddRange(entity.DomainEvents);
        await _modelMapper.MapAsync(entity);
    }
    public Task DeleteAsync(Guid id)
    {
        _context.Remove(_context.Comments.First(comment => comment.Id == id));
        return Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<Comment> entities)
    {
        var ids = entities.Select(comment => comment.Id);
        _context.RemoveRange(_context.Comments.Where(comment => ids.Contains(comment.Id)));
        return Task.CompletedTask;
    }

    public async Task<Comment?> GetAsync(Guid id)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(commentModel => commentModel.Id == id);
        return comment == null ? null : _aggregateMapper.Map(comment);
    }

    public async Task<IList<Comment>> FindAsync(ISpecification<Comment, ICommentSpecificationVisitor>? specification,
        IOrderBy<Comment, ICommentSortingVisitor>? orderBy = null, int? skip = null, int? take = null)
    {
        var query = _context.Comments.AsQueryable();
        if (specification != null)
        {
            var visitor = new CommentVisitor();
            specification.Accept(visitor);
            if (visitor.Expr != null) query = query.Where(visitor.Expr);
        }

        if (orderBy != null)
        {
            var visitor = new CommentSortingVisitor();
            orderBy.Accept(visitor);
            var firstQuery = visitor.SortItems.First();
            var orderedQuery = firstQuery.IsDescending
                ? query.OrderByDescending(firstQuery.Expr)
                : query.OrderBy(firstQuery.Expr);
            query = visitor.SortItems.Skip(1)
                .Aggregate(orderedQuery, (current, sort) => sort.IsDescending
                    ? current.ThenByDescending(sort.Expr)
                    : current.ThenBy(sort.Expr));
        }

        if (skip.HasValue) query = query.Skip(skip.Value);
        if (take.HasValue) query = query.Take(take.Value);

        return (await query.ToListAsync()).Select(_aggregateMapper.Map).ToList();
    }

    public Task<int> CountAsync(ISpecification<Comment, ICommentSpecificationVisitor>? specification)
    {
        var query = _context.Comments.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new CommentVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
}