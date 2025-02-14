using AutoMapper;
using BookItApi.Dtos.Admin;
using BookItApi.Models;

namespace BookItApi.Profiles;

/// <summary>
/// Configura os mapeamentos entre os DTOs de Admin e os modelos correspondentes.
/// Esta classe é usada pelo AutoMapper para mapear as propriedades entre as classes.
/// </summary>
public class AdminProfile : Profile {

    /// <summary>
    /// Construtor que define os mapeamentos entre os DTOs e os modelos de Admin.
    /// </summary>
    public AdminProfile() {

        CreateMap<CreateAdminDto, Admin>();
        CreateMap<Admin, ReadAdminDto>();
    }
}
