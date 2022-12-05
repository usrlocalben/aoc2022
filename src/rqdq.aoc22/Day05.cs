using BTU = rqdq.rclt.ByteTextUtil;

namespace rqdq.aoc22 {

class Day05 : ISolution {

  public void Solve(ReadOnlySpan<byte> text) {
    List<Stack<char>> stack1;
    List<Stack<char>> stack2;
    Stack<char> flip;

    const int N = 9;
    const int stride = N * 4;
    int height = 0;
    var picture = text;
    for (;; ++height) {
      var l = BTU.PopLine(ref text);
      if (l.IsEmpty) break; }

    flip = new();
    stack1 = new List<Stack<char>>(N);
    stack2 = new List<Stack<char>>(N);
    for (int i=0; i<N; ++i) {
      stack1.Add(new());
      stack2.Add(new()); }

    for (int y=height - 1; y>=0; --y) {
      for (int i=0; i<9; ++i) {
        var ch = (char)picture[y*stride + i*4+1];
        if (ch != ' ') {
          stack1[i].Push(ch);
          stack2[i].Push(ch); }}}

    while (!text.IsEmpty) {
      var l = BTU.PopLine(ref text);
      BTU.PopWord(ref l);                    BTU.ConsumeSpace(ref l);
      BTU.ConsumeValue(ref l, out int many); BTU.ConsumeSpace(ref l);
      BTU.PopWord(ref l);                    BTU.ConsumeSpace(ref l);
      BTU.ConsumeValue(ref l, out int src);  BTU.ConsumeSpace(ref l);
      BTU.PopWord(ref l);                    BTU.ConsumeSpace(ref l);
      BTU.ConsumeValue(ref l, out int dst);

      // p1
      for (int n=many-1; n>=0; --n) {
        stack1[dst-1].Push(stack1[src-1].Pop()); }

      // p2
      for (int n=0; n<many; ++n) {
        flip.Push(stack2[src-1].Pop()); }
      for (int n=many-1; n>=0; --n) {
        stack2[dst-1].Push(flip.Pop()); }}

    for (int i=0; i<N; ++i) {
      Console.Write(stack1[i].Peek()); }
    Console.WriteLine();
    for (int i=0; i<N; ++i) {
      Console.Write(stack2[i].Peek()); }
    Console.WriteLine(); }}


}  // close package namespace
