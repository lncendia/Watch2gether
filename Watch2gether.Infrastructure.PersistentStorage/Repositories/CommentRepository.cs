using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Watch2gether.Domain.Abstractions.Repositories;
using Watch2gether.Domain.Ordering.Abstractions;
using Watch2gether.Domain.Specifications.Abstractions;
using Watch2gether.Domain.Comments;
using Watch2gether.Domain.Comments.Ordering.Visitor;
using Watch2gether.Domain.Comments.Specifications.Visitor;
using Watch2gether.Infrastructure.PersistentStorage.Context;
using Watch2gether.Infrastructure.PersistentStorage.Models.Comments;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Sorting;
using Watch2gether.Infrastructure.PersistentStorage.Visitors.Specifications;

namespace Watch2gether.Infrastructure.PersistentStorage.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
        _mapper = GetMapper();
    }

    private Comment Map(CommentModel model)
    {
        var comment = _mapper.Map<Comment>(model);
        var x = comment.GetType();
        x.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(comment, model.Id);
        x.GetField("<CreatedAt>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(comment,
            model.CreatedAt);
        return comment;
    }

    public async Task AddAsync(Comment entity)
    {
        var comment = _mapper.Map<Comment, CommentModel>(entity);
        await _context.AddAsync(comment);
    }

    public async Task AddRangeAsync(IList<Comment> entities)
    {
        var comments = _mapper.Map<IList<Comment>, List<CommentModel>>(entities);
        await _context.AddRangeAsync(comments);
    }

    public async Task UpdateAsync(Comment entity)
    {
        var model = await _context.Comments.FirstAsync(x => x.Id == entity.Id);
        _mapper.Map(entity, model);
    }

    public async Task UpdateRangeAsync(IList<Comment> entities)
    {
        var ids = entities.Select(comment => comment.Id);
        var comments = await _context.Comments.Where(comment => ids.Contains(comment.Id)).ToListAsync();
        foreach (var entity in entities)
            _mapper.Map(entity, comments.First(commentModel => commentModel.Id == entity.Id));
    }

    public Task DeleteAsync(Comment entity)
    {
        _context.Remove(_context.Comments.First(comment => comment.Id == entity.Id));
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
        return comment == null ? null : Map(comment);
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

        return (await query.ToListAsync()).Select(Map).ToList();
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

    private static IMapper GetMapper() =>
        new Mapper(new MapperConfiguration(expr => expr.CreateMap<Comment, CommentModel>().ReverseMap()));
}