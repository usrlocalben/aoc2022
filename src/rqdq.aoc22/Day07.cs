﻿using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using BTU = rqdq.rclt.ByteTextUtil;

namespace rqdq.aoc22 {

class Day07 : ISolution {

  class INode {
    public Dictionary<string, int> dirs = new();
    public int anc;
    public int size = 0; };

  List<INode> _node = new() { (new(){anc=-1}) };
  const int root = 0;
  int _cwd = root;

  public
  void Solve(ReadOnlySpan<byte> t) {
    while (!t.IsEmpty) {
      var token = BTU.PopWordSp(ref t);
      if (token[0] == '$') {
        // command
        var cmd = BTU.PopWordSp(ref t);
        if (cmd[0] == 'c'/*d*/) {
          Chdir(BTU.Decode(BTU.PopWordSp(ref t))); }
        else /*cmd[0] == 'l's)*/ {
          /* pass */ }}
      else if (token[0] == 'd'/*ir*/) {
        BTU.PopWordSp(ref t);
        /* noop */ }
      else {
        // file entry
        int bytes = stoi(token);
        BTU.PopWordSp(ref t);
        for (int walk = _cwd; walk>=0; walk=_node[walk].anc) {
          _node[walk].size += bytes; }}}
       
    var p1 = _node.Select(it => it.size)
                  .Where(it => it < 100000)
                  .Sum();

    const int capacity = 70000000;
    const int requirement = 30000000;
    var consumed = _node[root].size;
    var free = capacity - consumed;
    var deficit = requirement - free;

    var p2 = _node.Select(it => it.size)
                  .Where(it => it >= deficit)
                  .Min();

    Console.WriteLine(p1);
    Console.WriteLine(p2);}

  void Chdir(string arg) {
    var here = _node[_cwd];
    if (arg == "..") {
      _cwd = here.anc; }
    else if (arg == "/") {
      _cwd = root; }
    else {
      if (!here.dirs.ContainsKey(arg)) {
        _node.Add(new INode() { anc = _cwd });
        here.dirs[arg] = _node.Count - 1; }
      _cwd = here.dirs[arg]; }}
     
  int stoi(ReadOnlySpan<byte> text) {
    System.Buffers.Text.Utf8Parser.TryParse(text, out int num, out _);
    return num; }}


}  // close package namespace
