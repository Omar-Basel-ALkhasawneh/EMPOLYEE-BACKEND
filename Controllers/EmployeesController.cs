using EmployeeApp.API.Data;
using EmployeeApp.API.Models;
using EmployeeApp.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(EmployeeDbContext db) : ControllerBase
{
    // GET /api/employees?search=it&department=HR&sortBy=hireDate&sortDir=desc&page=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<object>> GetAll(
        string? search, string? department,
        string sortBy = "hireDate", string sortDir = "desc",
        int page = 1, int pageSize = 50)
    {
        var q = db.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            q = q.Where(e =>
                e.FullName.ToLower().Contains(s) ||
                e.JobTitle.ToLower().Contains(s) ||
                e.Department.ToLower().Contains(s));
        }

        if (!string.IsNullOrWhiteSpace(department))
            q = q.Where(e => e.Department == department);

        // Sort (default HireDate desc)
        q = (sortBy.ToLower(), sortDir.ToLower()) switch
        {
            ("fullname", "asc") => q.OrderBy(e => e.FullName),
            ("fullname", _) => q.OrderByDescending(e => e.FullName),

            ("department", "asc") => q.OrderBy(e => e.Department),
            ("department", _) => q.OrderByDescending(e => e.Department),

            ("jobtitle", "asc") => q.OrderBy(e => e.JobTitle),
            ("jobtitle", _) => q.OrderByDescending(e => e.JobTitle),

            ("salary", "asc") => q.OrderBy(e => e.Salary),
            ("salary", _) => q.OrderByDescending(e => e.Salary),

            ("hiredate", "asc") => q.OrderBy(e => e.HireDate),
            _ => q.OrderByDescending(e => e.HireDate),
        };

        var total = await q.CountAsync();

        var items = await q.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Department = e.Department,
                JobTitle = e.JobTitle,
                Salary = e.Salary,
                HireDate = e.HireDate
            })
            .ToListAsync();

        return Ok(new { total, items });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var e = await db.Employees.FindAsync(id);
        if (e is null) return NotFound();

        return Ok(new EmployeeDto
        {
            Id = e.Id,
            FullName = e.FullName,
            Department = e.Department,
            JobTitle = e.JobTitle,
            Salary = e.Salary,
            HireDate = e.HireDate
        });
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(EmployeeCreateUpdateDto dto)
    {
        var e = new Employee
        {
            FullName = dto.FullName,
            Department = dto.Department,
            JobTitle = dto.JobTitle,
            Salary = dto.Salary,
            HireDate = dto.HireDate
        };
        db.Employees.Add(e);
        await db.SaveChangesAsync();

        var result = new EmployeeDto
        {
            Id = e.Id,
            FullName = e.FullName,
            Department = e.Department,
            JobTitle = e.JobTitle,
            Salary = e.Salary,
            HireDate = e.HireDate
        };

        return CreatedAtAction(nameof(GetById), new { id = e.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, EmployeeCreateUpdateDto dto)
    {
        var e = await db.Employees.FindAsync(id);
        if (e is null) return NotFound();

        e.FullName = dto.FullName;
        e.Department = dto.Department;
        e.JobTitle = dto.JobTitle;
        e.Salary = dto.Salary;
        e.HireDate = dto.HireDate;

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await db.Employees.FindAsync(id);
        if (e is null) return NotFound();
        db.Employees.Remove(e);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
