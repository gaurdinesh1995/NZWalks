using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3,ErrorMessage = "Code has to be minimum 3 char long.")]
        [MaxLength(6,ErrorMessage ="Code can be Max 6 char long")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage ="Name can not exceed 100 character")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
