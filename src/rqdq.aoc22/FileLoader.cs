using Microsoft.Win32.SafeHandles;
using System.IO.MemoryMappedFiles;

namespace rqdq.rcls {

public
class FileLoader : IDisposable {

  private long _sizeInBytes;
  private MemoryMappedFile _mm;
  private MemoryMappedViewStream _vs;
  private SafeMemoryMappedViewHandle _mmv;

  public
  FileLoader(string fn) {
    _sizeInBytes = new System.IO.FileInfo(fn).Length;
    _mm = MemoryMappedFile.CreateFromFile(fn, FileMode.Open);
    _vs = _mm.CreateViewStream();
    _mmv = _vs.SafeMemoryMappedViewHandle; }

  public
  void Dispose() {
    _mmv.Dispose();
    _vs.Dispose();
    _mm.Dispose(); }

  public
  ReadOnlySpan<byte> AsSpan() {
    ReadOnlySpan<byte> bytes;
    unsafe {
      byte* ptrMemMap = (byte*)0;
      _mmv.AcquirePointer(ref ptrMemMap);
      bytes = new ReadOnlySpan<byte>(ptrMemMap, (int)_sizeInBytes);
      _mmv.ReleasePointer(); }
      return bytes; }}


}  // close package namespace
