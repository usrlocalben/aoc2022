namespace rqdq.aoc22;

public readonly
struct IVec2 {

  public static IVec2 VMax(IVec2 a, IVec2 b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
  public static IVec2 VMin(IVec2 a, IVec2 b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y));

  public readonly long x, y;

  public IVec2(long a) => (x, y) = (a, a);
  public IVec2(long a, long b) => (x, y) = (a, b);
  public IVec2(IVec2 other) : this(other.x, other.y) { }

  public override int GetHashCode() => (x, y).GetHashCode();

  public override
  bool Equals(object? other) => (other is IVec2 inst) && inst == this;

  public override
  string ToString() => $"({x},{y})";

  public static bool operator==(IVec2 lhs, IVec2 rhs) => (lhs.x, lhs.y) == (rhs.x, rhs.y);
  public static bool operator!=(IVec2 lhs, IVec2 rhs) => (lhs.x, lhs.y) != (rhs.x, rhs.y);
  public static bool operator<(IVec2 lhs, IVec2 rhs) => lhs.x<rhs.x && lhs.y<rhs.y;
  public static bool operator>(IVec2 lhs, IVec2 rhs) => lhs.x>rhs.x && lhs.y>rhs.y;
  public static bool operator<=(IVec2 lhs, IVec2 rhs) => lhs.x<=rhs.x && lhs.y<=rhs.y;
  public static bool operator>=(IVec2 lhs, IVec2 rhs) => lhs.x>=rhs.x && lhs.y>=rhs.y;

  public IVec2 Sign() { return new IVec2(Math.Sign(x), Math.Sign(y)); }
  public IVec2 Abs() { return new IVec2(Math.Abs(x), Math.Abs(y)); }
  public long HMax() { return Math.Max(x, y); }
  public long HMin() { return Math.Min(x, y); }
  public IVec2 Min(IVec2 a) => new(Math.Min(a.x,x), Math.Min(a.y,y));
  public IVec2 Max(IVec2 a) => new(Math.Max(a.x,x), Math.Max(a.y,y));
  public long MDist(IVec2 a) => Math.Abs(a.x-x)+Math.Abs(a.y-y);
  public long Area() => x * y;

  private static
  long Mod(long a, long b) {
    var tmp = a % b;
    return tmp < 0 ? tmp + b : tmp; }

  public IVec2 Mod(IVec2 d) => new(Mod(x,d.x), Mod(y,d.y));
  public IVec2 Mod(long d) => new(Mod(x,d), Mod(y,d));

