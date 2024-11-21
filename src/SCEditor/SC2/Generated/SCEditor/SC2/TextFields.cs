// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace SCEditor.SC2
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct TextFields : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_24_3_25(); }
  public static TextFields GetRootAsTextFields(ByteBuffer _bb) { return GetRootAsTextFields(_bb, new TextFields()); }
  public static TextFields GetRootAsTextFields(ByteBuffer _bb, TextFields obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public static bool VerifyTextFields(ByteBuffer _bb) {Google.FlatBuffers.Verifier verifier = new Google.FlatBuffers.Verifier(_bb); return verifier.VerifyBuffer("", false, TextFieldsVerify.Verify); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public TextFields __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public SCEditor.SC2.TextField? Textfields(int j) { int o = __p.__offset(4); return o != 0 ? (SCEditor.SC2.TextField?)(new SCEditor.SC2.TextField()).__assign(__p.__vector(o) + j * 40, __p.bb) : null; }
  public int TextfieldsLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<SCEditor.SC2.TextFields> CreateTextFields(FlatBufferBuilder builder,
      VectorOffset textfieldsOffset = default(VectorOffset)) {
    builder.StartTable(1);
    TextFields.AddTextfields(builder, textfieldsOffset);
    return TextFields.EndTextFields(builder);
  }

  public static void StartTextFields(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddTextfields(FlatBufferBuilder builder, VectorOffset textfieldsOffset) { builder.AddOffset(0, textfieldsOffset.Value, 0); }
  public static void StartTextfieldsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(40, numElems, 4); }
  public static Offset<SCEditor.SC2.TextFields> EndTextFields(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<SCEditor.SC2.TextFields>(o);
  }
  public static void FinishTextFieldsBuffer(FlatBufferBuilder builder, Offset<SCEditor.SC2.TextFields> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixedTextFieldsBuffer(FlatBufferBuilder builder, Offset<SCEditor.SC2.TextFields> offset) { builder.FinishSizePrefixed(offset.Value); }
}


static public class TextFieldsVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyVectorOfData(tablePos, 4 /*Textfields*/, 40 /*SCEditor.SC2.TextField*/, false)
      && verifier.VerifyTableEnd(tablePos);
  }
}

}
