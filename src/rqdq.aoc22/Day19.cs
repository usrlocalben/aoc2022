using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

class Day19 : ISolution {
  byte[,,] BP;

  public void Solve(ReadOnlySpan<byte> t) {
    long p1 = 0, p2 = 0;

// Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 20 clay.
// Each geode robot costs 2 ore and 12 obsidian.
    BP = new byte[30,4,4];

    while (!t.IsEmpty) {
// "Blueprint 1: "
      BTU.PopWordSp(ref t);
      BTU.ConsumeValue(ref t, out int b); BTU.ConsumeChar(ref t); BTU.ConsumeSpace(ref t);
      b--;

// "Each ore robot costs 4 ore. "
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.ConsumeValue(ref t, out int bOre_ore); BTU.ConsumeSpace(ref t);
      BTU.PopWordSp(ref t);

// "Each clay robot costs 4 ore. "
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.ConsumeValue(ref t, out int bClay_ore); BTU.ConsumeSpace(ref t);
      BTU.PopWordSp(ref t);

// "Each obsidian robot costs 4 ore and 20 clay."
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.ConsumeValue(ref t, out int bObsidian_ore); BTU.ConsumeSpace(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.ConsumeValue(ref t, out int bObsidian_clay); BTU.ConsumeSpace(ref t);
      BTU.PopWordSp(ref t);

// Each geode robot costs 2 ore and 12 obsidian.
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.ConsumeValue(ref t, out int bGeode_ore); BTU.ConsumeSpace(ref t);
      BTU.PopWordSp(ref t);
      BTU.PopWordSp(ref t);
      BTU.ConsumeValue(ref t, out int bGeode_obsidian); BTU.ConsumeSpace(ref t);
      BTU.PopWordSp(ref t);
      BTU.ConsumeSpace(ref t); 

      BP[b,0,0] = (byte)bOre_ore;
      BP[b,1,0] = (byte)bClay_ore;
      BP[b,2,1] = (byte)bObsidian_clay;
      BP[b,2,0] = (byte)bObsidian_ore;
      BP[b,3,0] = (byte)bGeode_ore;
      BP[b,3,2] = (byte)bGeode_obsidian;
      // Console.WriteLine($"{b} ore:{bOre_ore} clay:{bClay_ore} obs:{bObsidian_ore},{bObsidian_clay} geode:{bGeode_ore},{bGeode_obsidian}"); 
      }


      for (int bi=0; bi<30; ++bi) {
        int[] bot = new int[] { 1,0,0,0 };
        int[] newBot = new int[4];
        int[] qty = new int[4];
        int[] tmp = new int[4];
        _memo.Clear();
        int geodes = DP(bi, 24, 1,0,0,0, 0,0,0,0);
        // Console.Write($"{bi} ");
        p1 += (bi+1)*geodes; }
      // Console.WriteLine();
      Console.WriteLine(p1);

      p2 = 1;
      for (int bi=0; bi<3; ++bi) {
        int[] bot = new int[] { 1,0,0,0 };
        int[] newBot = new int[4];
        int[] qty = new int[4];
        int[] tmp = new int[4];
        _memo.Clear();
        int geodes = DP(bi, 32, 1,0,0,0, 0,0,0,0);
        // Console.Write($"{bi} ");
        p2 *= geodes; }
      // Console.WriteLine();
      Console.WriteLine(p2); }

    Dictionary<string, int> _memo = new();
    int DP(int bi, int minutes, int b0,int b1, int b2, int b3, int q0, int q1, int q2, int q3) {
      const int oo = 9999;

      // Idea: we can only buy one bot/day, so with enough
      // bots, we effectively have infinite money. clamping
      // the values in the key represents this, and gives
      // the answer in ~2 seconds for pt.2.
      // I get AC but I'm not positive this a correct algorithm.

      // MNOP are the most that we could spend in one turn
      int M = Math.Max(Math.Max(Math.Max(BP[bi,0,0], BP[bi,1,0]), BP[bi,2,0]), BP[bi,3,0]);
      int N = Math.Max(Math.Max(Math.Max(BP[bi,0,1], BP[bi,1,1]), BP[bi,2,1]), BP[bi,3,1]);
      int O = Math.Max(Math.Max(Math.Max(BP[bi,0,2], BP[bi,1,2]), BP[bi,2,2]), BP[bi,3,2]);
      int P = Math.Max(Math.Max(Math.Max(BP[bi,0,3], BP[bi,1,3]), BP[bi,2,3]), BP[bi,3,3]);

      // clamp bots to maximum-useful, clamp money to max-spend
      var key = $"{minutes},{(b0>=M?M:b0)},{(b1>=N?N:b1)},{(b2>=O?O:b2)},{b3}," + 
                $"{(q0>M+1?oo:q0)},{(q1>N+1?oo:q1)},{(q2>O+1?oo:q2)},{q3}";
      if (_memo.TryGetValue(key, out int c)) {
        return c; }

      if (minutes == 0) {
        return q3; }

      int best = 0;

      for (int type=0; type<4; ++type) {
        if (q0 >= BP[bi,type,0] &&
            q1 >= BP[bi,type,1] &&
            q2 >= BP[bi,type,2])  {
          // we can afford this type

          // pay cost, receive bot output
          var _q3 = q3 - BP[bi,type,3] + b3;
          var _q2 = q2 - BP[bi,type,2] + b2;
          var _q1 = q1 - BP[bi,type,1] + b1;
          var _q0 = q0 - BP[bi,type,0] + b0;
          // new bots ready
          var _b0 = type==0 ? b0 + 1 : b0;
          var _b1 = type==1 ? b1 + 1 : b1;
          var _b2 = type==2 ? b2 + 1 : b2;
          var _b3 = type==3 ? b3 + 1 : b3;

          int amt = DP(bi, (byte)(minutes - 1), _b0,_b1,_b2,_b3, _q0,_q1,_q2,_q3);
          best = Math.Max(best,amt); }}

      {
        // wait instead of buy
        int amt = DP(bi, (byte)(minutes - 1), b0,b1,b2,b3, q0+b0,q1+b1,q2+b2,q3+b3);
        best = Math.Max(best,amt); }

    return _memo[key] = best; }}