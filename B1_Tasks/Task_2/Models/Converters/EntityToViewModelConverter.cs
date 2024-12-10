using Task_2.Models.Database.Entities;
using Task_2.Models.ViewModels;

namespace Task_2.Models.Converters
{
    public static class EntityToViewModelConverter
    {
        public static IEnumerable<UploadedFile> ExcelFilesToUploadedFiles(IEnumerable<ExcelFile> excelFiles)
        {
            return excelFiles.Select(e => new UploadedFile()
            {
                Id = e.Id,
                Name = e.Name,
                DataTime = e.DataTime,
                Uploaded = e.Uploaded,
            }).ToList();
        }
    }
}