  public static IVec2 operator -(IVec2 a)              => new(-a.x, -a.y);
  public static IVec2 operator +(IVec2 lhs, IVec2 rhs) => new(lhs.x + rhs.x, lhs.y + rhs.y);
  public static IVec2 operator -(IVec2 lhs, IVec2 rhs) => new(lhs.x - rhs.x, lhs.y - rhs.y);
  public static IVec2 operator *(IVec2 lhs, IVec2 rhs) => new(lhs.x * rhs.x, lhs.y * rhs.y);
  public static IVec2 operator /(IVec2 lhs, IVec2 rhs) => new(lhs.x / rhs.x, lhs.y / rhs.y);
  public static IVec2 operator +(IVec2 lhs, long rhs) => new(lhs.x + rhs, lhs.y + rhs);
  public static IVec2 operator -(IVec2 lhs, long rhs) => new(lhs.x - rhs, lhs.y - rhs);
  public static IVec2 operator *(IVec2 lhs, long rhs) => new(lhs.x * rhs, lhs.y * rhs);
  public static IVec2 operator /(IVec2 lhs, long rhs) => new(lhs.x / rhs, lhs.y / rhs); }


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
  string ToString() => $"({x},{y},{z})";

  public override
  int GetHashCode() { return new { A=x, B=y, C=z }.GetHashCode(); }

  public override
  bool Equals(object? other) => (other is IVec3 inst) && inst == this;

  public static bool operator==(IVec3 l, IVec3 r) => (l.x, l.y, l.z) == (r.x, r.y, r.z);
  public static bool operator!=(IVec3 l, IVec3 r) => (l.x, l.y, l.z) != (r.x, r.y, r.z);

  public static bool operator< (IVec3 lhs, IVec3 rhs) => lhs.x< rhs.x && lhs.y< rhs.y && lhs.z< rhs.z;
  public static bool operator> (IVec3 lhs, IVec3 rhs) => lhs.x> rhs.x && lhs.y> rhs.y && lhs.z> rhs.z;
  public static bool operator<=(IVec3 lhs, IVec3 rhs) => lhs.x<=rhs.x && lhs.y<=rhs.y && lhs.z<=rhs.z;
  public static bool operator>=(IVec3 lhs, IVec3 rhs) => lhs.x>=rhs.x && lhs.y>=rhs.y && lhs.z>=rhs.z;

  public static IVec3 operator-(IVec3 rhs) => new(-rhs.x, -rhs.y, -rhs.z);
  public static IVec3 operator+(IVec3 lhs, IVec3 rhs) => new(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
  public static IVec3 operator-(IVec3 lhs, IVec3 rhs) => new(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
  public static IVec3 operator*(IVec3 lhs, IVec3 rhs) => new(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
  public static IVec3 operator/(IVec3 lhs, IVec3 rhs) => new(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
  public static IVec3 operator+(IVec3 lhs, long rhs) => new(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
  public static IVec3 operator*(IVec3 lhs, long rhs) => new(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
  public static IVec3 operator/(IVec3 lhs, long rhs) => new(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);

  public IVec3 Cross(IVec3 a) => new IVec3(y*a.z, z*a.x, x*a.y) -
                                 new IVec3(z*a.y, x*a.z, y*a.x);

  public IVec2 XY() => new(x, y);

  public long Dot(IVec3 a) => x*a.x + y*a.y + z*a.z; }

struct IMat2 {
  static readonly long[] _icos = { 1, 0, -1, 0 };
  static readonly long[] _isin = { 0, 1, 0, -1 };
  long a00, a01, a10, a11;

  public
  IMat2(long aa00, long aa01,
        long aa10, long aa11) {
    a00 = aa00; a01 = aa01;
    a10 = aa10; a11 = aa11;}

  private static
  long Mod(long a, long b) {
    var tmp = a % b;
    return tmp < 0 ? tmp + b : tmp; }

  public static
  IMat2 Rotate(int th) {
    var ca = _icos[Mod(th,4)];
    var sa = _isin[Mod(th,4)];
    return new IMat2(ca, -sa,
                     sa, ca); }
    
  public
  IVec2 Mul(IVec2 a) {
    return new IVec2( a.x*a00 + a.y*a01,
                      a.x*a10 + a.y*a11 ); }}


struct IMat3 {
  static readonly long[] _icos = { 1, 0, -1, 0 };
  static readonly long[] _isin = { 0, 1, 0, -1 };

  IVec3 r0, r1, r2;

  private
  IMat3(long a00, long a01, long a02,
        long a10, long a11, long a12,
        long a20, long a21, long a22) {
    r0 = new(a00, a01, a02);
    r1 = new(a10, a11, a12);
    r2 = new(a20, a21, a22); }

  private static
  long Mod(long a, long b) {
    var tmp = a % b;
    return tmp < 0 ? tmp + b : tmp; }

  public static
  IMat3 RotateX(long th) {
    th = Mod(th, 4);
    return new IMat3(
      1, 0, 0,
      0, _icos[th], -_isin[th],
      0, _isin[th], _icos[th]); }

  public static
  IMat3 RotateY(long th) {
    th = Mod(th, 4);
    return new IMat3(
      _icos[th], 0, _isin[th],
      0, 1, 0,
      -_isin[th], 0, _icos[th]); }

  public static
  IMat3 RotateZ(long th) {
    th = Mod(th, 4);
    return new IMat3(
      _icos[th], -_isin[th], 0,
      _isin[th], _icos[th], 0,
      0, 0, 1); }

  public static
  IMat3 RotateArb(long th, IVec3 v) {
    var x = v.x;
    var y = v.y;
    var z = v.z;
    var c = _icos[Mod(th,4)];
    var s = _isin[Mod(th,4)];
    return new IMat3(
      x*x*(1-c)+c,   x*y*(1-c)-z*s, x*z*(1-c)+y*s,
      y*x*(1-c)+z*s, y*y*(1-c)+c,   y*z*(1-c)-x*s,
      x*z*(1-c)-y*s, y*z*(1-c)+x*s, z*z*(1-c)+c); }

  public
  IVec3 Mul(IVec3 a) => new( r0.Dot(a), r1.Dot(a), r2.Dot(a) ); }
