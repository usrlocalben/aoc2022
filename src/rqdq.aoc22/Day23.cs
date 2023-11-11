namespace rqdq.aoc22;

class Day23 : ISolution
{
  public void Solve(ReadOnlySpan<byte> t) {
    var stride = 0;
    for (stride = 0; t[stride] != '\n'; stride++);
    stride++;
    
    var map = new HashSet<IVec2>();
    for (int i = 0; i < t.Length; ++i)
      if (t[i] == '#')
        map.Add(new IVec2(i % stride, -i / stride));

    var nswe = new[] {
      new[] { new IVec2(-1, 1), new IVec2(0, 1), new IVec2(1, 1) }, // n
      new[] { new IVec2(-1, -1), new IVec2(0, -1), new IVec2(1, -1) }, // s
      new[] { new IVec2(-1, 1), new IVec2(-1, 0), new IVec2(-1, -1) }, // w
      new[] { new IVec2(1, 1), new IVec2(1, 0), new IVec2(1, -1) }, }; // e 

    var hits = new bool[4];
    var nx = new Dictionary<IVec2, List<IVec2>>();
    var n = -1;
    for(;;){
      n++;
      nx.Clear();
      var noop = 0;
      foreach (var pos in map) {
        bool any = false;
        for (var ff = 0; ff < 4; ff++) {
          hits[ff] = false;
          foreach (var ofs in nswe[(ff + n) % 4]) {
            any |= hits[ff] |= map.Contains(pos + ofs); } }

        if (!any) {
          noop++;
          FROM_TO(pos, pos); }
        else {
          int ff;
          for (ff = 0; ff < 4; ff++) {
            if (!hits[ff]) {
              var nextPos = pos + nswe[(ff + n) % 4][1];
              FROM_TO(pos, nextPos);
              break; } }
          if (ff == 4) {
            noop++;
            FROM_TO(pos, pos); } } }

      if (noop == map.Count) break;  // p2 found

      map.Clear();
      foreach (var (dest, ws) in nx)
        if (ws.Count == 1)
          map.Add(dest);
        else
          foreach (var w in ws) map.Add(w);

      if (n == 10) {
        const int oo = 0x3f3f3f3f;
        var ml = new IVec2(oo, oo);
        var mu = new IVec2(-oo, -oo);
        foreach (var pos in map) {
          ml = IVec2.VMin(ml, pos);
          mu = IVec2.VMax(mu, pos); }
        mu += new IVec2(1);
        var dim = mu - ml;
        var area = dim.x * dim.y;
        Console.WriteLine(area - map.Count);}}  // p1
      
    Console.WriteLine(n+1);  // p2

    void FROM_TO(IVec2 pos, IVec2 dest) {
      if (!nx.TryGetValue(dest, out var who)) {
          who = new();
          nx[dest] = who; }
      who.Add(pos); } } }
    
