// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_npc.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_npc.proto</summary>
  public static partial class BeanNpcReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_npc.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanNpcReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5iZWFuX25wYy5wcm90bxIJY29tLnByb3RvGgpiYXNlLnByb3RvIikKBU5w",
            "Y1BCEg4KBm5wY19pZBgBIAEoERIQCghucGNfbmFtZRgCIAEoCUIsCh9uZXQu",
            "Z2FsYXNwb3J0cy5iaWdzdGFyLnByb3RvY29sQglOcGNQcm90b3NiBnByb3Rv",
            "Mw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.NpcPB), global::Com.Proto.NpcPB.Parser, new[]{ "NpcId", "NpcName" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///NpcPB Npc
  /// </summary>
  public sealed partial class NpcPB : pb::IMessage<NpcPB> {
    private static readonly pb::MessageParser<NpcPB> _parser = new pb::MessageParser<NpcPB>(() => new NpcPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<NpcPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanNpcReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public NpcPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public NpcPB(NpcPB other) : this() {
      npcId_ = other.npcId_;
      npcName_ = other.npcName_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public NpcPB Clone() {
      return new NpcPB(this);
    }

    /// <summary>Field number for the "npc_id" field.</summary>
    public const int NpcIdFieldNumber = 1;
    private int npcId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int NpcId {
      get { return npcId_; }
      set {
        npcId_ = value;
      }
    }

    /// <summary>Field number for the "npc_name" field.</summary>
    public const int NpcNameFieldNumber = 2;
    private string npcName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string NpcName {
      get { return npcName_; }
      set {
        npcName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as NpcPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(NpcPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (NpcId != other.NpcId) return false;
      if (NpcName != other.NpcName) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (NpcId != 0) hash ^= NpcId.GetHashCode();
      if (NpcName.Length != 0) hash ^= NpcName.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (NpcId != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(NpcId);
      }
      if (NpcName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(NpcName);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (NpcId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(NpcId);
      }
      if (NpcName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(NpcName);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(NpcPB other) {
      if (other == null) {
        return;
      }
      if (other.NpcId != 0) {
        NpcId = other.NpcId;
      }
      if (other.NpcName.Length != 0) {
        NpcName = other.NpcName;
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
          case 8: {
            NpcId = input.ReadSInt32();
            break;
          }
          case 18: {
            NpcName = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code