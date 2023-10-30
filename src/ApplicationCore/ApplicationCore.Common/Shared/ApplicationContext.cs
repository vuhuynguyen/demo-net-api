using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Common.Shared
{
    public class ApplicationContext
    {
        private UserPrincipal _principal = UserPrincipal.System;
        public UserPrincipal Principal
        {
            
            get { return _principal; }
            set
            {
                if (_principal != null)
                {
                    return;
                    //throw new InvalidOperationException("User principal is already set");
                }

                _principal = value;
            }
        }

        public class UserPrincipal
        {
            public string UserId { get; }
            public string Username { get; }
            public string Role { get; }

            /// <summary>
            /// If this is system request like jobs ...
            /// </summary>
            public bool IsSystem => UserId == "System";

            public UserPrincipal(string userId, string userName, string role)
            {
                UserId = userId;
                Username = userName;
                Role = role;
            }

            public static UserPrincipal System => new("System", "System", "System");
        }
    }
}
