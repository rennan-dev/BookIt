using AutoMapper;
using BookItApi.Dtos.Servidor;
using BookItApi.Models;

namespace BookItApi.Profiles;

/// <summary>
/// Configura os mapeamentos entre os DTOs de Servidor e os modelos correspondentes.
/// Esta classe é usada pelo AutoMapper para mapear as propriedades entre as classes.
/// </summary>
public class ServidorProfile : Profile {

    /// <summary>
    /// Construtor que define os mapeamentos entre os DTOs e os modelos de Servidor.
    /// </summary>
    public ServidorProfile() {

        CreateMap<CreateServidorDto, Servidor>();
        CreateMap<Servidor, ReadServidorDto>();
    }
}
