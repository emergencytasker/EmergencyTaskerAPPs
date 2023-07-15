using System.Collections.Generic;

namespace EmergencyTask.Model
{
    public interface ISocketService
    {

        IEnumerable<PendingService> Action(int id, IEnumerable<PendingService> services);

    }
}