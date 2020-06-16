// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_activity_plot_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_activity_plot_rule.proto</summary>
  public static partial class BeanActivityPlotRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_activity_plot_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanActivityPlotRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch1iZWFuX2FjdGl2aXR5X3Bsb3RfcnVsZS5wcm90bxIJY29tLnByb3RvGgpi",
            "YXNlLnByb3RvIk0KEkFjdGl2aXR5UGxvdFJ1bGVQQhITCgthY3Rpdml0eV9p",
            "ZBgBIAEoERIPCgdwbG90X2lkGAIgASgJEhEKCW9wZW5fdGltZRgDIAEoEkI5",
            "Ch9uZXQuZ2FsYXNwb3J0cy5iaWdzdGFyLnByb3RvY29sQhZBY3Rpdml0eVBs",
            "b3RSdWxlUHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.ActivityPlotRulePB), global::Com.Proto.ActivityPlotRulePB.Parser, new[]{ "ActivityId", "PlotId", "OpenTime" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class ActivityPlotRulePB : pb::IMessage<ActivityPlotRulePB> {
    private static readonly pb::MessageParser<ActivityPlotRulePB> _parser = new pb::MessageParser<ActivityPlotRulePB>(() => new ActivityPlotRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ActivityPlotRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanActivityPlotRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActivityPlotRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActivityPlotRulePB(ActivityPlotRulePB other) : this() {
      activityId_ = other.activityId_;
      plotId_ = other.plotId_;
      openTime_ = other.openTime_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActivityPlotRulePB Clone() {
      return new ActivityPlotRulePB(this);
    }

    /// <summary>Field number for the "activity_id" field.</summary>
    public const int ActivityIdFieldNumber = 1;
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

    /// <summary>Field number for the "plot_id" field.</summary>
    public const int PlotIdFieldNumber = 2;
    private string plotId_ = "";
    /// <summary>
    ///场景ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string PlotId {
      get { return plotId_; }
      set {
        plotId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "open_time" field.</summary>
    public const int OpenTimeFieldNumber = 3;
    private long openTime_;
    /// <summary>
    ///开启时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long OpenTime {
      get { return openTime_; }
      set {
        openTime_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ActivityPlotRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ActivityPlotRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ActivityId != other.ActivityId) return false;
      if (PlotId != other.PlotId) return false;
      if (OpenTime != other.OpenTime) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ActivityId != 0) hash ^= ActivityId.GetHashCode();
      if (PlotId.Length != 0) hash ^= PlotId.GetHashCode();
      if (OpenTime != 0L) hash ^= OpenTime.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ActivityId != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(ActivityId);
      }
      if (PlotId.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(PlotId);
      }
      if (OpenTime != 0L) {
        output.WriteRawTag(24);
        output.WriteSInt64(OpenTime);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ActivityId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ActivityId);
      }
      if (PlotId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(PlotId);
      }
      if (OpenTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(OpenTime);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ActivityPlotRulePB other) {
      if (other == null) {
        return;
      }
      if (other.ActivityId != 0) {
        ActivityId = other.ActivityId;
      }
      if (other.PlotId.Length != 0) {
        PlotId = other.PlotId;
      }
      if (other.OpenTime != 0L) {
        OpenTime = other.OpenTime;
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
            ActivityId = input.ReadSInt32();
            break;
          }
          case 18: {
            PlotId = input.ReadString();
            break;
          }
          case 24: {
            OpenTime = input.ReadSInt64();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code