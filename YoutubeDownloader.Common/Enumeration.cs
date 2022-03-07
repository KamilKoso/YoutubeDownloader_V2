using System.Collections.Generic;

namespace YoutubeDownloader.Common
{
    public class Enumeration<T>
    {
        protected static HashSet<int> AllowedValues;
        protected static Dictionary<int, string> Descriptions;

        public int Id { get; }

        public Enumeration(int id)
        {
            if (AllowedValues != null && (!AllowedValues.Contains(id) && id != 0))
            {
                throw new KeyNotFoundException($"Value '{id}' is incorrect for {typeof(T).Name}.");
            }

            Id = id;
        }

        protected bool Equals(Enumeration<T> other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Enumeration<T>)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Enumeration<T> left, Enumeration<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Enumeration<T> left, Enumeration<T> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Descriptions?[Id];
        }
    }
}
