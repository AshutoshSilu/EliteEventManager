namespace EliteEvents.Domain.Enums;

public enum NotificationType
{
    Info = 1,
    Success = 2,
    Warning = 3,
    Error = 4,
    Reminder = 5
}

public enum NotificationChannel
{
    InApp = 1,
    Email = 2,
    SMS = 3,
    WhatsApp = 4
}
