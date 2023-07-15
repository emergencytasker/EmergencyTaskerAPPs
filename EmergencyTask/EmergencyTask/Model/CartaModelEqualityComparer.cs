using System.Collections.Generic;

namespace EmergencyTask.Model
{
    public class CartaModelEqualityComparer : EqualityComparer<CartaModel>
    {
        public override bool Equals(CartaModel x, CartaModel y)
        {
            if(x != null && y != null)
            {
                return x.Id == y.Id;
            }
            return false;
        }

        public override int GetHashCode(CartaModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
