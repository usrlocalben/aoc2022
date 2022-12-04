using System.Net;

namespace rqdq.aoc22 {

class App {
  const string kSessionKey = "RQDQ__AOC__SESSION_KEY";
  static readonly Uri kAocDomain = new("https://adventofcode.com");
  const int kYearNum = 2022;
  const int kDayBegin = 4;
  const int kDayEnd = kDayBegin + 1;

  static int Main(string[] argv) {
    var appDataDir = Environment.GetEnvironmentVariable("APPDATA");
    if (appDataDir == null) {
      throw new Exception("missing envvar APPDATA"); }

    for (int day=kDayBegin; day<kDayEnd; ++day) {
      var yearDir = Path.Join(appDataDir, $@"rqdq\aoc\{kYearNum}");
      var dataPath = Path.Join(yearDir, $"{day:D2}.txt");

      if (!File.Exists(dataPath)) {
        // try to retrieve from web
        var token = Environment.GetEnvironmentVariable(kSessionKey);
        if (token == null) {
          throw new Exception($"data file not found and {kSessionKey} session key not available"); }
        Directory.CreateDirectory(yearDir);
        var data = RetrieveInput(token, kYearNum, day);
        File.WriteAllText(dataPath, data); }

      using (var mm = new rcls.FileLoader(dataPath)) {
        SolverFactory(day).Solve(mm.AsSpan()); }}
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

static
class L {

  public static
  rmlv.IVec2[] NESW = new rmlv.IVec2[4] { new(0,-1), new(0,1), new(-1,0), new(1,0) };

  public const long oo64 = 0x3f3f3f3f3f3f3f3f;
  public const int oo = 0x3f3f3f3f;

  public static
  long Mod(long a, long b) {
    long tmp = a % b;
    return tmp < 0 ? tmp + b : tmp; }

  public static
  int Unhex(byte a) {
    if ('0' <= a && a <= '9') {
      return a - '0'; }
    if ('a' <= a && a <= 'f') {
      return a - 'a' + 10; }
    if ('A' <= a && a <= 'F') {
      return a - 'A' + 10; }
    throw new Exception($"bad hex digit \"{(char)a}\""); }

  public static
  (byte, bool) Decomp(byte a) {
    if ('a' <= a && a <= 'z') {
      return (a, false); }
    if ('A' <= a && a <= 'Z') {
      int x = a - 'A' + 'a';
      return ((byte)x, true); }
    return (a, false); }


  }  // L

}  // close package namespace