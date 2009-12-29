using System;

namespace Splits.Web
{
  public class Identifier : IEquatable<Identifier>
  {
    public string Name { get; private set; }
    public Type Type { get; private set; }

    public Identifier(string name, Type type)
    {
      Name = name;
      Type = type;
    }

    public bool Equals(Identifier other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(other.Name, Name) && Equals(other.Type, Type);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof(Identifier)) return false;
      return Equals((Identifier)obj);
    }

    public override Int32 GetHashCode()
    {
      return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Type != null ? Type.GetHashCode() : 0);
    }

    public static bool operator ==(Identifier left, Identifier right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(Identifier left, Identifier right)
    {
      return !Equals(left, right);
    }
  }
}