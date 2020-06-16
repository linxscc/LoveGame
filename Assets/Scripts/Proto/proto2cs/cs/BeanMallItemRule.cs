// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_mall_item_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_mall_item_rule.proto</summary>
  public static partial class BeanMallItemRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_mall_item_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanMallItemRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChliZWFuX21hbGxfaXRlbV9ydWxlLnByb3RvEgljb20ucHJvdG8aCmJhc2Uu",
            "cHJvdG8ipQEKDk1hbGxJdGVtUnVsZVBCEg8KB21hbGxfaWQYASABKBESDwoH",
            "aXRlbV9pZBgCIAEoERIgCgptb25leV90eXBlGAMgASgOMgwuTW9uZXlUeXBl",
            "UEISEQoJaXRlYW1fZGVzGAQgASgJEg8KB3VzZV9kZXMYBSABKAkSCwoDbnVt",
            "GAYgASgREg0KBXByaWNlGAcgASgREg8KB21heF9udW0YCCABKBFCNQofbmV0",
            "LmdhbGFzcG9ydHMuYmlnc3Rhci5wcm90b2NvbEISTWFsbEl0ZW1SdWxlUHJv",
            "dG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.MallItemRulePB), global::Com.Proto.MallItemRulePB.Parser, new[]{ "MallId", "ItemId", "MoneyType", "IteamDes", "UseDes", "Num", "Price", "MaxNum" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///MallItemRulePB MallItemRule
  /// </summary>
  public sealed partial class MallItemRulePB : pb::IMessage<MallItemRulePB> {
    private static readonly pb::MessageParser<MallItemRulePB> _parser = new pb::MessageParser<MallItemRulePB>(() => new MallItemRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MallItemRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanMallItemRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MallItemRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MallItemRulePB(MallItemRulePB other) : this() {
      mallId_ = other.mallId_;
      itemId_ = other.itemId_;
      moneyType_ = other.moneyType_;
      iteamDes_ = other.iteamDes_;
      useDes_ = other.useDes_;
      num_ = other.num_;
      price_ = other.price_;
      maxNum_ = other.maxNum_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MallItemRulePB Clone() {
      return new MallItemRulePB(this);
    }

    /// <summary>Field number for the "mall_id" field.</summary>
    public const int MallIdFieldNumber = 1;
    private int mallId_;
    /// <summary>
    ///商品id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MallId {
      get { return mallId_; }
      set {
        mallId_ = value;
      }
    }

    /// <summary>Field number for the "item_id" field.</summary>
    public const int ItemIdFieldNumber = 2;
    private int itemId_;
    /// <summary>
    ///道具ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ItemId {
      get { return itemId_; }
      set {
        itemId_ = value;
      }
    }

    /// <summary>Field number for the "money_type" field.</summary>
    public const int MoneyTypeFieldNumber = 3;
    private global::MoneyTypePB moneyType_ = 0;
    /// <summary>
    ///货币类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::MoneyTypePB MoneyType {
      get { return moneyType_; }
      set {
        moneyType_ = value;
      }
    }

    /// <summary>Field number for the "iteam_des" field.</summary>
    public const int IteamDesFieldNumber = 4;
    private string iteamDes_ = "";
    /// <summary>
    ///道具描述
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string IteamDes {
      get { return iteamDes_; }
      set {
        iteamDes_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "use_des" field.</summary>
    public const int UseDesFieldNumber = 5;
    private string useDes_ = "";
    /// <summary>
    ///使用描述
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UseDes {
      get { return useDes_; }
      set {
        useDes_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "num" field.</summary>
    public const int NumFieldNumber = 6;
    private int num_;
    /// <summary>
    ///道具数量
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Num {
      get { return num_; }
      set {
        num_ = value;
      }
    }

    /// <summary>Field number for the "price" field.</summary>
    public const int PriceFieldNumber = 7;
    private int price_;
    /// <summary>
    ///价格
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Price {
      get { return price_; }
      set {
        price_ = value;
      }
    }

    /// <summary>Field number for the "max_num" field.</summary>
    public const int MaxNumFieldNumber = 8;
    private int maxNum_;
    /// <summary>
    ///每天购买道具次数上限
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MaxNum {
      get { return maxNum_; }
      set {
        maxNum_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MallItemRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MallItemRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MallId != other.MallId) return false;
      if (ItemId != other.ItemId) return false;
      if (MoneyType != other.MoneyType) return false;
      if (IteamDes != other.IteamDes) return false;
      if (UseDes != other.UseDes) return false;
      if (Num != other.Num) return false;
      if (Price != other.Price) return false;
      if (MaxNum != other.MaxNum) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (MallId != 0) hash ^= MallId.GetHashCode();
      if (ItemId != 0) hash ^= ItemId.GetHashCode();
      if (MoneyType != 0) hash ^= MoneyType.GetHashCode();
      if (IteamDes.Length != 0) hash ^= IteamDes.GetHashCode();
      if (UseDes.Length != 0) hash ^= UseDes.GetHashCode();
      if (Num != 0) hash ^= Num.GetHashCode();
      if (Price != 0) hash ^= Price.GetHashCode();
      if (MaxNum != 0) hash ^= MaxNum.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (MallId != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(MallId);
      }
      if (ItemId != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(ItemId);
      }
      if (MoneyType != 0) {
        output.WriteRawTag(24);
        output.WriteEnum((int) MoneyType);
      }
      if (IteamDes.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(IteamDes);
      }
      if (UseDes.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(UseDes);
      }
      if (Num != 0) {
        output.WriteRawTag(48);
        output.WriteSInt32(Num);
      }
      if (Price != 0) {
        output.WriteRawTag(56);
        output.WriteSInt32(Price);
      }
      if (MaxNum != 0) {
        output.WriteRawTag(64);
        output.WriteSInt32(MaxNum);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (MallId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(MallId);
      }
      if (ItemId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ItemId);
      }
      if (MoneyType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) MoneyType);
      }
      if (IteamDes.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(IteamDes);
      }
      if (UseDes.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UseDes);
      }
      if (Num != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Num);
      }
      if (Price != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Price);
      }
      if (MaxNum != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(MaxNum);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MallItemRulePB other) {
      if (other == null) {
        return;
      }
      if (other.MallId != 0) {
        MallId = other.MallId;
      }
      if (other.ItemId != 0) {
        ItemId = other.ItemId;
      }
      if (other.MoneyType != 0) {
        MoneyType = other.MoneyType;
      }
      if (other.IteamDes.Length != 0) {
        IteamDes = other.IteamDes;
      }
      if (other.UseDes.Length != 0) {
        UseDes = other.UseDes;
      }
      if (other.Num != 0) {
        Num = other.Num;
      }
      if (other.Price != 0) {
        Price = other.Price;
      }
      if (other.MaxNum != 0) {
        MaxNum = other.MaxNum;
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
            MallId = input.ReadSInt32();
            break;
          }
          case 16: {
            ItemId = input.ReadSInt32();
            break;
          }
          case 24: {
            moneyType_ = (global::MoneyTypePB) input.ReadEnum();
            break;
          }
          case 34: {
            IteamDes = input.ReadString();
            break;
          }
          case 42: {
            UseDes = input.ReadString();
            break;
          }
          case 48: {
            Num = input.ReadSInt32();
            break;
          }
          case 56: {
            Price = input.ReadSInt32();
            break;
          }
          case 64: {
            MaxNum = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code