using Microsoft.EntityFrameworkCore;
using TourAndTravels.Data.Entities;
using TourAndTravels.Domain;
using User = TourAndTravels.Data.Entities.User;

namespace TourAndTravels.Data.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> Authenticate(LoginViewModel model);
    }

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(TourAndTravelsDbContext context, ILogger<UserRepository> logger) : base(context)
        {
            this._logger = logger;
        }

        public override async Task<IEnumerable<User>> All()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(UserRepository));
                return new List<User>();
            }
        }

        public override async Task<bool> Upsert(User entity)
        {
            try
            {
                var existingUser = await dbSet.Where(x => x.UserId == entity.UserId)
                                                    .FirstOrDefaultAsync();

                if (existingUser == null)
                    return await Add(entity);

                existingUser.Firstname = entity.Firstname;
                existingUser.Lastname = entity.Lastname;
                existingUser.Email = entity.Email;
                existingUser.PhoneNumber = entity.PhoneNumber;
                existingUser.ModifiedDate = DateTime.Now;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(UserRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var exist = await dbSet.Where(x => x.UserId == id)
                                        .FirstOrDefaultAsync();

                if (exist == null) return false;

                dbSet.Remove(exist);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(UserRepository));
                return false;
            }
        }

        public async Task<User> Authenticate(LoginViewModel model)
        {
            var users = await base.Find(u => u.Username == model.Username && u.Password == model.Password);
            return users?.FirstOrDefault();
        }
    }
}
