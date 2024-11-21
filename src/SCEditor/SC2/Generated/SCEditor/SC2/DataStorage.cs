// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace SCEditor.SC2
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct DataStorage : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_24_3_25(); }
  public static DataStorage GetRootAsDataStorage(ByteBuffer _bb) { return GetRootAsDataStorage(_bb, new DataStorage()); }
  public static DataStorage GetRootAsDataStorage(ByteBuffer _bb, DataStorage obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public static bool VerifyDataStorage(ByteBuffer _bb) {Google.FlatBuffers.Verifier verifier = new Google.FlatBuffers.Verifier(_bb); return verifier.VerifyBuffer("", false, DataStorageVerify.Verify); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public DataStorage __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  /// An array of strings that objects from other chunks access by their number 
  /// Example: strings[ref_id - 1]
  public string Strings(int j) { int o = __p.__offset(4); return o != 0 ? __p.__string(__p.__vector(o) + j * 4) : null; }
  public int StringsLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }
  public byte Unk2(int j) { int o = __p.__offset(6); return o != 0 ? __p.bb.Get(__p.__vector(o) + j * 1) : (byte)0; }
  public int Unk2Length { get { int o = __p.__offset(6); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<byte> GetUnk2Bytes() { return __p.__vector_as_span<byte>(6, 1); }
#else
  public ArraySegment<byte>? GetUnk2Bytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetUnk2Array() { return __p.__vector_as_array<byte>(6); }
  public byte Unk3(int j) { int o = __p.__offset(8); return o != 0 ? __p.bb.Get(__p.__vector(o) + j * 1) : (byte)0; }
  public int Unk3Length { get { int o = __p.__offset(8); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<byte> GetUnk3Bytes() { return __p.__vector_as_span<byte>(8, 1); }
#else
  public ArraySegment<byte>? GetUnk3Bytes() { return __p.__vector_as_arraysegment(8); }
#endif
  public byte[] GetUnk3Array() { return __p.__vector_as_array<byte>(8); }
  public SCEditor.SC2.Typing.Rect? Rectangles(int j) { int o = __p.__offset(10); return o != 0 ? (SCEditor.SC2.Typing.Rect?)(new SCEditor.SC2.Typing.Rect()).__assign(__p.__vector(o) + j * 16, __p.bb) : null; }
  public int RectanglesLength { get { int o = __p.__offset(10); return o != 0 ? __p.__vector_len(o) : 0; } }
  public byte MovieclipsFrameElements(int j) { int o = __p.__offset(12); return o != 0 ? __p.bb.Get(__p.__vector(o) + j * 1) : (byte)0; }
  public int MovieclipsFrameElementsLength { get { int o = __p.__offset(12); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<byte> GetMovieclipsFrameElementsBytes() { return __p.__vector_as_span<byte>(12, 1); }
#else
  public ArraySegment<byte>? GetMovieclipsFrameElementsBytes() { return __p.__vector_as_arraysegment(12); }
#endif
  public byte[] GetMovieclipsFrameElementsArray() { return __p.__vector_as_array<byte>(12); }
  public byte ShapesBitmapPoins(int j) { int o = __p.__offset(14); return o != 0 ? __p.bb.Get(__p.__vector(o) + j * 1) : (byte)0; }
  public int ShapesBitmapPoinsLength { get { int o = __p.__offset(14); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<byte> GetShapesBitmapPoinsBytes() { return __p.__vector_as_span<byte>(14, 1); }
#else
  public ArraySegment<byte>? GetShapesBitmapPoinsBytes() { return __p.__vector_as_arraysegment(14); }
#endif
  public byte[] GetShapesBitmapPoinsArray() { return __p.__vector_as_array<byte>(14); }
  public SCEditor.SC2.MatrixBank? MatrixBanks(int j) { int o = __p.__offset(16); return o != 0 ? (SCEditor.SC2.MatrixBank?)(new SCEditor.SC2.MatrixBank()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int MatrixBanksLength { get { int o = __p.__offset(16); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<SCEditor.SC2.DataStorage> CreateDataStorage(FlatBufferBuilder builder,
      VectorOffset stringsOffset = default(VectorOffset),
      VectorOffset unk2Offset = default(VectorOffset),
      VectorOffset unk3Offset = default(VectorOffset),
      VectorOffset rectanglesOffset = default(VectorOffset),
      VectorOffset movieclips_frame_elementsOffset = default(VectorOffset),
      VectorOffset shapes_bitmap_poinsOffset = default(VectorOffset),
      VectorOffset matrix_banksOffset = default(VectorOffset)) {
    builder.StartTable(7);
    DataStorage.AddMatrixBanks(builder, matrix_banksOffset);
    DataStorage.AddShapesBitmapPoins(builder, shapes_bitmap_poinsOffset);
    DataStorage.AddMovieclipsFrameElements(builder, movieclips_frame_elementsOffset);
    DataStorage.AddRectangles(builder, rectanglesOffset);
    DataStorage.AddUnk3(builder, unk3Offset);
    DataStorage.AddUnk2(builder, unk2Offset);
    DataStorage.AddStrings(builder, stringsOffset);
    return DataStorage.EndDataStorage(builder);
  }

  public static void StartDataStorage(FlatBufferBuilder builder) { builder.StartTable(7); }
  public static void AddStrings(FlatBufferBuilder builder, VectorOffset stringsOffset) { builder.AddOffset(0, stringsOffset.Value, 0); }
  public static VectorOffset CreateStringsVector(FlatBufferBuilder builder, StringOffset[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateStringsVectorBlock(FlatBufferBuilder builder, StringOffset[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateStringsVectorBlock(FlatBufferBuilder builder, ArraySegment<StringOffset> data) { builder.StartVector(4, data.Count, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateStringsVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<StringOffset>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartStringsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddUnk2(FlatBufferBuilder builder, VectorOffset unk2Offset) { builder.AddOffset(1, unk2Offset.Value, 0); }
  public static VectorOffset CreateUnk2Vector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateUnk2VectorBlock(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateUnk2VectorBlock(FlatBufferBuilder builder, ArraySegment<byte> data) { builder.StartVector(1, data.Count, 1); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateUnk2VectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<byte>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartUnk2Vector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddUnk3(FlatBufferBuilder builder, VectorOffset unk3Offset) { builder.AddOffset(2, unk3Offset.Value, 0); }
  public static VectorOffset CreateUnk3Vector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateUnk3VectorBlock(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateUnk3VectorBlock(FlatBufferBuilder builder, ArraySegment<byte> data) { builder.StartVector(1, data.Count, 1); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateUnk3VectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<byte>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartUnk3Vector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddRectangles(FlatBufferBuilder builder, VectorOffset rectanglesOffset) { builder.AddOffset(3, rectanglesOffset.Value, 0); }
  public static void StartRectanglesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(16, numElems, 4); }
  public static void AddMovieclipsFrameElements(FlatBufferBuilder builder, VectorOffset movieclipsFrameElementsOffset) { builder.AddOffset(4, movieclipsFrameElementsOffset.Value, 0); }
  public static VectorOffset CreateMovieclipsFrameElementsVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateMovieclipsFrameElementsVectorBlock(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateMovieclipsFrameElementsVectorBlock(FlatBufferBuilder builder, ArraySegment<byte> data) { builder.StartVector(1, data.Count, 1); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateMovieclipsFrameElementsVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<byte>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartMovieclipsFrameElementsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddShapesBitmapPoins(FlatBufferBuilder builder, VectorOffset shapesBitmapPoinsOffset) { builder.AddOffset(5, shapesBitmapPoinsOffset.Value, 0); }
  public static VectorOffset CreateShapesBitmapPoinsVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateShapesBitmapPoinsVectorBlock(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateShapesBitmapPoinsVectorBlock(FlatBufferBuilder builder, ArraySegment<byte> data) { builder.StartVector(1, data.Count, 1); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateShapesBitmapPoinsVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<byte>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartShapesBitmapPoinsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddMatrixBanks(FlatBufferBuilder builder, VectorOffset matrixBanksOffset) { builder.AddOffset(6, matrixBanksOffset.Value, 0); }
  public static VectorOffset CreateMatrixBanksVector(FlatBufferBuilder builder, Offset<SCEditor.SC2.MatrixBank>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateMatrixBanksVectorBlock(FlatBufferBuilder builder, Offset<SCEditor.SC2.MatrixBank>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateMatrixBanksVectorBlock(FlatBufferBuilder builder, ArraySegment<Offset<SCEditor.SC2.MatrixBank>> data) { builder.StartVector(4, data.Count, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateMatrixBanksVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<Offset<SCEditor.SC2.MatrixBank>>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartMatrixBanksVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<SCEditor.SC2.DataStorage> EndDataStorage(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<SCEditor.SC2.DataStorage>(o);
  }
  public static void FinishDataStorageBuffer(FlatBufferBuilder builder, Offset<SCEditor.SC2.DataStorage> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixedDataStorageBuffer(FlatBufferBuilder builder, Offset<SCEditor.SC2.DataStorage> offset) { builder.FinishSizePrefixed(offset.Value); }
}


static public class DataStorageVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyVectorOfStrings(tablePos, 4 /*Strings*/, false)
      && verifier.VerifyVectorOfData(tablePos, 6 /*Unk2*/, 1 /*byte*/, false)
      && verifier.VerifyVectorOfData(tablePos, 8 /*Unk3*/, 1 /*byte*/, false)
      && verifier.VerifyVectorOfData(tablePos, 10 /*Rectangles*/, 16 /*SCEditor.SC2.Typing.Rect*/, false)
      && verifier.VerifyVectorOfData(tablePos, 12 /*MovieclipsFrameElements*/, 1 /*byte*/, false)
      && verifier.VerifyVectorOfData(tablePos, 14 /*ShapesBitmapPoins*/, 1 /*byte*/, false)
      && verifier.VerifyVectorOfTables(tablePos, 16 /*MatrixBanks*/, SCEditor.SC2.MatrixBankVerify.Verify, false)
      && verifier.VerifyTableEnd(tablePos);
  }
}

}
