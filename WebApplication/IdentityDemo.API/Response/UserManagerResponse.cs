using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.API.Response
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool Issuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
