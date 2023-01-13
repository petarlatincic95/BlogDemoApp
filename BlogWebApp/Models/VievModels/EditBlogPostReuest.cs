using System.ComponentModel.DataAnnotations;

namespace BlogWebApp.Models.Domain.VievModels
{
    public class EditBlogPostReuest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Heading { get; set; }

        [Required]
        public string PageTitle { get; set; }


        [Required]
        public string Content { get; set; }


        [Required]
        public string ShortDescription { get; set; }



        public string FeaturedImageUrl { get; set; }


        [Required]
        public string UrlHandle { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PublishedDate { get; set; }


        [Required]
        public string Author { get; set; }


        [Required]
        public bool Visible { get; set; }

       

       
    }
}
