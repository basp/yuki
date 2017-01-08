using AutoMapper;

namespace Yuki.Tests
{
    public class CommandFixture
    {
        public CommandFixture()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());
        }
    }
}
