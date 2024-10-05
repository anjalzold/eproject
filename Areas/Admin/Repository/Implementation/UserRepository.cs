using eproject.Areas.Admin.Models;
using eproject.ViewModels;
using eproject.Data;
using eproject.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserRepository : Repository<User>, IUser
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    public UserRepository(AppDbContext context, UserManager<User> userManager) : base(context)
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task<User> GetByIdAsync(string id)

    {

        return await Context.Set<User>().FindAsync(id);

    }


    public async Task<IEnumerable<User>> GetAllAsync(){

        return await Context.Set<User>().ToListAsync();
    }

    // public async Task<IEnumerable<UserViewModel>> GetAllWithRolesAsync()
    // {
    //     var users = await _context.Users.ToListAsync();
    //     var userViewModels = new List<UserViewModel>();

    //     foreach (var user in users)
    //     {
    //         var roles = await _userManager.GetRolesAsync(user);
    //         var userViewModel = new UserViewModel
    //         {
    //             Id = user.Id,
    //             UserName = user.UserName,
    //             Email = user.Email,
    //             AvatarPath = user.AvatarPath,
    //             CreatedAt = user.CreatedAt,
    //             Role = user.Role
    //         };
    //         userViewModels.Add(userViewModel);
    //     }

    //     return userViewModels;
    // }
}