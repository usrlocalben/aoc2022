using rqdq.rmlv;

namespace rqdq.aoc22 {

class Day12 : ISolution {
  public void Solve(ReadOnlySpan<byte> map) {
    const int oo = 0x3f3f3f3f;
    var DIM = L.MapDim(map);
    var stride = DIM.x + 1;

    List<IVec2> aaa = new();
    var begin = new IVec2(-1, -1);
    var goal = new IVec2(-1, -1);
    for (int i=0; i<map.Length; ++i) {
      if (map[i] == (byte)'S') begin = new IVec2(i%stride, i/stride);
      if (map[i] == (byte)'E') goal = new IVec2(i%stride, i/stride);
      if (map[i] == (byte)'a' || map[i] == (byte)'S') {
        aaa.Add(new IVec2(i%stride, i/stride)); }}

    int p1 = oo, p2 = oo;
    Queue<Tuple<IVec2, int>> queue = new ();
    HashSet<IVec2> visited = new();
    foreach (var start in aaa) {
      int pathDist = oo;
      // BFS from start to E
      queue.Clear();
      visited.Clear();
      queue.Enqueue(new Tuple<IVec2, int>(start, 0));
      while (queue.Count > 0) {
        var (here, dist) = queue.Dequeue();
        if (visited.Contains(here)) continue;
        visited.Add(here);

        if (here == goal) {
          pathDist = dist;
          break; }

        int curLevel = Level(map[(int)(here.y * stride + here.x)]);
        foreach (var dir in L.NESW) {
          var nextHop = here + dir;
          if (new IVec2(0) <= nextHop && nextHop < DIM) {
            int nextLevel = Level(map[(int)(nextHop.y * stride + nextHop.x)]);
            if (nextLevel <= curLevel + 1) {
              queue.Enqueue(new (nextHop, dist + 1)); }}}}

      if (pathDist != oo) {
        p1 = start == begin ? pathDist : p1;
        p2 = Math.Min(p2, pathDist); }}

    Console.WriteLine(p1);
    Console.WriteLine(p2); }

    int Level(byte b) {
      if (b == (byte)'E') return 'z' - 'a';
      if (b == (byte)'S') return 0;
      return b - (byte)'a'; }}


}  // close package namespace
