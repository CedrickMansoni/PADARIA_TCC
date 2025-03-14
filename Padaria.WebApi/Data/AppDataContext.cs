using System;
using Microsoft.EntityFrameworkCore;
using Padaria.WebApi.Models;

namespace Padaria.WebApi.Data;

public class AppDataContext(DbContextOptions<AppDataContext> options) : DbContext(options)
{
    public DbSet<CategoriaFuncionarioModel> TabelaCategoriaFuncionarioModel { get; set; }
        public DbSet<FuncionarioModel> TabelaFuncionarioModel { get; set; }
        public DbSet<ClienteModel> TabelaClienteModel { get; set; }
        public DbSet<TelefoneModel> TabelaTelefoneModel { get; set; }
        public DbSet<CategoriaProdutoModel> TabelaCategoriaProdutoModel { get; set; }
        public DbSet<ProdutoModel> TabelaProdutoModel { get; set; }
        public DbSet<CaixaModel> TabelaCaixaModel { get; set; }
        public DbSet<FacturaModel> TabelaFacturaModel { get; set; }
        public DbSet<EncomendaModel> TabelaEncomendaModel { get; set; }
        public DbSet<VendaModel> TabelaVendaModel { get; set; }
}
