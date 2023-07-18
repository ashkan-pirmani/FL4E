using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FL_Server.Models.Models
{
    public abstract class FileModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? FileType { get; set; }
        public string? Extension { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedOn { get; set; }



        public int? StudyCatalogueId { get; set; }
        public virtual StudyCatalogue StudyCatalogue { get; set; }



        public int? ModelCatalogueId { get; set; }
        public virtual ModelCatalogue ModelCatalogue { get; set; }




        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }



    }
}
