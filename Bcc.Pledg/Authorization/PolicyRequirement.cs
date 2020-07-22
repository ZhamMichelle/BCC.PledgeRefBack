using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcc.Pledg.Authorization
{
    public class PolicyRequirement : IAuthorizationRequirement
    {
        protected internal List<string> policies;

        public PolicyRequirement(List<string> policies)
        {
            this.policies = policies;
        }
    }
}
