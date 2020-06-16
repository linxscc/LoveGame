// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_buy_activity_mall.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_buy_activity_mall.proto</summary>
  public static partial class BeanUserBuyActivityMallReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_buy_activity_mall.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserBuyActivityMallReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiFiZWFuX3VzZXJfYnV5X2FjdGl2aXR5X21hbGwucHJvdG8SCWNvbS5wcm90",
            "bxoKYmFzZS5wcm90byJfChVVc2VyQnV5QWN0aXZpdHlNYWxsUEISDwoHdXNl",
            "cl9pZBgBIAEoERITCgthY3Rpdml0eV9pZBgCIAEoERIPCgdtYWxsX2lkGAMg",
            "ASgREg8KB2J1eV9udW0YBCABKBFCPAofbmV0LmdhbGFzcG9ydHMuYmlnc3Rh",
            "ci5wcm90b2NvbEIZVXNlckJ1eUFjdGl2aXR5TWFsbFByb3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserBuyActivityMallPB), global::Com.Proto.UserBuyActivityMallPB.Parser, new[]{ "UserId", "ActivityId", "MallId", "BuyNum" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserBuyActivityMallPB UserBuyActivityMall
  /// </summary>
  public sealed partial class UserBuyActivityMallPB : pb::IMessage<UserBuyActivityMallPB> {
    private static readonly pb::MessageParser<UserBuyActivityMallPB> _parser = new pb::MessageParser<UserBuyActivityMallPB>(() => new UserBuyActivityMallPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserBuyActivityMallPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserBuyActivityMallReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserBuyActivityMallPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserBuyActivityMallPB(UserBuyActivityMallPB other) : this() {
      userId_ = other.userId_;
      activityId_ = other.activityId_;
      mallId_ = other.mallId_;
      buyNum_ = other.buyNum_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserBuyActivityMallPB Clone() {
      return new UserBuyActivityMallPB(this);
    }

    /// <summary>Field number for the "user_id" field.</summary>
    public const int UserIdFieldNumber = 1;
    private int userId_;
    /// <summary>
    ///用户id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int UserId {
      get { return userId_; }
      set {
        userId_ = value;
      }
    }

    /// <summary>Field number for the "activity_id" field.</summary>
    public const int ActivityIdFieldNumber = 2;
    private int activityId_;
    /// <summary>
    ///活动id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ActivityId {
      get { return activityId_; }
      set {
        activityId_ = value;
      }
    }

    /// <summary>Field number for the "mall_id" field.</summary>
    public const int MallIdFieldNumber = 3;
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

    /// <summary>Field number for the "buy_num" field.</summary>
    public const int BuyNumFieldNumber = 4;
    private int buyNum_;
    /// <summary>
    ///购买次数
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int BuyNum {
      get { return buyNum_; }
      set {
        buyNum_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserBuyActivityMallPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserBuyActivityMallPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (ActivityId != other.ActivityId) return false;
      if (MallId != other.MallId) return false;
      if (BuyNum != other.BuyNum) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (ActivityId != 0) hash ^= ActivityId.GetHashCode();
      if (MallId != 0) hash ^= MallId.GetHashCode();
      if (BuyNum != 0) hash ^= BuyNum.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (UserId != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(UserId);
      }
      if (ActivityId != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(ActivityId);
      }
      if (MallId != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(MallId);
      }
      if (BuyNum != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(BuyNum);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (ActivityId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ActivityId);
      }
      if (MallId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(MallId);
      }
      if (BuyNum != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(BuyNum);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserBuyActivityMallPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.ActivityId != 0) {
        ActivityId = other.ActivityId;
      }
      if (other.MallId != 0) {
        MallId = other.MallId;
      }
      if (other.BuyNum != 0) {
        BuyNum = other.BuyNum;
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
            UserId = input.ReadSInt32();
            break;
          }
          case 16: {
            ActivityId = input.ReadSInt32();
            break;
          }
          case 24: {
            MallId = input.ReadSInt32();
            break;
          }
          case 32: {
            BuyNum = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
