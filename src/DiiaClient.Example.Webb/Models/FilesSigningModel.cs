namespace DiiaClient.Example.Web.Models
{
    public class FilesSigningModel
    {
        public string BranchId { get; set; }
        public string OfferId { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
