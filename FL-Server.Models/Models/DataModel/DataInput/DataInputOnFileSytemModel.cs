using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FL_Server.Models.Models.DataModel
{
    public class DataInputModelOnFileSystemModel
    {


        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? FileType { get; set; }
        public Boolean? Status { get; set; }
        public string? Extension { get; set; }
        public string? Description { get; set; }
        public string? DataInput { get; set; }
        public string? DataInputPath { get; set; }
        public DateTime? LastUploadTime { get; set; }
        public DateTime? IntialUploadTime { get; set; }




        public int? DataDictionaryOnDatabaseModelId { get; set; }
        public virtual DataDictionaryOnDatabaseModel DataDictionaryOnDatabaseModel { get; set; }



        public int? DataInputModelOnDatabaseModelId { get; set; }
        public virtual DataInputModelOnDatabaseModel DataInputModelOnDatabaseModel { get; set; }



        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        
      


        
    }
}
