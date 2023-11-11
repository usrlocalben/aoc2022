using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

class Day13 : ISolution {
  public void Solve(ReadOnlySpan<byte> t) {
    const string div2 = "[[2]]";
    const string div6 = "[[6]]";
    List<string> buf = new(500) { div2, div6 };

    var p1 = 0;
    for (var n = 1; !t.IsEmpty; ++n) {
      var l = BTU.Decode(BTU.PopLine(ref t));
      var r = BTU.Decode(BTU.PopLine(ref t));
      BTU.ConsumeSpace(ref t);
      p1 += Comp(l, r) == -1 ? n : 0;
      buf.Add(l);  //p2
      buf.Add(r); }

    buf.Sort((a, b) => Comp(a, b));
    var p2 = buf.Select((v, i) => v==div2 || v==div6 ? i + 1 : 1)
                .Aggregate(1, (ax, it) => ax * it);

    Console.WriteLine(p1);
    Console.WriteLine(p2); }
   
  int Comp(string l, string r) {
    bool ll = l[0] == '[';
    bool rl = r[0] == '[';
    if (!ll && !rl) {
      // both int
      var ln = int.Parse(l);
      var rn = int.Parse(r);
      return ln < rn ? -1 : (ln > rn ? 1 : 0); }

    if (ll && !rl) {
      r = $"[{r}]"; }  // upgrade r
    else if (!ll && rl) {
      l = $"[{l}]"; }  // upgrade l

    // both list
    (l, r) = (l[1..^1], r[1..^1]);  // strip outer []s
    var lseg = L.Split(l).ToList();
    var rseg = L.Split(r).ToList();
    for (int i=0; i<Math.Max(lseg.Count, rseg.Count); ++i) {
      if (i == lseg.Count) return -1;  // left runs out first
      if (i == rseg.Count) return 1;  // right runs out first
      var res = Comp(l[lseg[i].Item1..lseg[i].Item2],
                     r[rseg[i].Item1..rseg[i].Item2]);
      if (res == -1 || res == 1) {
        return res; }}
    return 0; }}
