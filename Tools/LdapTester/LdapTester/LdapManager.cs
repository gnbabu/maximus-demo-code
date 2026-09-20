using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LdapTester
{
    /// <summary>
    /// Ldap related contracts
    /// </summary>
    public interface ILdapValidator
    {
        /// <summary>
        /// Check if user in Ldap 
        /// </summary>
        /// <param name="userId">Ldap user name without domain name</param>
        /// <param name="password">Ldap passsword</param>
        bool Validate(string userId, string password);
    }

    /// <summary>
    /// Ldap related tasks manager
    /// </summary>    
    public class LdapManager
    {
        /// <summary>
        /// Domain name from config file
        /// </summary>
        public readonly string DomainName;
        /// <summary>
        /// Port name form config file, default 389
        /// </summary>
        public readonly int PortNumber;

        public LdapManager(string domainName, int port = 389)
        {
            DomainName = domainName;
            PortNumber = port;
        }

        /// <summary>
        /// Check if user in Ldap 
        /// </summary>
        /// <param name="userId">Ldap user name without domain name</param>
        /// <param name="password">Ldap passsword</param>
        public bool Validate(string userId, string password)
        {
            try
            {
                string path = LdapPath();
                string username = UserFullId(userId);
                DirectoryEntry de = new DirectoryEntry
                         (path, username, password, AuthenticationTypes.None);
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.FindOne();
                return true;
            }
            catch (DirectoryServicesCOMException ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// User full id 
        /// </summary>
        /// <param name="userId">User name</param>
        /// <returns>userName@domain</returns>
        public string UserFullId(string userId)
        {
            string value = string.Format(@"{0}@{1}", userId, DomainName);
            return value;
        }

        /// <summary>
        /// Get Ldap path from domain and port
        /// </summary>
        /// <returns></returns>
        public string LdapPath()
        {
            string value = string.Format(@"LDAP://{0}:{1}", DomainName, PortNumber);
            return value;
        }

        public String ErrorMessage { get; set; }
    }
}
