using Microsoft.EntityFrameworkCore;
using Films.Domain.Abstractions.Repositories;
using Films.Domain.Comments;
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

public class CommentRepository(
    ApplicationDbContext context,
    IAggregateMapperUnit<Comment, CommentModel> aggregateMapper,
    IModelMapperUnit<CommentModel, Comment> modelMapper)
    : ICommentRepository
{
    public async Task AddAsync(Comment entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        var comment = await modelMapper.MapAsync(entity);
        await context.AddAsync(comment);
    }

    public async Task UpdateAsync(Comment entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        await modelMapper.MapAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        context.Remove(context.Comments.First(comment => comment.Id == id));
        return Task.CompletedTask;
    }

    public async Task<Comment?> GetAsync(Guid id)
    {
        var comment = await context.Comments.AsNoTracking().FirstOrDefaultAsync(commentModel => commentModel.Id == id);
        return comment == null ? null : aggregateMapper.Map(comment);
    }

    public async Task<IReadOnlyCollection<Comment>> FindAsync(
        ISpecification<Comment, ICommentSpecificationVisitor>? specification,
        IOrderBy<Comment, ICommentSortingVisitor>? orderBy = null, int? skip = null, int? take = null)
    {
        var query = context.Comments.AsQueryable();
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

            orderedQuery = visitor.SortItems.Skip(1)
                .Aggregate(orderedQuery, (current, sort) => sort.IsDescending
                    ? current.ThenByDescending(sort.Expr)
                    : current.ThenBy(sort.Expr));
            
            query = orderedQuery.ThenBy(v => v.Id);
        }
        else
        {
            query = query.OrderBy(x => x.Id);
        }

        if (skip.HasValue) query = query.Skip(skip.Value);
        if (take.HasValue) query = query.Take(take.Value);

        return (await query.AsNoTracking().ToArrayAsync()).Select(aggregateMapper.Map).ToArray();
    }

    public Task<int> CountAsync(ISpecification<Comment, ICommentSpecificationVisitor>? specification)
    {
        var query = context.Comments.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new CommentVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
}