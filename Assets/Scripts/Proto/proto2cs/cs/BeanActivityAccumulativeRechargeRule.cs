// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_activity_accumulative_recharge_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_activity_accumulative_recharge_rule.proto</summary>
  public static partial class BeanActivityAccumulativeRechargeRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_activity_accumulative_recharge_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanActivityAccumulativeRechargeRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ci5iZWFuX2FjdGl2aXR5X2FjY3VtdWxhdGl2ZV9yZWNoYXJnZV9ydWxlLnBy",
            "b3RvEgljb20ucHJvdG8aCmJhc2UucHJvdG8aEGJlYW5fYXdhcmQucHJvdG8i",
            "gwEKIkFjdGl2aXR5QWNjdW11bGF0aXZlUmVjaGFyZ2VSdWxlUEISDwoHZ2Vh",
            "cl9pZBgBIAEoERITCgthY3Rpdml0eV9pZBgCIAEoERITCgtnZWFyX2Ftb3Vu",
            "dBgDIAEoERIiCgZhd2FyZHMYBCADKAsyEi5jb20ucHJvdG8uQXdhcmRQQkJJ",
            "Ch9uZXQuZ2FsYXNwb3J0cy5iaWdzdGFyLnByb3RvY29sQiZBY3Rpdml0eUFj",
            "Y3VtdWxhdGl2ZVJlY2hhcmdlUnVsZVByb3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, global::Com.Proto.BeanAwardReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.ActivityAccumulativeRechargeRulePB), global::Com.Proto.ActivityAccumulativeRechargeRulePB.Parser, new[]{ "GearId", "ActivityId", "GearAmount", "Awards" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///ActivityAccumulativeRechargeRulePB ActivityAccumulativeRechargeRule
  /// </summary>
  public sealed partial class ActivityAccumulativeRechargeRulePB : pb::IMessage<ActivityAccumulativeRechargeRulePB> {
    private static readonly pb::MessageParser<ActivityAccumulativeRechargeRulePB> _parser = new pb::MessageParser<ActivityAccumulativeRechargeRulePB>(() => new ActivityAccumulativeRechargeRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ActivityAccumulativeRechargeRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanActivityAccumulativeRechargeRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActivityAccumulativeRechargeRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActivityAccumulativeRechargeRulePB(ActivityAccumulativeRechargeRulePB other) : this() {
      gearId_ = other.gearId_;
      activityId_ = other.activityId_;
      gearAmount_ = other.gearAmount_;
      awards_ = other.awards_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActivityAccumulativeRechargeRulePB Clone() {
      return new ActivityAccumulativeRechargeRulePB(this);
    }

    /// <summary>Field number for the "gear_id" field.</summary>
    public const int GearIdFieldNumber = 1;
    private int gearId_;
    /// <summary>
    ///档位id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int GearId {
      get { return gearId_; }
      set {
        gearId_ = value;
      }
    }

    /// <summary>Field number for the "activity_id" field.</summary>
    public const int ActivityIdFieldNumber = 2;
    private int activityId_;
    /// <summary>
    ///活动id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ActivityId {
      get { return activityId_; }
      set {
        activityId_ = value;
      }
    }

    /// <summary>Field number for the "gear_amount" field.</summary>
    public const int GearAmountFieldNumber = 3;
    private int gearAmount_;
    /// <summary>
    ///档位金额
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int GearAmount {
      get { return gearAmount_; }
      set {
        gearAmount_ = value;
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
      return Equals(other as ActivityAccumulativeRechargeRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ActivityAccumulativeRechargeRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (GearId != other.GearId) return false;
      if (ActivityId != other.ActivityId) return false;
      if (GearAmount != other.GearAmount) return false;
      if(!awards_.Equals(other.awards_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (GearId != 0) hash ^= GearId.GetHashCode();
      if (ActivityId != 0) hash ^= ActivityId.GetHashCode();
      if (GearAmount != 0) hash ^= GearAmount.GetHashCode();
      hash ^= awards_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (GearId != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(GearId);
      }
      if (ActivityId != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(ActivityId);
      }
      if (GearAmount != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(GearAmount);
      }
      awards_.WriteTo(output, _repeated_awards_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (GearId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(GearId);
      }
      if (ActivityId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ActivityId);
      }
      if (GearAmount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(GearAmount);
      }
      size += awards_.CalculateSize(_repeated_awards_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ActivityAccumulativeRechargeRulePB other) {
      if (other == null) {
        return;
      }
      if (other.GearId != 0) {
        GearId = other.GearId;
      }
      if (other.ActivityId != 0) {
        ActivityId = other.ActivityId;
      }
      if (other.GearAmount != 0) {
        GearAmount = other.GearAmount;
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
            GearId = input.ReadSInt32();
            break;
          }
          case 16: {
            ActivityId = input.ReadSInt32();
            break;
          }
          case 24: {
            GearAmount = input.ReadSInt32();
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
