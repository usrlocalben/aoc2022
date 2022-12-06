using System.Diagnostics;
using System.Numerics;
using BTU = rqdq.rclt.ByteTextUtil;

namespace rqdq.aoc22 {

class Day06 : ISolution {

  public void Solve(ReadOnlySpan<byte> text) {
    const int N1 = 4, N2 = 14;

    int p1 = -1, p2 = -1;
    var buf1 = new uint[N1];
    var buf2 = new uint[N2];

    for (int i=0; i<text.Length && (p1==-1||p2==-1); ++i) {
      buf1[i % N1] = buf2[i % N2] = alphaBit(text[i]);
      var bits1 = buf1.Aggregate(0U, (ax, it) => ax|it);
      var bits2 = buf2.Aggregate(0U, (ax, it) => ax|it);
      if (p1 == -1 && BitOperations.PopCount(bits1) == N1) {
        p1 = i + 1; }
      if (p2 == -1 && BitOperations.PopCount(bits2) == N2) {
        p2 = i + 1; }}

    Console.WriteLine(p1);
    Console.WriteLine(p2); }

  uint alphaBit(byte ch) => 1U << (ch - 'a'); }


}  // close package namespace
