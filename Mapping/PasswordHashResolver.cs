using AutoMapper;
using SimpleMinimalAPI.DTOs;
using SimpleMinimalAPI.Helper;
using SimpleMinimalAPI.Models;

namespace SimpleMinimalAPI.Mapping
{
    // <TSource, TDestination, TDestMember>
    public class PasswordHashResolver : IValueResolver<UserDTO, User, string>
    {
        public string Resolve(UserDTO source, User destination, string destMember, ResolutionContext context)
        {
            var pashwordHash = Utilities.HashPassword(source.Password);

            return pashwordHash;
        }
    }
}
