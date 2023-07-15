using System.Collections.Generic;
using System.Linq;

namespace EmergencyTask.Model
{
    public class ServiceListModelEqualityComparer : EqualityComparer<ServiceListModel>
    {
        public override bool Equals(ServiceListModel x, ServiceListModel y)
        {
            if(x != null && y != null)
            {
                return x.SequenceEqual(y, new CartaModelEqualityComparer());
            }
            return false;
        }

        public override int GetHashCode(ServiceListModel obj)
        {
            return obj.GetHashCode();
        }
    }
}