using Microsoft.EntityFrameworkCore;
using Overoom.Domain.User.Entities;
using Overoom.Infrastructure.Storage.Context;
using Overoom.Infrastructure.Storage.Mappers.Abstractions;
using Overoom.Infrastructure.Storage.Models.Users;

namespace Overoom.Infrastructure.Storage.Mappers.ModelMappers;

internal class UserModelMapper : IModelMapperUnit<UserModel, User>
{
    private readonly ApplicationDbContext _context;

    public UserModelMapper(ApplicationDbContext context) => _context = context;

    public async Task<UserModel> MapAsync(User model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.Id) ?? new UserModel { Id = model.Id };
        user.Name = model.Name;
        user.Email = model.Email;
        user.Id = model.Id;
        user.AvatarFileName = model.AvatarFileName;
        user.
        return user;
    }
}