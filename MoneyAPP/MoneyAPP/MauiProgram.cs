using Microsoft.Extensions.Logging;
using MoneyAPP.Services;

namespace MoneyAPP;

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
			})
             .ConfigureEssentials(essentials =>
             {
                 essentials.UseVersionTracking();
             });

        string dbPath = FileAccessHelper.GetLocalFilePath("record.db3");
        builder.Services.AddSingleton<SqliteService>(s => ActivatorUtilities.CreateInstance<SqliteService>(s, dbPath));

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
