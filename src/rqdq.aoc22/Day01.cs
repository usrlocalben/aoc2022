using BTU = rqdq.rclt.ByteTextUtil;

namespace rqdq.aoc22 {

class Day01 : ISolution {
  public void Solve(ReadOnlySpan<byte> text) {
    PriorityQueue<int, int> elf = new();
    elf.Enqueue(0,0);
    elf.Enqueue(0,0);
    elf.Enqueue(0,0);
    int ax = 0;
    while (!text.IsEmpty) {
      var line = BTU.PopLine(ref text);
      if (!line.IsEmpty) {
        ax += stoi(line); }
      else {
        elf.EnqueueDequeue(ax, ax);
        ax = 0; }}
    elf.EnqueueDequeue(ax, ax);

    var third  = elf.Dequeue();
    var second = elf.Dequeue();
    var first  = elf.Dequeue();

    Console.WriteLine(first);
    Console.WriteLine(first + second + third); }

  int stoi(ReadOnlySpan<byte> text) {
    System.Buffers.Text.Utf8Parser.TryParse(text, out int num, out _);
    return num; }}


}  // close package namespace
