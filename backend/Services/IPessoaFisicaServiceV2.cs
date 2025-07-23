using CadastroPessoasApi.DTOs.V2;

public interface IPessoaFisicaServiceV2
{
    Task<(bool Success, string? ErrorMessage, PessoaFisicaReadDtoV2? Result)> CreateAsync(PessoaFisicaCreateDtoV2 dto);
    Task<PessoaFisicaReadDtoV2?> GetByIdAsync(int id);
    Task<IEnumerable<PessoaFisicaReadDtoV2>> GetAllAsync();
    Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, PessoaFisicaCreateDtoV2 dto);
    Task<bool> DeleteAsync(int id);
}
