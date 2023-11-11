using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

class Day15 : ISolution {
  const int oo = 0x3f3f3f3f;

  struct Sen {
    public IVec2 p;
    public long r; }
    // public IVec2 b; }

  List<Sen> _sen = new();
  HashSet<IVec2> _known = new();

  public void Solve(ReadOnlySpan<byte> t) {
    Dictionary<IVec2, char> map = new();
    IVec2 zero = new(0,0);
    IVec2 DIM = zero, cur = zero;
    while (!t.IsEmpty) {
      BTU.PopWordSp(ref t); BTU.PopWordSp(ref t); BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t);
      BTU.ConsumeValue(ref t, out int sx);
      BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t);
      BTU.ConsumeValue(ref t, out int sy);
      BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t); // at
      BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t); // x=
      BTU.ConsumeValue(ref t, out int bx);
      BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t);
      BTU.ConsumeValue(ref t, out int by);
      BTU.ConsumeSpace(ref t); 

      IVec2 beacon = new(bx, by);
      _known.Add(beacon);
      Sen sen;
      sen.p = new(sx, sy);
      sen.r = sen.p.MDist(beacon);
      _sen.Add( sen); }

    const long yyy = 2000000;
    List<Tuple<long, long>> sl = new();
    foreach (var sen in _sen) {
      long d = Math.Abs(sen.p.y - yyy);
      if (d <= sen.r) {
        long rem = sen.r - d;
        sl.Add(new(sen.p.x - rem, sen.p.x + rem + 1)); }}

    sl.Sort();


    long aa = 0, bb = 0;
    bool st = false;
    long p1 = 0;
    foreach (var span in sl) {
      if (st == false) {
        st = true;
        (aa, bb) = span; }
      else {
        if (span.Item1 <= bb) {
          bb = Math.Max(span.Item2, bb); }
        else {

{HashSet<IVec2> hits = new();
foreach (var b in _known) {
  if (b.y==yyy && aa <=b.x && b.x < bb) {
    hits.Add(b); }}
p1 += bb - aa - hits.Count;}

          (aa, bb) = span; }}}

{HashSet<IVec2> hits = new();
foreach (var b in _known) {
  if (b.y==yyy && aa <=b.x && b.x < bb) {
    hits.Add(b); }}
p1 += bb - aa - hits.Count;}

    const long limit = 4000000;
    HashSet<IVec2> ax = new();
    Span<IVec2> pack = stackalloc IVec2[4];
    Span<IVec2> dirs = stackalloc IVec2[4];
    IVec2 p2 = new (-1,-1);
    foreach (var sen in _sen) {
      pack[0] = new(sen.p.x, sen.p.y - sen.r - 1); dirs[0] = new IVec2(-1, 1);  // ->sw
      pack[1] = new(sen.p.x, sen.p.y + sen.r + 1); dirs[1] = new IVec2( 1,-1);  // ->ne
      pack[2] = new(sen.p.x - sen.r - 1, sen.p.y); dirs[2] = new IVec2( 1, 1);  // ->se
      pack[3] = new(sen.p.x + sen.r + 1, sen.p.y); dirs[3] = new IVec2(-1,-1);  // ->nw
      while (sen.p.MDist(pack[0]) == sen.r + 1) {
         foreach (var coord in pack) {
            if (0 <= coord.x && coord.x <= limit &&
                0 <= coord.y && coord.y <= limit) {
              bool good = true;
              foreach (var check in _sen) {
                if (check.p.MDist(coord) <= check.r) { 
                  good = false;
                  break; }}
              if (good) {
                p2 = coord;
                goto found; }}}
         for (int i=0; i<4; ++i) {
           pack[i] += dirs[i]; }}}
found:
    Console.WriteLine(p1);
    Console.WriteLine($"{p2.x*4000000 +p2.y}"); }}