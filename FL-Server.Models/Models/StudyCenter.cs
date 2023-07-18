using FL_Server.Models.Models.DataModel.DataCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FL_Server.Models.Models
{
    public class StudyCenter
    {


        [Key]
        public int Id { get; set; }
        [Required]

        public string? Title { get; set; }


        public int StudyCatalogueId { get; set; }
        [ForeignKey("StudyCatalogueId")]
        [ValidateNever]
        public StudyCatalogue StudyCatalogue { get; set; }


        [Required]
        public bool StudyType { get; set; }


        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public int? FileModelId { get; set; }
        public virtual FileModel FileModel { get; set; }


        [Required]
        public DateTime LastUpdate { get; set; }


        public int? DataCoreFileOnDatabaseModelId { get; set; }
        public virtual DataCoreFileOnDatabaseModel DataCoreFileOnDatabaseModel { get; set; }



    }
}
