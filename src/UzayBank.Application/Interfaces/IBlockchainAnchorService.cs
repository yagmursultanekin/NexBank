namespace UzayBank.Application.Interfaces;

/// <summary>
/// İşlem zincirinin son hash'ini blockchain'e sabitler ve okur.
///
/// Application katmanı yalnızca bu arayüzü tanır; Nethereum, RPC, private key
/// gibi ayrıntılar Infrastructure'da kalır. Böylece blockchain sağlayıcısı
/// değişse bile iş mantığı etkilenmez.
/// </summary>
public interface IBlockchainAnchorService
{
    /// <summary>
    /// Bir hesabın son hash'ini zincire yazar.
    /// İşlem onaylanana kadar bekler ve işlem hash'ini döndürür.
    /// </summary>
    Task<string> AnchorAsync(int accountId, string hash);

    /// <summary>
    /// Bir hesabın zincire sabitlenmiş hash'ini okur.
    /// Kayıt yoksa null döner.
    /// </summary>
    Task<AnchorRecord?> GetAnchorAsync(int accountId);
}

/// <summary>
/// Blockchain'den okunan sabitleme kaydı.
/// </summary>
public record AnchorRecord(string Hash, DateTime AnchoredAt);