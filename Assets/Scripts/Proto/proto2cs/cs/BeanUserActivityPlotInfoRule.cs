// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_activity_plot_info_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_activity_plot_info_rule.proto</summary>
  public static partial class BeanUserActivityPlotInfoRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_activity_plot_info_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserActivityPlotInfoRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CidiZWFuX3VzZXJfYWN0aXZpdHlfcGxvdF9pbmZvX3J1bGUucHJvdG8SCWNv",
            "bS5wcm90bxoKYmFzZS5wcm90bxoQYmVhbl9hd2FyZC5wcm90byJQChZVc2Vy",
            "QWN0aXZpdHlQbG90SW5mb1BCEg8KB3VzZXJfaWQYASABKBESEwoLYWN0aXZp",
            "dHlfaWQYAiABKBESEAoIcGxvdF9pZHMYAyADKAlCPQofbmV0LmdhbGFzcG9y",
            "dHMuYmlnc3Rhci5wcm90b2NvbEIaVXNlckFjdGl2aXR5UGxvdEluZm9Qcm90",
            "b3NiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, global::Com.Proto.BeanAwardReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserActivityPlotInfoPB), global::Com.Proto.UserActivityPlotInfoPB.Parser, new[]{ "UserId", "ActivityId", "PlotIds" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserActivityPlotInfoPB UserActivityPlotInfo
  /// </summary>
  public sealed partial class UserActivityPlotInfoPB : pb::IMessage<UserActivityPlotInfoPB> {
    private static readonly pb::MessageParser<UserActivityPlotInfoPB> _parser = new pb::MessageParser<UserActivityPlotInfoPB>(() => new UserActivityPlotInfoPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserActivityPlotInfoPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserActivityPlotInfoRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserActivityPlotInfoPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserActivityPlotInfoPB(UserActivityPlotInfoPB other) : this() {
      userId_ = other.userId_;
      activityId_ = other.activityId_;
      plotIds_ = other.plotIds_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserActivityPlotInfoPB Clone() {
      return new UserActivityPlotInfoPB(this);
    }

    /// <summary>Field number for the "user_id" field.</summary>
    public const int UserIdFieldNumber = 1;
    private int userId_;
    /// <summary>
    ///玩家ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int UserId {
      get { return userId_; }
      set {
        userId_ = value;
      }
    }

    /// <summary>Field number for the "activity_id" field.</summary>
    public const int ActivityIdFieldNumber = 2;
    private int activityId_;
    /// <summary>
    ///活动ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ActivityId {
      get { return activityId_; }
      set {
        activityId_ = value;
      }
    }

    /// <summary>Field number for the "plot_ids" field.</summary>
    public const int PlotIdsFieldNumber = 3;
    private static readonly pb::FieldCodec<string> _repeated_plotIds_codec
        = pb::FieldCodec.ForString(26);
    private readonly pbc::RepeatedField<string> plotIds_ = new pbc::RepeatedField<string>();
    /// <summary>
    ///剧情进度
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> PlotIds {
      get { return plotIds_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserActivityPlotInfoPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserActivityPlotInfoPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (ActivityId != other.ActivityId) return false;
      if(!plotIds_.Equals(other.plotIds_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (ActivityId != 0) hash ^= ActivityId.GetHashCode();
      hash ^= plotIds_.GetHashCode();
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
      if (ActivityId != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(ActivityId);
      }
      plotIds_.WriteTo(output, _repeated_plotIds_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (ActivityId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ActivityId);
      }
      size += plotIds_.CalculateSize(_repeated_plotIds_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserActivityPlotInfoPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.ActivityId != 0) {
        ActivityId = other.ActivityId;
      }
      plotIds_.Add(other.plotIds_);
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
            ActivityId = input.ReadSInt32();
            break;
          }
          case 26: {
            plotIds_.AddEntriesFrom(input, _repeated_plotIds_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code