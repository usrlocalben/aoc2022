using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

class Day01 : ISolution {
  public void Solve(ReadOnlySpan<byte> text) {
    PriorityQueue<int, int> elf = new();
    elf.Enqueue(0,0);
    elf.Enqueue(0,0);
    elf.Enqueue(0,0);
    var ax = 0;
    while (!text.IsEmpty) {
      var line = BTU.PopLine(ref text);
      if (!line.IsEmpty) {
        ax += L.stoi(line); }
      else {
        elf.EnqueueDequeue(ax, ax);
        ax = 0; }}
    elf.EnqueueDequeue(ax, ax);

    var third  = elf.Dequeue();
    var second = elf.Dequeue();
    var first  = elf.Dequeue();

    Console.WriteLine(first);
    Console.WriteLine(first + second + third); }}
