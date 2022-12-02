using System.Buffers.Text;
using BTU = rqdq.rclt.ByteTextUtil;

namespace rqdq.aoc22 {

class Day01 {
  public const string fileName = "01.txt";

  public void Solve(ReadOnlySpan<byte> text) {
    PriorityQueue<int, int> elf = new();
    elf.Enqueue(0,0);
    elf.Enqueue(0,0);
    elf.Enqueue(0,0);
    int ax = 0;
    while (!text.IsEmpty) {
      var line = BTU.PopLine(ref text);
      if (!line.IsEmpty) {
        Utf8Parser.TryParse(line, out int num, out _);
        ax += num; }
      else {
        elf.EnqueueDequeue(ax, ax);
        ax = 0; }}
    elf.EnqueueDequeue(ax, ax);

    var third  = elf.Dequeue();
    var second = elf.Dequeue();
    var first  = elf.Dequeue();

    Console.WriteLine(first);
    Console.WriteLine(first + second + third); }}


}  // close package namespace
