using ContactBookClientApp.Infrastructure;

namespace ContactBookClientApp.Implementation
{
    public class AddImageFileToPathService: IAddImageFileToPathService
    {
        public string AddImageFileToPath(IFormFile imageFile)

        {

            // Process the uploaded file(eq. save it to disk)

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", imageFile.FileName);

            // Save the file to storage and set path

            using (var stream = new FileStream(filePath, FileMode.Create))

            {

                imageFile.CopyTo(stream);

                return imageFile.FileName;
            }

        }
    }
}
