namespace UzayBank.Infrastructure.Blockchain;

/// <summary>
/// Blockchain bağlantı ayarları.
///
/// Değerler user-secrets'tan okunuyor — özellikle PrivateKey asla
/// appsettings.json'a veya koda yazılmamalı, o dosyalar repoya gidiyor.
/// </summary>
public class BlockchainOptions
{
    /// <summary>appsettings/secrets içindeki bölüm adı.</summary>
    public const string SectionName = "Blockchain";

    /// <summary>
    /// Ethereum düğümünün adresi.
    /// Geliştirmede Anvil (http://127.0.0.1:8545).
    /// </summary>
    public string RpcUrl { get; set; } = string.Empty;

    /// <summary>
    /// IntegrityAnchor kontratının blockchain üzerindeki adresi.
    /// Deploy sırasında belirlenir.
    /// </summary>
    public string ContractAddress { get; set; } = string.Empty;

    /// <summary>
    /// İşlemleri imzalayan hesabın özel anahtarı.
    ///
    /// Bu hesap kontratın sahibi olmalı — aksi hâlde kontrat "yalnızca sahip
    /// yazabilir" diyerek reddeder.
    /// </summary>
    public string PrivateKey { get; set; } = string.Empty;
}