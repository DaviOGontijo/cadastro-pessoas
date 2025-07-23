using CadastroPessoasApi.DTOs.V1;

public interface IPessoaFisicaServiceV1
{
    Task<(bool Success, string ErrorMessage, PessoaFisicaReadDtoV1 Result)> CreateAsync(PessoaFisicaCreateDtoV1 dto);
    Task<PessoaFisicaReadDtoV1> GetByIdAsync(int id);
    Task<IEnumerable<PessoaFisicaReadDtoV1>> GetAllAsync();
    Task<(bool Success, string ErrorMessage)> UpdateAsync(int id, PessoaFisicaCreateDtoV1 dto);
    Task<bool> DeleteAsync(int id);
}