using BookItApi.Models;
using Microsoft.AspNetCore.Identity;

namespace BookItApi.Services;

/// <summary>
/// classe para a criação do usuário administrador
/// </summary>
public static class SeedData {

    /// <summary>
    /// Criação do usuário admin caso ainda não tenha
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task Initialize(IServiceProvider serviceProvider) {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        string adminEmail = "rennan@ufam.edu.br";
        string adminUserName = "123.456.789-00";
        string Password = "Aaaa123!"; 

        //verifique se o admin já existe
        if(await userManager.FindByEmailAsync(adminEmail) == null) {
            var adminUser = new User {
                UserName = adminUserName,
                Email = adminEmail,
                Name = "Rennan Alves",
                Siape = "999888", 
                Cpf = "123.456.789-00",
                IsAprovado = true,
                IsAdmin = true
            };

            var result = await userManager.CreateAsync(adminUser, Password);
            if(!result.Succeeded) {
                throw new Exception("Falha ao criar o usuário administrador.");
            }
        }
    }
}