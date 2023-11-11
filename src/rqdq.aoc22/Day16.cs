using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

class Day16 : ISolution {
  // given graph (rooms)
  List<int> _flow = new();
  List<int>[] _link = new List<int>[1024];
  Dictionary<string, int> _room = new();
  Dictionary<int, string> _roomName = new();

  // compacted, weighted graph (valves)
  int _N;
  int[] _flowRate;
  int[] _valveRoom;
  int[,] _distance;

  public void Solve(ReadOnlySpan<byte> t) {
    _flow.Resize(1024);

    int seq = 0;

    while (!t.IsEmpty) {
      BTU.PopWordSp(ref t);  // Valve

      var heretext = BTU.Decode(BTU.PopWordSp(ref t));
      int hk;
      {if (_room.TryGetValue(heretext, out var k)) {
        hk = k; }
      else {
        hk = seq++;
        _roomName[hk] = heretext;
        _room[heretext] = hk; }}
      _link[hk] = new();


      // has flow
      BTU.PopWordSp(ref t); BTU.PopWordSp(ref t);
      // rate=
      BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t); BTU.ConsumeChar(ref t);
      BTU.ConsumeValue(ref t, out int rate);
      _flow[hk] = (short)rate;


      BTU.ConsumeChar(ref t);
      BTU.ConsumeSpace(ref t);

      // tun lead to valves
      BTU.PopWordSp(ref t); BTU.PopWordSp(ref t); BTU.PopWordSp(ref t); BTU.PopWordSp(ref t);

      bool done = false;
      while (!done) {
        var desttext = BTU.Decode(BTU.PopWord(ref t)[0..2]);
        // Console.WriteLine($"{BTU.Decode(heretext)} -> {BTU.Decode(desttext)}");

        int dk;
        {if (_room.TryGetValue(desttext, out var k)) {
          dk = k; }
        else {
          _room[desttext] = dk = seq++; }}

        _link[hk].Add(dk);

        BTU.ConsumeSpace(ref t);
        done = t.IsEmpty || (t[0] == (byte)'V' && t[1] == (byte)'a'); }}


    // number of compacted nodes (AA + flow>0)
    _N = 1 + _room.Where((k, v) => _flow[v] > 0).Count();
    _valveRoom = new int[_N];
    _flowRate = new int[_N];
    _distance = new int[_N,_N];
    _flowRate[0] = 0;
    _valveRoom[0] = _room["AA"];  // zeroth room is AA
    {int idx = 1;
    foreach (var (key, id) in _room) {
      if (_flow[id] > 0) {
        var newId = idx++;
        _flowRate[newId] = _flow[id];
        _valveRoom[newId] = id; }}}

    // find weights for compacted graph
    HashSet<int> visited = new();
    Queue<(int, int)> queue = new();
    for (int i=0; i<_valveRoom.Length - 1; ++i) {
    for (int j=i + 1; j<_valveRoom.Length; ++j) {

      int found = -1;
      var start = _valveRoom[i];
      var target = _valveRoom[j];
      queue.Clear();
      queue.Enqueue((start, 0));
      visited.Clear();
      while (queue.Count > 0) {
        var (here, hdist) = queue.Dequeue();
        if (visited.Contains(here)) continue;
        visited.Add(here);

        if (here == target) {
          found = hdist;
          break; }
        foreach (var l in _link[here]) {
          queue.Enqueue((l, hdist + 1)); }}
      if (found == -1) {
        throw new Exception("dest not found"); }
      _distance[i,j] = _distance[j,i] = found; }}

    InitDP();
    int p1 = DP(1, 0, 30);

    int p2 = 0, allOnes = (1 << _N) - 1;
    for (int self=0; self<1<<_N; ++self) {
      // if (self % 1024==0) {
      //   Console.Write($" {self} ");}
      var other = self ^ allOnes;
      int ax = DP(self|1, 0, 26) +
               DP(other|1, 0, 26);
      p2 = Math.Max(ax, p2); }

    Console.WriteLine(p1);
    Console.WriteLine(p2); }

  short[] _memo;
  void InitDP() {
    /*
    input has 16 valves
    open-state   = 16 bits
    pos [0, 15]  = 4 bits
    time [0, 30] = 5 bits
                  -------
                  25 bits
    */
    _memo = new short[1<<25];  // 64MiB
    Array.Fill(_memo, (short)-1); }

  int DP(int open, int pos, byte minutes) {
    int key = open<<9 | pos<<5 | minutes;
    if (_memo[key] != -1) {
      return _memo[key]; }
    int here = minutes * _flowRate[pos];
    int best = 0;
    for (int n=0; n<_N; ++n) {
      if ((open&(1<<n)) > 0) continue;
      var dist = _distance[pos,n];
      if (dist < minutes) {
        int amt = DP(open | (1<<n), n, (byte)(minutes - dist - 1));
        if (amt > best) {
          best = amt; }}}
    return _memo[key] = (short)(here + best); }}