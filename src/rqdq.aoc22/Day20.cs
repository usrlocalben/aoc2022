using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

class Day20 : ISolution {
  const  long magic = 811589153;
  List<long> num = new();
  List<int>  next = new();
  List<int>  prev = new();

  public void Solve(ReadOnlySpan<byte> t) {
    long p1 = 0, p2 = 0;

    while (!t.IsEmpty) {
      BTU.ConsumeValue(ref t, out int ax); BTU.ConsumeSpace(ref t);
      int n = num.Count;
      num.Add(ax);
      next.Add(n + 1);
      prev.Add(n - 1); }
    int N = num.Count;
    prev[0] = N - 1;
    next[N - 1] = 0;
      
    for (int i=0; i<N; ++i) {

      // remove num
      next[prev[i]] = next[i];
      prev[next[i]] = prev[i];

      // find insertion-point
      var ip = prev[i];
      var dir = Math.Sign(num[i]);
      for (int n=0; n!=num[i]; n+=dir) {
        ip = dir > 0 ? next[ip] : prev[ip]; }

      // insert
      {var suc = next[ip];
      next[ip] = i;
      prev[i] = ip;
      next[i] = suc;
      prev[suc] = i;} }

    var pos = num.FindIndex(n => n == 0);
    1000.Times(() => { pos = next[pos]; }); p1 += num[pos];
    1000.Times(() => { pos = next[pos]; }); p1 += num[pos];
    1000.Times(() => { pos = next[pos]; }); p1 += num[pos];

    // reset order
    for (int i=0; i<N; ++i) {
      next[i] = i + 1;
      prev[i] = i - 1; }
    prev[0] = N - 1;
    next[N - 1] = 0;

    int[] path = new int[N];
    10.Times(() => {
      for (int i=0; i<N; ++i) {

        // dump path
        for (int j=0, p=i; j<N; ++j, p=next[p]) path[j] = p;

        long wrapped;
        wrapped = L.Mod(num[i] * magic, N - 1);
        if (wrapped == 0) continue;
        var ip = path[wrapped];

        // remove num
        next[prev[i]] = next[i];
        prev[next[i]] = prev[i];

        // insert
        {var suc = next[ip];
        next[ip] = i;
        prev[i] = ip;
        next[i] = suc;
        prev[suc] = i;}}});

    pos = num.FindIndex(n => n == 0);
    1000.Times(() => { pos = next[pos]; }); p2 += num[pos] * magic;
    1000.Times(() => { pos = next[pos]; }); p2 += num[pos] * magic;
    1000.Times(() => { pos = next[pos]; }); p2 += num[pos] * magic;

    Console.WriteLine(p1);
    Console.WriteLine(p2); }}