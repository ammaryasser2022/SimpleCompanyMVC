using System.Drawing.Drawing2D;

namespace Company.PL.wwwroot.Helpers
{
    public static class DocumentSettings
    {
        //Upload 
        public static string UploadFile(IFormFile file, string foldername) // el file elly will upload and folder name elly h5zn fehh hna f server 3la ymeneeen
        {
            //// 1 . Get Location of Folder (path)   wwwroot/files/images -----

            ////**  foldername will use it to complete the folder path in nex step ********** like galy images --> path+images


            //string folderpath = $"D:\\Route\\MVC\\Session_03\\Company Solution\\Company.PL\\wwwroot\\files\\{foldername}";

            ////Directory.GetCurrentDirectory(); // D:\\Route\\MVC\\Session_03\\Company Solution\\Company.PL   --> el part el dynamic 
            //string folderpath =  Directory.GetCurrentDirectory() + @"wwwroot\\files" + foldername;

            ////tare2a a7sn

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", foldername);

            // 2 . 
            //kda gbna path --> now name of image (uniqe)
            // hwa asln mb3oot (IFormFile file) but  i need to concat a guid with it to be unique
            // file name --> uniqe 

            string fileName = $"{Guid.NewGuid()} {file.FileName} ";


            // 3 . Path of file --> FolderPath + FileName  // to open 

            string filePath = Path.Combine(folderPath, fileName);


            // 4 . Save file in server -- Save As Stream

            using var fileStream = new FileStream(filePath, FileMode.Create );


            file.CopyTo( fileStream ); // for save 

            return fileName; //For DB

        }


        //Delete 
        public static void DeleteFile(string fileName , string foldername) 
        {

            //// 1 . 
            //string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", foldername); 

            //// 2 . --> file Name alredy exist 

            //// 3 . file path 

            //string filePath = Path.Combine(folderPath, fileName);



            ////**  U CAN MAKE THIS 2 STEPS IN ONE 

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", foldername , fileName );

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }




    }
}
