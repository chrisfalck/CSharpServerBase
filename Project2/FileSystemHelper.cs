using System;
using System.IO;
using System.Text;

namespace cfalck.FileSystem
{
    class FileSystemHelper
    {
        string basePath = "";

        public FileSystemHelper()
        {
            // Set the base path as the path to the project's public folder.
            // The base path ends in a directory separator character.
            this.basePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            this.basePath += Path.DirectorySeparatorChar + "public" + Path.DirectorySeparatorChar;
        }

        // Just to get some practice using delegates and lambdas.
        delegate byte[] StringAsBytes(string s);
        StringAsBytes strBytes = s => Encoding.UTF8.GetBytes(s);

        public byte[] GetFileAsBytes(string fileName)
        {
            try
            {
                return strBytes(GetFileAsString(fileName));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public string GetFileAsString(string fileName)
        {
            try
            {
                // Make sure the file the client asked for was public.
                if (fileName.Contains("../") || fileName.Contains("..\\"))
                {
                    throw new System.Security.SecurityException("Access to non public files is prohibited.");
                }
                else
                {
                    return File.ReadAllText(this.basePath + fileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                try
                {
                    // Return the index page if the client tried to access an illegal file.
                    return File.ReadAllText(this.basePath + "index.html");
                }
                catch (Exception e2)
                {
                    // In a last ditch effort, return default html if the index is unavailable.
                    Console.WriteLine(e2.ToString());
                    return "<!Doctype html><html><body>Error retrieving index</body></html>";
                }
            }
        }
    }
}
