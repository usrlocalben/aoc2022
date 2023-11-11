using BTU = rqdq.aoc22.ByteTextUtil;

namespace rqdq.aoc22;

class Day08 : ISolution {
  public
  void Solve(ReadOnlySpan<byte> map) {
    long p1 = 0, p2 = 0;

    const int dim = 99;
    const int stride = dim + 1;
    for (int y=0; y<dim; ++y) {
    for (int x=0; x<dim; ++x) {
      var here = map[y*stride+x] - (byte)'0';

      bool gn,gs,ge,gw; gn = gs = ge = gw = true;
      long cn,cs,ce,cw; cn = cs = ce = cw = 0;

      for (int cx=x-1; cx>=0; --cx) {
        ++cw;
        if (map[y*stride + cx] - (byte)'0' >= here) {
          gw = false;
          break; }}

      for (int cx=x+1; cx<dim; ++cx) {
        ++ce;
        if (map[y*stride + cx] - (byte)'0' >= here) {
          ge = false;
          break; }}

      for (int cy=y-1; cy>=0; --cy) {
        ++cn;
        if (map[cy*stride + x] - (byte)'0' >= here) {
          gn = false;
          break; }}

      for (int cy=y+1; cy<dim; ++cy) {
        ++cs;
        if (map[cy*stride + x] - (byte)'0' >= here) {
          gs = false;
          break; }}

      p1 += gn||gs||ge||gw ? 1 : 0;
      p2 = Math.Max(p2, cn * cs * ce * cw); }}
        
    Console.WriteLine(p1);
    Console.WriteLine(p2);}}