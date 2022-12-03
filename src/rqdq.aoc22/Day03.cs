using System.Numerics;
using BTU = rqdq.rclt.ByteTextUtil;

namespace rqdq.aoc22 {

class Day03 {
  public const string fileName = "03.txt";

  public void Solve(ReadOnlySpan<byte> text) {
    int p1 = 0, p2 = 0;

    ulong[] group = new ulong[3];
    int gi = 0;
    while (!text.IsEmpty) {
      var line = BTU.PopLine(ref text);
      ulong left = 0, right = 0;
      var mid = line.Length / 2;
      for (int i=0; i<mid; ++i) {
        var bit = 1UL << Priority(line[i]);
        group[gi] |= bit;
        left      |= bit; }
      for (int i=mid; i<line.Length; ++i) {
        var bit = 1UL << Priority(line[i]);
        group[gi] |= bit;
        right     |= bit; }
      p1 += BitOperations.TrailingZeroCount(left & right);
      if (++gi == 3) {
        p2 += BitOperations.TrailingZeroCount(group[0] & group[1] & group[2]);
        gi = 0;
        Array.Clear(group); }}

    Console.WriteLine(p1);
    Console.WriteLine(p2); }

    int Priority(byte ch) {
      if ((byte)'A' <= ch && ch <= (byte)'Z') {
        return ch - (byte)'A' + 27; }
      return ch - (byte)'a' + 1; }}


}  // close package namespace
