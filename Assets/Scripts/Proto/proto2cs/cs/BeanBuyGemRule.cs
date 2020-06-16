// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_buy_gem_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_buy_gem_rule.proto</summary>
  public static partial class BeanBuyGemRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_buy_gem_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanBuyGemRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdiZWFuX2J1eV9nZW1fcnVsZS5wcm90bxIJY29tLnByb3RvGgpiYXNlLnBy",
            "b3RvIlkKDEJ1eUdlbVJ1bGVQQhIfCghidXlfdHlwZRgBIAEoDjINLkJ1eUdl",
            "bVR5cGVQQhILCgNudW0YAiABKBESCwoDZ2VtGAMgASgREg4KBmFtb3VudBgE",
            "IAEoEUIzCh9uZXQuZ2FsYXNwb3J0cy5iaWdzdGFyLnByb3RvY29sQhBCdXlH",
            "ZW1SdWxlUHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.BuyGemRulePB), global::Com.Proto.BuyGemRulePB.Parser, new[]{ "BuyType", "Num", "Gem", "Amount" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///BuyGemRulePB BuyGemRule
  /// </summary>
  public sealed partial class BuyGemRulePB : pb::IMessage<BuyGemRulePB> {
    private static readonly pb::MessageParser<BuyGemRulePB> _parser = new pb::MessageParser<BuyGemRulePB>(() => new BuyGemRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<BuyGemRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanBuyGemRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public BuyGemRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public BuyGemRulePB(BuyGemRulePB other) : this() {
      buyType_ = other.buyType_;
      num_ = other.num_;
      gem_ = other.gem_;
      amount_ = other.amount_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public BuyGemRulePB Clone() {
      return new BuyGemRulePB(this);
    }

    /// <summary>Field number for the "buy_type" field.</summary>
    public const int BuyTypeFieldNumber = 1;
    private global::BuyGemTypePB buyType_ = 0;
    /// <summary>
    ///购买类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::BuyGemTypePB BuyType {
      get { return buyType_; }
      set {
        buyType_ = value;
      }
    }

    /// <summary>Field number for the "num" field.</summary>
    public const int NumFieldNumber = 2;
    private int num_;
    /// <summary>
    ///数量
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Num {
      get { return num_; }
      set {
        num_ = value;
      }
    }

    /// <summary>Field number for the "gem" field.</summary>
    public const int GemFieldNumber = 3;
    private int gem_;
    /// <summary>
    ///宝石数量
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Gem {
      get { return gem_; }
      set {
        gem_ = value;
      }
    }

    /// <summary>Field number for the "amount" field.</summary>
    public const int AmountFieldNumber = 4;
    private int amount_;
    /// <summary>
    ///数量
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Amount {
      get { return amount_; }
      set {
        amount_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as BuyGemRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(BuyGemRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (BuyType != other.BuyType) return false;
      if (Num != other.Num) return false;
      if (Gem != other.Gem) return false;
      if (Amount != other.Amount) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (BuyType != 0) hash ^= BuyType.GetHashCode();
      if (Num != 0) hash ^= Num.GetHashCode();
      if (Gem != 0) hash ^= Gem.GetHashCode();
      if (Amount != 0) hash ^= Amount.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (BuyType != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) BuyType);
      }
      if (Num != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(Num);
      }
      if (Gem != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Gem);
      }
      if (Amount != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(Amount);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (BuyType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) BuyType);
      }
      if (Num != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Num);
      }
      if (Gem != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Gem);
      }
      if (Amount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Amount);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(BuyGemRulePB other) {
      if (other == null) {
        return;
      }
      if (other.BuyType != 0) {
        BuyType = other.BuyType;
      }
      if (other.Num != 0) {
        Num = other.Num;
      }
      if (other.Gem != 0) {
        Gem = other.Gem;
      }
      if (other.Amount != 0) {
        Amount = other.Amount;
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
            buyType_ = (global::BuyGemTypePB) input.ReadEnum();
            break;
          }
          case 16: {
            Num = input.ReadSInt32();
            break;
          }
          case 24: {
            Gem = input.ReadSInt32();
            break;
          }
          case 32: {
            Amount = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code