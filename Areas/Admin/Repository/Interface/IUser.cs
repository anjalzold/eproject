using eproject.Areas.Admin.Models;
using eproject.ViewModels;
using eproject.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using eproject.Areas.Admin.Repository.Interface;

public interface IUser : IRepository<User>
{
    // Task<IEnumerable<UserViewModel>> GetAllWithRolesAsync();

    Task<IEnumerable<User>> GetAllAsync();

    Task<User> GetByIdAsync(string id);
}