// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_coax_sleep_info.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_coax_sleep_info.proto</summary>
  public static partial class BeanUserCoaxSleepInfoReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_coax_sleep_info.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserCoaxSleepInfoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch9iZWFuX3VzZXJfY29heF9zbGVlcF9pbmZvLnByb3RvEgljb20ucHJvdG8a",
            "CmJhc2UucHJvdG8irAEKE1VzZXJDb2F4U2xlZXBJbmZvUEISDwoHdXNlcl9p",
            "ZBgBIAEoERIZCgZwbGF5ZXIYAiABKA4yCS5QbGF5ZXJQQhI6CgZhdWRpb3MY",
            "AyADKAsyKi5jb20ucHJvdG8uVXNlckNvYXhTbGVlcEluZm9QQi5BdWRpb3NF",
            "bnRyeRotCgtBdWRpb3NFbnRyeRILCgNrZXkYASABKBESDQoFdmFsdWUYAiAB",
            "KBE6AjgBQjoKH25ldC5nYWxhc3BvcnRzLmJpZ3N0YXIucHJvdG9jb2xCF1Vz",
            "ZXJDb2F4U2xlZXBJbmZvUHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserCoaxSleepInfoPB), global::Com.Proto.UserCoaxSleepInfoPB.Parser, new[]{ "UserId", "Player", "Audios" }, null, null, new pbr::GeneratedClrTypeInfo[] { null, })
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserCoaxSleepInfoPB UserCoaxSleepInfo
  /// </summary>
  public sealed partial class UserCoaxSleepInfoPB : pb::IMessage<UserCoaxSleepInfoPB> {
    private static readonly pb::MessageParser<UserCoaxSleepInfoPB> _parser = new pb::MessageParser<UserCoaxSleepInfoPB>(() => new UserCoaxSleepInfoPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserCoaxSleepInfoPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserCoaxSleepInfoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserCoaxSleepInfoPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserCoaxSleepInfoPB(UserCoaxSleepInfoPB other) : this() {
      userId_ = other.userId_;
      player_ = other.player_;
      audios_ = other.audios_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserCoaxSleepInfoPB Clone() {
      return new UserCoaxSleepInfoPB(this);
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

    /// <summary>Field number for the "player" field.</summary>
    public const int PlayerFieldNumber = 2;
    private global::PlayerPB player_ = 0;
    /// <summary>
    ///角色id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::PlayerPB Player {
      get { return player_; }
      set {
        player_ = value;
      }
    }

    /// <summary>Field number for the "audios" field.</summary>
    public const int AudiosFieldNumber = 3;
    private static readonly pbc::MapField<int, int>.Codec _map_audios_codec
        = new pbc::MapField<int, int>.Codec(pb::FieldCodec.ForSInt32(8), pb::FieldCodec.ForSInt32(16), 26);
    private readonly pbc::MapField<int, int> audios_ = new pbc::MapField<int, int>();
    /// <summary>
    ///解锁音频&lt;audiosId,AudiosUnlockTypePB.getType>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::MapField<int, int> Audios {
      get { return audios_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserCoaxSleepInfoPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserCoaxSleepInfoPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (Player != other.Player) return false;
      if (!Audios.Equals(other.Audios)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (Player != 0) hash ^= Player.GetHashCode();
      hash ^= Audios.GetHashCode();
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
      if (Player != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) Player);
      }
      audios_.WriteTo(output, _map_audios_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (Player != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Player);
      }
      size += audios_.CalculateSize(_map_audios_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserCoaxSleepInfoPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.Player != 0) {
        Player = other.Player;
      }
      audios_.Add(other.audios_);
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
            player_ = (global::PlayerPB) input.ReadEnum();
            break;
          }
          case 26: {
            audios_.AddEntriesFrom(input, _map_audios_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
