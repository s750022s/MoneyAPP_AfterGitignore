using Microsoft.Extensions.Logging;
using ZMoney.Services;
using ZMoney.Pages;

namespace ZMoney
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            ///資料庫路徑
            string dbPath = FileAccessHelper.GetLocalFilePath("ZMoney.db");

            //===依賴注入===

            //logger生成器
            builder.Services.AddSingleton<LocalFileLogger>();

            //Sqlite串接Service
            builder.Services.AddSingleton<IDbService>();

            //DB管理員；所有page都是透過DbManager存取資料庫。
            builder.Services.AddSingleton<DbManager>();

            //有用到DbManager的page都需要註冊。
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<RecordAddPage>();
            builder.Services.AddSingleton<RecordUpdatePage>();
            builder.Services.AddSingleton<BackupAndReset>();
            builder.Services.AddSingleton<ZmoneyInfo>();
            builder.Services.AddSingleton<ListSetting>();
            builder.Services.AddSingleton<StatisticsPage>();
            builder.Services.AddSingleton<RecordsByClass>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
