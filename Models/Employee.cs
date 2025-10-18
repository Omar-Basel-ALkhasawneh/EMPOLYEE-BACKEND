using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore; // for [Precision]

namespace EmployeeApp.API.Models;

public class Employee
{
    public int Id { get; set; } // PK (Identity) will be inferred by EF for int

    [Required, StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string JobTitle { get; set; } = string.Empty;

    // decimal(10,2)
    [Precision(10, 2)]
    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }

    public DateTime HireDate { get; set; }
}
