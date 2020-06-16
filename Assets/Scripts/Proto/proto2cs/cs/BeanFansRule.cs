// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_fans_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_fans_rule.proto</summary>
  public static partial class BeanFansRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_fans_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanFansRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChRiZWFuX2ZhbnNfcnVsZS5wcm90bxIJY29tLnByb3RvGgpiYXNlLnByb3Rv",
            "IlQKCkZhbnNSdWxlUEISEQoJZmFuc190eXBlGAEgASgREg0KBXBvd2VyGAIg",
            "ASgREhEKCWZhbnNfbmFtZRgDIAEoCRIRCglmYW5zX2Rlc2MYBCABKAlCMQof",
            "bmV0LmdhbGFzcG9ydHMuYmlnc3Rhci5wcm90b2NvbEIORmFuc1J1bGVQcm90",
            "b3NiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.FansRulePB), global::Com.Proto.FansRulePB.Parser, new[]{ "FansType", "Power", "FansName", "FansDesc" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///ItemPB Item
  /// </summary>
  public sealed partial class FansRulePB : pb::IMessage<FansRulePB> {
    private static readonly pb::MessageParser<FansRulePB> _parser = new pb::MessageParser<FansRulePB>(() => new FansRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FansRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanFansRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FansRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FansRulePB(FansRulePB other) : this() {
      fansType_ = other.fansType_;
      power_ = other.power_;
      fansName_ = other.fansName_;
      fansDesc_ = other.fansDesc_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FansRulePB Clone() {
      return new FansRulePB(this);
    }

    /// <summary>Field number for the "fans_type" field.</summary>
    public const int FansTypeFieldNumber = 1;
    private int fansType_;
    /// <summary>
    ///id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int FansType {
      get { return fansType_; }
      set {
        fansType_ = value;
      }
    }

    /// <summary>Field number for the "power" field.</summary>
    public const int PowerFieldNumber = 2;
    private int power_;
    /// <summary>
    ///粉丝提供的能力
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Power {
      get { return power_; }
      set {
        power_ = value;
      }
    }

    /// <summary>Field number for the "fans_name" field.</summary>
    public const int FansNameFieldNumber = 3;
    private string fansName_ = "";
    /// <summary>
    ///fans名字
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string FansName {
      get { return fansName_; }
      set {
        fansName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "fans_desc" field.</summary>
    public const int FansDescFieldNumber = 4;
    private string fansDesc_ = "";
    /// <summary>
    ///fans描述
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string FansDesc {
      get { return fansDesc_; }
      set {
        fansDesc_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FansRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FansRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (FansType != other.FansType) return false;
      if (Power != other.Power) return false;
      if (FansName != other.FansName) return false;
      if (FansDesc != other.FansDesc) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (FansType != 0) hash ^= FansType.GetHashCode();
      if (Power != 0) hash ^= Power.GetHashCode();
      if (FansName.Length != 0) hash ^= FansName.GetHashCode();
      if (FansDesc.Length != 0) hash ^= FansDesc.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (FansType != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(FansType);
      }
      if (Power != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(Power);
      }
      if (FansName.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(FansName);
      }
      if (FansDesc.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(FansDesc);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (FansType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(FansType);
      }
      if (Power != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Power);
      }
      if (FansName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(FansName);
      }
      if (FansDesc.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(FansDesc);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FansRulePB other) {
      if (other == null) {
        return;
      }
      if (other.FansType != 0) {
        FansType = other.FansType;
      }
      if (other.Power != 0) {
        Power = other.Power;
      }
      if (other.FansName.Length != 0) {
        FansName = other.FansName;
      }
      if (other.FansDesc.Length != 0) {
        FansDesc = other.FansDesc;
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
            FansType = input.ReadSInt32();
            break;
          }
          case 16: {
            Power = input.ReadSInt32();
            break;
          }
          case 26: {
            FansName = input.ReadString();
            break;
          }
          case 34: {
            FansDesc = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
