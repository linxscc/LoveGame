// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_activity_power_get.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_activity_power_get.proto</summary>
  public static partial class BeanActivityPowerGetReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_activity_power_get.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanActivityPowerGetReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch1iZWFuX2FjdGl2aXR5X3Bvd2VyX2dldC5wcm90bxIJY29tLnByb3RvGgpi",
            "YXNlLnByb3RvGhBiZWFuX2F3YXJkLnByb3RvInAKFkFjdGl2aXR5UG93ZXJH",
            "ZXRSdWxlUEISCgoCaWQYASABKBESIQoFYXdhcmQYAiADKAsyEi5jb20ucHJv",
            "dG8uQXdhcmRQQhILCgNnZW0YAyABKBESDQoFc3RhcnQYBCABKAkSCwoDZW5k",
            "GAUgASgJQj0KH25ldC5nYWxhc3BvcnRzLmJpZ3N0YXIucHJvdG9jb2xCGkFj",
            "dGl2aXR5UG93ZXJHZXRSdWxlUHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, global::Com.Proto.BeanAwardReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.ActivityPowerGetRulePB), global::Com.Proto.ActivityPowerGetRulePB.Parser, new[]{ "Id", "Award", "Gem", "Start", "End" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///ActivityPowerGetRulePB ActivityPowerGetRule
  /// </summary>
  public sealed partial class ActivityPowerGetRulePB : pb::IMessage<ActivityPowerGetRulePB> {
    private static readonly pb::MessageParser<ActivityPowerGetRulePB> _parser = new pb::MessageParser<ActivityPowerGetRulePB>(() => new ActivityPowerGetRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ActivityPowerGetRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanActivityPowerGetReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActivityPowerGetRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActivityPowerGetRulePB(ActivityPowerGetRulePB other) : this() {
      id_ = other.id_;
      award_ = other.award_.Clone();
      gem_ = other.gem_;
      start_ = other.start_;
      end_ = other.end_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActivityPowerGetRulePB Clone() {
      return new ActivityPowerGetRulePB(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private int id_;
    /// <summary>
    ///id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "award" field.</summary>
    public const int AwardFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Com.Proto.AwardPB> _repeated_award_codec
        = pb::FieldCodec.ForMessage(18, global::Com.Proto.AwardPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.AwardPB> award_ = new pbc::RepeatedField<global::Com.Proto.AwardPB>();
    /// <summary>
    ///奖励
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.AwardPB> Award {
      get { return award_; }
    }

    /// <summary>Field number for the "gem" field.</summary>
    public const int GemFieldNumber = 3;
    private int gem_;
    /// <summary>
    ///补领钻石
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Gem {
      get { return gem_; }
      set {
        gem_ = value;
      }
    }

    /// <summary>Field number for the "start" field.</summary>
    public const int StartFieldNumber = 4;
    private string start_ = "";
    /// <summary>
    ///开始时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Start {
      get { return start_; }
      set {
        start_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "end" field.</summary>
    public const int EndFieldNumber = 5;
    private string end_ = "";
    /// <summary>
    ///结束时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string End {
      get { return end_; }
      set {
        end_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ActivityPowerGetRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ActivityPowerGetRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if(!award_.Equals(other.award_)) return false;
      if (Gem != other.Gem) return false;
      if (Start != other.Start) return false;
      if (End != other.End) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      hash ^= award_.GetHashCode();
      if (Gem != 0) hash ^= Gem.GetHashCode();
      if (Start.Length != 0) hash ^= Start.GetHashCode();
      if (End.Length != 0) hash ^= End.GetHashCode();
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
      award_.WriteTo(output, _repeated_award_codec);
      if (Gem != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Gem);
      }
      if (Start.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Start);
      }
      if (End.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(End);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Id);
      }
      size += award_.CalculateSize(_repeated_award_codec);
      if (Gem != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Gem);
      }
      if (Start.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Start);
      }
      if (End.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(End);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ActivityPowerGetRulePB other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      award_.Add(other.award_);
      if (other.Gem != 0) {
        Gem = other.Gem;
      }
      if (other.Start.Length != 0) {
        Start = other.Start;
      }
      if (other.End.Length != 0) {
        End = other.End;
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
          case 18: {
            award_.AddEntriesFrom(input, _repeated_award_codec);
            break;
          }
          case 24: {
            Gem = input.ReadSInt32();
            break;
          }
          case 34: {
            Start = input.ReadString();
            break;
          }
          case 42: {
            End = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
