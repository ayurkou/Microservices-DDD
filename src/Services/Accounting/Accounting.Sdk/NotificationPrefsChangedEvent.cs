namespace Solera.Daytona.Services.Accounting.Sdk
{
    public class NotificationPrefsChangedEvent: DomainEvent
    {
        public string Title { get; set; }
    }
}