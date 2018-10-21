using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MRBooker.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace MRBooker.Wrappers
{
    public class ApplicationUserManager<T> : UserManager<T> where T : IdentityUser
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public ApplicationUserManager(IUserStore<T> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<T> passwordHasher,
            IEnumerable<IUserValidator<T>> userValidators,
            IEnumerable<IPasswordValidator<T>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<T>> logger) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _dbContext = (ApplicationDbContext)services.GetService(typeof(ApplicationDbContext));
            _logger = logger;
            _serviceProvider = services;
        }

        public T GetUserWithDataByName(string userName)
        {
            try
            {
                var dbCtx = _serviceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
                var appUsr = dbCtx?.Users.Include(x => x.Reservations).FirstOrDefault(x => x.UserName == userName);

                return (T) (object) appUsr;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw;
            }
        }
    }
}
