namespace Medrio.Caching.DataChangeNotification.Notifiers
{
    public interface INotifierFactory
    {
        IDataChangeNotifier[] GetNotifiers(InvalidationDestinations destinations);
    }
}
