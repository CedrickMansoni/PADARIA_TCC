using System;

namespace Padaria.WebApi.SalvarArquivos;

public interface IArquivoService
{
    Task<string> SalvarArquivoAsync(IFormFile arquivo, string storagePath, string pasta);
}
