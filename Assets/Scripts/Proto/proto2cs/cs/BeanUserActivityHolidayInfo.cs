// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_activity_holiday_info.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_activity_holiday_info.proto</summary>
  public static partial class BeanUserActivityHolidayInfoReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_activity_holiday_info.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserActivityHolidayInfoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiViZWFuX3VzZXJfYWN0aXZpdHlfaG9saWRheV9pbmZvLnByb3RvEgljb20u",
            "cHJvdG8aCmJhc2UucHJvdG8aIGJlYW5fYWN0aXZpdHlfaG9saWRheV9ydWxl",
            "LnByb3RvIpIBChlVc2VyQWN0aXZpdHlIb2xpZGF5SW5mb1BCEhMKC2FjdGl2",
            "aXR5X2lkGAEgASgREjQKEWRyb3BfcHJvZ3Jlc3NfbWFwGAIgAygLMhkuY29t",
            "LnByb3RvLkRyb3BQcm9ncmVzc1BCEhcKD2FjdGl2ZV9wcm9ncmVzcxgDIAMo",
            "ERIRCglkcmF3Q291bnQYBCABKAUibAoORHJvcFByb2dyZXNzUEISMAoMaG9s",
            "aWRheV90eXBlGAEgASgOMhouY29tLnByb3RvLkhvbGlkYXlNb2R1bGVQQhIo",
            "Cglkcm9wX2l0ZW0YAiADKAsyFS5jb20ucHJvdG8uRHJvcEl0ZW1QQiJ7Cg5E",
            "cm9wcGluZ0l0ZW1QQhITCgtyZXNvdXJjZV9pZBgBIAEoERIdCghyZXNvdXJj",
            "ZRgCIAEoDjILLlJlc291cmNlUEISCwoDbnVtGAMgASgREhMKC2N1cnJlbnRf",
            "bnVtGAQgASgREhMKC2FjdGl2aXR5X2lkGAUgASgRQkAKH25ldC5nYWxhc3Bv",
            "cnRzLmJpZ3N0YXIucHJvdG9jb2xCHVVzZXJBY3Rpdml0eUhvbGlkYXlJbmZv",
            "UHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, global::Com.Proto.BeanActivityHolidayRuleReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserActivityHolidayInfoPB), global::Com.Proto.UserActivityHolidayInfoPB.Parser, new[]{ "ActivityId", "DropProgressMap", "ActiveProgress", "DrawCount" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.DropProgressPB), global::Com.Proto.DropProgressPB.Parser, new[]{ "HolidayType", "DropItem" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.DroppingItemPB), global::Com.Proto.DroppingItemPB.Parser, new[]{ "ResourceId", "Resource", "Num", "CurrentNum", "ActivityId" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class UserActivityHolidayInfoPB : pb::IMessage<UserActivityHolidayInfoPB> {
    private static readonly pb::MessageParser<UserActivityHolidayInfoPB> _parser = new pb::MessageParser<UserActivityHolidayInfoPB>(() => new UserActivityHolidayInfoPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserActivityHolidayInfoPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserActivityHolidayInfoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserActivityHolidayInfoPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserActivityHolidayInfoPB(UserActivityHolidayInfoPB other) : this() {
      activityId_ = other.activityId_;
      dropProgressMap_ = other.dropProgressMap_.Clone();
      activeProgress_ = other.activeProgress_.Clone();
      drawCount_ = other.drawCount_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserActivityHolidayInfoPB Clone() {
      return new UserActivityHolidayInfoPB(this);
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

    /// <summary>Field number for the "drop_progress_map" field.</summary>
    public const int DropProgressMapFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Com.Proto.DropProgressPB> _repeated_dropProgressMap_codec
        = pb::FieldCodec.ForMessage(18, global::Com.Proto.DropProgressPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.DropProgressPB> dropProgressMap_ = new pbc::RepeatedField<global::Com.Proto.DropProgressPB>();
    /// <summary>
    ///模块掉落进度
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.DropProgressPB> DropProgressMap {
      get { return dropProgressMap_; }
    }

    /// <summary>Field number for the "active_progress" field.</summary>
    public const int ActiveProgressFieldNumber = 3;
    private static readonly pb::FieldCodec<int> _repeated_activeProgress_codec
        = pb::FieldCodec.ForSInt32(26);
    private readonly pbc::RepeatedField<int> activeProgress_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///活跃进度领取进度
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> ActiveProgress {
      get { return activeProgress_; }
    }

    /// <summary>Field number for the "drawCount" field.</summary>
    public const int DrawCountFieldNumber = 4;
    private int drawCount_;
    /// <summary>
    ///抽取次数
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int DrawCount {
      get { return drawCount_; }
      set {
        drawCount_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserActivityHolidayInfoPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserActivityHolidayInfoPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ActivityId != other.ActivityId) return false;
      if(!dropProgressMap_.Equals(other.dropProgressMap_)) return false;
      if(!activeProgress_.Equals(other.activeProgress_)) return false;
      if (DrawCount != other.DrawCount) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ActivityId != 0) hash ^= ActivityId.GetHashCode();
      hash ^= dropProgressMap_.GetHashCode();
      hash ^= activeProgress_.GetHashCode();
      if (DrawCount != 0) hash ^= DrawCount.GetHashCode();
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
      dropProgressMap_.WriteTo(output, _repeated_dropProgressMap_codec);
      activeProgress_.WriteTo(output, _repeated_activeProgress_codec);
      if (DrawCount != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(DrawCount);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ActivityId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ActivityId);
      }
      size += dropProgressMap_.CalculateSize(_repeated_dropProgressMap_codec);
      size += activeProgress_.CalculateSize(_repeated_activeProgress_codec);
      if (DrawCount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(DrawCount);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserActivityHolidayInfoPB other) {
      if (other == null) {
        return;
      }
      if (other.ActivityId != 0) {
        ActivityId = other.ActivityId;
      }
      dropProgressMap_.Add(other.dropProgressMap_);
      activeProgress_.Add(other.activeProgress_);
      if (other.DrawCount != 0) {
        DrawCount = other.DrawCount;
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
            dropProgressMap_.AddEntriesFrom(input, _repeated_dropProgressMap_codec);
            break;
          }
          case 26:
          case 24: {
            activeProgress_.AddEntriesFrom(input, _repeated_activeProgress_codec);
            break;
          }
          case 32: {
            DrawCount = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class DropProgressPB : pb::IMessage<DropProgressPB> {
    private static readonly pb::MessageParser<DropProgressPB> _parser = new pb::MessageParser<DropProgressPB>(() => new DropProgressPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DropProgressPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserActivityHolidayInfoReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DropProgressPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DropProgressPB(DropProgressPB other) : this() {
      holidayType_ = other.holidayType_;
      dropItem_ = other.dropItem_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DropProgressPB Clone() {
      return new DropProgressPB(this);
    }

    /// <summary>Field number for the "holiday_type" field.</summary>
    public const int HolidayTypeFieldNumber = 1;
    private global::Com.Proto.HolidayModulePB holidayType_ = 0;
    /// <summary>
    ///功能模块
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Proto.HolidayModulePB HolidayType {
      get { return holidayType_; }
      set {
        holidayType_ = value;
      }
    }

    /// <summary>Field number for the "drop_item" field.</summary>
    public const int DropItemFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Com.Proto.DropItemPB> _repeated_dropItem_codec
        = pb::FieldCodec.ForMessage(18, global::Com.Proto.DropItemPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.DropItemPB> dropItem_ = new pbc::RepeatedField<global::Com.Proto.DropItemPB>();
    /// <summary>
    ///掉落物品
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.DropItemPB> DropItem {
      get { return dropItem_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DropProgressPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DropProgressPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (HolidayType != other.HolidayType) return false;
      if(!dropItem_.Equals(other.dropItem_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (HolidayType != 0) hash ^= HolidayType.GetHashCode();
      hash ^= dropItem_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (HolidayType != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) HolidayType);
      }
      dropItem_.WriteTo(output, _repeated_dropItem_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (HolidayType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) HolidayType);
      }
      size += dropItem_.CalculateSize(_repeated_dropItem_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DropProgressPB other) {
      if (other == null) {
        return;
      }
      if (other.HolidayType != 0) {
        HolidayType = other.HolidayType;
      }
      dropItem_.Add(other.dropItem_);
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
            holidayType_ = (global::Com.Proto.HolidayModulePB) input.ReadEnum();
            break;
          }
          case 18: {
            dropItem_.AddEntriesFrom(input, _repeated_dropItem_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class DroppingItemPB : pb::IMessage<DroppingItemPB> {
    private static readonly pb::MessageParser<DroppingItemPB> _parser = new pb::MessageParser<DroppingItemPB>(() => new DroppingItemPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DroppingItemPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserActivityHolidayInfoReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DroppingItemPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DroppingItemPB(DroppingItemPB other) : this() {
      resourceId_ = other.resourceId_;
      resource_ = other.resource_;
      num_ = other.num_;
      currentNum_ = other.currentNum_;
      activityId_ = other.activityId_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DroppingItemPB Clone() {
      return new DroppingItemPB(this);
    }

    /// <summary>Field number for the "resource_id" field.</summary>
    public const int ResourceIdFieldNumber = 1;
    private int resourceId_;
    /// <summary>
    ///物品ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ResourceId {
      get { return resourceId_; }
      set {
        resourceId_ = value;
      }
    }

    /// <summary>Field number for the "resource" field.</summary>
    public const int ResourceFieldNumber = 2;
    private global::ResourcePB resource_ = 0;
    /// <summary>
    ///物品类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::ResourcePB Resource {
      get { return resource_; }
      set {
        resource_ = value;
      }
    }

    /// <summary>Field number for the "num" field.</summary>
    public const int NumFieldNumber = 3;
    private int num_;
    /// <summary>
    ///数量
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Num {
      get { return num_; }
      set {
        num_ = value;
      }
    }

    /// <summary>Field number for the "current_num" field.</summary>
    public const int CurrentNumFieldNumber = 4;
    private int currentNum_;
    /// <summary>
    ///当前数量
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CurrentNum {
      get { return currentNum_; }
      set {
        currentNum_ = value;
      }
    }

    /// <summary>Field number for the "activity_id" field.</summary>
    public const int ActivityIdFieldNumber = 5;
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

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DroppingItemPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DroppingItemPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ResourceId != other.ResourceId) return false;
      if (Resource != other.Resource) return false;
      if (Num != other.Num) return false;
      if (CurrentNum != other.CurrentNum) return false;
      if (ActivityId != other.ActivityId) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ResourceId != 0) hash ^= ResourceId.GetHashCode();
      if (Resource != 0) hash ^= Resource.GetHashCode();
      if (Num != 0) hash ^= Num.GetHashCode();
      if (CurrentNum != 0) hash ^= CurrentNum.GetHashCode();
      if (ActivityId != 0) hash ^= ActivityId.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ResourceId != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(ResourceId);
      }
      if (Resource != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) Resource);
      }
      if (Num != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Num);
      }
      if (CurrentNum != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(CurrentNum);
      }
      if (ActivityId != 0) {
        output.WriteRawTag(40);
        output.WriteSInt32(ActivityId);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ResourceId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ResourceId);
      }
      if (Resource != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Resource);
      }
      if (Num != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Num);
      }
      if (CurrentNum != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(CurrentNum);
      }
      if (ActivityId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ActivityId);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DroppingItemPB other) {
      if (other == null) {
        return;
      }
      if (other.ResourceId != 0) {
        ResourceId = other.ResourceId;
      }
      if (other.Resource != 0) {
        Resource = other.Resource;
      }
      if (other.Num != 0) {
        Num = other.Num;
      }
      if (other.CurrentNum != 0) {
        CurrentNum = other.CurrentNum;
      }
      if (other.ActivityId != 0) {
        ActivityId = other.ActivityId;
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
            ResourceId = input.ReadSInt32();
            break;
          }
          case 16: {
            resource_ = (global::ResourcePB) input.ReadEnum();
            break;
          }
          case 24: {
            Num = input.ReadSInt32();
            break;
          }
          case 32: {
            CurrentNum = input.ReadSInt32();
            break;
          }
          case 40: {
            ActivityId = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
