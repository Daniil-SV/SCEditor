// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace SCEditor.SC2.Typing
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct Matrix2x3 : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p = new Struct(_i, _bb); }
  public Matrix2x3 __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public float A { get { return __p.bb.GetFloat(__p.bb_pos + 0); } }
  public float B { get { return __p.bb.GetFloat(__p.bb_pos + 4); } }
  public float C { get { return __p.bb.GetFloat(__p.bb_pos + 8); } }
  public float D { get { return __p.bb.GetFloat(__p.bb_pos + 12); } }
  public float Tx { get { return __p.bb.GetFloat(__p.bb_pos + 16); } }
  public float Ty { get { return __p.bb.GetFloat(__p.bb_pos + 20); } }

  public static Offset<SCEditor.SC2.Typing.Matrix2x3> CreateMatrix2x3(FlatBufferBuilder builder, float A, float B, float C, float D, float Tx, float Ty) {
    builder.Prep(4, 24);
    builder.PutFloat(Ty);
    builder.PutFloat(Tx);
    builder.PutFloat(D);
    builder.PutFloat(C);
    builder.PutFloat(B);
    builder.PutFloat(A);
    return new Offset<SCEditor.SC2.Typing.Matrix2x3>(builder.Offset);
  }
}


}