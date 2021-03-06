// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_activity_mall_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_activity_mall_rule.proto</summary>
  public static partial class BeanActivityMallRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_activity_mall_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanActivityMallRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch1iZWFuX2FjdGl2aXR5X21hbGxfcnVsZS5wcm90bxIJY29tLnByb3RvGgpi",
            "YXNlLnByb3RvGhBiZWFuX2F3YXJkLnByb3RvIuMBChJBY3Rpdml0eU1hbGxS",
            "dWxlUEISEwoLYWN0aXZpdHlfaWQYASABKBESDwoHbWFsbF9pZBgCIAEoERIU",
            "CgxtYWxsX2l0ZW1faWQYAyABKBESEQoJbWFsbF9uYW1lGAQgASgJEhEKCW1h",
            "bGxfZGVzYxgFIAEoCRISCgpnaWZ0X2ltYWdlGAYgASgJEhMKC2xhYmVsX2lt",
            "YWdlGAcgASgJEg0KBXByaWNlGAggASgREg8KB2J1eV9tYXgYCSABKBESIgoG",
            "YXdhcmRzGAogAygLMhIuY29tLnByb3RvLkF3YXJkUEJCOQofbmV0LmdhbGFz",
            "cG9ydHMuYmlnc3Rhci5wcm90b2NvbEIWQWN0aXZpdHlNYWxsUnVsZVByb3Rv",
            "c2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, global::Com.Proto.BeanAwardReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.ActivityMallRulePB), global::Com.Proto.ActivityMallRulePB.Parser, new[]{ "ActivityId", "MallId", "MallItemId", "MallName", "MallDesc", "GiftImage", "LabelImage", "Price", "BuyMax", "Awards" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///ActivityMallRulePB ActivityMallRule
  /// </summary>
  public sealed partial class ActivityMallRulePB : pb::IMessage<ActivityMallRulePB> {
    private static readonly pb::MessageParser<ActivityMallRulePB> _parser = new pb::MessageParser<ActivityMallRulePB>(() => new ActivityMallRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ActivityMallRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanActivityMallRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActivityMallRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActivityMallRulePB(ActivityMallRulePB other) : this() {
      activityId_ = other.activityId_;
      mallId_ = other.mallId_;
      mallItemId_ = other.mallItemId_;
      mallName_ = other.mallName_;
      mallDesc_ = other.mallDesc_;
      giftImage_ = other.giftImage_;
      labelImage_ = other.labelImage_;
      price_ = other.price_;
      buyMax_ = other.buyMax_;
      awards_ = other.awards_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActivityMallRulePB Clone() {
      return new ActivityMallRulePB(this);
    }

    /// <summary>Field number for the "activity_id" field.</summary>
    public const int ActivityIdFieldNumber = 1;
    private int activityId_;
    /// <summary>
    ///活动Id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ActivityId {
      get { return activityId_; }
      set {
        activityId_ = value;
      }
    }

    /// <summary>Field number for the "mall_id" field.</summary>
    public const int MallIdFieldNumber = 2;
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

    /// <summary>Field number for the "mall_item_id" field.</summary>
    public const int MallItemIdFieldNumber = 3;
    private int mallItemId_;
    /// <summary>
    ///兑换道具ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MallItemId {
      get { return mallItemId_; }
      set {
        mallItemId_ = value;
      }
    }

    /// <summary>Field number for the "mall_name" field.</summary>
    public const int MallNameFieldNumber = 4;
    private string mallName_ = "";
    /// <summary>
    ///商品名称
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MallName {
      get { return mallName_; }
      set {
        mallName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "mall_desc" field.</summary>
    public const int MallDescFieldNumber = 5;
    private string mallDesc_ = "";
    /// <summary>
    ///商品描述
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MallDesc {
      get { return mallDesc_; }
      set {
        mallDesc_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "gift_image" field.</summary>
    public const int GiftImageFieldNumber = 6;
    private string giftImage_ = "";
    /// <summary>
    ///礼包图片
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string GiftImage {
      get { return giftImage_; }
      set {
        giftImage_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "label_image" field.</summary>
    public const int LabelImageFieldNumber = 7;
    private string labelImage_ = "";
    /// <summary>
    ///商品类型展示图片
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string LabelImage {
      get { return labelImage_; }
      set {
        labelImage_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "price" field.</summary>
    public const int PriceFieldNumber = 8;
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

    /// <summary>Field number for the "buy_max" field.</summary>
    public const int BuyMaxFieldNumber = 9;
    private int buyMax_;
    /// <summary>
    ///购买上限
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int BuyMax {
      get { return buyMax_; }
      set {
        buyMax_ = value;
      }
    }

    /// <summary>Field number for the "awards" field.</summary>
    public const int AwardsFieldNumber = 10;
    private static readonly pb::FieldCodec<global::Com.Proto.AwardPB> _repeated_awards_codec
        = pb::FieldCodec.ForMessage(82, global::Com.Proto.AwardPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.AwardPB> awards_ = new pbc::RepeatedField<global::Com.Proto.AwardPB>();
    /// <summary>
    ///商品奖励内容
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.AwardPB> Awards {
      get { return awards_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ActivityMallRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ActivityMallRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ActivityId != other.ActivityId) return false;
      if (MallId != other.MallId) return false;
      if (MallItemId != other.MallItemId) return false;
      if (MallName != other.MallName) return false;
      if (MallDesc != other.MallDesc) return false;
      if (GiftImage != other.GiftImage) return false;
      if (LabelImage != other.LabelImage) return false;
      if (Price != other.Price) return false;
      if (BuyMax != other.BuyMax) return false;
      if(!awards_.Equals(other.awards_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ActivityId != 0) hash ^= ActivityId.GetHashCode();
      if (MallId != 0) hash ^= MallId.GetHashCode();
      if (MallItemId != 0) hash ^= MallItemId.GetHashCode();
      if (MallName.Length != 0) hash ^= MallName.GetHashCode();
      if (MallDesc.Length != 0) hash ^= MallDesc.GetHashCode();
      if (GiftImage.Length != 0) hash ^= GiftImage.GetHashCode();
      if (LabelImage.Length != 0) hash ^= LabelImage.GetHashCode();
      if (Price != 0) hash ^= Price.GetHashCode();
      if (BuyMax != 0) hash ^= BuyMax.GetHashCode();
      hash ^= awards_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ActivityId != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(ActivityId);
      }
      if (MallId != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(MallId);
      }
      if (MallItemId != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(MallItemId);
      }
      if (MallName.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(MallName);
      }
      if (MallDesc.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(MallDesc);
      }
      if (GiftImage.Length != 0) {
        output.WriteRawTag(50);
        output.WriteString(GiftImage);
      }
      if (LabelImage.Length != 0) {
        output.WriteRawTag(58);
        output.WriteString(LabelImage);
      }
      if (Price != 0) {
        output.WriteRawTag(64);
        output.WriteSInt32(Price);
      }
      if (BuyMax != 0) {
        output.WriteRawTag(72);
        output.WriteSInt32(BuyMax);
      }
      awards_.WriteTo(output, _repeated_awards_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ActivityId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ActivityId);
      }
      if (MallId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(MallId);
      }
      if (MallItemId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(MallItemId);
      }
      if (MallName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MallName);
      }
      if (MallDesc.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MallDesc);
      }
      if (GiftImage.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(GiftImage);
      }
      if (LabelImage.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(LabelImage);
      }
      if (Price != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Price);
      }
      if (BuyMax != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(BuyMax);
      }
      size += awards_.CalculateSize(_repeated_awards_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ActivityMallRulePB other) {
      if (other == null) {
        return;
      }
      if (other.ActivityId != 0) {
        ActivityId = other.ActivityId;
      }
      if (other.MallId != 0) {
        MallId = other.MallId;
      }
      if (other.MallItemId != 0) {
        MallItemId = other.MallItemId;
      }
      if (other.MallName.Length != 0) {
        MallName = other.MallName;
      }
      if (other.MallDesc.Length != 0) {
        MallDesc = other.MallDesc;
      }
      if (other.GiftImage.Length != 0) {
        GiftImage = other.GiftImage;
      }
      if (other.LabelImage.Length != 0) {
        LabelImage = other.LabelImage;
      }
      if (other.Price != 0) {
        Price = other.Price;
      }
      if (other.BuyMax != 0) {
        BuyMax = other.BuyMax;
      }
      awards_.Add(other.awards_);
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
            ActivityId = input.ReadSInt32();
            break;
          }
          case 16: {
            MallId = input.ReadSInt32();
            break;
          }
          case 24: {
            MallItemId = input.ReadSInt32();
            break;
          }
          case 34: {
            MallName = input.ReadString();
            break;
          }
          case 42: {
            MallDesc = input.ReadString();
            break;
          }
          case 50: {
            GiftImage = input.ReadString();
            break;
          }
          case 58: {
            LabelImage = input.ReadString();
            break;
          }
          case 64: {
            Price = input.ReadSInt32();
            break;
          }
          case 72: {
            BuyMax = input.ReadSInt32();
            break;
          }
          case 82: {
            awards_.AddEntriesFrom(input, _repeated_awards_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
