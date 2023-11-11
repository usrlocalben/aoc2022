namespace rqdq.aoc22;
  
internal static class App {
  private const string SessionKey = "RQDQ__AOC__SESSION_KEY";
  private static readonly DateTime beginDt = new(2022, 12, 1);
  private static readonly DateTime endDt = new(2022, 12, 26);
  private static readonly TimeSpan oneDay = new(1, 0, 0, 0);

  private static int Main(string[] argv) {
    var appDataDir = Environment.GetEnvironmentVariable("APPDATA");
    if (appDataDir == null) {
      throw new Exception("missing envvar APPDATA"); }
    var token = Environment.GetEnvironmentVariable(SessionKey);
    if (token == null) {
      throw new Exception($"data file not found and {SessionKey} session key not available"); }

    var retriever = new InputRetriever(token);

    for (var dt=beginDt; dt<endDt; dt += oneDay) {
      var yearDir = Path.Join(appDataDir, $@"rqdq\aoc\{dt.Year}");
      var dataPath = Path.Join(yearDir, $"{dt.Day:D2}.txt");

      if (!File.Exists(dataPath)) {
        // try to retrieve from web
        Directory.CreateDirectory(yearDir);
        var data = retriever.Fetch(dt);
        File.WriteAllText(dataPath, data); }

      using var mm = new rcls.FileLoader(dataPath);
      SolverFactory(dt).Solve(mm.AsSpan());
      Console.WriteLine(); }
    return 0; }

  private static
  ISolution SolverFactory(DateTime dt) {
    var type = Type.GetType($"rqdq.aoc22.Day{dt.Day:D2}");
    if (type == null) {
      throw new Exception($"no factory impl for day {dt.Day}"); }
    if (Activator.CreateInstance(type) is ISolution instance) {
      return instance; }
    else {
      throw new Exception("expected ISolution"); }}}


internal static class L {

  public static
  IVec2[] NESW = new IVec2[4] { new(0,-1), new(0,1), new(-1,0), new(1,0) };

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

  public static
  void Times(this int many, Action action) {
    for (int i=0; i<many; ++i) {
      action(); } }

  public static
  void Resize<T>(this List<T> list, int size, T element = default(T)) {
    int count = list.Count;

    if (size < count) {
      list.RemoveRange(size, count - size); }
    else if (size > count) {
      if (size > list.Capacity) {   // Optimization
        list.Capacity = size; }
      list.AddRange(Enumerable.Repeat(element, size - count)); } }

  public static
  void Resize<T>(this List<T> list, int size) where T : new() {
    int count = list.Count;

    if (size < count) {
      list.RemoveRange(size, count - size); }
    else if (size > count) {
      if (size > list.Capacity) {   // Optimization
        list.Capacity = size; }
      for (int n=0; n<size - count; ++n) {
        list.Add(new T()); } } }

  public static
  IVec2 MapDimSafe(ReadOnlySpan<byte> map) {
      int h = 1;
      int w = ByteTextUtil.PopLine(ref map).Length;
      while (!map.IsEmpty) {
        int lw = ByteTextUtil.PopLine(ref map).Length;
        if (lw != w) {
          throw new Exception("saw unexpected width while probing dimensions. first={w} line{h + 1}={lw}"); }
        ++h; }
      return new(w, h); }

  public static
  IVec2 MapDim(ReadOnlySpan<byte> map) {
      var sizeInBytes = map.Length;
      int w = ByteTextUtil.PopLine(ref map).Length;
      int h = sizeInBytes / (w + 1);
      return new IVec2(w, h); }

  public static
  int stoi(ReadOnlySpan<byte> text) {
    System.Buffers.Text.Utf8Parser.TryParse(text, out int num, out _);
    return num; }

  public static
  IEnumerable<Tuple<int, int>> Split(string text) {
    if (text.Length > 0) {
      int level=0, j=0, k;
      while (true) {
        bool found = false;
        for (k=j; k<text.Length; ++k) {
               if (text[k] == '[') { ++level; }
          else if (text[k] == ']') { --level; }
          else if (text[k] == ',' && level == 0) {
            yield return new Tuple<int, int>(j, k);
            j = k + 1;
            found = true;
            break; }}
        if (!found) {
          break; }}
      yield return new Tuple<int, int>(j, k); } }

  public static
  long Pow(long a, long b) {
    if (b == 0) return 1;
    if (b == 1) return a;
    var tmp = Pow(a, b/2);
    if (b % 2 == 0) {
      return tmp * tmp; }
    else {
      return a * tmp * tmp;}}

  public static
  long GCF(long a, long b) {
    while (b != 0) {
      var temp = b;
      b = a % b;
      a = temp; }
    return a; }

  public static
  long LCM(long a, long b) {
    return (a / GCF(a, b)) * b; }}