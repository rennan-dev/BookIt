using AutoMapper;
using docente_tecnico_api.Dtos;
using docente_tecnico_api.Models;

namespace docente_tecnico_api.Profiles;

public class UserProfile : Profile {

    public UserProfile() {

        CreateMap<CreateUserDto, User>();
    }
}