using System;
using Padaria.Share.Comprovativos.DTO;

namespace Padaria.WebApi.Service.Comprovativo;

public interface IComprovativoService
{
    Task<string> SalvarComprovativo(ComprovativoDTO comprovativo);
    Task<ComprovativoResponse?> DownloadComprativo(int id); 
}
