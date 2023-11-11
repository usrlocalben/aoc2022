using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

class Day05 : ISolution {

  public void Solve(ReadOnlySpan<byte> text) {
    const int N = 9;
    List<Stack<char>> stack1 = new(N);  stack1.Resize(N);
    List<Stack<char>> stack2 = new(N);  stack2.Resize(N);
    Stack<char> flip = new();

    // find picture/instrs break (and picture height)
    int height = 0;
    var picture = text;
    for (;; ++height) {
      if (BTU.PopLine(ref text).IsEmpty) {
        break; }}

    // load stacks from picture
    const int width = N * 4;
    for (int y=height - 1; y>=0; --y) {
      for (int i=0; i<N; ++i) {
        var ch = (char)picture[y*width + i*4+1];
        if (ch != ' ') {
          stack1[i].Push(ch);
          stack2[i].Push(ch); }}}

    var t = text;
    while (!t.IsEmpty) {
      BTU.PopWord(ref t);                    BTU.ConsumeSpace(ref t);
      BTU.ConsumeValue(ref t, out int many); BTU.ConsumeSpace(ref t);
      BTU.PopWord(ref t);                    BTU.ConsumeSpace(ref t);
      BTU.ConsumeValue(ref t, out int src);  BTU.ConsumeSpace(ref t);
      BTU.PopWord(ref t);                    BTU.ConsumeSpace(ref t);
      BTU.ConsumeValue(ref t, out int dst);  BTU.ConsumeSpace(ref t);
      --src; --dst; // 0+

      // p1
      many.Times(() => stack1[dst].Push(stack1[src].Pop()));

      // p2
      many.Times(() => flip.Push(stack2[src].Pop()));
      many.Times(() => stack2[dst].Push(flip.Pop())); }

    var p1 = stack1.Aggregate("", (ax, it) => ax + it.Peek());
    var p2 = stack2.Aggregate("", (ax, it) => ax + it.Peek());
    Console.WriteLine(p1);
    Console.WriteLine(p2); } }
