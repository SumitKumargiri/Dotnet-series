using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PhonePeTransaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid TransactionId { get; set; } = Guid.NewGuid();

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; } = Guid.NewGuid(); // Auto-generate user_id

    [Required]
    [Column("recipient_id")]
    public Guid RecipientId { get; set; } = Guid.NewGuid(); // Auto-generate recipient_id

    [Required]
    //[Column(TypeName = "numeric(10,2)")]
    [Column("amount")]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(20)]
    [Column("transaction_type")]
    public string TransactionType { get; set; } = "SEND"; // Default: SEND

    [Required]
    [StringLength(20)]
    [Column("status")]
    public string Status { get; set; } = "PENDING"; // Default: PENDING

    [Required]
    [StringLength(20)]
    [Column("payment_method")]
    public string PaymentMethod { get; set; } = "UPI"; // Default: UPI

    [Column("reference_id")]
    public Guid? ReferenceId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
