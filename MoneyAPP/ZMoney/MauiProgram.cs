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


            //依賴注入
            string dbPath = FileAccessHelper.GetLocalFilePath("ZMoney.db");
            builder.Services.AddSingleton<LocalFileLogger>();
            builder.Services.AddSingleton<IDbService>(s =>
            {
                var logger = s.GetService<LocalFileLogger>();
                if (logger == null)
                {
                    throw new Exception();
                }
                return new SqliteService(dbPath, logger);
            });

            builder.Services.AddSingleton<DbManager>();

            //有用到的page都需要註冊
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
