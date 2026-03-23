namespace LibApp.Models.ViewModels;

public class ReturnBookViewModel
{
    public Loan Loan { get; set; } = null!;
    public bool IsOverdue { get; set; }
    public int OverdueDays { get; set; }

    // Штраф за просрочку
    public decimal PricePerDay { get; set; }
    public decimal TotalFine => PricePerDay * OverdueDays;

    // Штраф за повреждение
    public bool IsDamaged { get; set; }
    public decimal DamageFineAmount { get; set; }
    public string? DamageComment { get; set; }
    public BookCondition NewCondition { get; set; } = BookCondition.Good;
}