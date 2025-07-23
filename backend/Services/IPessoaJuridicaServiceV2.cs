using CadastroPessoasApi.DTOs.V2;

public interface IPessoaJuridicaServiceV2
{
    Task<(bool Success, string? ErrorMessage, PessoaJuridicaReadDtoV2? Result)> CreateAsync(PessoaJuridicaCreateDtoV2 dto);
    Task<PessoaJuridicaReadDtoV2?> GetByIdAsync(int id);
    Task<IEnumerable<PessoaJuridicaReadDtoV2>> GetAllAsync();
    Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, PessoaJuridicaCreateDtoV2 dto);
    Task<bool> DeleteAsync(int id);
}
