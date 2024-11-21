// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace SCEditor.SC2
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct MovieClipFrame : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public MovieClipFrame __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public uint UsedTransform { get { return __p.bb.GetUint(__p.bb_pos + 0); } }
  public uint LabelRefId { get { return __p.bb.GetUint(__p.bb_pos + 4); } }

  public static Offset<SCEditor.SC2.MovieClipFrame> CreateMovieClipFrame(FlatBufferBuilder builder, uint UsedTransform, uint LabelRefId) {
    builder.Prep(4, 8);
    builder.PutUint(LabelRefId);
    builder.PutUint(UsedTransform);
    return new Offset<SCEditor.SC2.MovieClipFrame>(builder.Offset);
  }
}


}