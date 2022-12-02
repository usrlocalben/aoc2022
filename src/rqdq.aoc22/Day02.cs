using BTU = rqdq.rclt.ByteTextUtil;

namespace rqdq.aoc22 {

class Day02 {
  public const string fileName = "02.txt";
  const int Win = 3;
  const int Tie = 2;

  public
  void Solve(ReadOnlySpan<byte> text) {
    int p1 = 0, p2 = 0;
    while (!text.IsEmpty) {
      var line = BTU.PopLine(ref text);
      var other = line[0] - (byte)'A' + 1;
      var me = line[2] - (byte)'X' + 1;
      p1 += Score(me, other);
      p2 += Score(Move(other, me), other); }

    Console.WriteLine(p1);
    Console.WriteLine(p2); }

  static
  int Score(int player, int other) {
    if (player == other) {
      return player + 3; }
    else {
      return player + (Winner(player, other) == player ? 6 : 0); }}

  static
  int Winner(int a, int b) {
    if (a == b) return a;  // tie
    if (a == 1) return b==3 ? a : b;  // r>s
    if (a == 2) return b==1 ? a : b;  // p>r
    if (a == 3) return b==2 ? a : b;  // s>p
    throw new Exception("should never reach here"); }

  static
  int Move(int a, int wanted) {
    if (wanted == Tie) return a;
    if (a == 1) return wanted==Win ? 2 : 3;
    if (a == 2) return wanted==Win ? 3 : 1;
    if (a == 3) return wanted==Win ? 1 : 2;
    throw new Exception("should never reach here"); } }


}  // close package namespace
