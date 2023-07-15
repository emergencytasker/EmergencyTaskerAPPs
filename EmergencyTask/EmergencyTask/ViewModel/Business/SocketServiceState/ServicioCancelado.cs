using EmergencyTask.Model;
using System.Collections.Generic;
using System.Linq;

namespace EmergencyTask.ViewModel.Business.SocketServiceState
{
    public class ServicioCancelado : ISocketService
    {
        public IEnumerable<PendingService> Action(int id, IEnumerable<PendingService> services)
        {
            var service = services.FirstOrDefault(s => s.Id == id);
            if (service == null) return services;
            var list = new List<PendingService>();
            foreach (var item in services)
            {
                if (item.Id == id) continue;
                list.Add(item);
            }
            return list;
        }
    }
}