using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Exceptions
{
    [Serializable]
    public class AuthorizationDeniedException : Exception
    {
        public AuthorizationDeniedException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
