using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FL_Server.Models.Models.DataModel
{
    public class DataCleanFunctionOnDatabaseModel 
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? FileType { get; set; }
        public Boolean? Status { get; set; }
        public string? Extension { get; set; }
        public string? Description { get; set; }
        public byte[]? DataCleanFunction { get; set; }
        public DateTime? LastUploadTime { get; set; }
        public DateTime? IntialUploadTime { get; set; }





        public int? DataDictionaryOnDatabaseModelId { get; set; }
        public virtual DataDictionaryOnDatabaseModel DataDictionaryOnDatabaseModel { get; set; }




    }
}
