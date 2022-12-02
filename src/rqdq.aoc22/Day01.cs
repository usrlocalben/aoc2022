using System.Buffers.Text;
using BTU = rqdq.rclt.ByteTextUtil;

namespace rqdq.aoc22 {

class Day01 {
  public const string fileName = "01.txt";

  public void Solve(ReadOnlySpan<byte> text) {
    PriorityQueue<int, int> elf = new();
    int ax = 0;
    while (!text.IsEmpty) {
      var line = BTU.PopLine(ref text);
      if (!line.IsEmpty) {
        Utf8Parser.TryParse(line, out int num, out _);
        ax += num; }
      else {
        elf.Enqueue(ax, ax);
        while (elf.Count > 3) elf.Dequeue();
        ax = 0; }}
    elf.Enqueue(ax, ax);
    while (elf.Count > 3) elf.Dequeue();

    var third  = elf.Dequeue();
    var second = elf.Dequeue();
    var first  = elf.Dequeue();

    Console.WriteLine(first);
    Console.WriteLine(first + second + third); }}


}  // close package namespace
