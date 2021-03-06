// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_favorability_info.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_favorability_info.proto</summary>
  public static partial class BeanUserFavorabilityInfoReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_favorability_info.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserFavorabilityInfoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiFiZWFuX3VzZXJfZmF2b3JhYmlsaXR5X2luZm8ucHJvdG8SCWNvbS5wcm90",
            "bxoKYmFzZS5wcm90byJmChZVc2VyRmF2b3JhYmlsaXR5SW5mb1BCEg8KB3Vz",
            "ZXJfaWQYASABKBESFAoMdHJpZ2dlcl9zaG93GAIgASgREhIKCnJlc2V0X3Rp",
            "bWUYAyABKBISEQoJYmdfcGljX2lkGAQgASgRQj0KH25ldC5nYWxhc3BvcnRz",
            "LmJpZ3N0YXIucHJvdG9jb2xCGlVzZXJGYXZvcmFiaWxpdHlJbmZvUHJvdG9z",
            "YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserFavorabilityInfoPB), global::Com.Proto.UserFavorabilityInfoPB.Parser, new[]{ "UserId", "TriggerShow", "ResetTime", "BgPicId" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserFavorabilityInfoPB UserFavorabilityInfo
  /// </summary>
  public sealed partial class UserFavorabilityInfoPB : pb::IMessage<UserFavorabilityInfoPB> {
    private static readonly pb::MessageParser<UserFavorabilityInfoPB> _parser = new pb::MessageParser<UserFavorabilityInfoPB>(() => new UserFavorabilityInfoPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserFavorabilityInfoPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserFavorabilityInfoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserFavorabilityInfoPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserFavorabilityInfoPB(UserFavorabilityInfoPB other) : this() {
      userId_ = other.userId_;
      triggerShow_ = other.triggerShow_;
      resetTime_ = other.resetTime_;
      bgPicId_ = other.bgPicId_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserFavorabilityInfoPB Clone() {
      return new UserFavorabilityInfoPB(this);
    }

    /// <summary>Field number for the "user_id" field.</summary>
    public const int UserIdFieldNumber = 1;
    private int userId_;
    /// <summary>
    ///用户ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int UserId {
      get { return userId_; }
      set {
        userId_ = value;
      }
    }

    /// <summary>Field number for the "trigger_show" field.</summary>
    public const int TriggerShowFieldNumber = 2;
    private int triggerShow_;
    /// <summary>
    ///触发展示次数
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int TriggerShow {
      get { return triggerShow_; }
      set {
        triggerShow_ = value;
      }
    }

    /// <summary>Field number for the "reset_time" field.</summary>
    public const int ResetTimeFieldNumber = 3;
    private long resetTime_;
    /// <summary>
    ///重置时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long ResetTime {
      get { return resetTime_; }
      set {
        resetTime_ = value;
      }
    }

    /// <summary>Field number for the "bg_pic_id" field.</summary>
    public const int BgPicIdFieldNumber = 4;
    private int bgPicId_;
    /// <summary>
    ///主界面背景男主ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int BgPicId {
      get { return bgPicId_; }
      set {
        bgPicId_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserFavorabilityInfoPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserFavorabilityInfoPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (TriggerShow != other.TriggerShow) return false;
      if (ResetTime != other.ResetTime) return false;
      if (BgPicId != other.BgPicId) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (TriggerShow != 0) hash ^= TriggerShow.GetHashCode();
      if (ResetTime != 0L) hash ^= ResetTime.GetHashCode();
      if (BgPicId != 0) hash ^= BgPicId.GetHashCode();
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
      if (TriggerShow != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(TriggerShow);
      }
      if (ResetTime != 0L) {
        output.WriteRawTag(24);
        output.WriteSInt64(ResetTime);
      }
      if (BgPicId != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(BgPicId);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (TriggerShow != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(TriggerShow);
      }
      if (ResetTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(ResetTime);
      }
      if (BgPicId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(BgPicId);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserFavorabilityInfoPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.TriggerShow != 0) {
        TriggerShow = other.TriggerShow;
      }
      if (other.ResetTime != 0L) {
        ResetTime = other.ResetTime;
      }
      if (other.BgPicId != 0) {
        BgPicId = other.BgPicId;
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
            TriggerShow = input.ReadSInt32();
            break;
          }
          case 24: {
            ResetTime = input.ReadSInt64();
            break;
          }
          case 32: {
            BgPicId = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
