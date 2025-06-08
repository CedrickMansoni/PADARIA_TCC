using System;
using Padaria.Share.Comprovativos.DTO;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Repository.Comprovativo;

public interface IComprovativoRepository
{
    Task<string> SalvarComprovativo(ComprovativoModels comprovativo);
    Task<ComprovativoResponse?> DownloadComprativo(int id); 
}
