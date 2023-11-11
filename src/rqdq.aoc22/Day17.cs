namespace rqdq.aoc22;

class Day17 : ISolution {

  public void Solve(ReadOnlySpan<byte> t) {
    long p1 = -1, p2 = -1;

    // sprites & field are upside down!, +y goes "up"
    string o0 = "0000";
    string b0 = "####";

    string o1 = "101";
    string b1 = ".#." +
                "###" +
                ".#.";

    string o2 = "000";
    string b2 = "###" +
                "..#" +
                "..#";

    string o3 = "0";
    string b3 = "#" +
                "#" +
                "#" +
                "#";

    string o4 = "00";
    string b4 = "##" +
                "##";

    // sprites
    List<string> sprMap = new() { b0, b1, b2, b3, b4 };
    List<string> sprOfs = new() { o0, o1, o2, o3, o4 };
    List<int> sprWidth = new() { 4, 3, 3, 1, 2 };
    List<int> sprHeight = new() { 1, 3, 3, 4, 2 };

    // field
    const int flHeight = 10000;
    // int fHeight = 100000;
    const int flWidth = 7;
    char[] flMap = new char[flHeight*flWidth];  Array.Fill(flMap, '.');
    int level = 0;

    // sprite, x,y origin is bottom-left
    int sprIdx = 0;
    int sprX = 2;  // 2 from left
    int sprY = level + 3;

    // p2 stuff
    List<int> deltas = new();
    Dictionary<ulong, int> seen = new();

    int ti = 0;  // input offset
    int rocks = 0;
    while (p1==-1 || p2==-1) {
      var wind = t[ti]; ti = (ti+1)%(t.Length - 1);

      /*for (int yyy=5; yyy>=0; --yyy) {
        for (int xxx=0; xxx<7; ++xxx) {
          Console.Write(map[yyy*7+xxx]); }
        Console.WriteLine(); }*/
       
      //Console.WriteLine($"{bi} {sprX},{sprY}, {wind} {level} {rocks}");

      var sMap = sprMap[sprIdx];
      var sWidth = sprWidth[sprIdx];
      var sHeight = sprHeight[sprIdx];

      bool canMove;
      if (wind == (byte)'<') {
        // moving west
        canMove = false;
        int tx = sprX - 1;
        if (tx >= 0) {
          canMove = true;
          for (int y=0; y<sHeight; ++y) {
            for (int x=0; x<sWidth; ++x) {
              var sc = sMap[y*sWidth + x];
              var fc = flMap[(sprY+y)*flWidth + tx+x];
              if (sc=='#' && fc=='#') {
                canMove = false;
                break; } } } }
        if (canMove) {
          sprX--; } }
      else {
        // moving east
        canMove = false;
        int tx = sprX + 1;
        if (tx <= flWidth-sWidth) {
          canMove = true;
          for (int y=0; y<sHeight; ++y) {
            for (int x=0; x<sWidth; ++x) {
              var sc = sMap[y*sWidth + x];
              var fc = flMap[(sprY+y)*flWidth + tx+x];
              if (sc=='#' && fc=='#') {
                canMove = false;
                break; } } } }
        if (canMove) {
          sprX++; } }

      // south
      canMove = true;
      for (int bx=0; bx<sWidth; ++bx) {
        int by = sprOfs[sprIdx][bx] - '0';
        if (sprY-1 < 0 ||
            // filled pixel in bottom row       and  filled pixel in lower field row
            sMap[by*sWidth+bx]=='#' && flMap[(sprY+by-1)*flWidth + (sprX+bx)] == '#') { 
          canMove = false;
          break; } }
      if (canMove) {
        sprY--; }
      else {
        // rock lands

        // draw rock in field
        for (int y=0; y<sHeight; ++y) {
        for (int x=0; x<sWidth; ++x) {
          if (sMap[y*sWidth+x] == '#') {
            flMap[(sprY+y)*flWidth + sprX+x] = sMap[y*sWidth + x]; } } }

        // update level, track delta for p2
        var newLevel = Math.Max(level, sprY + sHeight);
        var levelChange = newLevel - level;
        if (p2 == -1) {
          deltas.Add(levelChange); }
        level = newLevel;

        // start new rock
        rocks++;
        sprIdx = (sprIdx + 1) % 5;
        sprX = 2;  // 2 from left
        sprY = level + 3;

        if (p1 == -1 && rocks == 2022) {
          p1 = level; }

        if (p2 == -1 && level > 4) {
          ulong key = 0;
          // 28 bits of field
          for (int foo=level; foo>level-4; --foo) {
            for (int xx=0; xx<7; ++xx) {
              key |= flMap[foo*flWidth+xx] == '#' ? 1U : 0U;
              key <<= 1;  } }
          // 15 bits of input pos
          key <<= 15; key |= (uint) ti;
          // 3 bits of piece num
          key <<= 3;  key |= (uint)sprIdx;
          // == 46 bits

          if (seen.ContainsKey(key)) {
            // cycle detected!
            int cycleBegin = seen[key];
            int cycleEnd = rocks;
            int cycleLen = cycleEnd - cycleBegin;

            int cycleLevelDelta = 0;
            for (int ci=cycleBegin; ci<cycleEnd; ++ci) {
              cycleLevelDelta += deltas[ci-1]; }

            long target = 1000000000000;
            var neededRocks = target - rocks;
            var neededCycles = neededRocks / cycleLen;
            if (neededRocks % cycleLen == 0) {
              p2 = (level - 0) + neededCycles * cycleLevelDelta; }}
          else {
            seen[key] = rocks; } } } } // end input

    Console.WriteLine(p1);
    Console.WriteLine(p2); }}
