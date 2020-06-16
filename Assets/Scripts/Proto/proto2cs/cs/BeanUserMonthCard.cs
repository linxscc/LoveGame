// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_month_card.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_month_card.proto</summary>
  public static partial class BeanUserMonthCardReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_month_card.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserMonthCardReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChpiZWFuX3VzZXJfbW9udGhfY2FyZC5wcm90bxIJY29tLnByb3RvGgpiYXNl",
            "LnByb3RvIkgKD1VzZXJNb250aENhcmRQQhIPCgd1c2VyX2lkGAEgASgREhAK",
            "CGVuZF90aW1lGAMgASgSEhIKCnByaXplX3RpbWUYBCABKBJCMgofbmV0Lmdh",
            "bGFzcG9ydHMuYmlnc3Rhci5wcm90b2NvbEIPTW9udGhDYXJkUHJvdG9zYgZw",
            "cm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserMonthCardPB), global::Com.Proto.UserMonthCardPB.Parser, new[]{ "UserId", "EndTime", "PrizeTime" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///MonthCardPB MonthCard
  /// </summary>
  public sealed partial class UserMonthCardPB : pb::IMessage<UserMonthCardPB> {
    private static readonly pb::MessageParser<UserMonthCardPB> _parser = new pb::MessageParser<UserMonthCardPB>(() => new UserMonthCardPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserMonthCardPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserMonthCardReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMonthCardPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMonthCardPB(UserMonthCardPB other) : this() {
      userId_ = other.userId_;
      endTime_ = other.endTime_;
      prizeTime_ = other.prizeTime_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMonthCardPB Clone() {
      return new UserMonthCardPB(this);
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

    /// <summary>Field number for the "end_time" field.</summary>
    public const int EndTimeFieldNumber = 3;
    private long endTime_;
    /// <summary>
    ///到期时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long EndTime {
      get { return endTime_; }
      set {
        endTime_ = value;
      }
    }

    /// <summary>Field number for the "prize_time" field.</summary>
    public const int PrizeTimeFieldNumber = 4;
    private long prizeTime_;
    /// <summary>
    ///领取时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long PrizeTime {
      get { return prizeTime_; }
      set {
        prizeTime_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserMonthCardPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserMonthCardPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (EndTime != other.EndTime) return false;
      if (PrizeTime != other.PrizeTime) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (EndTime != 0L) hash ^= EndTime.GetHashCode();
      if (PrizeTime != 0L) hash ^= PrizeTime.GetHashCode();
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
      if (EndTime != 0L) {
        output.WriteRawTag(24);
        output.WriteSInt64(EndTime);
      }
      if (PrizeTime != 0L) {
        output.WriteRawTag(32);
        output.WriteSInt64(PrizeTime);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (EndTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(EndTime);
      }
      if (PrizeTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(PrizeTime);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserMonthCardPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.EndTime != 0L) {
        EndTime = other.EndTime;
      }
      if (other.PrizeTime != 0L) {
        PrizeTime = other.PrizeTime;
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
          case 24: {
            EndTime = input.ReadSInt64();
            break;
          }
          case 32: {
            PrizeTime = input.ReadSInt64();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code