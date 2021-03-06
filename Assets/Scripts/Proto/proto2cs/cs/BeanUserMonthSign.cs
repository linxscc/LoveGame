// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_month_sign.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_month_sign.proto</summary>
  public static partial class BeanUserMonthSignReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_month_sign.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserMonthSignReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChpiZWFuX3VzZXJfbW9udGhfc2lnbi5wcm90bxIJY29tLnByb3RvGgpiYXNl",
            "LnByb3RvIlEKD1VzZXJNb250aFNpZ25QQhINCgVkYXRlcxgBIAMoERISCgpi",
            "dXlfY291bnRzGAIgASgREhsKE2V4dHJhX3Jld2FyZHNfc3RhdGUYAyABKBFC",
            "NgofbmV0LmdhbGFzcG9ydHMuYmlnc3Rhci5wcm90b2NvbEITVXNlck1vbnRo",
            "U2lnblByb3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserMonthSignPB), global::Com.Proto.UserMonthSignPB.Parser, new[]{ "Dates", "BuyCounts", "ExtraRewardsState" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserMonthSignPB UserMonthSign
  /// </summary>
  public sealed partial class UserMonthSignPB : pb::IMessage<UserMonthSignPB> {
    private static readonly pb::MessageParser<UserMonthSignPB> _parser = new pb::MessageParser<UserMonthSignPB>(() => new UserMonthSignPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserMonthSignPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserMonthSignReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMonthSignPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMonthSignPB(UserMonthSignPB other) : this() {
      dates_ = other.dates_.Clone();
      buyCounts_ = other.buyCounts_;
      extraRewardsState_ = other.extraRewardsState_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMonthSignPB Clone() {
      return new UserMonthSignPB(this);
    }

    /// <summary>Field number for the "dates" field.</summary>
    public const int DatesFieldNumber = 1;
    private static readonly pb::FieldCodec<int> _repeated_dates_codec
        = pb::FieldCodec.ForSInt32(10);
    private readonly pbc::RepeatedField<int> dates_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///签到的日期列表
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> Dates {
      get { return dates_; }
    }

    /// <summary>Field number for the "buy_counts" field.</summary>
    public const int BuyCountsFieldNumber = 2;
    private int buyCounts_;
    /// <summary>
    ///已经补签次数
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int BuyCounts {
      get { return buyCounts_; }
      set {
        buyCounts_ = value;
      }
    }

    /// <summary>Field number for the "extra_rewards_state" field.</summary>
    public const int ExtraRewardsStateFieldNumber = 3;
    private int extraRewardsState_;
    /// <summary>
    ///累计签到奖励领取状态0未领取1已领取
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ExtraRewardsState {
      get { return extraRewardsState_; }
      set {
        extraRewardsState_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserMonthSignPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserMonthSignPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!dates_.Equals(other.dates_)) return false;
      if (BuyCounts != other.BuyCounts) return false;
      if (ExtraRewardsState != other.ExtraRewardsState) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= dates_.GetHashCode();
      if (BuyCounts != 0) hash ^= BuyCounts.GetHashCode();
      if (ExtraRewardsState != 0) hash ^= ExtraRewardsState.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      dates_.WriteTo(output, _repeated_dates_codec);
      if (BuyCounts != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(BuyCounts);
      }
      if (ExtraRewardsState != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(ExtraRewardsState);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += dates_.CalculateSize(_repeated_dates_codec);
      if (BuyCounts != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(BuyCounts);
      }
      if (ExtraRewardsState != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ExtraRewardsState);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserMonthSignPB other) {
      if (other == null) {
        return;
      }
      dates_.Add(other.dates_);
      if (other.BuyCounts != 0) {
        BuyCounts = other.BuyCounts;
      }
      if (other.ExtraRewardsState != 0) {
        ExtraRewardsState = other.ExtraRewardsState;
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
          case 10:
          case 8: {
            dates_.AddEntriesFrom(input, _repeated_dates_codec);
            break;
          }
          case 16: {
            BuyCounts = input.ReadSInt32();
            break;
          }
          case 24: {
            ExtraRewardsState = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
