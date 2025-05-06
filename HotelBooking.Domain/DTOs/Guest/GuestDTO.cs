using HotelBooking.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Domain.DTOs.Guest
{
    public class GuestDTO
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string? FullName { get; set; }

        [StringLength(20)]
        public string? IdentityNumber { get; set; }

        public DateTime? IdentityIssueDate { get; set; }

        [StringLength(100)]
        public string? IdentityIssuePlace { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? Nationality { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(50)]
        public string? Province { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }
    }

    public class CreateGuestDTO
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(20)]
        public string IdentityNumber { get; set; }

        [Required]
        public DateTime IdentityIssueDate { get; set; }

        [Required]
        [StringLength(100)]
        public string IdentityIssuePlace { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string Nationality { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Province { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }
    }
} 