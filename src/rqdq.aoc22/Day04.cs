using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

class Day04 : ISolution {
  public void Solve(ReadOnlySpan<byte> text) {
    long p1 = 0, p2 = 0;
    while (!text.IsEmpty) {
      BTU.ConsumeValue(ref text, out int a); BTU.ConsumeChar(ref text);
      BTU.ConsumeValue(ref text, out int b); BTU.ConsumeChar(ref text);
      BTU.ConsumeValue(ref text, out int x); BTU.ConsumeChar(ref text);
      BTU.ConsumeValue(ref text, out int y); BTU.ConsumeSpace(ref text);
      p1 += Contains(a, b, x, y) || Contains(x, y, a, b) ? 1 : 0;
      p2 += !Outside(a, b, x, y) ? 1 : 0; }

    Console.WriteLine(p1);
    Console.WriteLine(p2); }

  bool Outside(int a, int b, int x, int y) => y < a || x > b;
  bool Contains(int a, int b, int x, int y) => a <= x && y <= b; }
