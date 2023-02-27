namespace MedlabApi.Helpers
{
    public class FileManager
    {


        public static string Save(IFormFile file, string rootPath, string folder, int maxSize)
        {
            string fileName = file.FileName;

            string newFileName = Guid.NewGuid().ToString() + (fileName.Length > (maxSize - 36) ? fileName.Substring(file.FileName.Length - (file.FileName.Length - maxSize) - 36) : fileName);

            string myPath = Path.Combine(rootPath, folder, newFileName);

            using (FileStream fs = new FileStream(myPath, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            return newFileName;
        }

        public static void Delete(string rootPath, string folder, string fileName)
        {
            string myPath = Path.Combine(rootPath, folder, fileName);

            if (File.Exists(myPath))
            {
                File.Delete(myPath);
            }
        }
    }
}
