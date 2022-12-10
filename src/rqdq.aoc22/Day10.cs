/*
#### #  # ###  ###  #### ####  ##  #
   # #  # #  # #  # #    #    #  # #
  #  #  # #  # #  # ###  ###  #    #
 #   #  # ###  ###  #    #    #    #
#    #  # #    # #  #    #    #  # #
####  ##  #    #  # #    ####  ##  ####
uint z = 0111100010010010010001111b
uint u = 0100110011001100110010110b
uint p = 0111010011001111010001000b
uint r = 0111010011001111010101001b
uint f = 0111110001110100010001000b
uint e = 0111110001110100010001111b
uint c = 0011010011000100010010110b
uint l = 0100010001000100010001111b
*/
using BTU = rqdq.rclt.ByteTextUtil;
using rqdq.rmlv;

namespace rqdq.aoc22 {

class Day10 : ISolution {

  enum opcode { addx, noop }

  struct instr {
    public opcode op;
    public int data; }

  public
  void Solve(ReadOnlySpan<byte> t) {
    List<instr> pgm = new();
    pgm.Add(new () { op = opcode.noop, data = 0 });  // noop for reset
    while (!t.IsEmpty) {
      var dir = BTU.PopWordSp(ref t);
      if (dir[0] == 'a') {
        BTU.ConsumeValue(ref t, out int data);
        BTU.ConsumeSpace(ref t);
        pgm.Add( new() { op = opcode.addx, data = data }); }
      else {
        pgm.Add(new () { op = opcode.noop, data = 0 }); }}

    const int W = 40, H = 6;
    char[] frame = new char[H*W];

    const int p1period = 40;
    int p1hit = 20, p1 = 0;
    int rx = 1, ip = 0;
    for (int cycle=0, tick=0; cycle<240; ++cycle, --tick) {

      if (tick == 0) {
        switch (pgm[ip].op) {
        case opcode.noop: break;
        case opcode.addx: {
          rx += pgm[ip].data; }
          break; }
        ++ip;
        tick = pgm[ip].op == opcode.addx ? 2 : 1; }

      if (cycle+1 == p1hit) {
        p1 += (cycle+1) * rx;
        p1hit += p1period; }

      var raster = cycle % W;
      frame[cycle] = (rx-1) <= raster && raster <=(rx+1) ? '#' : ' '; }

    Console.WriteLine(p1);
    for (int y=0; y<H; ++y) {
      for (int x=0; x<W; ++x) {
        Console.Write(frame[y*W+x]); }
      Console.WriteLine(); } } }


}  // close package namespace
