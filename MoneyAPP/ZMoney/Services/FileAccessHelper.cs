#if ANDROID
using Android.App;
using Android.Content;
#endif

using Microsoft.Maui.Controls.PlatformConfiguration;

namespace ZMoney.Services;

/// <summary>
/// 實做DB路徑管理系統，分為Android及windows兩種方式
/// </summary>

/// Android儲存空間分為三個區域：內部APP專屬目錄、外部APP專屬目錄、共享目錄，需要事先宣告檔案權限

/// 內部APP專屬目錄:APP只能讀寫自己區域內的檔案，無法移動到其他區域、不可見，也無法供其他APP或手段讀寫。
/// 路徑呼叫：FileSystem.AppDataDirectory(永久檔案)、FileSystem.CacheDirectory(快取)

/// 外部APP專屬目錄:APP的專屬空間，可見亦可移動。
/// 路徑呼叫：Android.App.Application.Context.GetExternalFilesDir

/// 共享目錄：包含Download等資料夾，操作需要額外的權限
/// 路徑呼叫：Android.App.Application.Context.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS);

///windows:C:\Users\s7500\AppData\Local\Packages\e23a7240-73c7-4e9a-a9c0-ef7336097c34_9zz4h110yvjzm\LocalState\
///路徑呼叫：FileSystem.AppDataDirectory(永久檔案)、FileSystem.CacheDirectory(快取)

public class FileAccessHelper
{
    public static string GetLocalFilePath(string filename)
    {
#if ANDROID
        // 在 Android 上使用 Android.App.Application.Context.GetExternalFilesDir
        var path = Android.App.Application.Context.GetExternalFilesDir(null)?.AbsolutePath;
        if (path == null) 
        {
            throw new Exception();
        }
        return System.IO.Path.Combine(path, filename);
#else
        return System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);
#endif
    }
}
