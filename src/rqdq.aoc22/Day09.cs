using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

class Day09 : ISolution {
  public
  void Solve(ReadOnlySpan<byte> t) {
    IVec2 head = new(0,0);
    IVec2[] tail = new IVec2[9];

    IVec2 R = new IVec2(-1,0);
    IVec2 L = new IVec2( 1,0);
    IVec2 U = new IVec2( 0,1);
    IVec2 D = new IVec2( 0,-1);

    HashSet<IVec2> V1 = new(), V2 = new();
    V1.Add(head);
    V2.Add(head);

    while (!t.IsEmpty) {
      var dir = BTU.PopWordSp(ref t);
      BTU.ConsumeValue(ref t, out int amt);
      BTU.ConsumeSpace(ref t);

      var hdir = dir[0] switch {
        (byte)'R' => R, (byte)'L' => L,
        (byte)'U' => U, (byte)'D' => D };

      amt.Times(() => {
        var prev = head += hdir;
        for (int ti=0; ti<tail.Length; ++ti) {
          var d = prev - tail[ti];
          if (d.Abs().HMax() > 1) {
            tail[ti] += d.Sign(); }
          prev = tail[ti]; }
        V1.Add(tail.First());
        V2.Add(tail.Last()); }); }

    Console.WriteLine(V1.Count);
    Console.WriteLine(V2.Count);}}