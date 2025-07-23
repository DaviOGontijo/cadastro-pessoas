using CadastroPessoasApi.DTOs.V1;
public interface IPessoaJuridicaServiceV1
{
    Task<(bool Success, string? ErrorMessage, PessoaJuridicaReadDtoV1? Result)> CreateAsync(PessoaJuridicaCreateDtoV1 dto);
    Task<PessoaJuridicaReadDtoV1?> GetByIdAsync(int id);
    Task<IEnumerable<PessoaJuridicaReadDtoV1>> GetAllAsync();
    Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, PessoaJuridicaCreateDtoV1 dto);
    Task<bool> DeleteAsync(int id);
}
