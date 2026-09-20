using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReportingService.Services.Security
{
    public class User
    {
        public int Id { get; set; }

        private List<int> _roleIds;

        [JsonProperty("user_guid")]
        public Guid UserGuid { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        ///     Gets or sets the email
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("language_id")]
        public int? LanguageId { get; set; }

        
        [JsonProperty("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        
        /// <summary>
        ///     Gets or sets a value indicating whether the user is active
        /// </summary>
        [JsonProperty("active")]
        public bool? Active { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the user has been deleted
        /// </summary>
        [JsonProperty("deleted")]
        public bool? Deleted { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the user account is system
        /// </summary>
        [JsonProperty("is_system_account")]
        public bool? IsSystemAccount { get; set; }

        /// <summary>
        ///     Gets or sets the user system name
        /// </summary>
        [JsonProperty("system_name")]
        public string SystemName { get; set; }

        /// <summary>
        ///     Gets or sets the last IP address
        /// </summary>
        [JsonProperty("last_ip_address")]
        public string LastIpAddress { get; set; }

        /// <summary>
        ///     Gets or sets the date and time of entity creation
        /// </summary>
        [JsonProperty("created_on_utc")]
        public DateTime? CreatedOnUtc { get; set; }

        /// <summary>
        ///     Gets or sets the date and time of last login
        /// </summary>
        [JsonProperty("last_login_date_utc")]
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        ///     Gets or sets the date and time of last activity
        /// </summary>
        [JsonProperty("last_activity_date_utc")]
        public DateTime? LastActivityDateUtc { get; set; }

        /// <summary>
        ///     Gets or sets the client identifier in which user registered
        /// </summary>
        [JsonProperty("registered_in_client_id")]
        public int? RegisteredInClientId { get; set; }

        [JsonProperty("role_ids")]
        public List<int> RoleIds
        {
            get
            {
                if (_roleIds == null)
                {
                    _roleIds = new List<int>();
                }

                return _roleIds;
            }
            set => _roleIds = value;
        }
    }
}
