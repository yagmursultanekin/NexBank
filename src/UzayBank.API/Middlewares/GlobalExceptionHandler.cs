using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UzayBank.API.Middlewares;

/// <summary>
/// Yakalanmayan tüm hataları tek noktada ele alır.
///
/// NEDEN GEREKLİ:
/// Bu handler olmadan, beklenmedik bir hata olduğunda ASP.NET Core'un
/// varsayılan davranışı devreye girer — Development'ta stack trace istemciye
/// gider (bilgi sızıntısı), Production'da içi boş bir 500 döner (hata kaydı yok).
/// İkisi de istenmeyen durum.
///
/// Burada hem hatayı logluyoruz (sunucu tarafında tam detay) hem de istemciye
/// makine-okunur, ama içerik sızdırmayan bir cevap dönüyoruz.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Hatanın tamamı (stack trace dahil) sunucu loguna yazılır.
        // İstemciye bu detaylar ASLA gitmez.
        _logger.LogError(
            exception,
            "Beklenmeyen hata. Yol: {Path}, Metot: {Method}",
            httpContext.Request.Path,
            httpContext.Request.Method);

        // İstemciye standart bir hata gövdesi dönüyoruz.
        // ProblemDetails, RFC 9457 ile tanımlı standart bir formattır;
        // frontend'in hataları tutarlı şekilde işlemesini sağlar.
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Sunucu hatası",
            // Hata mesajını değil, sabit bir kod döndürüyoruz.
            // Frontend bu kodu kendi dilinde çeviriyor — mevcut i18n
            // yaklaşımıyla (backend kod döndürür, istemci çevirir) uyumlu.
            Detail = "INTERNAL_ERROR"
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // true = "hatayı ben hallettim, başka handler'a gerek yok"
        return true;
    }
}