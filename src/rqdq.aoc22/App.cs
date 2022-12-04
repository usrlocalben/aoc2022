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
      SolverFactory(kDayNum).Solve(mm.AsSpan()); }
    return 0; }

    static
    ISolution SolverFactory(int day) {
      var type = Type.GetType($"rqdq.aoc22.Day{day:D2}");
      if (type == null) {
        throw new Exception($"no factory impl for day {day}"); }
      return (ISolution)Activator.CreateInstance(type); }

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