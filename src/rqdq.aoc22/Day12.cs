namespace rqdq.aoc22;

class Day12 : ISolution {
  public void Solve(ReadOnlySpan<byte> map) {
    var DIM = L.MapDim(map);
    var stride = DIM.x + 1;

    List<IVec2> aaa = new();
    var start = new IVec2(-1, -1);
    var end = new IVec2(-1, -1);
    for (var i=0; i<map.Length; ++i) {
      if (map[i] == (byte)'S') start = new IVec2(i%stride, i/stride);
      if (map[i] == (byte)'E') end = new IVec2(i%stride, i/stride);
      if (map[i] == (byte)'a' || map[i] == (byte)'S') {
        aaa.Add(new IVec2(i%stride, i/stride)); }}

    int p1 = -1, p2 = -1;
    Queue<Tuple<IVec2, int>> queue = new ();
    HashSet<IVec2> visited = new();

    // BFS from start to end
    queue.Enqueue(new(start, 0));
    while (queue.Count > 0) {
      var (here, dist) = queue.Dequeue();
      if (visited.Contains(here)) continue;
      visited.Add(here);

      if (here == end) {
        p1 = dist;
        break; }

      var height = Height(map[(int)(here.y * stride + here.x)]);
      foreach (var dir in L.NESW) {
        var nextCoord = here + dir;
        if (new IVec2(0) <= nextCoord && nextCoord < DIM) {
          var nextHeight = Height(map[(int)(nextCoord.y * stride + nextCoord.x)]);
          if (nextHeight <= height + 1) {
            queue.Enqueue(new (nextCoord, dist + 1)); }}}}

    // BFS from end to any 'a'
    visited.Clear();
    queue.Clear(); queue.Enqueue(new(end, 0));
    while (queue.Count > 0) {
      var (here, dist) = queue.Dequeue();
      if (visited.Contains(here)) continue;
      visited.Add(here);

      var height = Height(map[(int)(here.y * stride + here.x)]);
      if (height == 1) {
        p2 = dist;
        break; }

      foreach (var dir in L.NESW) {
        var nextCoord = here + dir;
        if (new IVec2(0) <= nextCoord && nextCoord < DIM) {
          var nextHeight = Height(map[(int)(nextCoord.y * stride + nextCoord.x)]);
          if (nextHeight >= height - 1) {
            queue.Enqueue(new (nextCoord, dist + 1)); }}}}

    Console.WriteLine(p1);
    Console.WriteLine(p2); }

  static
  int Height(byte b) {
      if (b == (byte)'E') return 'z' - 'a';
      if (b == (byte)'S') return 0;
      return b - (byte)'a'; }}