// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_card_star_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_card_star_rule.proto</summary>
  public static partial class BeanCardStarRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_card_star_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanCardStarRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChliZWFuX2NhcmRfc3Rhcl9ydWxlLnByb3RvEgljb20ucHJvdG8aCmJhc2Uu",
            "cHJvdG8iZAoOQ2FyZFN0YXJSdWxlUEISGQoGY3JlZGl0GAEgASgOMgkuQ3Jl",
            "ZGl0UEISFQoEc3RhchgCIAEoDjIHLlN0YXJQQhINCgVwb3dlchgDIAEoERIR",
            "CgltYXhfbGV2ZWwYBCABKBFCNQofbmV0LmdhbGFzcG9ydHMuYmlnc3Rhci5w",
            "cm90b2NvbEISQ2FyZFN0YXJSdWxlUHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.CardStarRulePB), global::Com.Proto.CardStarRulePB.Parser, new[]{ "Credit", "Star", "Power", "MaxLevel" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///CardStarRulePB CardStarRule
  /// </summary>
  public sealed partial class CardStarRulePB : pb::IMessage<CardStarRulePB> {
    private static readonly pb::MessageParser<CardStarRulePB> _parser = new pb::MessageParser<CardStarRulePB>(() => new CardStarRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CardStarRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanCardStarRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CardStarRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CardStarRulePB(CardStarRulePB other) : this() {
      credit_ = other.credit_;
      star_ = other.star_;
      power_ = other.power_;
      maxLevel_ = other.maxLevel_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CardStarRulePB Clone() {
      return new CardStarRulePB(this);
    }

    /// <summary>Field number for the "credit" field.</summary>
    public const int CreditFieldNumber = 1;
    private global::CreditPB credit_ = 0;
    /// <summary>
    ///id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::CreditPB Credit {
      get { return credit_; }
      set {
        credit_ = value;
      }
    }

    /// <summary>Field number for the "star" field.</summary>
    public const int StarFieldNumber = 2;
    private global::StarPB star_ = 0;
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::StarPB Star {
      get { return star_; }
      set {
        star_ = value;
      }
    }

    /// <summary>Field number for the "power" field.</summary>
    public const int PowerFieldNumber = 3;
    private int power_;
    /// <summary>
    ///power
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Power {
      get { return power_; }
      set {
        power_ = value;
      }
    }

    /// <summary>Field number for the "max_level" field.</summary>
    public const int MaxLevelFieldNumber = 4;
    private int maxLevel_;
    /// <summary>
    ///当前星级可以升到的最大等级
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MaxLevel {
      get { return maxLevel_; }
      set {
        maxLevel_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CardStarRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CardStarRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Credit != other.Credit) return false;
      if (Star != other.Star) return false;
      if (Power != other.Power) return false;
      if (MaxLevel != other.MaxLevel) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Credit != 0) hash ^= Credit.GetHashCode();
      if (Star != 0) hash ^= Star.GetHashCode();
      if (Power != 0) hash ^= Power.GetHashCode();
      if (MaxLevel != 0) hash ^= MaxLevel.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Credit != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Credit);
      }
      if (Star != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) Star);
      }
      if (Power != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Power);
      }
      if (MaxLevel != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(MaxLevel);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Credit != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Credit);
      }
      if (Star != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Star);
      }
      if (Power != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Power);
      }
      if (MaxLevel != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(MaxLevel);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CardStarRulePB other) {
      if (other == null) {
        return;
      }
      if (other.Credit != 0) {
        Credit = other.Credit;
      }
      if (other.Star != 0) {
        Star = other.Star;
      }
      if (other.Power != 0) {
        Power = other.Power;
      }
      if (other.MaxLevel != 0) {
        MaxLevel = other.MaxLevel;
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
            credit_ = (global::CreditPB) input.ReadEnum();
            break;
          }
          case 16: {
            star_ = (global::StarPB) input.ReadEnum();
            break;
          }
          case 24: {
            Power = input.ReadSInt32();
            break;
          }
          case 32: {
            MaxLevel = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code