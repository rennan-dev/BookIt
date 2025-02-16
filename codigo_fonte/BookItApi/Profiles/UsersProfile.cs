using AutoMapper;
using BookItApi.Dtos.User;
using BookItApi.Models;

namespace BookItApi.Profiles;

/// <summary>
/// Configura os mapeamentos entre os DTOs dos Users e os modelos correspondentes.
/// Esta classe é usada pelo AutoMapper para mapear as propriedades entre as classes.
/// </summary>
public class UsersProfile : Profile {

    /// <summary>
    /// Construtor que define os mapeamentos entre os DTOs e os modelos dos Usuários.
    /// </summary>
    public UsersProfile() {

        CreateMap<CreateUserDto, User>();
        CreateMap<User, ReadUserDto>();
    }
}
