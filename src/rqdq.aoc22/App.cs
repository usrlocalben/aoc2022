namespace rqdq.aoc22 {

using Day = Day02;

class App {
  static int Main(string[] argv) {
    var appDataDir = Environment.GetEnvironmentVariable("APPDATA");
    var path = Path.Join(appDataDir, @"rqdq\aoc\2022", Day.fileName);
    using (var mm = new rcls.FileLoader(path)) {
      new Day().Solve(mm.AsSpan()); }
    return 0; } }


}  // close package namespace