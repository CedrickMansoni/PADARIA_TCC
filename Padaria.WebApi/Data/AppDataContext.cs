using System;
using Microsoft.EntityFrameworkCore;

namespace Padaria.WebApi.Data;

public class AppDataContext(DbContextOptions<AppDataContext> options) : DbContext(options)
{

}
