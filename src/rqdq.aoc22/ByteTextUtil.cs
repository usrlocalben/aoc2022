using System.Buffers.Text;
using System.Runtime.InteropServices;
using System.Text;

namespace rqdq.aoc22;

/// <summary>
/// utility methods for text-parsing over a span in-place (i.e.
/// mutating the passed span). facilitates writing parsers that
/// avoid copies or allocation.
/// </summary>
public static
class ByteTextUtil {

  const byte NL = (byte)'\n';  // new-line char as byte
  const byte CR = (byte)'\r';  // carriage-return as byte (DOS format)
  const byte SP = (byte)' ';   // space char as byte
  const byte TAB= (byte)'\t';   // space char as byte

  /// <summary>
  /// split a span at the first newline, and "pop" it by advancing
  /// the begin pos of the input span, and returning the line. if
  /// a carriage-return precedes the newline, it is removed. if no
  /// new-line is found, then the entire input is returned and the
  /// input span is cleared (i.e. it is the last line)
  /// </summary>
  /// <param name="text">ascii text span to extract from</param>
  /// <returns>first line of text</returns>
  public static
  ReadOnlySpan<byte> PopLine(ref ReadOnlySpan<byte> text) {
    return PopDelim(ref text, '\n'); }

  /// <summary>
  /// split a span at the first newline, and "pop" it by advancing
  /// the begin pos of the input span, and returning the line. if
  /// a carriage-return precedes the newline, it is removed. if no
  /// new-line is found, then the entire input is returned and the
  /// input span is cleared (i.e. it is the last line)
  /// </summary>
  /// <param name="text">ascii text span to extract from</param>
  /// <returns>first line of text</returns>
  public static
  ReadOnlySpan<byte> PopDelim(ref ReadOnlySpan<byte> text, char delim) {
    var pos = text.IndexOf((byte)delim);
    ReadOnlySpan<byte> extract;
    if (pos == -1) {
      // not found, this is the last line
      extract = text;
      text = ReadOnlySpan<byte>.Empty; }
    else {
      var endPos = pos;
      if (pos > 0 && text[pos-1] == CR) {
        --endPos; /* DOS-format detected */ }
      extract = text[..endPos];
      text = text[(pos+1)..]; }
    return extract; } 

  /// <summary>
  /// extract the first space-delimited word/token from text
  /// </summary>
  /// <param name="text">text span to extract from</param>
  /// <returns>first word</returns>
  public static
  ReadOnlySpan<byte> PopWord(ref ReadOnlySpan<byte> text) {

    var pos = -1;
    for (var i=0; i<text.Length; ++i) {
      if (text[i]==SP || text[i]==TAB || text[i]==CR || text[i]==NL) {
        pos = i;
        break; }}

    // var pos = text.IndexOf(SP);


    ReadOnlySpan<byte> extract;
    if (pos == -1) {
      extract = text;
      text = ReadOnlySpan<byte>.Empty; }
    else {
      extract = text[..pos];
      text = text[(pos+1)..].TrimStart(SP); }
    return extract; }

  public static
  ReadOnlySpan<byte> PopWordSp(ref ReadOnlySpan<byte> text) {
    var tmp = PopWord(ref text);
    ConsumeSpace(ref text);
    return tmp; }

  /// <summary>
  /// remove leading spaces from a span in-place
  /// </summary>
  /// <param name="text">span to remove from</param>
  public static
  void LTrim(ref ReadOnlySpan<byte> text) {
    text = text.TrimStart(SP); }

  public static
  void ConsumeSpace(ref ReadOnlySpan<byte> text) {
    while (!text.IsEmpty && (text[0]==SP || text[0]==TAB || text[0]==CR || text[0]==NL)) {
      text = text[1..]; }}

  /// <summary>
  /// try to remove a prefix from a span
  /// </summary>
  /// <param name="text">span to compare and remove from</param>
  /// <param name="prefix">prefix as ascii bytes</param>
  /// <returns>false unless the prefix was matched and removed</returns>
  public static
  bool ConsumePrefix(ref ReadOnlySpan<byte> text, byte[] prefix) {
    if (text.StartsWith(prefix)) {
      text = text[prefix.Length..];
      return true; }
    return false; }

  public static
  bool ConsumeValue(ref ReadOnlySpan<byte> text, out float value) {
    var good = Utf8Parser.TryParse(text, out value, out var taken);
    if (good) {
      text = text[taken..]; }
    return good; }

  public static
  bool ConsumeValue(ref ReadOnlySpan<byte> text, out int value) {
    var good = Utf8Parser.TryParse(text, out value, out var taken);
    if (good) {
      text = text[taken..]; }
    return good; }

  public static unsafe
  string Decode(in ReadOnlySpan<byte> text) {
    fixed (byte* begin = &MemoryMarshal.GetReference(text)) {
      var lengthInCodepoints = Encoding.UTF8.GetCharCount(begin, text.Length);
      fixed (char* buf = stackalloc char[lengthInCodepoints]) {
        var lengthInChars = Encoding.UTF8.GetChars(begin, text.Length, buf, lengthInCodepoints);
        return new string(buf, 0, lengthInChars); } } }

  /// <summary>
  /// consume a single character from a span if possible
  /// </summary>
  /// <param name="text"></param>
  /// <returns>false unless a character was removed</returns>
  public static
  bool ConsumeChar(ref ReadOnlySpan<byte> text) {
    if (text.IsEmpty) return false;
    text = text[1..];
    return true; }}
