using System.Reflection;

namespace TickerAlert.Domain.Common
{
    public abstract class ValueObject
    {
        private IEnumerable<object> GetProperties()
        {
            return GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .Select(p => p.GetValue(this));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;
            return GetProperties().SequenceEqual(other.GetProperties());
        }

        public override int GetHashCode()
        {
            return GetProperties()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }
    }
}
