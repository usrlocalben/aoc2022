using BTU = rqdq.rclt.ByteTextUtil;
using rqdq.rmlv;

namespace rqdq.aoc22 {

class Day18 : ISolution {
  const int N = 20;
  readonly IVec3 Zero = new(0);
  readonly IVec3 Coord1 = new(1);
  readonly IVec3 Coord19 = new(19);
  readonly IVec3 Lim = new(N);

  readonly IVec3[] Dirs = new IVec3[] {
    new IVec3(-1,0,0), new IVec3(1,0,0),
    new IVec3(0,-1,0), new IVec3(0,1,0),
    new IVec3(0,0,-1), new IVec3(0,0,1) };

  bool[,,] _pts = new bool[N,N,N];
  bool[,,] _air = new bool[N,N,N];

  public void Solve(ReadOnlySpan<byte> t) {
    long p1 = 0, p2 = 0;

    /*for (int x=0; x<N; ++x) {
    for (int y=0; y<N; ++y) {
    for (int z=0; z<N; ++z) {
      _air[x,y,z] = false; 
      _pts[x,y,z] = false; }}}*/

    while (!t.IsEmpty) {
      BTU.ConsumeValue(ref t, out int x); BTU.ConsumeChar(ref t);
      BTU.ConsumeValue(ref t, out int y); BTU.ConsumeChar(ref t);
      BTU.ConsumeValue(ref t, out int z); BTU.ConsumeChar(ref t);
      _pts[x,y,z] = true; }

    HashSet<IVec3> visited = new();
    Queue<IVec3> queue = new();

    // probe for air
    visited.Clear(); queue.Clear();
    for (int x=0; x<N; ++x) {
    for (int y=0; y<N; ++y) {
    for (int z=0; z<N; ++z) {
      IVec3 coord = new(x,y,z);
      if (Coord1 <= coord && coord < Coord19) continue;
      if (visited.Contains(coord)) continue;
      if (Lava(coord)) continue;
      queue.Clear();
      queue.Enqueue(coord);
      while (queue.TryDequeue(out var pos)) {
        if (visited.Contains(pos)) continue;
        visited.Add(pos);
        _air[pos.x,pos.y,pos.z] = true;
        foreach (var dir in Dirs) {
          IVec3 next = pos + dir;
          if (Zero <= next && next < Lim && !Lava(next)) {
            queue.Enqueue(next); }}}}}}

    visited.Clear(); queue.Clear();
    for (int x=0; x<N; ++x) {
    for (int y=0; y<N; ++y) {
    for (int z=0; z<N; ++z) {
      IVec3 coord = new(x,y,z);
      if (visited.Contains(coord)) continue;
      if (!Lava(coord)) continue;
      queue.Clear();
      queue.Enqueue(coord);
      while (queue.TryDequeue(out var pos)) {
        if (visited.Contains(pos)) continue;
        visited.Add(pos);
        foreach (var dir in Dirs) {
          IVec3 next = pos + dir;
          if (Lava(next)) {
            queue.Enqueue(next); }
           else {
            p1++;
            p2 += Air(next); }}}}}}

      Console.WriteLine(p1);
      Console.WriteLine(p2); }

    bool Lava(IVec3 p) {
      if (Zero <= p && p < Lim) {
        return _pts[p.x,p.y,p.z]; }
      return false; }

    int Air(IVec3 p) {
      if (Zero <= p && p < Lim) {
        return _air[p.x,p.y,p.z] ? 1 : 0; }
      return 1; }

    }//class



}  // close package namespace
