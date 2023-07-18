namespace FL_Server.Models.Models.Upload
{
    public class FileUploadViewModel
    {
        public List<FileOnFileSystemModel> FilesOnFileSystem { get; set; }
        public FileOnFileSystemModel filesOnFileSystem { get; set; }
        public List<FileOnDatabaseModel> FilesOnDatabase { get; set; }
        public FileOnDatabaseModel filesOnDatabase { get; set; }
        public List<ApplicationUser> ApplicationUser { get; set; }
        public List<StudyCenter> StudyCenter { get; set; }
        public StudyCenter studyCenter { get; set; }

        public StudyCatalogue studyCatalogue { get; set; }
        public List<StudyCatalogue> studyCatalogueList { get; set; }
        public ModelCatalogue modelCatalogue { get; set; }
        public List<ModelCatalogue> modelCatalogueList { get; set; }



    }
}
