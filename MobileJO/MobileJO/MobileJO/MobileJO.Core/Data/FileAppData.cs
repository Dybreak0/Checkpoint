using PCLStorage;
using System.IO;
using System.Threading.Tasks;

namespace MobileJO.Core.Data
{
    public static class FileAppData
    {
    
        public async static Task<IFolder> CreateFolder(this string folderName, string folderPath)
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            IFolder folder = await rootFolder.CreateFolderAsync(folderPath, CreationCollisionOption.OpenIfExists);

            folder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.ReplaceExisting);
            return folder;
        }

        public async static Task<IFile> CreateFile(this string filename, IFolder rootFolder = null)
        {
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;
            IFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            return file;
        }

        public async static Task<string> SaveFile(this byte[] fileByteArray, string fileName, string folderPath)
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            IFolder folder = await rootFolder.CreateFolderAsync(folderPath, CreationCollisionOption.OpenIfExists);

            // create a file, overwriting any existing file  
            IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            // populate the file with image data  
            using (Stream stream = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
            {
                stream.Write(fileByteArray, 0, fileByteArray.Length);
            }

            return file.Path;
        }

        public async static Task<byte[]> LoadFile(string fileName, string folderPath)
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            var folderExists = await IsFolderExistAsync(folderPath);

            if (folderExists)
            {
                IFolder targetFolder = await rootFolder.GetFolderAsync(folderPath);

                IFile file = await targetFolder.GetFileAsync(fileName);

                using (Stream stream = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                {
                    long length = stream.Length;

                    byte[] streamBuffer = new byte[length];

                    stream.Read(streamBuffer, 0, (int)length);

                    return streamBuffer;
                }
            }
            else
                return null;
        }

        public async static Task<bool> IsFolderExistAsync(this string folderPath)
        {
            // get hold of the file system  
            IFolder folder = FileSystem.Current.LocalStorage;

            ExistenceCheckResult folderexist = await folder.CheckExistsAsync(folderPath);

            // already run at least once, don't overwrite what's there  
            if (folderexist == ExistenceCheckResult.FolderExists)
            {
                return true;

            }
            return false;
        }

        public async static Task<bool> IsFileExistAsync(this string fileName, string folderPath)
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            // get hold of the file system 
            var folderExists = await IsFolderExistAsync(folderPath);

            if (folderExists)
            {
                IFolder folder = await rootFolder.GetFolderAsync(folderPath);

                ExistenceCheckResult fileExist = await folder.CheckExistsAsync(fileName);

                // already run at least once, don't overwrite what's there  
                if (fileExist == ExistenceCheckResult.FileExists)
                {
                    return true;
                }
            }            
            return false;
        }

        public async static Task<bool> DeleteFile(this string fileName, string folderPath)
        {
            IFolder folder = FileSystem.Current.LocalStorage;

            IFolder targetFolder = await folder.GetFolderAsync(folderPath);

            bool exist = await IsFileExistAsync(fileName, folderPath);

            if (exist == true)
            {
                IFile file = await targetFolder.GetFileAsync(fileName);
                await file.DeleteAsync();
                return true;
            }
            return false;
        }

        public async static Task<bool> WriteTextAllAsync(this string filename, string content = "", IFolder rootFolder = null)
        {
            IFile file = await filename.CreateFile(rootFolder);
            await file.WriteAllTextAsync(content);
            return true;
        }

        public async static Task DeleteAllFiles()
        {
            IFolder folder = FileSystem.Current.LocalStorage;

            var folders = await folder.GetFoldersAsync();

            foreach(var fileFolder in folders)
            {
                await fileFolder.DeleteAsync();
            }
        }

    }
}
