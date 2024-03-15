using Microsoft.EntityFrameworkCore;
using Room.Domain.Abstractions.Repositories;
using Room.Domain.Messages.YoutubeMessages;
using Room.Domain.Messages.YoutubeMessages.Ordering.Visitor;
using Room.Domain.Messages.YoutubeMessages.Specifications.Visitor;
using Room.Domain.Ordering.Abstractions;
using Room.Domain.Specifications.Abstractions;
using Room.Infrastructure.Storage.Context;
using Room.Infrastructure.Storage.Mappers.Abstractions;
using Room.Infrastructure.Storage.Models.YoutubeRooms;
using Room.Infrastructure.Storage.Models.Messages;
using Room.Infrastructure.Storage.Visitors.Sorting;
using Room.Infrastructure.Storage.Visitors.Specifications;

namespace Room.Infrastructure.Storage.Repositories;

public class YoutubeMessageRepository(
    ApplicationDbContext context,
    IAggregateMapperUnit<YoutubeMessage, MessageModel<YoutubeRoomModel>> aggregateMapper,
    IModelMapperUnit<MessageModel<YoutubeRoomModel>, YoutubeMessage> modelMapper)
    : IYoutubeMessageRepository
{
    public async Task AddAsync(YoutubeMessage entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        var message = await modelMapper.MapAsync(entity);
        await context.AddAsync(message);
    }

    public async Task UpdateAsync(YoutubeMessage entity)
    {
        context.Notifications.AddRange(entity.DomainEvents);
        await modelMapper.MapAsync(entity);
    }

    public Task DeleteAsync(Guid id)
    {
        context.Remove(context.YoutubeMessages.First(message => message.Id == id));
        return Task.CompletedTask;
    }

    public async Task<YoutubeMessage?> GetAsync(Guid id)
    {
        var message = await context.YoutubeMessages
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return message == null ? null : aggregateMapper.Map(message);
    }

 public async Task<IReadOnlyCollection<YoutubeMessage>> FindAsync(
        ISpecification<YoutubeMessage, IYoutubeMessageSpecificationVisitor>? specification,
        IOrderBy<YoutubeMessage, IYoutubeMessageSortingVisitor>? orderBy = null, int? skip = null,
        int? take = null)
    {
        var query = context.YoutubeMessages.AsQueryable();
        if (specification != null)
        {
            var visitor = new YoutubeMessageVisitor();
            specification.Accept(visitor);
            if (visitor.Expr != null) query = query.Where(visitor.Expr);
        }

        if (orderBy != null)
        {
            var visitor = new YoutubeMessageSortingVisitor();
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

        var models = await query            
            .AsNoTracking()
            .ToArrayAsync();
        
        return models.Select(aggregateMapper.Map).ToArray();
    }

    public Task<int> CountAsync(ISpecification<YoutubeMessage, IYoutubeMessageSpecificationVisitor>? specification)
    {
        var query = context.YoutubeMessages.AsQueryable();
        if (specification == null) return query.CountAsync();
        var visitor = new YoutubeMessageVisitor();
        specification.Accept(visitor);
        if (visitor.Expr != null) query = query.Where(visitor.Expr);

        return query.CountAsync();
    }
}