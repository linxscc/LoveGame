// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_first_recharge.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_first_recharge.proto</summary>
  public static partial class BeanFirstRechargeReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_first_recharge.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanFirstRechargeReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChliZWFuX2ZpcnN0X3JlY2hhcmdlLnByb3RvEgljb20ucHJvdG8iRwoPRmly",
            "c3RSZWNoYXJnZVBCEg8KB3VzZXJfaWQYASABKBESDgoGYW1vdW50GAIgASgR",
            "EhMKC2NyZWF0ZV90aW1lGAMgASgSQj0KJmNvbS5saW1ib3dvcmtzLmJhc2tl",
            "dGJhbGwuYml6LnByb3RvY29sQhNGaXJzdFJlY2hhcmdlUHJvdG9zYgZwcm90",
            "bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.FirstRechargePB), global::Com.Proto.FirstRechargePB.Parser, new[]{ "UserId", "Amount", "CreateTime" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///FirstRechargePB FirstRecharge
  /// </summary>
  public sealed partial class FirstRechargePB : pb::IMessage<FirstRechargePB> {
    private static readonly pb::MessageParser<FirstRechargePB> _parser = new pb::MessageParser<FirstRechargePB>(() => new FirstRechargePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FirstRechargePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanFirstRechargeReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FirstRechargePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FirstRechargePB(FirstRechargePB other) : this() {
      userId_ = other.userId_;
      amount_ = other.amount_;
      createTime_ = other.createTime_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FirstRechargePB Clone() {
      return new FirstRechargePB(this);
    }

    /// <summary>Field number for the "user_id" field.</summary>
    public const int UserIdFieldNumber = 1;
    private int userId_;
    /// <summary>
    ///用户Id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int UserId {
      get { return userId_; }
      set {
        userId_ = value;
      }
    }

    /// <summary>Field number for the "amount" field.</summary>
    public const int AmountFieldNumber = 2;
    private int amount_;
    /// <summary>
    ///金额 rmb
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Amount {
      get { return amount_; }
      set {
        amount_ = value;
      }
    }

    /// <summary>Field number for the "create_time" field.</summary>
    public const int CreateTimeFieldNumber = 3;
    private long createTime_;
    /// <summary>
    ///创建时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long CreateTime {
      get { return createTime_; }
      set {
        createTime_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FirstRechargePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FirstRechargePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (Amount != other.Amount) return false;
      if (CreateTime != other.CreateTime) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (Amount != 0) hash ^= Amount.GetHashCode();
      if (CreateTime != 0L) hash ^= CreateTime.GetHashCode();
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
      if (Amount != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(Amount);
      }
      if (CreateTime != 0L) {
        output.WriteRawTag(24);
        output.WriteSInt64(CreateTime);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (Amount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Amount);
      }
      if (CreateTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(CreateTime);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FirstRechargePB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.Amount != 0) {
        Amount = other.Amount;
      }
      if (other.CreateTime != 0L) {
        CreateTime = other.CreateTime;
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
            Amount = input.ReadSInt32();
            break;
          }
          case 24: {
            CreateTime = input.ReadSInt64();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code