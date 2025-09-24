// File: DTOs/MemberDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace FintcsApi.DTOs
{
    public class MemberCreateUpdateDto
    {
        // Required
        [Required] public string Name { get; set; } = null!;
        [Required] public string FHName { get; set; } = null!;
        [Required] public string Mobile { get; set; } = null!;
        [Required] [EmailAddress] public string Email { get; set; } = null!;
        public string Status { get; set; } = "Active";

        // Optional
        public string OfficeAddress { get; set; } = "";
        public string City { get; set; } = "";
        public string PhoneOffice { get; set; } = "";
        public string Branch { get; set; } = "";
        public string PhoneRes { get; set; } = "";
        public string Designation { get; set; } = "";
        public string ResidenceAddress { get; set; } = "";
        public DateTime? DOB { get; set; }
        public DateTime? DOJSociety { get; set; }
        public DateTime? DOR { get; set; }
        public string Nominee { get; set; } = "";
        public string NomineeRelation { get; set; } = "";
        public string cdAmount { get; set; } = "0";
        public string Email2 { get; set; } = "";
        public string Mobile2 { get; set; } = "";
        public string Pincode { get; set; } = "";
        public string BankName { get; set; } = "";
        public string AccountNumber { get; set; } = "";
        public string PayableAt { get; set; } = "";
        public string? Share { get; set; }
    }

    public class MemberDto : MemberCreateUpdateDto
    {
        public Guid Id { get; set; }
        public Guid SocietyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
