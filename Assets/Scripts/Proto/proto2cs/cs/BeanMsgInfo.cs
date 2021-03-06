// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_msg_info.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_msg_info.proto</summary>
  public static partial class BeanMsgInfoReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_msg_info.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanMsgInfoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChNiZWFuX21zZ19pbmZvLnByb3RvEgljb20ucHJvdG8aCmJhc2UucHJvdG8i",
            "KwoJTXNnSW5mb1BCEg8KB21zZ19rZXkYASABKAkSDQoFZXh0X2kYAiABKBFC",
            "MAofbmV0LmdhbGFzcG9ydHMuYmlnc3Rhci5wcm90b2NvbEINTXNnSW5mb1By",
            "b3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.MsgInfoPB), global::Com.Proto.MsgInfoPB.Parser, new[]{ "MsgKey", "ExtI" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///MsgInfoPB MsgInfo
  /// </summary>
  public sealed partial class MsgInfoPB : pb::IMessage<MsgInfoPB> {
    private static readonly pb::MessageParser<MsgInfoPB> _parser = new pb::MessageParser<MsgInfoPB>(() => new MsgInfoPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MsgInfoPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanMsgInfoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MsgInfoPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MsgInfoPB(MsgInfoPB other) : this() {
      msgKey_ = other.msgKey_;
      extI_ = other.extI_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MsgInfoPB Clone() {
      return new MsgInfoPB(this);
    }

    /// <summary>Field number for the "msg_key" field.</summary>
    public const int MsgKeyFieldNumber = 1;
    private string msgKey_ = "";
    /// <summary>
    ///消息KEY名称
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MsgKey {
      get { return msgKey_; }
      set {
        msgKey_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "ext_i" field.</summary>
    public const int ExtIFieldNumber = 2;
    private int extI_;
    /// <summary>
    ///额外信息
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ExtI {
      get { return extI_; }
      set {
        extI_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MsgInfoPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MsgInfoPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MsgKey != other.MsgKey) return false;
      if (ExtI != other.ExtI) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (MsgKey.Length != 0) hash ^= MsgKey.GetHashCode();
      if (ExtI != 0) hash ^= ExtI.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (MsgKey.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(MsgKey);
      }
      if (ExtI != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(ExtI);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (MsgKey.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MsgKey);
      }
      if (ExtI != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ExtI);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MsgInfoPB other) {
      if (other == null) {
        return;
      }
      if (other.MsgKey.Length != 0) {
        MsgKey = other.MsgKey;
      }
      if (other.ExtI != 0) {
        ExtI = other.ExtI;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            MsgKey = input.ReadString();
            break;
          }
          case 16: {
            ExtI = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
