using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

class Day22 : ISolution {

  public void Solve(ReadOnlySpan<byte> t) {
    List<string> map = new();
    var width = 0;
    while (!t.IsEmpty) {
      var l = BTU.PopLine(ref t);
      if (l.Length == 0) break;
      map.Add(BTU.Decode(l));
      width = Math.Max(width, map.Last().Length); }

    IVec2 dim = new(width, map.Count);
    var i = 0;
    while (map[0][i] != '.') i++;
    var init = new IVec2(i, 0);

    var p1 = Part1(map, dim, t, init);
    var p2 = Part2(map, dim, t, init);
    Console.WriteLine(p1);
    Console.WriteLine(p2); }

  long Part1(IReadOnlyList<string> map, IVec2 dim, ReadOnlySpan<byte> instr, IVec2 init) {
    var pos = init;
    IVec2 dir = new(1, 0);
    
    while (!instr.IsEmpty) {
      if ((byte)'0' <= instr[0] && instr[0] <= (byte)'9') {
        // amt
        BTU.ConsumeValue(ref instr, out int amt);
        Forward(amt); }
      else  {
        var cd = (char)instr[0];
        BTU.ConsumeChar(ref instr);
        if (cd == 'L') {
          Left(); }
        else {
          Right(); } }}
    
    var diramt = dir.x == 1 ? 0 : (dir.y == 1 ? 1 : (dir.x == -1 ? 2 : 3));
    // xxx why off by one?
    return 4*(pos.x+1) + 1000*(pos.y+1) + diramt;
    
    void Forward(int n) {
      var height = map.Count();
      var width = 150;
      for (var i=0; i<n; ++i ) {

        var next = pos + dir;
        while (At(next) == ' ') {
          next = (next+dir).Mod(new IVec2(width, height)); }

        if (At(next) != '#') {
          pos = next; }}}

    void Left() {
      var rl = IMat2.Rotate(-1); 
      dir = rl.Mul(dir); }
    void Right() {
      var rr = IMat2.Rotate(1); 
      dir = rr.Mul(dir); }
    
    char At(IVec2 coord) {
      if (0 <= coord.y && coord.y < map.Count) {
        var line = map[(int)coord.y];
        if (0 <= coord.x && coord.x < line.Length) {
          return line[(int)coord.x]; }}
      return ' '; } }
  
  
  long Part2(IReadOnlyList<string> map, IVec2 dim, ReadOnlySpan<byte> instr, IVec2 init) {
    // compute sqsz: the size of a square in the map.
    // it should be the greatest value that evenly divides map width & height
    var sqsz = 0;
    for (var x=dim.HMin(); x>=1; --x) {
      if (dim.Mod(x) == new IVec2(0)) {
        sqsz = (int)x;  // found likely candidate for square dim
        break; }}
    var dimSq = dim / new IVec2(sqsz);  // dim in squares
    
    var sqmap = new int[dimSq.x,dimSq.y];
    int seq = 0;
    for (int y=0; y<dimSq.y; ++y) {
      for (int x=0; x<dimSq.x; ++x) {
        var tl = new IVec2(x,y) * sqsz;
        var br = tl + sqsz;
        var midpt = (tl + br) / 2;
        // Console.WriteLine(midpt);
        sqmap[x,y] = At(midpt) != ' ' ? seq++ : -1; }}
    // sqmap is e.g. (w/ -1 in the spaces)
    
    // mine, sqdim=50, sqmap=
    // .01
    // .2.
    // 34.
    // 5..
   
    // example, sqdim=4, sqmap=
    // ..0.
    // 123.
    // ..45

    // find first square in sqmap
    IVec2 surf = new(-1,-1);
    for (int y=0; y<dimSq.y; ++y) {
      for (int x=0; x<dimSq.x; ++x) {
        if (sqmap[x,y] != -1) {
          surf = new(x,y);
          goto found; }}}
found:
    // surf is coord in sqmap with starting pos

    // walk the sqmap, rotating in 3d as we cross edges
    // build a map of 3d coords to 2d/uv coords in input
    // important! space is expanded by 1 unit in each dir,
    //            and maps to the texture position "below"
    //            it. this solves the problem at the edges
    //            where up to three values map to the same
    //            3d coord.
    Dictionary<IVec3, IVec2> space = new();  // 3d -> 2d
    var x0y0 = new IVec2(0);
    
    // BFS around the map, rotating the texture origin/orientation as we move
    
    // sqmap-coord, up-vector, forward-vector, texture-origin
    Queue<(IVec2, IVec3, IVec3, IVec3)> queue = new();
    // start block will face +Z -- (**)important when setting up path later
    queue.Enqueue((surf, new(0,0,1), new(0,-1,0), new(-1,1,1)));
    HashSet<IVec2> visited = new();
    while (queue.TryDequeue(out var state)) {
      var (here, up, dir, texo) = state;
      if (!(x0y0 <= here && here < dimSq)) continue;  // OOB
      if (sqmap[here.x,here.y] == -1) continue;  // not on surface
      if (visited.Contains(here)) continue;
      visited.Add(here);

      var right = up.Cross(dir);
      
      // Console.WriteLine($"{sqmap[here.x, here.y]}: went({went}) UP({up}) DIR({dir}) RIGHT({right}) texO({texo})");
      IVec3 tcoord = new(
        texo.x == -1 ? 0 : sqsz - 1,
        texo.y == -1 ? 0 : sqsz - 1,
        texo.z == -1 ? 0 : sqsz - 1 );
      tcoord += up;  // write into the layer "above"
      for (var y=0; y<sqsz; ++y, tcoord += dir) {
        var tcoord2 = tcoord;
        for (var x=0; x<sqsz; ++x, tcoord2 += right) {
          space[tcoord2] = here*sqsz + new IVec2(x,y); } }

      IMat3 m;
      m = IMat3.RotateArb(1, right); // down
      queue.Enqueue((here + new IVec2(0,1), m.Mul(up), m.Mul(dir), m.Mul(texo)));
     
      m = IMat3.RotateArb(-1, right); // up
      queue.Enqueue((here + new IVec2(0,-1), m.Mul(up), m.Mul(dir), m.Mul(texo)));

      m = IMat3.RotateArb(1, dir); // left
      queue.Enqueue((here + new IVec2(-1, 0), m.Mul(up), m.Mul(dir), m.Mul(texo)));

      m = IMat3.RotateArb(-1, dir); // right
      queue.Enqueue((here + new IVec2(1, 0), m.Mul(up), m.Mul(dir), m.Mul(texo))); }
    
    // find start location in 3d
    IVec3 init3d = new();
    foreach (var (d3, d2) in space) {
      if (d2 == init) {
        init3d = d3;
        break; }}

    // (**)start location will be in the +z facing side
    var up3 = new IVec3(0,0,1);
    var pos3 = init3d - up3;  // remove the offset, move around on the surface
    var forward3 = new IVec3(1,0,0);
    // bounding volume [0, Limit)
    var (x0y0z0, xLyLzL) = (new IVec3(0), new IVec3(sqsz));

    while (!instr.IsEmpty) {
      // Console.WriteLine($"here({pos3}) up({up3}) dir({forward3}) fwA({up3.Cross(forward3)}) UV({space[pos3+up3]})");
      if ((byte)'0' <= instr[0] && instr[0] <= (byte)'9') {
        // amt
        BTU.ConsumeValue(ref instr, out int amt);
        Forward3(amt); }
      else if ((char)instr[0] == 'L' || (char)instr[0] == 'R') {
        var cd = (char)instr[0];
        BTU.ConsumeChar(ref instr);
        Turn(cd == 'L' ? 1 : -1); }
      else break;}  // trailing whitespace

    // ouch! problem wants facing in _2d_ space
    // partial derivative should work, but we
    // need a pair of 2d points that are in the
    // space map. either ahead or behind should
    // be a valid point.
    var endUv = space[pos3 + up3];
    IVec2 dydx;
    if (space.TryGetValue((pos3 - forward3) + up3, out var prevUv))
      dydx = endUv - prevUv;
    else if (space.TryGetValue((pos3 + forward3) + up3, out var nextUv))
      dydx = nextUv - endUv;
    else
      throw new Exception("should be impossible to reach here");

    endUv += new IVec2(1);  // problem wants 1-indexed coords
    var facing = dydx.x == 1 ? 0 : (dydx.y == 1 ? 1 : (dydx.x == -1 ? 2 : 3));
    return endUv.y * 1000 + endUv.x * 4 + facing;

    void Turn(int k) {
      forward3 = IMat3.RotateArb(k, up3).Mul(forward3); }
    
    void Forward3(int n) {
      while (n-- > 0) {
        var next = PeekNext();
        if (At3(next.pos + next.up) == '#') break;
        (pos3, up3, forward3) = next; } }

    (IVec3 pos, IVec3 up, IVec3 forward) PeekNext() {
      var a = pos3 + forward3;
      if (x0y0z0 <= a && a < xLyLzL)
        return (a, up3, forward3); // still in bounds
      else
        return (pos3, forward3, -up3); }
    
    char At(IVec2 coord) {
      if (0 <= coord.y && coord.y < map.Count) {
        var line = map[(int)coord.y];
        if (0 <= coord.x && coord.x < line.Length) {
          return line[(int)coord.x]; }}
      return ' '; }

    char At3(IVec3 coord) {
      var uv = space[coord];
      return map[(int)uv.y][(int)uv.x]; } } }