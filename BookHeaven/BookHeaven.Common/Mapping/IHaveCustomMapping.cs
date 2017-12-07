using AutoMapper;

namespace BookHeaven.Common.Mapping
{
    public interface IHaveCustomMapping
    {
        void ConfigureMapping(Profile profile);
    }
}