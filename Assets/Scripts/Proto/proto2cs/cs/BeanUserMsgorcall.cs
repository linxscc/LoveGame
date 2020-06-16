// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_msgorcall.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_msgorcall.proto</summary>
  public static partial class BeanUserMsgorcallReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_msgorcall.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserMsgorcallReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChliZWFuX3VzZXJfbXNnb3JjYWxsLnByb3RvEgljb20ucHJvdG8aCmJhc2Uu",
            "cHJvdG8ivQEKD1VzZXJNc2dPckNhbGxQQhIPCgd1c2VyX2lkGAEgASgREhAK",
            "CHNjZW5lX2lkGAIgASgREg4KBm5wY19pZBgDIAEoERISCgpzZWxlY3RfaWRz",
            "GAQgAygFEhIKCnJlYWRfc3RhdGUYBSABKBESEwoLY3JlYXRlX3RpbWUYBiAB",
            "KBISEgoKbGlzdGVuX2lkcxgHIAMoBRITCgtmaW5pc2hfdGltZRgIIAEoEhIR",
            "Cgl0aXBfc3RhdGUYCSABKBFCNgofbmV0LmdhbGFzcG9ydHMuYmlnc3Rhci5w",
            "cm90b2NvbEITVXNlck1zZ09yQ2FsbFByb3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserMsgOrCallPB), global::Com.Proto.UserMsgOrCallPB.Parser, new[]{ "UserId", "SceneId", "NpcId", "SelectIds", "ReadState", "CreateTime", "ListenIds", "FinishTime", "TipState" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserMsgOrCallPB UserMsgOrCall
  /// </summary>
  public sealed partial class UserMsgOrCallPB : pb::IMessage<UserMsgOrCallPB> {
    private static readonly pb::MessageParser<UserMsgOrCallPB> _parser = new pb::MessageParser<UserMsgOrCallPB>(() => new UserMsgOrCallPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserMsgOrCallPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserMsgorcallReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMsgOrCallPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMsgOrCallPB(UserMsgOrCallPB other) : this() {
      userId_ = other.userId_;
      sceneId_ = other.sceneId_;
      npcId_ = other.npcId_;
      selectIds_ = other.selectIds_.Clone();
      readState_ = other.readState_;
      createTime_ = other.createTime_;
      listenIds_ = other.listenIds_.Clone();
      finishTime_ = other.finishTime_;
      tipState_ = other.tipState_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMsgOrCallPB Clone() {
      return new UserMsgOrCallPB(this);
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

    /// <summary>Field number for the "scene_id" field.</summary>
    public const int SceneIdFieldNumber = 2;
    private int sceneId_;
    /// <summary>
    ///情景ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int SceneId {
      get { return sceneId_; }
      set {
        sceneId_ = value;
      }
    }

    /// <summary>Field number for the "npc_id" field.</summary>
    public const int NpcIdFieldNumber = 3;
    private int npcId_;
    /// <summary>
    ///是跟谁的对话
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int NpcId {
      get { return npcId_; }
      set {
        npcId_ = value;
      }
    }

    /// <summary>Field number for the "select_ids" field.</summary>
    public const int SelectIdsFieldNumber = 4;
    private static readonly pb::FieldCodec<int> _repeated_selectIds_codec
        = pb::FieldCodec.ForInt32(34);
    private readonly pbc::RepeatedField<int> selectIds_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///选过哪些选项
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> SelectIds {
      get { return selectIds_; }
    }

    /// <summary>Field number for the "read_state" field.</summary>
    public const int ReadStateFieldNumber = 5;
    private int readState_;
    /// <summary>
    ///0未读1已读
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ReadState {
      get { return readState_; }
      set {
        readState_ = value;
      }
    }

    /// <summary>Field number for the "create_time" field.</summary>
    public const int CreateTimeFieldNumber = 6;
    private long createTime_;
    /// <summary>
    ///触发时间									
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long CreateTime {
      get { return createTime_; }
      set {
        createTime_ = value;
      }
    }

    /// <summary>Field number for the "listen_ids" field.</summary>
    public const int ListenIdsFieldNumber = 7;
    private static readonly pb::FieldCodec<int> _repeated_listenIds_codec
        = pb::FieldCodec.ForInt32(58);
    private readonly pbc::RepeatedField<int> listenIds_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///读过音频的对话ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> ListenIds {
      get { return listenIds_; }
    }

    /// <summary>Field number for the "finish_time" field.</summary>
    public const int FinishTimeFieldNumber = 8;
    private long finishTime_;
    /// <summary>
    ///读完的时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long FinishTime {
      get { return finishTime_; }
      set {
        finishTime_ = value;
      }
    }

    /// <summary>Field number for the "tip_state" field.</summary>
    public const int TipStateFieldNumber = 9;
    private int tipState_;
    /// <summary>
    ///0新触发未弹窗1已经弹窗过
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int TipState {
      get { return tipState_; }
      set {
        tipState_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserMsgOrCallPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserMsgOrCallPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (SceneId != other.SceneId) return false;
      if (NpcId != other.NpcId) return false;
      if(!selectIds_.Equals(other.selectIds_)) return false;
      if (ReadState != other.ReadState) return false;
      if (CreateTime != other.CreateTime) return false;
      if(!listenIds_.Equals(other.listenIds_)) return false;
      if (FinishTime != other.FinishTime) return false;
      if (TipState != other.TipState) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (SceneId != 0) hash ^= SceneId.GetHashCode();
      if (NpcId != 0) hash ^= NpcId.GetHashCode();
      hash ^= selectIds_.GetHashCode();
      if (ReadState != 0) hash ^= ReadState.GetHashCode();
      if (CreateTime != 0L) hash ^= CreateTime.GetHashCode();
      hash ^= listenIds_.GetHashCode();
      if (FinishTime != 0L) hash ^= FinishTime.GetHashCode();
      if (TipState != 0) hash ^= TipState.GetHashCode();
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
      if (SceneId != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(SceneId);
      }
      if (NpcId != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(NpcId);
      }
      selectIds_.WriteTo(output, _repeated_selectIds_codec);
      if (ReadState != 0) {
        output.WriteRawTag(40);
        output.WriteSInt32(ReadState);
      }
      if (CreateTime != 0L) {
        output.WriteRawTag(48);
        output.WriteSInt64(CreateTime);
      }
      listenIds_.WriteTo(output, _repeated_listenIds_codec);
      if (FinishTime != 0L) {
        output.WriteRawTag(64);
        output.WriteSInt64(FinishTime);
      }
      if (TipState != 0) {
        output.WriteRawTag(72);
        output.WriteSInt32(TipState);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (SceneId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(SceneId);
      }
      if (NpcId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(NpcId);
      }
      size += selectIds_.CalculateSize(_repeated_selectIds_codec);
      if (ReadState != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ReadState);
      }
      if (CreateTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(CreateTime);
      }
      size += listenIds_.CalculateSize(_repeated_listenIds_codec);
      if (FinishTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(FinishTime);
      }
      if (TipState != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(TipState);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserMsgOrCallPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.SceneId != 0) {
        SceneId = other.SceneId;
      }
      if (other.NpcId != 0) {
        NpcId = other.NpcId;
      }
      selectIds_.Add(other.selectIds_);
      if (other.ReadState != 0) {
        ReadState = other.ReadState;
      }
      if (other.CreateTime != 0L) {
        CreateTime = other.CreateTime;
      }
      listenIds_.Add(other.listenIds_);
      if (other.FinishTime != 0L) {
        FinishTime = other.FinishTime;
      }
      if (other.TipState != 0) {
        TipState = other.TipState;
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
            SceneId = input.ReadSInt32();
            break;
          }
          case 24: {
            NpcId = input.ReadSInt32();
            break;
          }
          case 34:
          case 32: {
            selectIds_.AddEntriesFrom(input, _repeated_selectIds_codec);
            break;
          }
          case 40: {
            ReadState = input.ReadSInt32();
            break;
          }
          case 48: {
            CreateTime = input.ReadSInt64();
            break;
          }
          case 58:
          case 56: {
            listenIds_.AddEntriesFrom(input, _repeated_listenIds_codec);
            break;
          }
          case 64: {
            FinishTime = input.ReadSInt64();
            break;
          }
          case 72: {
            TipState = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
