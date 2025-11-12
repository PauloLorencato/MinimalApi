using MinimalApi.Domain.Enums;

namespace MinimalApi.Domain.DTOs;

public class AdministradorDTO
{
    public string Email { get; set; } = default;
    public string Senha { get; set; } = default;
    public PerfilEnum Perfil { get; set; } = default;
}