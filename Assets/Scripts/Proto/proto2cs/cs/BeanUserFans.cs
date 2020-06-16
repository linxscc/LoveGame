// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_fans.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_fans.proto</summary>
  public static partial class BeanUserFansReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_fans.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserFansReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChRiZWFuX3VzZXJfZmFucy5wcm90bxIJY29tLnByb3RvGgpiYXNlLnByb3Rv",
            "Ij0KClVzZXJGYW5zUEISDwoHdXNlcl9pZBgBIAEoERIRCglmYW5zX3R5cGUY",
            "AiABKBESCwoDbnVtGAMgASgRQjEKH25ldC5nYWxhc3BvcnRzLmJpZ3N0YXIu",
            "cHJvdG9jb2xCDlVzZXJGYW5zUHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserFansPB), global::Com.Proto.UserFansPB.Parser, new[]{ "UserId", "FansType", "Num" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserFansPB UserFans
  /// </summary>
  public sealed partial class UserFansPB : pb::IMessage<UserFansPB> {
    private static readonly pb::MessageParser<UserFansPB> _parser = new pb::MessageParser<UserFansPB>(() => new UserFansPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserFansPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserFansReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserFansPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserFansPB(UserFansPB other) : this() {
      userId_ = other.userId_;
      fansType_ = other.fansType_;
      num_ = other.num_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserFansPB Clone() {
      return new UserFansPB(this);
    }

    /// <summary>Field number for the "user_id" field.</summary>
    public const int UserIdFieldNumber = 1;
    private int userId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int UserId {
      get { return userId_; }
      set {
        userId_ = value;
      }
    }

    /// <summary>Field number for the "fans_type" field.</summary>
    public const int FansTypeFieldNumber = 2;
    private int fansType_;
    /// <summary>
    ///fans_id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int FansType {
      get { return fansType_; }
      set {
        fansType_ = value;
      }
    }

    /// <summary>Field number for the "num" field.</summary>
    public const int NumFieldNumber = 3;
    private int num_;
    /// <summary>
    ///fans_num
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Num {
      get { return num_; }
      set {
        num_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserFansPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserFansPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (FansType != other.FansType) return false;
      if (Num != other.Num) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (FansType != 0) hash ^= FansType.GetHashCode();
      if (Num != 0) hash ^= Num.GetHashCode();
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
      if (FansType != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(FansType);
      }
      if (Num != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Num);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (FansType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(FansType);
      }
      if (Num != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Num);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserFansPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.FansType != 0) {
        FansType = other.FansType;
      }
      if (other.Num != 0) {
        Num = other.Num;
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
            FansType = input.ReadSInt32();
            break;
          }
          case 24: {
            Num = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
