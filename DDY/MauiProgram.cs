using DDY.Services;
using Microsoft.Extensions.Logging;

namespace DDY
{
    public static class MauiProgram
    {
        // Backend OData propio (DDY.Api). El emulador de Android ve el host como 10.0.2.2.
        // Para un dispositivo físico: cambiar por la IP LAN del equipo y arrancar la API con --urls http://0.0.0.0:5080
        public static string ApiBaseUrl =>
            DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5080/"
                : "http://localhost:5080/";

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

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // Servicio tipado: única fuente de datos (API OData propia)
            builder.Services.AddHttpClient<PokemonApiService>(client =>
            {
                client.BaseAddress = new Uri(ApiBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            builder.Services.AddSingleton<FavoritosService>();

            return builder.Build();
        }
    }
}
