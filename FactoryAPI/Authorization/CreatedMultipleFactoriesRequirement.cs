using Microsoft.AspNetCore.Authorization;

namespace FactoryAPI.Authorization
{
    public class CreatedMultipleFactoryRequirement : IAuthorizationRequirement
    {
        public CreatedMultipleFactoryRequirement(int minimumFactoryCreated)
        {
            MinimumFactoryCreated = minimumFactoryCreated;
        }
        public int MinimumFactoryCreated { get; }
    }
}
