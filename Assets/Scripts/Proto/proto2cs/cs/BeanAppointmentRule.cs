// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_appointment_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_appointment_rule.proto</summary>
  public static partial class BeanAppointmentRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_appointment_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanAppointmentRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChtiZWFuX2FwcG9pbnRtZW50X3J1bGUucHJvdG8SCWNvbS5wcm90bxoKYmFz",
            "ZS5wcm90bxoQYmVhbl9hd2FyZC5wcm90byKiAQoRQXBwb2ludG1lbnRSdWxl",
            "UEISCgoCaWQYASABKBESFAoMYWN0aXZlX2NhcmRzGAIgAygREhQKDGFjdGl2",
            "ZV9mYXZvchgDIAEoERI0CgpnYXRlX2luZm9zGAQgAygLMiAuY29tLnByb3Rv",
            "LkFwcG9pbnRtZW50R2F0ZVJ1bGVQQhIMCgRuYW1lGAUgASgJEhEKCXN3ZWV0",
            "bmVzcxgGIAEoCSLmAQoVQXBwb2ludG1lbnRHYXRlUnVsZVBCEgwKBGdhdGUY",
            "ASABKBESPgoHY29zdW1lcxgCIAMoCzItLmNvbS5wcm90by5BcHBvaW50bWVu",
            "dEdhdGVSdWxlUEIuQ29zdW1lc0VudHJ5EhAKCHNjZW5lX2lkGAMgASgJEgwK",
            "BHN0YXIYBCABKBESCwoDZXZvGAUgASgREiIKBmF3YXJkcxgGIAMoCzISLmNv",
            "bS5wcm90by5Bd2FyZFBCGi4KDENvc3VtZXNFbnRyeRILCgNrZXkYASABKBES",
            "DQoFdmFsdWUYAiABKBE6AjgBQjgKH25ldC5nYWxhc3BvcnRzLmJpZ3N0YXIu",
            "cHJvdG9jb2xCFUFwcG9pbnRtZW50UnVsZVByb3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, global::Com.Proto.BeanAwardReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.AppointmentRulePB), global::Com.Proto.AppointmentRulePB.Parser, new[]{ "Id", "ActiveCards", "ActiveFavor", "GateInfos", "Name", "Sweetness" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.AppointmentGateRulePB), global::Com.Proto.AppointmentGateRulePB.Parser, new[]{ "Gate", "Cosumes", "SceneId", "Star", "Evo", "Awards" }, null, null, new pbr::GeneratedClrTypeInfo[] { null, })
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///AppointmentRulePBPB AppointmentRule
  /// </summary>
  public sealed partial class AppointmentRulePB : pb::IMessage<AppointmentRulePB> {
    private static readonly pb::MessageParser<AppointmentRulePB> _parser = new pb::MessageParser<AppointmentRulePB>(() => new AppointmentRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AppointmentRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanAppointmentRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AppointmentRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AppointmentRulePB(AppointmentRulePB other) : this() {
      id_ = other.id_;
      activeCards_ = other.activeCards_.Clone();
      activeFavor_ = other.activeFavor_;
      gateInfos_ = other.gateInfos_.Clone();
      name_ = other.name_;
      sweetness_ = other.sweetness_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AppointmentRulePB Clone() {
      return new AppointmentRulePB(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private int id_;
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "active_cards" field.</summary>
    public const int ActiveCardsFieldNumber = 2;
    private static readonly pb::FieldCodec<int> _repeated_activeCards_codec
        = pb::FieldCodec.ForSInt32(18);
    private readonly pbc::RepeatedField<int> activeCards_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///解锁需要获得的卡牌
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> ActiveCards {
      get { return activeCards_; }
    }

    /// <summary>Field number for the "active_favor" field.</summary>
    public const int ActiveFavorFieldNumber = 3;
    private int activeFavor_;
    /// <summary>
    ///解锁需要的好感度
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ActiveFavor {
      get { return activeFavor_; }
      set {
        activeFavor_ = value;
      }
    }

    /// <summary>Field number for the "gate_infos" field.</summary>
    public const int GateInfosFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Com.Proto.AppointmentGateRulePB> _repeated_gateInfos_codec
        = pb::FieldCodec.ForMessage(34, global::Com.Proto.AppointmentGateRulePB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.AppointmentGateRulePB> gateInfos_ = new pbc::RepeatedField<global::Com.Proto.AppointmentGateRulePB>();
    /// <summary>
    ///每关需要的数据
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.AppointmentGateRulePB> GateInfos {
      get { return gateInfos_; }
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 5;
    private string name_ = "";
    /// <summary>
    ///名字
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "sweetness" field.</summary>
    public const int SweetnessFieldNumber = 6;
    private string sweetness_ = "";
    /// <summary>
    ///甜蜜度
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Sweetness {
      get { return sweetness_; }
      set {
        sweetness_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AppointmentRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AppointmentRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if(!activeCards_.Equals(other.activeCards_)) return false;
      if (ActiveFavor != other.ActiveFavor) return false;
      if(!gateInfos_.Equals(other.gateInfos_)) return false;
      if (Name != other.Name) return false;
      if (Sweetness != other.Sweetness) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      hash ^= activeCards_.GetHashCode();
      if (ActiveFavor != 0) hash ^= ActiveFavor.GetHashCode();
      hash ^= gateInfos_.GetHashCode();
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Sweetness.Length != 0) hash ^= Sweetness.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(Id);
      }
      activeCards_.WriteTo(output, _repeated_activeCards_codec);
      if (ActiveFavor != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(ActiveFavor);
      }
      gateInfos_.WriteTo(output, _repeated_gateInfos_codec);
      if (Name.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(Name);
      }
      if (Sweetness.Length != 0) {
        output.WriteRawTag(50);
        output.WriteString(Sweetness);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Id);
      }
      size += activeCards_.CalculateSize(_repeated_activeCards_codec);
      if (ActiveFavor != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ActiveFavor);
      }
      size += gateInfos_.CalculateSize(_repeated_gateInfos_codec);
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (Sweetness.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Sweetness);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AppointmentRulePB other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      activeCards_.Add(other.activeCards_);
      if (other.ActiveFavor != 0) {
        ActiveFavor = other.ActiveFavor;
      }
      gateInfos_.Add(other.gateInfos_);
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Sweetness.Length != 0) {
        Sweetness = other.Sweetness;
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
            Id = input.ReadSInt32();
            break;
          }
          case 18:
          case 16: {
            activeCards_.AddEntriesFrom(input, _repeated_activeCards_codec);
            break;
          }
          case 24: {
            ActiveFavor = input.ReadSInt32();
            break;
          }
          case 34: {
            gateInfos_.AddEntriesFrom(input, _repeated_gateInfos_codec);
            break;
          }
          case 42: {
            Name = input.ReadString();
            break;
          }
          case 50: {
            Sweetness = input.ReadString();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///AppointmentRulePBPB AppointmentRule
  /// </summary>
  public sealed partial class AppointmentGateRulePB : pb::IMessage<AppointmentGateRulePB> {
    private static readonly pb::MessageParser<AppointmentGateRulePB> _parser = new pb::MessageParser<AppointmentGateRulePB>(() => new AppointmentGateRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AppointmentGateRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanAppointmentRuleReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AppointmentGateRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AppointmentGateRulePB(AppointmentGateRulePB other) : this() {
      gate_ = other.gate_;
      cosumes_ = other.cosumes_.Clone();
      sceneId_ = other.sceneId_;
      star_ = other.star_;
      evo_ = other.evo_;
      awards_ = other.awards_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AppointmentGateRulePB Clone() {
      return new AppointmentGateRulePB(this);
    }

    /// <summary>Field number for the "gate" field.</summary>
    public const int GateFieldNumber = 1;
    private int gate_;
    /// <summary>
    ///关卡
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Gate {
      get { return gate_; }
      set {
        gate_ = value;
      }
    }

    /// <summary>Field number for the "cosumes" field.</summary>
    public const int CosumesFieldNumber = 2;
    private static readonly pbc::MapField<int, int>.Codec _map_cosumes_codec
        = new pbc::MapField<int, int>.Codec(pb::FieldCodec.ForSInt32(8), pb::FieldCodec.ForSInt32(16), 18);
    private readonly pbc::MapField<int, int> cosumes_ = new pbc::MapField<int, int>();
    /// <summary>
    ///解锁需要消耗的道具
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::MapField<int, int> Cosumes {
      get { return cosumes_; }
    }

    /// <summary>Field number for the "scene_id" field.</summary>
    public const int SceneIdFieldNumber = 3;
    private string sceneId_ = "";
    /// <summary>
    ///对应的剧情
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string SceneId {
      get { return sceneId_; }
      set {
        sceneId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "star" field.</summary>
    public const int StarFieldNumber = 4;
    private int star_;
    /// <summary>
    ///需要的卡牌星级
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Star {
      get { return star_; }
      set {
        star_ = value;
      }
    }

    /// <summary>Field number for the "evo" field.</summary>
    public const int EvoFieldNumber = 5;
    private int evo_;
    /// <summary>
    ///卡牌是否需要进化0否1是
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Evo {
      get { return evo_; }
      set {
        evo_ = value;
      }
    }

    /// <summary>Field number for the "awards" field.</summary>
    public const int AwardsFieldNumber = 6;
    private static readonly pb::FieldCodec<global::Com.Proto.AwardPB> _repeated_awards_codec
        = pb::FieldCodec.ForMessage(50, global::Com.Proto.AwardPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.AwardPB> awards_ = new pbc::RepeatedField<global::Com.Proto.AwardPB>();
    /// <summary>
    ///通关关卡获得的奖励
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.AwardPB> Awards {
      get { return awards_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AppointmentGateRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AppointmentGateRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Gate != other.Gate) return false;
      if (!Cosumes.Equals(other.Cosumes)) return false;
      if (SceneId != other.SceneId) return false;
      if (Star != other.Star) return false;
      if (Evo != other.Evo) return false;
      if(!awards_.Equals(other.awards_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Gate != 0) hash ^= Gate.GetHashCode();
      hash ^= Cosumes.GetHashCode();
      if (SceneId.Length != 0) hash ^= SceneId.GetHashCode();
      if (Star != 0) hash ^= Star.GetHashCode();
      if (Evo != 0) hash ^= Evo.GetHashCode();
      hash ^= awards_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Gate != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(Gate);
      }
      cosumes_.WriteTo(output, _map_cosumes_codec);
      if (SceneId.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(SceneId);
      }
      if (Star != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(Star);
      }
      if (Evo != 0) {
        output.WriteRawTag(40);
        output.WriteSInt32(Evo);
      }
      awards_.WriteTo(output, _repeated_awards_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Gate != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Gate);
      }
      size += cosumes_.CalculateSize(_map_cosumes_codec);
      if (SceneId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(SceneId);
      }
      if (Star != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Star);
      }
      if (Evo != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Evo);
      }
      size += awards_.CalculateSize(_repeated_awards_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AppointmentGateRulePB other) {
      if (other == null) {
        return;
      }
      if (other.Gate != 0) {
        Gate = other.Gate;
      }
      cosumes_.Add(other.cosumes_);
      if (other.SceneId.Length != 0) {
        SceneId = other.SceneId;
      }
      if (other.Star != 0) {
        Star = other.Star;
      }
      if (other.Evo != 0) {
        Evo = other.Evo;
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
            Gate = input.ReadSInt32();
            break;
          }
          case 18: {
            cosumes_.AddEntriesFrom(input, _map_cosumes_codec);
            break;
          }
          case 26: {
            SceneId = input.ReadString();
            break;
          }
          case 32: {
            Star = input.ReadSInt32();
            break;
          }
          case 40: {
            Evo = input.ReadSInt32();
            break;
          }
          case 50: {
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