using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

class Day25 : ISolution
{
  public void Solve(ReadOnlySpan<byte> t) {
    long ax = 0;
    while (!t.IsEmpty) {
      var line = BTU.PopLine(ref t);
      ax += Snafu.Parse(line); }
    Console.WriteLine(Snafu.Stringify(ax)); }
}

class Snafu
{
  private static readonly Dictionary<int, char> v2n =
    new() { {-2,'='}, {-1,'-'}, {0,'0'}, {1,'1'}, {2,'2'} };
  private static readonly Dictionary<char, int> n2v =
    new() { {'=',-2}, {'-',-1}, {'0',0}, {'1',1}, {'2',2} };

  public static string Stringify(long n) {
    var len = mspl(n) + 1;
    var buf = new int[len];
    while (n != 0) {
      var pl = mspl(n);
      var r = (long)Math.Pow(5, pl);
      int num;
      if (n > 0)
        num = Math.Abs(r - n) <= Math.Abs(r * 2 - n) ? 1 : 2;
      else
        num = Math.Abs(r + n) <= Math.Abs(r * 2 + n) ? -1 : -2;
      // Console.WriteLine($"n={n} pl={pl} r={r} num={num}");
      buf[pl] = num;
      n -= r * num; }

    var ax = "";
    for (long i = len - 1; i >= 0; i--)
      ax += v2n[buf[i]];
    return ax; }

  private static long mspl(long n) {
    long p = 0, r = 1, cmp = r*2;
    while (n < 0 ? -cmp > n : cmp < n) {
      r *= 5;
      cmp += r * 2;
      p++; }
    return p; }

  public static long Parse(ReadOnlySpan<byte> s) {
    long r = 1, ax = 0;
    for (int si = s.Length - 1; si >= 0; --si) {
      ax += r * n2v[(char)s[si]];
      r *= 5; }
    return ax; }
}