namespace rqdq.aoc22;

class Day24 : ISolution
{
  public void Solve(ReadOnlySpan<byte> t) {
    int stride;
    for (stride = 0; t[stride] != '\n'; stride++);
    stride++;
    
    var M = new char[t.Length];
    for (int i = 0; i < t.Length; i++) M[i] = (char)t[i];

    var mapWidth = stride - 1;  // 2 for crlf
    var mapHeight = t.Length / stride;
    
    var W = mapWidth - 2;
    var H = mapHeight - 2;
    var x0y0 = new IVec2(0);
    var dim = new IVec2(W, H);

    var begin = new IVec2(0, -1);
    var end = new IVec2(W - 1, H);
    
    var period = Lcm(W, H);  // 700
    
    char At(IVec2 coord) => M[(coord.y + 1) * stride + (coord.x + 1)];

    bool Bad(IVec2 pos, int i) {
      long x = pos.x, y = pos.y;
      return At(new IVec2(x, L.Mod(y + i, H))) == '^' ||
             At(new IVec2(x, L.Mod(y - i, H))) == 'v' ||
             At(new IVec2(L.Mod(x - i, W), y)) == '>' ||
             At(new IVec2(L.Mod(x + i, W), y)) == '<'; }
    
    var NSEW0 = new[] { new IVec2(0, -1), new IVec2(0, 1),
                        new IVec2(1, 0), new IVec2(-1, 0),
                        new IVec2(0,0) };

    int time = 0;
    var init = begin;
    var goal = end;
    for (int pass = 0; pass < 3; pass++) {
      int best = 0x3f3f3f3f;
      Queue<(int t, IVec2 coord)> queue = new();
      queue.Enqueue((time, init));
      HashSet<(int t, IVec2 coord)> visited = new();
      while (queue.TryDequeue(out var state)) {
        var k = (state.t % period, state.coord);  // prune
        if (visited.Contains(k)) continue; visited.Add(k);
        
        if (state.coord == goal) {
          best = Math.Min(best, state.t);
          continue; }

        if (x0y0 <= state.coord && state.coord < dim &&
            Bad(state.coord, state.t)) continue; // bad ending

        foreach (var ofs in NSEW0) {
          var next = state.coord + ofs;
          if ((x0y0 <= next && next < dim) || next==goal || next==init)
            queue.Enqueue((state.t + 1, next)); } }

      if (pass % 2 == 0)
        Console.WriteLine(best);
      time = best;
      (init, goal) = (goal, init); }}

  static int Gcf(int a, int b) {
    while (b != 0) {
      var temp = b;
      b = a % b;
      a = temp; }
    return a; }

  static int Lcm(int a, int b) => (a / Gcf(a, b)) * b;
}

    
