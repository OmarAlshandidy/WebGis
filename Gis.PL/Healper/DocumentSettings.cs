namespace Gis.PL.Healper
{
    public class DocumentSettings
    {
        //Upload 
        public static string UploadFile(IFormFile file,string folderName)
        { 
            // Get Folder Path 
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files\",folderName);

            // Get File Name And Make Unqiue
            var fileName = $"{Guid.NewGuid}{file.FileName}";
            // Get  File Path 
             string  filePath =  Path.Combine(folderPath,fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
            

        }

        public  static void DeleteFile(string  fileName,string folderName)
        {
            string filePath  = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files\", folderName ,fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }
    }  
}
