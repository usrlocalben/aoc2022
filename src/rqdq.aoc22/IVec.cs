using System.Numerics;

namespace rqdq.rmlv {

public readonly
struct IVec2 {

  public static
  IVec2 VMax(IVec2 a, IVec2 b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y));

  public readonly long x, y;

  public IVec2(long a) => (x, y) = (a, a);
  public IVec2(long a, long b) => (x, y) = (a, b);
  public IVec2(IVec2 other) : this(other.x, other.y) { }

  public override
  int GetHashCode() { return new { A=x, B=y }.GetHashCode(); }

  public override
  bool Equals(Object? other) => (other is IVec2 inst) ? inst == this : false;

  public static bool operator==(IVec2 lhs, IVec2 rhs) => (lhs.x, lhs.y) == (rhs.x, rhs.y);
  public static bool operator!=(IVec2 lhs, IVec2 rhs) => (lhs.x, lhs.y) != (rhs.x, rhs.y);

  public IVec2 Sign() { return new IVec2(Math.Sign(x), Math.Sign(y)); }

  public static IVec2 operator -(IVec2 a)              => new(-a.x, -a.y);
  public static IVec2 operator +(IVec2 lhs, IVec2 rhs) => new(lhs.x + rhs.x, lhs.y + rhs.y);
  public static IVec2 operator -(IVec2 lhs, IVec2 rhs) => new(lhs.x - rhs.x, lhs.y - rhs.y);
  public static IVec2 operator *(IVec2 lhs, IVec2 rhs) => new(lhs.x * rhs.x, lhs.y * rhs.y);
  public static IVec2 operator /(IVec2 lhs, IVec2 rhs) => new(lhs.x / rhs.x, lhs.y / rhs.y); }


public
struct IVec3 {
  public static
  IVec3 VMax(IVec3 a, IVec3 b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));

  public long x, y, z;

  public IVec3(long a) => (x, y, z) = (a, a, a);
  public IVec3(long a, long b, long c) => (x, y, z) = (a, b, c);
  public IVec3(IVec2 other, long z) : this(other.x, other.y, z) { }
  public IVec3(IVec3 other) : this(other.x, other.y, other.z) { }

  public override
  int GetHashCode() { return new { A=x, B=y, C=z }.GetHashCode(); }

  public override
  bool Equals(Object? other) => (other is IVec3 inst) ? inst == this : false;

  public static bool operator==(IVec3 l, IVec3 r) => (l.x, l.y, l.z) == (r.x, r.y, r.z);
  public static bool operator!=(IVec3 l, IVec3 r) => (l.x, l.y, l.z) != (r.x, r.y, r.z);

  public static IVec3 operator-(IVec3 rhs) => new(-rhs.x, -rhs.y, -rhs.z);
  public static IVec3 operator+(IVec3 lhs, IVec3 rhs) => new(lhs.x + rhs.x, lhs.x + rhs.y, lhs.z + rhs.z);
  public static IVec3 operator-(IVec3 lhs, IVec3 rhs) => new(lhs.x - rhs.x, lhs.x - rhs.y, lhs.z - rhs.z);
  public static IVec3 operator*(IVec3 lhs, IVec3 rhs) => new(lhs.x * rhs.x, lhs.x * rhs.y, lhs.z * rhs.z);
  public static IVec3 operator/(IVec3 lhs, IVec3 rhs) => new(lhs.x / rhs.x, lhs.x / rhs.y, lhs.z / rhs.z);

  public IVec2 XY() => new(x, y); }


}  // close package namespace
