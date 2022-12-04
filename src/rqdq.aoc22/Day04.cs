using System.Numerics;
using BTU = rqdq.rclt.ByteTextUtil;

namespace rqdq.aoc22 {

class Day04 : ISolution {
  public void Solve(ReadOnlySpan<byte> text) {
    long p1 = 0, p2 = 0;
    while (!text.IsEmpty) {
      BTU.ConsumeValue(ref text, out int a); BTU.ConsumeChar(ref text);
      BTU.ConsumeValue(ref text, out int b); BTU.ConsumeChar(ref text);
      BTU.ConsumeValue(ref text, out int x); BTU.ConsumeChar(ref text);
      BTU.ConsumeValue(ref text, out int y); BTU.ConsumeSpace(ref text);

      if (Contains(a, b, x, y) || Contains(x, y, a, b)) {
        ++p1; }
      if (!Outside(a, b, x, y)) {
        ++p2; }}

    Console.WriteLine(p1);
    Console.WriteLine(p2); }

  bool Contains(int a, int b, int x, int y) {
    return a <= x && y <= b; }

  bool Outside(int a, int b, int x, int y) {
    return (y < a || x > b); }}


}  // close package namespace
