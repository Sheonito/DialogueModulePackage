using System.IO;

namespace Lucecita.StorylineEngine
{
    public static class PathUtil
    {
        public static string Combine(string path1, string path2)
        {
            if (string.IsNullOrEmpty(path1) || string.IsNullOrEmpty(path2))
                return null;
            
            string path = Path.Combine(path1, path2); 
            path = path.Replace("\\","/");

            return path;
        }
    }
   
}