using BTU = rqdq.rclt.ByteTextUtil;

namespace rqdq.aoc22 {

class Day02 : ISolution {
  const int Tie = 2, Win = 3;

  public void Solve(ReadOnlySpan<byte> text) {
    int p1 = 0, p2 = 0;
    while (!text.IsEmpty) {
      var line = BTU.PopLine(ref text);
      var them = line[0] - (byte)'A' + 1;  // [1, 3]
      var us   = line[2] - (byte)'X' + 1;  // [1, 3]
      p1 += Score(them, us);
      p2 += Score(them, Move(them, us)); }
    Console.WriteLine(p1);
    Console.WriteLine(p2); }

  int Score(int them, int us) {
    int score;
    if (us == them) {
      score = 3; }
    else {
      score = Winner(us, them) == us ? 6 : 0; }
    return us + score; }

  int Winner(int a, int b) => (a, b) switch {
    (1, 1) => 1, (1, 2) => 2, (1, 3) => 1,
    (2, 1) => 2, (2, 2) => 2, (2, 3) => 3,
    (3, 1) => 3, (3, 2) => 3, (3, 3) => 3 };

  int Move(int them, int wanted) {
    if (wanted == Tie) return them;
    if (them == 1) return wanted==Win ? 2 : 3;
    if (them == 2) return wanted==Win ? 3 : 1;
    /* them== 3 */ return wanted==Win ? 1 : 2; } }


}  // close package namespace
