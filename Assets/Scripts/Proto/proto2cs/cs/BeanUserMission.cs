// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_mission.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_mission.proto</summary>
  public static partial class BeanUserMissionReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_mission.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserMissionReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdiZWFuX3VzZXJfbWlzc2lvbi5wcm90bxIJY29tLnByb3RvGgpiYXNlLnBy",
            "b3RvIp4BCg1Vc2VyTWlzc2lvblBCEg8KB3VzZXJfaWQYASABKBESEgoKbWlz",
            "c2lvbl9pZBgCIAEoERIkCgxtaXNzaW9uX3R5cGUYAyABKA4yDi5NaXNzaW9u",
            "VHlwZVBCEiAKBnN0YXR1cxgEIAEoDjIQLk1pc3Npb25TdGF0dXNQQhIQCghw",
            "cm9ncmVzcxgFIAEoEhIOCgZmaW5pc2gYBiABKBIifAoZVXNlck1pc3Npb25B",
            "Y3Rpdml0eUluZm9QQhIkCgxtaXNzaW9uX3R5cGUYASABKA4yDi5NaXNzaW9u",
            "VHlwZVBCEhkKBnBsYXllchgCIAEoDjIJLlBsYXllclBCEhAKCHByb2dyZXNz",
            "GAMgASgREgwKBGxpc3QYBCADKBEimgEKEVVzZXJNaXNzaW9uSW5mb1BCEg8K",
            "B3VzZXJfaWQYASABKBESPAoOYWN0aXZpdHlfaW5mb3MYAiADKAsyJC5jb20u",
            "cHJvdG8uVXNlck1pc3Npb25BY3Rpdml0eUluZm9QQhIYChBpc19zZXZlbl9k",
            "Y19vcGVuGAMgASgREhwKFHNldmVuX2RjX2NyZWF0ZV90aW1lGAQgASgSQjQK",
            "H25ldC5nYWxhc3BvcnRzLmJpZ3N0YXIucHJvdG9jb2xCEVVzZXJNaXNzaW9u",
            "UHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserMissionPB), global::Com.Proto.UserMissionPB.Parser, new[]{ "UserId", "MissionId", "MissionType", "Status", "Progress", "Finish" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserMissionActivityInfoPB), global::Com.Proto.UserMissionActivityInfoPB.Parser, new[]{ "MissionType", "Player", "Progress", "List" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserMissionInfoPB), global::Com.Proto.UserMissionInfoPB.Parser, new[]{ "UserId", "ActivityInfos", "IsSevenDcOpen", "SevenDcCreateTime" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserMissionPB UserMission
  /// </summary>
  public sealed partial class UserMissionPB : pb::IMessage<UserMissionPB> {
    private static readonly pb::MessageParser<UserMissionPB> _parser = new pb::MessageParser<UserMissionPB>(() => new UserMissionPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserMissionPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserMissionReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMissionPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMissionPB(UserMissionPB other) : this() {
      userId_ = other.userId_;
      missionId_ = other.missionId_;
      missionType_ = other.missionType_;
      status_ = other.status_;
      progress_ = other.progress_;
      finish_ = other.finish_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMissionPB Clone() {
      return new UserMissionPB(this);
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

    /// <summary>Field number for the "mission_id" field.</summary>
    public const int MissionIdFieldNumber = 2;
    private int missionId_;
    /// <summary>
    ///任务序号
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MissionId {
      get { return missionId_; }
      set {
        missionId_ = value;
      }
    }

    /// <summary>Field number for the "mission_type" field.</summary>
    public const int MissionTypeFieldNumber = 3;
    private global::MissionTypePB missionType_ = 0;
    /// <summary>
    ///任务类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::MissionTypePB MissionType {
      get { return missionType_; }
      set {
        missionType_ = value;
      }
    }

    /// <summary>Field number for the "status" field.</summary>
    public const int StatusFieldNumber = 4;
    private global::MissionStatusPB status_ = 0;
    /// <summary>
    ///任务状态
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::MissionStatusPB Status {
      get { return status_; }
      set {
        status_ = value;
      }
    }

    /// <summary>Field number for the "progress" field.</summary>
    public const int ProgressFieldNumber = 5;
    private long progress_;
    /// <summary>
    ///任务进度数值
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long Progress {
      get { return progress_; }
      set {
        progress_ = value;
      }
    }

    /// <summary>Field number for the "finish" field.</summary>
    public const int FinishFieldNumber = 6;
    private long finish_;
    /// <summary>
    ///任务完成数值
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long Finish {
      get { return finish_; }
      set {
        finish_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserMissionPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserMissionPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (MissionId != other.MissionId) return false;
      if (MissionType != other.MissionType) return false;
      if (Status != other.Status) return false;
      if (Progress != other.Progress) return false;
      if (Finish != other.Finish) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (MissionId != 0) hash ^= MissionId.GetHashCode();
      if (MissionType != 0) hash ^= MissionType.GetHashCode();
      if (Status != 0) hash ^= Status.GetHashCode();
      if (Progress != 0L) hash ^= Progress.GetHashCode();
      if (Finish != 0L) hash ^= Finish.GetHashCode();
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
      if (MissionId != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(MissionId);
      }
      if (MissionType != 0) {
        output.WriteRawTag(24);
        output.WriteEnum((int) MissionType);
      }
      if (Status != 0) {
        output.WriteRawTag(32);
        output.WriteEnum((int) Status);
      }
      if (Progress != 0L) {
        output.WriteRawTag(40);
        output.WriteSInt64(Progress);
      }
      if (Finish != 0L) {
        output.WriteRawTag(48);
        output.WriteSInt64(Finish);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (MissionId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(MissionId);
      }
      if (MissionType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) MissionType);
      }
      if (Status != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Status);
      }
      if (Progress != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(Progress);
      }
      if (Finish != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(Finish);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserMissionPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.MissionId != 0) {
        MissionId = other.MissionId;
      }
      if (other.MissionType != 0) {
        MissionType = other.MissionType;
      }
      if (other.Status != 0) {
        Status = other.Status;
      }
      if (other.Progress != 0L) {
        Progress = other.Progress;
      }
      if (other.Finish != 0L) {
        Finish = other.Finish;
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
            MissionId = input.ReadSInt32();
            break;
          }
          case 24: {
            missionType_ = (global::MissionTypePB) input.ReadEnum();
            break;
          }
          case 32: {
            status_ = (global::MissionStatusPB) input.ReadEnum();
            break;
          }
          case 40: {
            Progress = input.ReadSInt64();
            break;
          }
          case 48: {
            Finish = input.ReadSInt64();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///UserMissionActivityInfoPB UserMissionActivityInfo
  /// </summary>
  public sealed partial class UserMissionActivityInfoPB : pb::IMessage<UserMissionActivityInfoPB> {
    private static readonly pb::MessageParser<UserMissionActivityInfoPB> _parser = new pb::MessageParser<UserMissionActivityInfoPB>(() => new UserMissionActivityInfoPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserMissionActivityInfoPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserMissionReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMissionActivityInfoPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMissionActivityInfoPB(UserMissionActivityInfoPB other) : this() {
      missionType_ = other.missionType_;
      player_ = other.player_;
      progress_ = other.progress_;
      list_ = other.list_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMissionActivityInfoPB Clone() {
      return new UserMissionActivityInfoPB(this);
    }

    /// <summary>Field number for the "mission_type" field.</summary>
    public const int MissionTypeFieldNumber = 1;
    private global::MissionTypePB missionType_ = 0;
    /// <summary>
    ///任务类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::MissionTypePB MissionType {
      get { return missionType_; }
      set {
        missionType_ = value;
      }
    }

    /// <summary>Field number for the "player" field.</summary>
    public const int PlayerFieldNumber = 2;
    private global::PlayerPB player_ = 0;
    /// <summary>
    ///男主（星路历程才有）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::PlayerPB Player {
      get { return player_; }
      set {
        player_ = value;
      }
    }

    /// <summary>Field number for the "progress" field.</summary>
    public const int ProgressFieldNumber = 3;
    private int progress_;
    /// <summary>
    ///进度
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Progress {
      get { return progress_; }
      set {
        progress_ = value;
      }
    }

    /// <summary>Field number for the "list" field.</summary>
    public const int ListFieldNumber = 4;
    private static readonly pb::FieldCodec<int> _repeated_list_codec
        = pb::FieldCodec.ForSInt32(34);
    private readonly pbc::RepeatedField<int> list_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///领取的奖励ID列表
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> List {
      get { return list_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserMissionActivityInfoPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserMissionActivityInfoPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MissionType != other.MissionType) return false;
      if (Player != other.Player) return false;
      if (Progress != other.Progress) return false;
      if(!list_.Equals(other.list_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (MissionType != 0) hash ^= MissionType.GetHashCode();
      if (Player != 0) hash ^= Player.GetHashCode();
      if (Progress != 0) hash ^= Progress.GetHashCode();
      hash ^= list_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (MissionType != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) MissionType);
      }
      if (Player != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) Player);
      }
      if (Progress != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Progress);
      }
      list_.WriteTo(output, _repeated_list_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (MissionType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) MissionType);
      }
      if (Player != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Player);
      }
      if (Progress != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Progress);
      }
      size += list_.CalculateSize(_repeated_list_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserMissionActivityInfoPB other) {
      if (other == null) {
        return;
      }
      if (other.MissionType != 0) {
        MissionType = other.MissionType;
      }
      if (other.Player != 0) {
        Player = other.Player;
      }
      if (other.Progress != 0) {
        Progress = other.Progress;
      }
      list_.Add(other.list_);
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
            missionType_ = (global::MissionTypePB) input.ReadEnum();
            break;
          }
          case 16: {
            player_ = (global::PlayerPB) input.ReadEnum();
            break;
          }
          case 24: {
            Progress = input.ReadSInt32();
            break;
          }
          case 34:
          case 32: {
            list_.AddEntriesFrom(input, _repeated_list_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///UserMissionInfoPB UserMissionInfo
  /// </summary>
  public sealed partial class UserMissionInfoPB : pb::IMessage<UserMissionInfoPB> {
    private static readonly pb::MessageParser<UserMissionInfoPB> _parser = new pb::MessageParser<UserMissionInfoPB>(() => new UserMissionInfoPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserMissionInfoPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserMissionReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMissionInfoPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMissionInfoPB(UserMissionInfoPB other) : this() {
      userId_ = other.userId_;
      activityInfos_ = other.activityInfos_.Clone();
      isSevenDcOpen_ = other.isSevenDcOpen_;
      sevenDcCreateTime_ = other.sevenDcCreateTime_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserMissionInfoPB Clone() {
      return new UserMissionInfoPB(this);
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

    /// <summary>Field number for the "activity_infos" field.</summary>
    public const int ActivityInfosFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Com.Proto.UserMissionActivityInfoPB> _repeated_activityInfos_codec
        = pb::FieldCodec.ForMessage(18, global::Com.Proto.UserMissionActivityInfoPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.UserMissionActivityInfoPB> activityInfos_ = new pbc::RepeatedField<global::Com.Proto.UserMissionActivityInfoPB>();
    /// <summary>
    ///任务活跃信息（包含星路历程）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.UserMissionActivityInfoPB> ActivityInfos {
      get { return activityInfos_; }
    }

    /// <summary>Field number for the "is_seven_dc_open" field.</summary>
    public const int IsSevenDcOpenFieldNumber = 3;
    private int isSevenDcOpen_;
    /// <summary>
    ///七天活动是否开放（0未开放，1已开放）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int IsSevenDcOpen {
      get { return isSevenDcOpen_; }
      set {
        isSevenDcOpen_ = value;
      }
    }

    /// <summary>Field number for the "seven_dc_create_time" field.</summary>
    public const int SevenDcCreateTimeFieldNumber = 4;
    private long sevenDcCreateTime_;
    /// <summary>
    ///七天狂欢创建时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long SevenDcCreateTime {
      get { return sevenDcCreateTime_; }
      set {
        sevenDcCreateTime_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserMissionInfoPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserMissionInfoPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if(!activityInfos_.Equals(other.activityInfos_)) return false;
      if (IsSevenDcOpen != other.IsSevenDcOpen) return false;
      if (SevenDcCreateTime != other.SevenDcCreateTime) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      hash ^= activityInfos_.GetHashCode();
      if (IsSevenDcOpen != 0) hash ^= IsSevenDcOpen.GetHashCode();
      if (SevenDcCreateTime != 0L) hash ^= SevenDcCreateTime.GetHashCode();
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
      activityInfos_.WriteTo(output, _repeated_activityInfos_codec);
      if (IsSevenDcOpen != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(IsSevenDcOpen);
      }
      if (SevenDcCreateTime != 0L) {
        output.WriteRawTag(32);
        output.WriteSInt64(SevenDcCreateTime);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      size += activityInfos_.CalculateSize(_repeated_activityInfos_codec);
      if (IsSevenDcOpen != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(IsSevenDcOpen);
      }
      if (SevenDcCreateTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(SevenDcCreateTime);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserMissionInfoPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      activityInfos_.Add(other.activityInfos_);
      if (other.IsSevenDcOpen != 0) {
        IsSevenDcOpen = other.IsSevenDcOpen;
      }
      if (other.SevenDcCreateTime != 0L) {
        SevenDcCreateTime = other.SevenDcCreateTime;
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
          case 18: {
            activityInfos_.AddEntriesFrom(input, _repeated_activityInfos_codec);
            break;
          }
          case 24: {
            IsSevenDcOpen = input.ReadSInt32();
            break;
          }
          case 32: {
            SevenDcCreateTime = input.ReadSInt64();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code