namespace UzayBank.Domain.Entities;

/// <summary>
/// Hangi kullanıcının hangi VakıfBank hesabına erişebileceğini tutar.
///
/// Sandbox tek kurumsal kimlik verdiği için bu eşleme uygulama tarafında kurulur.
/// Gerçek açık bankacılıkta bu bilgi rıza (consent) ile bankadan gelir.
///
/// Bakiye burada TUTULMAZ — her istekte VakıfBank'tan taze çekilir,
/// çünkü kopyalanan bakiye anında bayatlar.
/// </summary>
public class UserAccount
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    /// <summary>VakıfBank'ın gerçek hesap kimliği — liste sırasından üretilen sahte Id değil.</summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>Değişmeyen bilgiler — tutmak güvenli.</summary>
    public string IBAN { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;

    public DateTime LinkedAt { get; set; } = DateTime.UtcNow;
}