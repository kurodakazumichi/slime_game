using System.IO;

public static class PathUtil
{
  public static bool IsDirectory(string path)
  {
    return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
  }
}