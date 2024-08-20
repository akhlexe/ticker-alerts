using System.ComponentModel.DataAnnotations;

namespace TickerAlert.Domain.Enums;

public enum AlertState
{
    [Display(Name = "Pending")]
    PENDING = 0,
    
    [Display(Name = "Triggered")]
    TRIGGERED = 1,
    
    [Display(Name = "Notified")]
    NOTIFIED = 2,

    [Display(Name = "Received")]
    RECEIVED = 3,

    [Display(Name = "Canceled")]
    CANCELED = 4
}