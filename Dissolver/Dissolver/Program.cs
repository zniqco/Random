using System;
using System.IO;
using System.Linq;

namespace Dissolver
{
    class Program
    {
        private static Random random = new Random();

        static void Main(string[] args)
        {
            foreach (var path in args)
            {
                if (Directory.Exists(path))
                {
                    var tempPath = path + "_" + GetRandomString(12);

                    while (Directory.Exists(tempPath))
                        tempPath = path + "_" + GetRandomString(12);

                    Directory.Move(path, tempPath);

                    var upperPath = Path.GetDirectoryName(tempPath);

                    foreach (var folder in Directory.EnumerateDirectories(tempPath))
                    {
                        var destination = Path.Combine(upperPath, Path.GetFileName(folder));

                        try
                        {
                            Directory.Move(folder, destination);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} -> {1} Failed: {2}", folder, destination, e.Message);
                        }
                    }

                    foreach (var file in Directory.EnumerateFiles(tempPath))
                    {
                        var destination = Path.Combine(upperPath, Path.GetFileName(file));

                        try
                        {
                            File.Move(file, destination);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} -> {1} Failed: {2}", file, destination, e.Message);
                        }
                    }

                    if (IsDirectoryEmpty(tempPath))
                        Directory.Delete(tempPath);
                }
            }
        }

        private static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private static string GetRandomString(int length)
        {
            const string characters = "abcdefghijklmnopqrstuvwxyz0123456789";

            var randomString = new char[length];

            for (int i = 0; i < length; ++i)
                randomString[i] = characters[random.Next(characters.Length)];

            return new string(randomString);
        }
    }
}
