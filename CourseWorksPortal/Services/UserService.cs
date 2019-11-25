using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseWorksPortal.Models;
using CourseWorksPortal.Models.OtherModels;
using Microsoft.EntityFrameworkCore;

namespace CourseWorksPortal.Services
{
    public interface IUserService
    {
        Task<OperationResult> CreateUser(User newUser);
        Task<List<User>> GetAll();
        Task<User> GetUserForEdit(int? id);
        Task<OperationResult> EditUser(User user);
        Task<OperationResult> ConfiemDelete(int? id);
    }
    public class UserService : IUserService
    {
        private AppDbContext _dbContext;
        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<OperationResult> Delete(int? id)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == (id ?? 0));
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return new OperationResult(true,"");
            }
            return new OperationResult(false,"");
        }
        public async Task<OperationResult> ConfiemDelete(int? id)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == (id ?? 0));
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
                return new OperationResult(true,"");
            }
            return new OperationResult(false,"");
        }
        public async Task<OperationResult> EditUser(User user)
        {
            try
            {
                User old_user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == user.Id);
                if (user.Password != null) user.Password = HashingHelper.getHash(user.Username, user.Password);
                else if (user.Password == null) user.Password = old_user.Password;

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return new OperationResult(true,"");
            }
            catch (Exception e)
            {
                return new OperationResult(false,"");
            }
            
        }
        public async Task<User> GetUserForEdit(int? id)
        {
            User user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == (id ?? 0));
            user.Password = "";
            return user;
        }
        public async Task<List<User>> GetAll()
        {
            return await _dbContext.Users
                .ToListAsync();
        }
        public async Task<OperationResult> CreateUser(User newUser)
        {
            User dublicate_user = await _dbContext.Users.FirstOrDefaultAsync(p => p.Username == newUser.Username);
            if (dublicate_user == null)
            {
                newUser.Password = HashingHelper.getHash(newUser.Username, newUser.Password);
                _dbContext.Users.Add(newUser);
                await _dbContext.SaveChangesAsync();
                return new OperationResult(true,"succed");
            }
            return new OperationResult(false, "fail");
        }
    }
    
}