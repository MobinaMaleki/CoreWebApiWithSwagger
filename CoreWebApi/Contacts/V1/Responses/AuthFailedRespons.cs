using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Contacts.V1.Responses
{
    public class AuthFailedRespons
    {
        public IEnumerable<string> Errors{ get; set; }
    }
}
