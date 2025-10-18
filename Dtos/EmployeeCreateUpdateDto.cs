// File: Dtos/EmployeeCreateUpdateDto.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.API.Dtos;

public class EmployeeCreateUpdateDto
{
    [Required, StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string JobTitle { get; set; } = string.Empty;

    [Precision(10, 2)]
    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }

    [Required]
    public DateTime HireDate { get; set; }
}
