// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace SCEditor.SC2
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct MovieClip : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_24_3_25(); }
  public static MovieClip GetRootAsMovieClip(ByteBuffer _bb) { return GetRootAsMovieClip(_bb, new MovieClip()); }
  public static MovieClip GetRootAsMovieClip(ByteBuffer _bb, MovieClip obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public MovieClip __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public ushort Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetUshort(o + __p.bb_pos) : (ushort)0; } }
  public uint? ExportNameRefId { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint?)null; } }
  public byte Framerate { get { int o = __p.__offset(8); return o != 0 ? __p.bb.Get(o + __p.bb_pos) : (byte)0; } }
  public ushort FramesCount { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetUshort(o + __p.bb_pos) : (ushort)0; } }
  public byte UnknownBool { get { int o = __p.__offset(12); return o != 0 ? __p.bb.Get(o + __p.bb_pos) : (byte)0; } }
  public ushort ChildrenIds(int j) { int o = __p.__offset(14); return o != 0 ? __p.bb.GetUshort(__p.__vector(o) + j * 2) : (ushort)0; }
  public int ChildrenIdsLength { get { int o = __p.__offset(14); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<ushort> GetChildrenIdsBytes() { return __p.__vector_as_span<ushort>(14, 2); }
#else
  public ArraySegment<byte>? GetChildrenIdsBytes() { return __p.__vector_as_arraysegment(14); }
#endif
  public ushort[] GetChildrenIdsArray() { return __p.__vector_as_array<ushort>(14); }
  public uint ChildrenNameRefIds(int j) { int o = __p.__offset(16); return o != 0 ? __p.bb.GetUint(__p.__vector(o) + j * 4) : (uint)0; }
  public int ChildrenNameRefIdsLength { get { int o = __p.__offset(16); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<uint> GetChildrenNameRefIdsBytes() { return __p.__vector_as_span<uint>(16, 4); }
#else
  public ArraySegment<byte>? GetChildrenNameRefIdsBytes() { return __p.__vector_as_arraysegment(16); }
#endif
  public uint[] GetChildrenNameRefIdsArray() { return __p.__vector_as_array<uint>(16); }
  public byte ChildrenBlending(int j) { int o = __p.__offset(18); return o != 0 ? __p.bb.Get(__p.__vector(o) + j * 1) : (byte)0; }
  public int ChildrenBlendingLength { get { int o = __p.__offset(18); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<byte> GetChildrenBlendingBytes() { return __p.__vector_as_span<byte>(18, 1); }
#else
  public ArraySegment<byte>? GetChildrenBlendingBytes() { return __p.__vector_as_arraysegment(18); }
#endif
  public byte[] GetChildrenBlendingArray() { return __p.__vector_as_array<byte>(18); }
  public SCEditor.SC2.MovieClipFrame? Frames(int j) { int o = __p.__offset(20); return o != 0 ? (SCEditor.SC2.MovieClipFrame?)(new SCEditor.SC2.MovieClipFrame()).__assign(__p.__vector(o) + j * 8, __p.bb) : null; }
  public int FramesLength { get { int o = __p.__offset(20); return o != 0 ? __p.__vector_len(o) : 0; } }
  public uint FrameElementsOffset { get { int o = __p.__offset(22); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public uint MatrixBankIndex { get { int o = __p.__offset(24); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint)0; } }
  public uint? ScalingGridIndex { get { int o = __p.__offset(26); return o != 0 ? __p.bb.GetUint(o + __p.bb_pos) : (uint?)null; } }

  public static Offset<SCEditor.SC2.MovieClip> CreateMovieClip(FlatBufferBuilder builder,
      ushort id = 0,
      uint? export_name_ref_id = null,
      byte framerate = 0,
      ushort frames_count = 0,
      byte unknown_bool = 0,
      VectorOffset children_idsOffset = default(VectorOffset),
      VectorOffset children_name_ref_idsOffset = default(VectorOffset),
      VectorOffset children_blendingOffset = default(VectorOffset),
      VectorOffset framesOffset = default(VectorOffset),
      uint frame_elements_offset = 0,
      uint matrix_bank_index = 0,
      uint? scaling_grid_index = null) {
    builder.StartTable(12);
    MovieClip.AddScalingGridIndex(builder, scaling_grid_index);
    MovieClip.AddMatrixBankIndex(builder, matrix_bank_index);
    MovieClip.AddFrameElementsOffset(builder, frame_elements_offset);
    MovieClip.AddFrames(builder, framesOffset);
    MovieClip.AddChildrenBlending(builder, children_blendingOffset);
    MovieClip.AddChildrenNameRefIds(builder, children_name_ref_idsOffset);
    MovieClip.AddChildrenIds(builder, children_idsOffset);
    MovieClip.AddExportNameRefId(builder, export_name_ref_id);
    MovieClip.AddFramesCount(builder, frames_count);
    MovieClip.AddId(builder, id);
    MovieClip.AddUnknownBool(builder, unknown_bool);
    MovieClip.AddFramerate(builder, framerate);
    return MovieClip.EndMovieClip(builder);
  }

  public static void StartMovieClip(FlatBufferBuilder builder) { builder.StartTable(12); }
  public static void AddId(FlatBufferBuilder builder, ushort id) { builder.AddUshort(0, id, 0); }
  public static void AddExportNameRefId(FlatBufferBuilder builder, uint? exportNameRefId) { builder.AddUint(1, exportNameRefId); }
  public static void AddFramerate(FlatBufferBuilder builder, byte framerate) { builder.AddByte(2, framerate, 0); }
  public static void AddFramesCount(FlatBufferBuilder builder, ushort framesCount) { builder.AddUshort(3, framesCount, 0); }
  public static void AddUnknownBool(FlatBufferBuilder builder, byte unknownBool) { builder.AddByte(4, unknownBool, 0); }
  public static void AddChildrenIds(FlatBufferBuilder builder, VectorOffset childrenIdsOffset) { builder.AddOffset(5, childrenIdsOffset.Value, 0); }
  public static VectorOffset CreateChildrenIdsVector(FlatBufferBuilder builder, ushort[] data) { builder.StartVector(2, data.Length, 2); for (int i = data.Length - 1; i >= 0; i--) builder.AddUshort(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateChildrenIdsVectorBlock(FlatBufferBuilder builder, ushort[] data) { builder.StartVector(2, data.Length, 2); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateChildrenIdsVectorBlock(FlatBufferBuilder builder, ArraySegment<ushort> data) { builder.StartVector(2, data.Count, 2); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateChildrenIdsVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<ushort>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartChildrenIdsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(2, numElems, 2); }
  public static void AddChildrenNameRefIds(FlatBufferBuilder builder, VectorOffset childrenNameRefIdsOffset) { builder.AddOffset(6, childrenNameRefIdsOffset.Value, 0); }
  public static VectorOffset CreateChildrenNameRefIdsVector(FlatBufferBuilder builder, uint[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddUint(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateChildrenNameRefIdsVectorBlock(FlatBufferBuilder builder, uint[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateChildrenNameRefIdsVectorBlock(FlatBufferBuilder builder, ArraySegment<uint> data) { builder.StartVector(4, data.Count, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateChildrenNameRefIdsVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<uint>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartChildrenNameRefIdsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddChildrenBlending(FlatBufferBuilder builder, VectorOffset childrenBlendingOffset) { builder.AddOffset(7, childrenBlendingOffset.Value, 0); }
  public static VectorOffset CreateChildrenBlendingVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateChildrenBlendingVectorBlock(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateChildrenBlendingVectorBlock(FlatBufferBuilder builder, ArraySegment<byte> data) { builder.StartVector(1, data.Count, 1); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateChildrenBlendingVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<byte>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartChildrenBlendingVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddFrames(FlatBufferBuilder builder, VectorOffset framesOffset) { builder.AddOffset(8, framesOffset.Value, 0); }
  public static void StartFramesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(8, numElems, 4); }
  public static void AddFrameElementsOffset(FlatBufferBuilder builder, uint frameElementsOffset) { builder.AddUint(9, frameElementsOffset, 0); }
  public static void AddMatrixBankIndex(FlatBufferBuilder builder, uint matrixBankIndex) { builder.AddUint(10, matrixBankIndex, 0); }
  public static void AddScalingGridIndex(FlatBufferBuilder builder, uint? scalingGridIndex) { builder.AddUint(11, scalingGridIndex); }
  public static Offset<SCEditor.SC2.MovieClip> EndMovieClip(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<SCEditor.SC2.MovieClip>(o);
  }
}


static public class MovieClipVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyField(tablePos, 4 /*Id*/, 2 /*ushort*/, 2, false)
      && verifier.VerifyField(tablePos, 6 /*ExportNameRefId*/, 4 /*uint*/, 4, false)
      && verifier.VerifyField(tablePos, 8 /*Framerate*/, 1 /*byte*/, 1, false)
      && verifier.VerifyField(tablePos, 10 /*FramesCount*/, 2 /*ushort*/, 2, false)
      && verifier.VerifyField(tablePos, 12 /*UnknownBool*/, 1 /*byte*/, 1, false)
      && verifier.VerifyVectorOfData(tablePos, 14 /*ChildrenIds*/, 2 /*ushort*/, false)
      && verifier.VerifyVectorOfData(tablePos, 16 /*ChildrenNameRefIds*/, 4 /*uint*/, false)
      && verifier.VerifyVectorOfData(tablePos, 18 /*ChildrenBlending*/, 1 /*byte*/, false)
      && verifier.VerifyVectorOfData(tablePos, 20 /*Frames*/, 8 /*SCEditor.SC2.MovieClipFrame*/, false)
      && verifier.VerifyField(tablePos, 22 /*FrameElementsOffset*/, 4 /*uint*/, 4, false)
      && verifier.VerifyField(tablePos, 24 /*MatrixBankIndex*/, 4 /*uint*/, 4, false)
      && verifier.VerifyField(tablePos, 26 /*ScalingGridIndex*/, 4 /*uint*/, 4, false)
      && verifier.VerifyTableEnd(tablePos);
  }
}

}