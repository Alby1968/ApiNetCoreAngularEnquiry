using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCoreAngularEnquiry.Model
{
    [Table("Enquiry")]
    public class EnquiryModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int enquiryId { get; set; }
        public int enquiryTypeId { get; set; }
        public int enquiryStatusId { get; set; }
        [Required]
        public string customerName { get; set; } = string.Empty;
        [Required]
        public string mobileNo     { get; set; } = string.Empty;
        [EmailAddress]
        public string email        { get; set; } = string.Empty;
        [Required]
        public string message      { get; set; } = string.Empty;
        [Required]
        public DateTime createdDate { get; set; } 
        public string resolution { get; set; } = string.Empty;
    }
}

