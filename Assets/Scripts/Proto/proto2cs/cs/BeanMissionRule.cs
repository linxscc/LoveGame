// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_mission_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_mission_rule.proto</summary>
  public static partial class BeanMissionRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_mission_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanMissionRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdiZWFuX21pc3Npb25fcnVsZS5wcm90bxIJY29tLnByb3RvGgpiYXNlLnBy",
            "b3RvGhBiZWFuX2F3YXJkLnByb3RvIo0CCg1NaXNzaW9uUnVsZVBCEhIKCm1p",
            "c3Npb25faWQYASABKBESFAoMbWlzc2lvbl9uYW1lGAIgASgJEiQKDG1pc3Np",
            "b25fdHlwZRgDIAEoDjIOLk1pc3Npb25UeXBlUEISIQoFYXdhcmQYBCADKAsy",
            "Ei5jb20ucHJvdG8uQXdhcmRQQhIUCgxtaXNzaW9uX2Rlc2MYBSABKAkSDwoH",
            "anVtcF90bxgGIAEoCRIOCgZ3ZWlnaHQYByABKBESGQoGcGxheWVyGAggASgO",
            "MgkuUGxheWVyUEISDwoHcG9wX3VwcxgJIAEoERImCgVleHRyYRgKIAEoCzIX",
            "LmNvbS5wcm90by5FeHRyYVZhbHVlUEIiMQoMRXh0cmFWYWx1ZVBCEgwKBGRh",
            "eXMYASABKBESEwoLbGltaXRfdmFsdWUYAiABKBEikgEKG01pc3Npb25BY3Rp",
            "dml0eVJld2FyZFJ1bGVQQhIkCgxtaXNzaW9uX3R5cGUYASABKA4yDi5NaXNz",
            "aW9uVHlwZVBCEhkKBnBsYXllchgCIAEoDjIJLlBsYXllclBCEg4KBndlaWdo",
            "dBgDIAEoERIiCgZhd2FyZHMYBCADKAsyEi5jb20ucHJvdG8uQXdhcmRQQkI0",
            "Ch9uZXQuZ2FsYXNwb3J0cy5iaWdzdGFyLnByb3RvY29sQhFNaXNzaW9uUnVs",
            "ZVByb3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, global::Com.Proto.BeanAwardReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.MissionRulePB), global::Com.Proto.MissionRulePB.Parser, new[]{ "MissionId", "MissionName", "MissionType", "Award", "MissionDesc", "JumpTo", "Weight", "Player", "PopUps", "Extra" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.ExtraValuePB), global::Com.Proto.ExtraValuePB.Parser, new[]{ "Days", "LimitValue" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.MissionActivityRewardRulePB), global::Com.Proto.MissionActivityRewardRulePB.Parser, new[]{ "MissionType", "Player", "Weight", "Awards" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///MissionRulePB MissionRule
  /// </summary>
  public sealed partial class MissionRulePB : pb::IMessage<MissionRulePB> {
    private static readonly pb::MessageParser<MissionRulePB> _parser = new pb::MessageParser<MissionRulePB>(() => new MissionRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MissionRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanMissionRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MissionRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MissionRulePB(MissionRulePB other) : this() {
      missionId_ = other.missionId_;
      missionName_ = other.missionName_;
      missionType_ = other.missionType_;
      award_ = other.award_.Clone();
      missionDesc_ = other.missionDesc_;
      jumpTo_ = other.jumpTo_;
      weight_ = other.weight_;
      player_ = other.player_;
      popUps_ = other.popUps_;
      Extra = other.extra_ != null ? other.Extra.Clone() : null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MissionRulePB Clone() {
      return new MissionRulePB(this);
    }

    /// <summary>Field number for the "mission_id" field.</summary>
    public const int MissionIdFieldNumber = 1;
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

    /// <summary>Field number for the "mission_name" field.</summary>
    public const int MissionNameFieldNumber = 2;
    private string missionName_ = "";
    /// <summary>
    ///任务名称
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MissionName {
      get { return missionName_; }
      set {
        missionName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
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

    /// <summary>Field number for the "award" field.</summary>
    public const int AwardFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Com.Proto.AwardPB> _repeated_award_codec
        = pb::FieldCodec.ForMessage(34, global::Com.Proto.AwardPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.AwardPB> award_ = new pbc::RepeatedField<global::Com.Proto.AwardPB>();
    /// <summary>
    ///奖励
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.AwardPB> Award {
      get { return award_; }
    }

    /// <summary>Field number for the "mission_desc" field.</summary>
    public const int MissionDescFieldNumber = 5;
    private string missionDesc_ = "";
    /// <summary>
    ///任务描述
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string MissionDesc {
      get { return missionDesc_; }
      set {
        missionDesc_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "jump_to" field.</summary>
    public const int JumpToFieldNumber = 6;
    private string jumpTo_ = "";
    /// <summary>
    ///跳转链接
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string JumpTo {
      get { return jumpTo_; }
      set {
        jumpTo_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "weight" field.</summary>
    public const int WeightFieldNumber = 7;
    private int weight_;
    /// <summary>
    ///完成任务进度权重(日常)
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Weight {
      get { return weight_; }
      set {
        weight_ = value;
      }
    }

    /// <summary>Field number for the "player" field.</summary>
    public const int PlayerFieldNumber = 8;
    private global::PlayerPB player_ = 0;
    /// <summary>
    ///PlayerPB 该任务相关的男主，星路历程0表示应援会
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::PlayerPB Player {
      get { return player_; }
      set {
        player_ = value;
      }
    }

    /// <summary>Field number for the "pop_ups" field.</summary>
    public const int PopUpsFieldNumber = 9;
    private int popUps_;
    /// <summary>
    ///弹窗（0不弹，1弹）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int PopUps {
      get { return popUps_; }
      set {
        popUps_ = value;
      }
    }

    /// <summary>Field number for the "extra" field.</summary>
    public const int ExtraFieldNumber = 10;
    private global::Com.Proto.ExtraValuePB extra_;
    /// <summary>
    ///额外参数
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Proto.ExtraValuePB Extra {
      get { return extra_; }
      set {
        extra_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MissionRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MissionRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MissionId != other.MissionId) return false;
      if (MissionName != other.MissionName) return false;
      if (MissionType != other.MissionType) return false;
      if(!award_.Equals(other.award_)) return false;
      if (MissionDesc != other.MissionDesc) return false;
      if (JumpTo != other.JumpTo) return false;
      if (Weight != other.Weight) return false;
      if (Player != other.Player) return false;
      if (PopUps != other.PopUps) return false;
      if (!object.Equals(Extra, other.Extra)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (MissionId != 0) hash ^= MissionId.GetHashCode();
      if (MissionName.Length != 0) hash ^= MissionName.GetHashCode();
      if (MissionType != 0) hash ^= MissionType.GetHashCode();
      hash ^= award_.GetHashCode();
      if (MissionDesc.Length != 0) hash ^= MissionDesc.GetHashCode();
      if (JumpTo.Length != 0) hash ^= JumpTo.GetHashCode();
      if (Weight != 0) hash ^= Weight.GetHashCode();
      if (Player != 0) hash ^= Player.GetHashCode();
      if (PopUps != 0) hash ^= PopUps.GetHashCode();
      if (extra_ != null) hash ^= Extra.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (MissionId != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(MissionId);
      }
      if (MissionName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(MissionName);
      }
      if (MissionType != 0) {
        output.WriteRawTag(24);
        output.WriteEnum((int) MissionType);
      }
      award_.WriteTo(output, _repeated_award_codec);
      if (MissionDesc.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(MissionDesc);
      }
      if (JumpTo.Length != 0) {
        output.WriteRawTag(50);
        output.WriteString(JumpTo);
      }
      if (Weight != 0) {
        output.WriteRawTag(56);
        output.WriteSInt32(Weight);
      }
      if (Player != 0) {
        output.WriteRawTag(64);
        output.WriteEnum((int) Player);
      }
      if (PopUps != 0) {
        output.WriteRawTag(72);
        output.WriteSInt32(PopUps);
      }
      if (extra_ != null) {
        output.WriteRawTag(82);
        output.WriteMessage(Extra);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (MissionId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(MissionId);
      }
      if (MissionName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MissionName);
      }
      if (MissionType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) MissionType);
      }
      size += award_.CalculateSize(_repeated_award_codec);
      if (MissionDesc.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(MissionDesc);
      }
      if (JumpTo.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(JumpTo);
      }
      if (Weight != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Weight);
      }
      if (Player != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Player);
      }
      if (PopUps != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(PopUps);
      }
      if (extra_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Extra);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MissionRulePB other) {
      if (other == null) {
        return;
      }
      if (other.MissionId != 0) {
        MissionId = other.MissionId;
      }
      if (other.MissionName.Length != 0) {
        MissionName = other.MissionName;
      }
      if (other.MissionType != 0) {
        MissionType = other.MissionType;
      }
      award_.Add(other.award_);
      if (other.MissionDesc.Length != 0) {
        MissionDesc = other.MissionDesc;
      }
      if (other.JumpTo.Length != 0) {
        JumpTo = other.JumpTo;
      }
      if (other.Weight != 0) {
        Weight = other.Weight;
      }
      if (other.Player != 0) {
        Player = other.Player;
      }
      if (other.PopUps != 0) {
        PopUps = other.PopUps;
      }
      if (other.extra_ != null) {
        if (extra_ == null) {
          extra_ = new global::Com.Proto.ExtraValuePB();
        }
        Extra.MergeFrom(other.Extra);
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
            MissionId = input.ReadSInt32();
            break;
          }
          case 18: {
            MissionName = input.ReadString();
            break;
          }
          case 24: {
            missionType_ = (global::MissionTypePB) input.ReadEnum();
            break;
          }
          case 34: {
            award_.AddEntriesFrom(input, _repeated_award_codec);
            break;
          }
          case 42: {
            MissionDesc = input.ReadString();
            break;
          }
          case 50: {
            JumpTo = input.ReadString();
            break;
          }
          case 56: {
            Weight = input.ReadSInt32();
            break;
          }
          case 64: {
            player_ = (global::PlayerPB) input.ReadEnum();
            break;
          }
          case 72: {
            PopUps = input.ReadSInt32();
            break;
          }
          case 82: {
            if (extra_ == null) {
              extra_ = new global::Com.Proto.ExtraValuePB();
            }
            input.ReadMessage(extra_);
            break;
          }
        }
      }
    }

  }

  public sealed partial class ExtraValuePB : pb::IMessage<ExtraValuePB> {
    private static readonly pb::MessageParser<ExtraValuePB> _parser = new pb::MessageParser<ExtraValuePB>(() => new ExtraValuePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ExtraValuePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanMissionRuleReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ExtraValuePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ExtraValuePB(ExtraValuePB other) : this() {
      days_ = other.days_;
      limitValue_ = other.limitValue_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ExtraValuePB Clone() {
      return new ExtraValuePB(this);
    }

    /// <summary>Field number for the "days" field.</summary>
    public const int DaysFieldNumber = 1;
    private int days_;
    /// <summary>
    ///任务天数
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Days {
      get { return days_; }
      set {
        days_ = value;
      }
    }

    /// <summary>Field number for the "limit_value" field.</summary>
    public const int LimitValueFieldNumber = 2;
    private int limitValue_;
    /// <summary>
    ///限制条件数值
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int LimitValue {
      get { return limitValue_; }
      set {
        limitValue_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ExtraValuePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ExtraValuePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Days != other.Days) return false;
      if (LimitValue != other.LimitValue) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Days != 0) hash ^= Days.GetHashCode();
      if (LimitValue != 0) hash ^= LimitValue.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Days != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(Days);
      }
      if (LimitValue != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(LimitValue);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Days != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Days);
      }
      if (LimitValue != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(LimitValue);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ExtraValuePB other) {
      if (other == null) {
        return;
      }
      if (other.Days != 0) {
        Days = other.Days;
      }
      if (other.LimitValue != 0) {
        LimitValue = other.LimitValue;
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
            Days = input.ReadSInt32();
            break;
          }
          case 16: {
            LimitValue = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///MissionActivityRewardRulePB MissionActivityRewardRule
  /// </summary>
  public sealed partial class MissionActivityRewardRulePB : pb::IMessage<MissionActivityRewardRulePB> {
    private static readonly pb::MessageParser<MissionActivityRewardRulePB> _parser = new pb::MessageParser<MissionActivityRewardRulePB>(() => new MissionActivityRewardRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MissionActivityRewardRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanMissionRuleReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MissionActivityRewardRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MissionActivityRewardRulePB(MissionActivityRewardRulePB other) : this() {
      missionType_ = other.missionType_;
      player_ = other.player_;
      weight_ = other.weight_;
      awards_ = other.awards_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MissionActivityRewardRulePB Clone() {
      return new MissionActivityRewardRulePB(this);
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
    ///PlayerPB 该任务相关的男主，星路历程0表示应援会
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::PlayerPB Player {
      get { return player_; }
      set {
        player_ = value;
      }
    }

    /// <summary>Field number for the "weight" field.</summary>
    public const int WeightFieldNumber = 3;
    private int weight_;
    /// <summary>
    ///进度
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Weight {
      get { return weight_; }
      set {
        weight_ = value;
      }
    }

    /// <summary>Field number for the "awards" field.</summary>
    public const int AwardsFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Com.Proto.AwardPB> _repeated_awards_codec
        = pb::FieldCodec.ForMessage(34, global::Com.Proto.AwardPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.AwardPB> awards_ = new pbc::RepeatedField<global::Com.Proto.AwardPB>();
    /// <summary>
    ///奖励
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.AwardPB> Awards {
      get { return awards_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MissionActivityRewardRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MissionActivityRewardRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (MissionType != other.MissionType) return false;
      if (Player != other.Player) return false;
      if (Weight != other.Weight) return false;
      if(!awards_.Equals(other.awards_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (MissionType != 0) hash ^= MissionType.GetHashCode();
      if (Player != 0) hash ^= Player.GetHashCode();
      if (Weight != 0) hash ^= Weight.GetHashCode();
      hash ^= awards_.GetHashCode();
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
      if (Weight != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Weight);
      }
      awards_.WriteTo(output, _repeated_awards_codec);
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
      if (Weight != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Weight);
      }
      size += awards_.CalculateSize(_repeated_awards_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MissionActivityRewardRulePB other) {
      if (other == null) {
        return;
      }
      if (other.MissionType != 0) {
        MissionType = other.MissionType;
      }
      if (other.Player != 0) {
        Player = other.Player;
      }
      if (other.Weight != 0) {
        Weight = other.Weight;
      }
      awards_.Add(other.awards_);
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
            Weight = input.ReadSInt32();
            break;
          }
          case 34: {
            awards_.AddEntriesFrom(input, _repeated_awards_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
