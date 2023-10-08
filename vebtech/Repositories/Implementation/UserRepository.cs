using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Net;
using vebtech.Context;
using vebtech.CustomException;
using vebtech.Models;
using vebtech.Models.DTO;
using vebtech.Models.Enums;
using vebtech.Repositories.Abstract;

namespace vebtech.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;
    private readonly Mapper _mapper;
    public UserRepository(ApplicationContext context, Mapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IEnumerable<User> Get(PaginationParameters paginationParameters,
        SortParameters sortParameters, FilterParameters filterParameters)
    {
        IEnumerable<User> users = _context.Users
            .Include(user => user.Roles)
            .AsQueryable();

        if (!users.Any())
        {
            throw new HttpResponseException(HttpStatusCode.BadRequest, "The list of users is empty");
        }

        if (filterParameters != null)
        {
            users = Filter(users, filterParameters);
        }

        if (!string.IsNullOrEmpty(sortParameters.OrderBy))
        {
            if (sortParameters.OrderAsc != SortDirection.Ascending
                && sortParameters.OrderAsc != SortDirection.Descending)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Not valid sort order");
            }

            users = Sort(users, sortParameters);
        }

        return users
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize);
    }

    public async Task<User> GetUser(int id)
    {
        var user = await _context.Users
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(user => user.Id == id);

        return user ?? throw new HttpResponseException(HttpStatusCode.NotFound, "User not found");
    }

    public async Task<User> Create(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> Update(int id, UserDto userDto)
    {
        var oldUser = await GetUser(id);
        if (oldUser == null)
        {
            return null;
        }

        _mapper.Map(userDto, oldUser);
        await _context.SaveChangesAsync();
        return oldUser;
    }

    public async Task Delete(int id)
    {
        _context.Users.Remove(await GetUser(id));
        await _context.SaveChangesAsync();
    }

    public async Task<Role> CreateRole(int userId, string name)
    {
        var user = await GetUser(userId);
        var role = new Role()
        {
            Name = name,
            User = user,
        };

        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<bool> IsEmailExist(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == email) != null;
    }

    private IEnumerable<User> Sort(IEnumerable<User> users, SortParameters sortParameters)
    {
        return sortParameters.OrderBy.ToLower() switch
        {
            "id" => sortParameters.OrderAsc == SortDirection.Ascending
                                ? users.OrderBy(i => i.Id)
                                : users.OrderByDescending(i => i.Id),
            "name" => sortParameters.OrderAsc == SortDirection.Ascending
                                ? users.OrderBy(i => i.Name)
                                : users.OrderByDescending(i => i.Name),
            "age" => sortParameters.OrderAsc == SortDirection.Ascending
                                ? users.OrderBy(i => i.Age)
                                : users.OrderByDescending(i => i.Age),
            "email" => sortParameters.OrderAsc == SortDirection.Ascending
                                ? users.OrderBy(i => i.Email)
                                : users.OrderByDescending(i => i.Email),
            _ => throw new HttpResponseException(HttpStatusCode.BadRequest, "Not valid sort field"),
        };
    }

    private IEnumerable<User> Filter(IEnumerable<User> users, FilterParameters filterParameters)
    {
        if (!string.IsNullOrEmpty(filterParameters.SearchQuery))
        {
            users = users.Where(user => user.Name.Contains(filterParameters.SearchQuery)
            || user.Email.Contains(filterParameters.SearchQuery)
            || user.Age.ToString().Contains(filterParameters.SearchQuery)
            || (user.Roles != null && user.Roles.Any(role => role.Name.Contains(filterParameters.SearchQuery))));
        }

        if (!string.IsNullOrEmpty(filterParameters.Name))
        {
            users = users.Where(user => user.Name.Contains(filterParameters.Name));
        }

        if (filterParameters.Age != null)
        {
            users = users.Where(user => user.Age == filterParameters.Age);
        }

        if (!string.IsNullOrEmpty(filterParameters.Email))
        {
            users = users.Where(user => user.Email.Contains(filterParameters.Email));
        }

        if (!string.IsNullOrEmpty(filterParameters.RoleName))
        {
            users = users.Where(user => user.Roles.Any(role => role.Name.Contains(filterParameters.RoleName)));
        }

        return users;
    }
}
