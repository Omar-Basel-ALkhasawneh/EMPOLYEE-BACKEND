using EmployeeApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EmployeeApp.API.Data;

public class EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
}
