#if ANDROID
using Android.App;
using Android.Content;
#endif

namespace MoneyAPP;

public class FileAccessHelper
{
    public static string GetLocalFilePath(string filename)
    {
        #if ANDROID
        {
            // 在 Android 上使用 Android.App.Application.Context.GetExternalFilesDir
            return System.IO.Path.Combine(Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath, filename);
        }
        #endif
        {
            return System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);
        }
    }
}
