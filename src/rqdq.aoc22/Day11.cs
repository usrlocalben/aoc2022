using System.Numerics;
using BTU = rqdq.rclt.ByteTextUtil;
using rqdq.rmlv;
using System.Globalization;
using System.Diagnostics;

namespace rqdq.aoc22 {

class Day11 : ISolution {
  const int NN = 8;

  struct Mono {
    public int pow, mul, add, mod, dt, df;
    public long Eval(long n) => n * L.Pow(n, pow) * mul + add;
    public long Dest(long n) => n % mod == 0 ? dt : df; }

  Mono[] mono = new Mono[NN];
  long[] mload = new long[NN];
  long p2mod = 1;
  List<long>[] inv1 = new List<long>[NN];
  List<long>[] inv2 = new List<long>[NN];

  public
  void Solve(ReadOnlySpan<byte> t) {
    for (int i=0; i<NN; ++i) {
      inv1[i] = new();
      inv2[i] = new();
      BTU.PopLine(ref t); // monkey n:

      BTU.ConsumeSpace(ref t);
      // starting items:
      BTU.PopWordSp(ref t); BTU.PopWordSp(ref t);
      for (;;) {
        BTU.ConsumeValue(ref t, out int worry);
        inv1[i].Add(worry);
        inv2[i].Add(worry);
        if (t[0] == (byte)'\n') {
          break; }
        BTU.PopWordSp(ref t);  /* ", " */ }

      BTU.ConsumeSpace(ref t);

      // operation new = old
      BTU.PopWordSp(ref t); BTU.PopWordSp(ref t); BTU.PopWordSp(ref t); BTU.PopWordSp(ref t);
      var op = BTU.PopWordSp(ref t); // "*" or "+"
      var data = BTU.Decode(BTU.PopWordSp(ref t));
      mono[i].pow = 0; mono[i].mul = 1; mono[i].add = 0;
      if (data == "old") {
        if (op[0] != (byte)'*') {
          throw new Exception("unexpected op with old"); }
        mono[i].pow = 1; }
      else if (int.TryParse(data, out int xx)) {
        if (op[0] == (byte)'+') mono[i].add = xx;
        else mono[i].mul = xx; }
      else {
        throw new Exception("cant parse int for op"); }
        
      BTU.ConsumeSpace(ref t);

      // test divisible by
      BTU.PopWordSp(ref t); BTU.PopWordSp(ref t); BTU.PopWordSp(ref t);
      BTU.ConsumeValue(ref t, out int divBy);
      mono[i].mod = divBy;
      p2mod = L.LCM(p2mod, divBy);

      BTU.ConsumeSpace(ref t);

      // if true throw to monkey
      BTU.PopWordSp(ref t); BTU.PopWordSp(ref t); BTU.PopWordSp(ref t); BTU.PopWordSp(ref t); BTU.PopWordSp(ref t);
      BTU.ConsumeValue(ref t, out int trueDest);
      mono[i].dt = trueDest;

      BTU.ConsumeSpace(ref t);

      // if false throw to monkey
      BTU.PopWordSp(ref t); BTU.PopWordSp(ref t); BTU.PopWordSp(ref t); BTU.PopWordSp(ref t); BTU.PopWordSp(ref t);
      BTU.ConsumeValue(ref t, out int falseDest);
      mono[i].df = falseDest;

      BTU.ConsumeSpace(ref t); }

    // part 1
    for (int r=0; r<20; ++r) {
      for (int m=0; m<NN; ++m) {
        foreach (var it in inv1[m]) {
          var level = mono[m].Eval(it) / 3;
          var dest = mono[m].Dest(level);
          inv1[dest].Add(level); }
        mload[m] += inv1[m].Count;
        inv1[m].Clear(); }}
    var p1 = mload.OrderByDescending(n => n)
                  .Take(2)
                  .Aggregate(1L, (ax, it) => ax*it);

    // part 2
    Array.Clear(mload);
    for (int r=0; r<10000; ++r) {
      for (int m=0; m<NN; ++m) {
        foreach (var it in inv2[m]) {
          var level = mono[m].Eval(it) % p2mod;
          var dest = mono[m].Dest(level);
          inv2[dest].Add(level); }
        mload[m] += inv2[m].Count;
        inv2[m].Clear(); }}
    var p2 = mload.OrderByDescending(n => n)
                  .Take(2)
                  .Aggregate(1L, (ax, it) => ax*it);

    Console.WriteLine(p1);
    Console.WriteLine(p2); }}


}  // close package namespace
