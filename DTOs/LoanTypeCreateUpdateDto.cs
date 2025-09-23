// // File: FintcsApi/DTOs/LoanTypeCreateUpdateDto.cs
// using System.ComponentModel.DataAnnotations;

// namespace FintcsApi.DTOs;

// public class LoanTypeCreateUpdateDto
// {
//     public Guid? LoanTypeId { get; set; }
    
//     [Required]
//     [StringLength(255)]
//     public string Name { get; set; } = string.Empty;
    
//     [Required]
//     [Range(0, 100)]
//     public decimal InterestPercent { get; set; }
    
//     [Required]
//     [Range(0, double.MaxValue)]
//     public decimal LimitAmount { get; set; }
    
//     [Required]
//     [Range(0, double.MaxValue)]
//     public decimal CompulsoryDeposit { get; set; }
    
//     [Required]
//     [Range(0, double.MaxValue)]
//     public decimal OptionalDeposit { get; set; }
    
//     [Required]
//     [Range(0, double.MaxValue)]
//     public decimal ShareAmount { get; set; }
    
//     [Required]
//     [Range(1, int.MaxValue)]
//     public int XTimes { get; set; }
// }
// File: FintcsApi/DTOs/LoanTypeCreateUpdateDto.cs
using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs;

public class LoanTypeCreateUpdateDto
{
    public Guid? LoanTypeId { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, 100)]
    public decimal InterestPercent { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal LimitAmount { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal CompulsoryDeposit { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal OptionalDeposit { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ShareAmount { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int XTimes { get; set; }
}
