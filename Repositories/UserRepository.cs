using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Xml.Linq;
using WebApp_Tak4.Data;
using WebApp_Tak4.Models;

namespace WebApp_Task4.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(Task4DbContext dbContext) : base(dbContext) { }

        public override async Task<IEnumerable<User>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }
        public async Task<User?> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }
        public async Task<List<User>> GetById(int[] indexes)
        {
            var selectedUsers = await _context.Users.Where(u => indexes.Contains(u.Userid)).ToListAsync();
            return selectedUsers;
        }

        public async Task<User?> GetByEmail(string? email)
        {
            if (string.IsNullOrEmpty(email)) 
                return null;

            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public override async Task Create(User entity)
        {
            try
            {
                _context.Users.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public override async Task Delete(User entity)
        {
            try
            {
                _context.Users.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteRange(IEnumerable<User> entities)
        {
            try
            {
                _context.Users.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public override async Task Update(User entity)
        {
            try
            {
                _context.Users.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task UpdateRange(IEnumerable<User> entities)
        {
            try
            {
                _context.Users.UpdateRange(entities);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task BlockById(int id)
        {
            var user = await GetById(id);
            user.UserState = UserState.blocked;
            await Update(user);
        }
        public async Task BlockById(int[] id)
        {
            var users = await GetById(id);
            foreach(var user in users)
            {
                user.UserState = UserState.blocked;
            }
            await UpdateRange(users);
        }

        public async Task UnBlockById(int id)
        {
            var user = await GetById(id);
            user!.UserState = UserState.active;
            await Update(user);
        }

        public async Task UnBlockById(int[] id)
        {
            var users = await GetById(id);
            foreach (var user in users)
            {
                user.UserState = UserState.active;
            }
            await UpdateRange(users);
        }

        public async Task<List<User>?> FilterByEmail(string? email)
        {
            return await _context.Users.Where(u => EF.Functions.Like(u.Email, $"%{email}%")).ToListAsync();            
        }

    }
}
