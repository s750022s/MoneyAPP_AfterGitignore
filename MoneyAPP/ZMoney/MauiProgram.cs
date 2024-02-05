using Microsoft.Extensions.Logging;
using ZMoney.Services;

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
            builder.Services.AddSingleton<IDbServices>(s =>
            {
                var logger = s.GetService<LocalFileLogger>();
                return new SqliteServices(dbPath, logger);
            });

            //有用到的page都需要註冊
            builder.Services.AddSingleton<MainPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
