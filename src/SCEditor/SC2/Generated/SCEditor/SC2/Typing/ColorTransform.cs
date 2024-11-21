// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace SCEditor.SC2.Typing
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct ColorTransform : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public ColorTransform __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public byte RMul { get { return __p.bb.Get(__p.bb_pos + 0); } }
  public byte GMul { get { return __p.bb.Get(__p.bb_pos + 1); } }
  public byte BMul { get { return __p.bb.Get(__p.bb_pos + 2); } }
  public byte Alpha { get { return __p.bb.Get(__p.bb_pos + 3); } }
  public byte RAdd { get { return __p.bb.Get(__p.bb_pos + 4); } }
  public byte GAdd { get { return __p.bb.Get(__p.bb_pos + 5); } }
  public byte BAdd { get { return __p.bb.Get(__p.bb_pos + 6); } }

  public static Offset<SCEditor.SC2.Typing.ColorTransform> CreateColorTransform(FlatBufferBuilder builder, byte RMul, byte GMul, byte BMul, byte Alpha, byte RAdd, byte GAdd, byte BAdd) {
    builder.Prep(1, 7);
    builder.PutByte(BAdd);
    builder.PutByte(GAdd);
    builder.PutByte(RAdd);
    builder.PutByte(Alpha);
    builder.PutByte(BMul);
    builder.PutByte(GMul);
    builder.PutByte(RMul);
    return new Offset<SCEditor.SC2.Typing.ColorTransform>(builder.Offset);
  }
}


}
