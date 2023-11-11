using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

interface IExpr {
  string Name();
  IExpr? Find(string n);
  long Foo(long target);
  long Eval(); }

class Monkey {
  public string name;
  public string Name() => name; }

class MOp : Monkey, IExpr {
  public char op;
  public string ln, rn;
  public IExpr l, r;
  public IExpr? Find(string n) {
    if (n == name) return this;
    var tmp = l.Find(n);
    if (tmp != null) return tmp;
    return r.Find(n); }

  public long Foo(long target) {
    if (l.Find("humn") != null) {
      // left is human
      var rv = r.Eval();
      switch (op) {
      case '+': return l.Foo(target - rv);
      case '-': return l.Foo(target + rv);
      case '*': return l.Foo(target / rv);
      case '/': return l.Foo(target * rv);}
      throw new Exception("badness on the right");}
    else {
      // right is human
      var lv = l.Eval();
      switch(op) {
      case '+': return r.Foo(target - lv);
      case '-': return r.Foo(lv - target);
      case '*': return r.Foo(target / lv);
      case '/': return r.Foo(target / lv);}
      throw new Exception("badness on the left"); }}

  public long Eval() {
    switch (op) {
    case '+': return l.Eval() + r.Eval();
    case '-': return l.Eval() - r.Eval();
    case '*': return l.Eval() * r.Eval();
    case '/': return l.Eval() / r.Eval(); }
    throw new Exception("bad op char"); }}
 
class MValue : Monkey, IExpr {
  public long value;
  public long Eval() => value;
  public long Foo(long target) => target;
  public IExpr? Find(string n) {
    if (n == name) {
      return this; }
    return null; } }
    

class Day21 : ISolution {
  List<IExpr> _m = new();

  public void Solve(ReadOnlySpan<byte> t) {
    long p1 = 0, p2 = 0;

    while (!t.IsEmpty) {
      var l = BTU.PopLine(ref t);
      if (l.Length == 17) {
        // op
        var name = BTU.Decode(l[0..4]);
        var ln = BTU.Decode(l[6..10]);
        var rn = BTU.Decode(l[13..17]);
        var op = (char)l[11];
        // Console.WriteLine($"{name}, {left}, {right}, {op}");
        _m.Add(new MOp () { name = name, ln = ln, rn = rn, op = op }); }
      else {
        // literal
        var name = BTU.Decode(l[0..4]);
        var x = int.Parse(BTU.Decode(l[6..]));
        // Console.WriteLine($"{name}, {x}");
        _m.Add(new MValue () { name = name, value = x }); }}

    Dictionary<string, IExpr> byName = new();
    foreach (var n in _m) {
      byName[n.Name()] = n; }

    // hardlink
    foreach (var n in _m) {
      if (n is MOp inst) {
        inst.l = byName[inst.ln];
        inst.r = byName[inst.rn]; }}

    var root = byName["root"] as MOp;
    p1 = root.Eval();

    IExpr human, other;
    if (root.l.Find("humn") == null) {
      other = root.l;
      human = root.r;}
    else {
      human = root.l;
      other = root.r;}

    var humanSide = root.l == other ? root.r : root.l;
    p2 = humanSide.Foo(other.Eval());

    Console.WriteLine(p1);
    Console.WriteLine(p2); }}