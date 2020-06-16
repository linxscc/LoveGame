// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_item.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_item.proto</summary>
  public static partial class BeanUserItemReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_item.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserItemReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChRiZWFuX3VzZXJfaXRlbS5wcm90bxIJY29tLnByb3RvGgpiYXNlLnByb3Rv",
            "IjsKClVzZXJJdGVtUEISDwoHdXNlcl9pZBgBIAEoERIPCgdpdGVtX2lkGAIg",
            "ASgREgsKA251bRgDIAEoEUIxCh9uZXQuZ2FsYXNwb3J0cy5iaWdzdGFyLnBy",
            "b3RvY29sQg5Vc2VySXRlbVByb3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserItemPB), global::Com.Proto.UserItemPB.Parser, new[]{ "UserId", "ItemId", "Num" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserItemPB UserItem
  /// </summary>
  public sealed partial class UserItemPB : pb::IMessage<UserItemPB> {
    private static readonly pb::MessageParser<UserItemPB> _parser = new pb::MessageParser<UserItemPB>(() => new UserItemPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserItemPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserItemReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserItemPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserItemPB(UserItemPB other) : this() {
      userId_ = other.userId_;
      itemId_ = other.itemId_;
      num_ = other.num_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserItemPB Clone() {
      return new UserItemPB(this);
    }

    /// <summary>Field number for the "user_id" field.</summary>
    public const int UserIdFieldNumber = 1;
    private int userId_;
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int UserId {
      get { return userId_; }
      set {
        userId_ = value;
      }
    }

    /// <summary>Field number for the "item_id" field.</summary>
    public const int ItemIdFieldNumber = 2;
    private int itemId_;
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ItemId {
      get { return itemId_; }
      set {
        itemId_ = value;
      }
    }

    /// <summary>Field number for the "num" field.</summary>
    public const int NumFieldNumber = 3;
    private int num_;
    /// <summary>
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
      return Equals(other as UserItemPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserItemPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (ItemId != other.ItemId) return false;
      if (Num != other.Num) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (ItemId != 0) hash ^= ItemId.GetHashCode();
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
      if (ItemId != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(ItemId);
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
      if (ItemId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ItemId);
      }
      if (Num != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Num);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserItemPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.ItemId != 0) {
        ItemId = other.ItemId;
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
            ItemId = input.ReadSInt32();
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
