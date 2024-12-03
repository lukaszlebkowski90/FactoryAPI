using FactoryAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FactoryAPI.Authorization
{
    public class CreatedMultipleFactoriesRequirementHandler : AuthorizationHandler<CreatedMultipleFactoryRequirement>
    {
        private readonly FactoryDbContext _context;

        public CreatedMultipleFactoriesRequirementHandler(FactoryDbContext context)
        {
            _context = context;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            CreatedMultipleFactoryRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var createdFactoryCount = _context
                .Factories
                .Count(r => r.CreatedById == userId);

            if (createdFactoryCount >= requirement.MinimumFactoryCreated)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
