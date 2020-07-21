using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace BCC.PledgeRefBack.Authorization
{
    public class PolicyHandler : AuthorizationHandler<PolicyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "auth"))
            {
                var auth = context.User.FindAll(c => c.Type == "auth");
                var roles = auth.Select(c => JsonConvert.DeserializeObject<Role>(c.Value));
                if (roles.Any(c => requirement.policies.Any(p => p.ToUpper() == c.Authority.ToUpper())))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
