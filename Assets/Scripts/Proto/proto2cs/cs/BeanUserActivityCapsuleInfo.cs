// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_activity_capsule_info.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_activity_capsule_info.proto</summary>
  public static partial class BeanUserActivityCapsuleInfoReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_activity_capsule_info.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserActivityCapsuleInfoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiViZWFuX3VzZXJfYWN0aXZpdHlfY2Fwc3VsZV9pbmZvLnByb3RvEgljb20u",
            "cHJvdG8aCmJhc2UucHJvdG8iSgoZVXNlckFjdGl2aXR5Q2Fwc3VsZUluZm9Q",
            "QhIbChNkcmF3X2F3YXJkX3Byb2dyZXNzGAEgAygFEhAKCHBsb3RfaWRzGAIg",
            "AygJQkAKH25ldC5nYWxhc3BvcnRzLmJpZ3N0YXIucHJvdG9jb2xCHVVzZXJB",
            "Y3Rpdml0eUNhcHN1bGVJbmZvUHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserActivityCapsuleInfoPB), global::Com.Proto.UserActivityCapsuleInfoPB.Parser, new[]{ "DrawAwardProgress", "PlotIds" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class UserActivityCapsuleInfoPB : pb::IMessage<UserActivityCapsuleInfoPB> {
    private static readonly pb::MessageParser<UserActivityCapsuleInfoPB> _parser = new pb::MessageParser<UserActivityCapsuleInfoPB>(() => new UserActivityCapsuleInfoPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserActivityCapsuleInfoPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserActivityCapsuleInfoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserActivityCapsuleInfoPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserActivityCapsuleInfoPB(UserActivityCapsuleInfoPB other) : this() {
      drawAwardProgress_ = other.drawAwardProgress_.Clone();
      plotIds_ = other.plotIds_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserActivityCapsuleInfoPB Clone() {
      return new UserActivityCapsuleInfoPB(this);
    }

    /// <summary>Field number for the "draw_award_progress" field.</summary>
    public const int DrawAwardProgressFieldNumber = 1;
    private static readonly pb::FieldCodec<int> _repeated_drawAwardProgress_codec
        = pb::FieldCodec.ForInt32(10);
    private readonly pbc::RepeatedField<int> drawAwardProgress_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///已获取的奖励列表(序号ID)
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> DrawAwardProgress {
      get { return drawAwardProgress_; }
    }

    /// <summary>Field number for the "plot_ids" field.</summary>
    public const int PlotIdsFieldNumber = 2;
    private static readonly pb::FieldCodec<string> _repeated_plotIds_codec
        = pb::FieldCodec.ForString(18);
    private readonly pbc::RepeatedField<string> plotIds_ = new pbc::RepeatedField<string>();
    /// <summary>
    ///已读的剧情ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> PlotIds {
      get { return plotIds_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserActivityCapsuleInfoPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserActivityCapsuleInfoPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!drawAwardProgress_.Equals(other.drawAwardProgress_)) return false;
      if(!plotIds_.Equals(other.plotIds_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= drawAwardProgress_.GetHashCode();
      hash ^= plotIds_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      drawAwardProgress_.WriteTo(output, _repeated_drawAwardProgress_codec);
      plotIds_.WriteTo(output, _repeated_plotIds_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += drawAwardProgress_.CalculateSize(_repeated_drawAwardProgress_codec);
      size += plotIds_.CalculateSize(_repeated_plotIds_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserActivityCapsuleInfoPB other) {
      if (other == null) {
        return;
      }
      drawAwardProgress_.Add(other.drawAwardProgress_);
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
          case 10:
          case 8: {
            drawAwardProgress_.AddEntriesFrom(input, _repeated_drawAwardProgress_codec);
            break;
          }
          case 18: {
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
