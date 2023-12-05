using AutoMapper;
using AutoMapper.Internal;

namespace BookStoreApp.API.Configurations;

public class GlobalMappingConfiguration : Profile
{
    public GlobalMappingConfiguration()
    {
        ((IProfileExpressionInternal)this).ForAllMaps((map, exp) =>
        {
            foreach (var member in map.IncludedMembersNames)
            {
                if (member.GetType() == typeof(string))
                {
                    exp.ForMember(member, opt => opt.Condition((src, dest, srcMember) => srcMember != null));
                }
            }
        });
    }
}