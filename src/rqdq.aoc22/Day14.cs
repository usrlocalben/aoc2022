using BTU = rqdq.rclt.ByteTextUtil;
using rqdq.rmlv;

namespace rqdq.aoc22 {

class Day14 : ISolution {
  public void Solve(ReadOnlySpan<byte> t) {
    Dictionary<IVec2, char> map = new();
    IVec2 zero = new(0,0);
    IVec2 DIM = zero, cur = zero;
    while (!t.IsEmpty) {
      BTU.ConsumeValue(ref t, out int x);
      BTU.ConsumeChar(ref t); // ,
      BTU.ConsumeValue(ref t, out int y);
      IVec2 coord = new(x, y);
      if (cur != new IVec2(0)) {
          var dir = (coord - cur).Sign();
          for (var xy=cur; ; xy+=dir) {
            DIM = DIM.Max(xy);
            map[xy] = '#';
            if (xy==coord) break; }}
      cur = coord;
      if (!t.IsEmpty && t[0] == (byte)'\n') {
          cur = zero; }
      else {
          BTU.ConsumeSpace(ref t);
          BTU.PopWord(ref t); }
      BTU.ConsumeSpace(ref t); }
    DIM += new IVec2(1);

    IVec2[] dirs = new IVec2[] { new IVec2(0,1), new IVec2(-1,1), new IVec2(1,1) };
    long p1 = 0, p1incr = 1, p2 = 0;
    IVec2 src = new(500,0);
    while (!map.ContainsKey(src)) {
        IVec2 sp = src;
        while (true) {
          if (DIM.y <= sp.y) {
            p1incr = 0; }
          IVec2 next = zero;
          foreach (var dir in dirs) {
            var n = sp + dir;
            if (!map.ContainsKey(n) && n.y < DIM.y + 1) {
              next = n;
              break; } }
          if (next != zero) {
            sp = next; }
          else {
            map[sp] = 'o';
            p1 += p1incr;
            ++p2;
            break; } } }

    Console.WriteLine(p1);
    Console.WriteLine(p2); }}


}  // close package namespace
