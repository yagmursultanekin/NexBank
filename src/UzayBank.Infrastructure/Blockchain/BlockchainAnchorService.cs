using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using UzayBank.Application.Interfaces;

namespace UzayBank.Infrastructure.Blockchain;

/// <summary>
/// IntegrityAnchor kontratıyla konuşan servis.
///
/// Nethereum, C# sınıflarını kontrat fonksiyonlarına eşliyor:
/// aşağıdaki [Function] ve [Parameter] öznitelikleri, Solidity'deki
/// fonksiyon imzalarının birebir karşılığı olmak zorunda — aksi hâlde
/// çağrı kontratta karşılık bulmaz.
/// </summary>
public class BlockchainAnchorService : IBlockchainAnchorService
{
    private readonly BlockchainOptions _options;
    private readonly ILogger<BlockchainAnchorService> _logger;
    private readonly Web3 _web3;

    public BlockchainAnchorService(
         IOptions<BlockchainOptions> options,
         ILogger<BlockchainAnchorService> logger)
    {
        _options = options.Value;
        _logger = logger;

        // Hesap, private key'den türetiliyor. Bu hesap işlemleri imzalayacak.
        // Kontratın sahibi bu adres olmalı, aksi hâlde yazma reddedilir.
        var account = new Account(_options.PrivateKey);

        // Web3, Ethereum düğümüyle konuşan ana nesne.
        _web3 = new Web3(account, _options.RpcUrl);
    }

    public async Task<string> AnchorAsync(int accountId, string hash)
    {
        // Hash veritabanında hex STRING olarak duruyor ("D46DFEB4...").
        // Kontrat ise bytes32 bekliyor — yani 32 baytlık ham veri.
        // Dönüşümü burada yapıyoruz.
        var hashBytes = Convert.FromHexString(hash);

        if (hashBytes.Length != 32)
            throw new ArgumentException(
                $"Hash 32 bayt olmalı, {hashBytes.Length} bayt geldi.", nameof(hash));

        var handler = _web3.Eth.GetContractTransactionHandler<AnchorFunction>();

        var function = new AnchorFunction
        {
            AccountId = accountId,
            Hash = hashBytes
        };

        // SendRequestAndWaitForReceiptAsync: işlemi gönderir ve blok onayını bekler.
        //
        // Bu bekleme, isteği yavaşlatan kısım. Anvil'de anında onaylanıyor,
        // gerçek bir ağda saniyeler sürerdi. Bu yüzden ileride bu çağrının
        // istek yolundan çıkarılıp arka plana taşınması gerekebilir.
        var receipt = await handler.SendRequestAndWaitForReceiptAsync(
            _options.ContractAddress, function);

        _logger.LogInformation(
            "Hesap {AccountId} zincire sabitlendi. İşlem: {TxHash}, Blok: {Block}",
            accountId, receipt.TransactionHash, receipt.BlockNumber.Value);

        return receipt.TransactionHash;
    }

    public async Task<AnchorRecord?> GetAnchorAsync(int accountId)
    {
        var handler = _web3.Eth.GetContractQueryHandler<GetAnchorFunction>();

        // QueryAsync: yalnızca okuma yapar (Solidity'deki "view" fonksiyonu).
        // Zincire bir şey yazmadığı için ücretsiz ve anında sonuç döner.
        var result = await handler.QueryDeserializingToObjectAsync<GetAnchorOutput>(
            new GetAnchorFunction { AccountId = accountId },
            _options.ContractAddress);

        if (!result.Exists)
            return null;

        // Kontrat zamanı Unix saniye olarak tutuyor (block.timestamp).
        var anchoredAt = DateTimeOffset
            .FromUnixTimeSeconds((long)result.Timestamp)
            .UtcDateTime;

        return new AnchorRecord(Convert.ToHexString(result.Hash), anchoredAt);
    }

    // --- Kontrat fonksiyon tanımları ---
    //
    // Bu sınıflar Solidity'deki fonksiyonların C# karşılığı.
    // İsimler ve parametre sıraları kontratla BİREBİR aynı olmalı.

    /// <summary>Solidity: anchor(uint256 accountId, bytes32 hash)</summary>
    [Function("anchor")]
    private class AnchorFunction : FunctionMessage
    {
        [Parameter("uint256", "accountId", 1)]
        public int AccountId { get; set; }

        [Parameter("bytes32", "hash", 2)]
        public byte[] Hash { get; set; } = Array.Empty<byte>();
    }

    /// <summary>Solidity: getAnchor(uint256 accountId)</summary>
    [Function("getAnchor", typeof(GetAnchorOutput))]
    private class GetAnchorFunction : FunctionMessage
    {
        [Parameter("uint256", "accountId", 1)]
        public int AccountId { get; set; }
    }

    /// <summary>
    /// getAnchor'ın dönüş değerleri.
    /// Sıra numaraları (1, 2, 3) Solidity'deki return sırasıyla aynı olmalı.
    /// </summary>
    [FunctionOutput]
    private class GetAnchorOutput : IFunctionOutputDTO
    {
        [Parameter("bytes32", "hash", 1)]
        public byte[] Hash { get; set; } = Array.Empty<byte>();

        [Parameter("uint256", "timestamp", 2)]
        public System.Numerics.BigInteger Timestamp { get; set; }

        [Parameter("bool", "exists", 3)]
        public bool Exists { get; set; }
    }
}