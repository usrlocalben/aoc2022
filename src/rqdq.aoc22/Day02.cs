using BTU = rqdq.rclt.ByteTextUtil;

namespace rqdq.aoc22 {

class Day02 {
  public const string fileName = "02.txt";
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
    if (us == them) {
      return us + 3; }
    else {
      return us + (Winner(us, them) == us ? 6 : 0); }}

  int Winner(int a, int b) {
    if (a == b) return a;  // tie
    if (a == 1) return b==3 ? a : b;   // r>s
    if (a == 2) return b==1 ? a : b;   // p>r
    /* a == 3*/ return b==2 ? a : b; } // s>p

  int Move(int them, int wanted) {
    if (wanted == Tie) return them;
    if (them == 1) return wanted==Win ? 2 : 3;
    if (them == 2) return wanted==Win ? 3 : 1;
    /* them== 3 */ return wanted==Win ? 1 : 2; } }


}  // close package namespace
