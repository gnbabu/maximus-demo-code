using ReportingService.Services.Security;
using ReportingService.Services.Interfaces;

namespace MainBoldReportsAPI.Web.Services
{
    /// <summary>
    /// Represents work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Fields

        private readonly IAuthenticationService _authenticationService;


        private User _cachedUser;

        #endregion

        #region Ctor

        public WebWorkContext(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        #endregion



        #region Properties

        public virtual Task ClearUserCache()
        {
            _cachedUser = null;
            return Task.FromResult<bool?>(null);
        }

        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task<User> GetCurrentUserAsync()
        {
            //whether there is a cached value
            if (_cachedUser != null)
                return _cachedUser;

            _cachedUser = await _authenticationService.GetAuthenticatedUserAsync();

            return _cachedUser;
        }

        public virtual User GetCurrentUser()
        {
            //whether there is a cached value
            if (_cachedUser != null)
                return _cachedUser;

            _cachedUser = _authenticationService.GetAuthenticatedUser();

            return _cachedUser;
        }

       
        public virtual async Task<string> GetCurrentUserNameAsync()
        {
            var user = await GetCurrentUserAsync();
            return user?.Username;
        }

        #endregion
    }
}