using System.Net;

namespace rqdq.aoc22 {

class App {
  const string kSessionKey = "RQDQ__AOC__SESSION_KEY";
  static readonly Uri kAocDomain = new("https://adventofcode.com");
  const int kYearNum = 2022;
  const int kDayNum = 3;

  delegate void Solver(ReadOnlySpan<byte> text);

  static int Main(string[] argv) {
    var appDataDir = Environment.GetEnvironmentVariable("APPDATA");
    if (appDataDir == null) {
      throw new Exception("missing envvar APPDATA"); }
    var yearDir = Path.Join(appDataDir, $@"rqdq\aoc\{kYearNum}");
    var dataPath = Path.Join(yearDir, $"{kDayNum:D2}.txt");

    if (!File.Exists(dataPath)) {
      // try to retrieve from web
      var token = Environment.GetEnvironmentVariable(kSessionKey);
      if (token == null) {
        throw new Exception($"data file not found and {kSessionKey} session key not available"); }
      Directory.CreateDirectory(yearDir);
      var data = RetrieveInput(token, kYearNum, kDayNum);
      File.WriteAllText(dataPath, data); }

    using (var mm = new rcls.FileLoader(dataPath)) {
      var solver = SolverFactory(kDayNum);
      solver(mm.AsSpan()); }
    return 0; }

    static
    Solver SolverFactory(int daynum) {
      // XXX dedupe using reflection??
      switch (daynum) {
      case 1: return text => new Day01().Solve(text);
      case 2: return text => new Day02().Solve(text);
      case 3: return text => new Day03().Solve(text); }
      throw new Exception($"no factory impl for day {daynum}"); }

    static
    string RetrieveInput(string token, int year, int day) {
      var cookieContainer = new CookieContainer();
      cookieContainer.Add(kAocDomain, new Cookie("session", token));
      var client = new HttpClient(new HttpClientHandler() {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        CookieContainer = cookieContainer });
      client.BaseAddress = kAocDomain;
      var response = client.GetAsync($@"{year}/day/{day}/input").Result;
      response.EnsureSuccessStatusCode();
      string data = response.Content.ReadAsStringAsync().Result;
      return data; } }


}  // close package namespace