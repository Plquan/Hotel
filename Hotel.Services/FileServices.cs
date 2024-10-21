using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Services
{

    public interface IFileServices {
        string Upload(string file);
        void Delete(string file);
    }

    public class FileServices : IFileServices
    {
        public  string Upload(string base64Image)
        {
            try
            {       
                byte[] imageBytes = Convert.FromBase64String(base64Image);

               
                var fileName = $"{Guid.NewGuid()}.jpeg";
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                var filePath =  Path.Combine(imagesFolder, fileName);


                System.IO.File.WriteAllBytes(filePath, imageBytes);

                return fileName;
            }
            catch 
            {
                return base64Image;
            }

        }



        public void Delete(string fileName)
        {
            try
            {              
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                var filePath = Path.Combine(imagesFolder, fileName);

                
                if (System.IO.File.Exists(filePath))
                {               
                    System.IO.File.Delete(filePath);
                    
                }
                else
                {
                    throw new FileNotFoundException("File not found.");
                }
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý lỗi nếu cần
                throw new Exception($"Error while deleting image: {ex.Message}");
            }
        }

    }
}
