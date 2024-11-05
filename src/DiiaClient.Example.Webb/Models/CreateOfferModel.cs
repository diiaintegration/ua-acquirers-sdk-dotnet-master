using System.ComponentModel.DataAnnotations;

namespace DiiaClient.Example.Web.Models
{
    public class CreateOfferModel
    {
        [Required]
        public string Name { get; set; }
        public string ReturnLink { get; set; }
        public string Sharing { get; set; }
        public string DiiaId { get; set; }
        [Required]
        public string BranchId { get; set; }

    }
}
