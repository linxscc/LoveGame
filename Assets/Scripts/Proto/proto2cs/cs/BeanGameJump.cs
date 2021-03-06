// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_game_jump.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_game_jump.proto</summary>
  public static partial class BeanGameJumpReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_game_jump.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanGameJumpReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChRiZWFuX2dhbWVfanVtcC5wcm90bxIJY29tLnByb3RvGgpiYXNlLnByb3Rv",
            "IjkKDkdhbWVKdW1wUnVsZVBCEgoKAmlkGAEgASgREgwKBHRpbWUYAiABKBES",
            "DQoFY291bnQYAyABKBEigQEKFEdhbWVKdW1wQXBwZWFySW5mb1BCEi4KC2Fw",
            "cGVhcl90eXBlGAEgASgOMhkuY29tLnByb3RvLkFwcGVhclR5cGVFbnVtEgwK",
            "BHJhdGUYAiABKAISDAoEdGltZRgDIAEoAhILCgNudW0YBCABKBESEAoIaW50",
            "ZXJ2YWwYBSABKAIiwAEKEkdhbWVKdW1wSXRlbVJ1bGVQQhIqCglpdGVtX3R5",
            "cGUYASABKA4yFy5jb20ucHJvdG8uSXRlbVR5cGVFbnVtEhIKCnJlb3VyY2Vf",
            "aWQYAiABKBESDwoHcmVvdXJjZRgDIAEoERIUCgxlZmZlY3RfdmFsdWUYBCAB",
            "KBESNAoLYXBwZWFyX2luZm8YBSABKAsyHy5jb20ucHJvdG8uR2FtZUp1bXBB",
            "cHBlYXJJbmZvUEISDQoFc3BlZWQYBiABKBEibAoTR2FtZUp1bXBMZXZlbFJ1",
            "bGVQQhIqCglpdGVtX3R5cGUYASABKA4yFy5jb20ucHJvdG8uSXRlbVR5cGVF",
            "bnVtEg0KBW9yZGVyGAIgASgREg0KBWxldmVsGAMgASgREgsKA251bRgEIAEo",
            "ESJ/Cg5HYW1lSnVtcEl0ZW1QQhIqCglpdGVtX3R5cGUYASABKA4yFy5jb20u",
            "cHJvdG8uSXRlbVR5cGVFbnVtEhIKCnJlb3VyY2VfaWQYAiABKBESDwoHcmVv",
            "dXJjZRgDIAEoERINCgVjb3VudBgEIAEoERINCgVncm91cBgFIAEoESprCgxJ",
            "dGVtVHlwZUVudW0SCAoETk9ORRAAEgoKBk5PUk1BTBABEggKBFJBUkUQAhIK",
            "CgZET1VCTEUQAxIMCghPVkVSVElNRRAEEggKBERFQUQQBRINCglHSVJMX1NU",
            "QVIQBhIICgRHRU1TEAcqPwoOQXBwZWFyVHlwZUVudW0SDwoLQVBQRUFSX05P",
            "TkUQABINCglGUkVRVUVOQ1kQARINCglGSVhFRF9OVU0QAkIxCh9uZXQuZ2Fs",
            "YXNwb3J0cy5iaWdzdGFyLnByb3RvY29sQg5HYW1lSnVtcFByb3Rvc2IGcHJv",
            "dG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Com.Proto.ItemTypeEnum), typeof(global::Com.Proto.AppearTypeEnum), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.GameJumpRulePB), global::Com.Proto.GameJumpRulePB.Parser, new[]{ "Id", "Time", "Count" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.GameJumpAppearInfoPB), global::Com.Proto.GameJumpAppearInfoPB.Parser, new[]{ "AppearType", "Rate", "Time", "Num", "Interval" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.GameJumpItemRulePB), global::Com.Proto.GameJumpItemRulePB.Parser, new[]{ "ItemType", "ReourceId", "Reource", "EffectValue", "AppearInfo", "Speed" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.GameJumpLevelRulePB), global::Com.Proto.GameJumpLevelRulePB.Parser, new[]{ "ItemType", "Order", "Level", "Num" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.GameJumpItemPB), global::Com.Proto.GameJumpItemPB.Parser, new[]{ "ItemType", "ReourceId", "Reource", "Count", "Group" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  /// <summary>
  ///道具类型
  /// </summary>
  public enum ItemTypeEnum {
    /// <summary>
    ///占位
    /// </summary>
    [pbr::OriginalName("NONE")] None = 0,
    /// <summary>
    ///普通道具
    /// </summary>
    [pbr::OriginalName("NORMAL")] Normal = 1,
    /// <summary>
    ///稀有道具
    /// </summary>
    [pbr::OriginalName("RARE")] Rare = 2,
    /// <summary>
    ///翻倍道具
    /// </summary>
    [pbr::OriginalName("DOUBLE")] Double = 3,
    /// <summary>
    ///加时道具
    /// </summary>
    [pbr::OriginalName("OVERTIME")] Overtime = 4,
    /// <summary>
    ///死亡道具
    /// </summary>
    [pbr::OriginalName("DEAD")] Dead = 5,
    /// <summary>
    ///少女星
    /// </summary>
    [pbr::OriginalName("GIRL_STAR")] GirlStar = 6,
    /// <summary>
    ///星钻
    /// </summary>
    [pbr::OriginalName("GEMS")] Gems = 7,
  }

  /// <summary>
  ///出现类型
  /// </summary>
  public enum AppearTypeEnum {
    /// <summary>
    ///占位
    /// </summary>
    [pbr::OriginalName("APPEAR_NONE")] AppearNone = 0,
    /// <summary>
    ///固定频率
    /// </summary>
    [pbr::OriginalName("FREQUENCY")] Frequency = 1,
    /// <summary>
    ///固定次数
    /// </summary>
    [pbr::OriginalName("FIXED_NUM")] FixedNum = 2,
  }

  #endregion

  #region Messages
  /// <summary>
  ///GameJumpRulePB GameJumpRule
  /// </summary>
  public sealed partial class GameJumpRulePB : pb::IMessage<GameJumpRulePB> {
    private static readonly pb::MessageParser<GameJumpRulePB> _parser = new pb::MessageParser<GameJumpRulePB>(() => new GameJumpRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GameJumpRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanGameJumpReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpRulePB(GameJumpRulePB other) : this() {
      id_ = other.id_;
      time_ = other.time_;
      count_ = other.count_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpRulePB Clone() {
      return new GameJumpRulePB(this);
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

    /// <summary>Field number for the "time" field.</summary>
    public const int TimeFieldNumber = 2;
    private int time_;
    /// <summary>
    ///游戏基本时长
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Time {
      get { return time_; }
      set {
        time_ = value;
      }
    }

    /// <summary>Field number for the "count" field.</summary>
    public const int CountFieldNumber = 3;
    private int count_;
    /// <summary>
    ///每日可参与次数
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Count {
      get { return count_; }
      set {
        count_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GameJumpRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GameJumpRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (Time != other.Time) return false;
      if (Count != other.Count) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (Time != 0) hash ^= Time.GetHashCode();
      if (Count != 0) hash ^= Count.GetHashCode();
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
      if (Time != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(Time);
      }
      if (Count != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Count);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Id);
      }
      if (Time != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Time);
      }
      if (Count != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Count);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GameJumpRulePB other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.Time != 0) {
        Time = other.Time;
      }
      if (other.Count != 0) {
        Count = other.Count;
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
          case 16: {
            Time = input.ReadSInt32();
            break;
          }
          case 24: {
            Count = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///GameJumpAppearInfoPB GameJumpAppearInfo
  /// </summary>
  public sealed partial class GameJumpAppearInfoPB : pb::IMessage<GameJumpAppearInfoPB> {
    private static readonly pb::MessageParser<GameJumpAppearInfoPB> _parser = new pb::MessageParser<GameJumpAppearInfoPB>(() => new GameJumpAppearInfoPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GameJumpAppearInfoPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanGameJumpReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpAppearInfoPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpAppearInfoPB(GameJumpAppearInfoPB other) : this() {
      appearType_ = other.appearType_;
      rate_ = other.rate_;
      time_ = other.time_;
      num_ = other.num_;
      interval_ = other.interval_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpAppearInfoPB Clone() {
      return new GameJumpAppearInfoPB(this);
    }

    /// <summary>Field number for the "appear_type" field.</summary>
    public const int AppearTypeFieldNumber = 1;
    private global::Com.Proto.AppearTypeEnum appearType_ = 0;
    /// <summary>
    ///出现类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Proto.AppearTypeEnum AppearType {
      get { return appearType_; }
      set {
        appearType_ = value;
      }
    }

    /// <summary>Field number for the "rate" field.</summary>
    public const int RateFieldNumber = 2;
    private float rate_;
    /// <summary>
    ///频率
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Rate {
      get { return rate_; }
      set {
        rate_ = value;
      }
    }

    /// <summary>Field number for the "time" field.</summary>
    public const int TimeFieldNumber = 3;
    private float time_;
    /// <summary>
    ///时间范围
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Time {
      get { return time_; }
      set {
        time_ = value;
      }
    }

    /// <summary>Field number for the "num" field.</summary>
    public const int NumFieldNumber = 4;
    private int num_;
    /// <summary>
    ///次数
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Num {
      get { return num_; }
      set {
        num_ = value;
      }
    }

    /// <summary>Field number for the "interval" field.</summary>
    public const int IntervalFieldNumber = 5;
    private float interval_;
    /// <summary>
    ///间隔
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Interval {
      get { return interval_; }
      set {
        interval_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GameJumpAppearInfoPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GameJumpAppearInfoPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (AppearType != other.AppearType) return false;
      if (Rate != other.Rate) return false;
      if (Time != other.Time) return false;
      if (Num != other.Num) return false;
      if (Interval != other.Interval) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (AppearType != 0) hash ^= AppearType.GetHashCode();
      if (Rate != 0F) hash ^= Rate.GetHashCode();
      if (Time != 0F) hash ^= Time.GetHashCode();
      if (Num != 0) hash ^= Num.GetHashCode();
      if (Interval != 0F) hash ^= Interval.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (AppearType != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) AppearType);
      }
      if (Rate != 0F) {
        output.WriteRawTag(21);
        output.WriteFloat(Rate);
      }
      if (Time != 0F) {
        output.WriteRawTag(29);
        output.WriteFloat(Time);
      }
      if (Num != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(Num);
      }
      if (Interval != 0F) {
        output.WriteRawTag(45);
        output.WriteFloat(Interval);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (AppearType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) AppearType);
      }
      if (Rate != 0F) {
        size += 1 + 4;
      }
      if (Time != 0F) {
        size += 1 + 4;
      }
      if (Num != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Num);
      }
      if (Interval != 0F) {
        size += 1 + 4;
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GameJumpAppearInfoPB other) {
      if (other == null) {
        return;
      }
      if (other.AppearType != 0) {
        AppearType = other.AppearType;
      }
      if (other.Rate != 0F) {
        Rate = other.Rate;
      }
      if (other.Time != 0F) {
        Time = other.Time;
      }
      if (other.Num != 0) {
        Num = other.Num;
      }
      if (other.Interval != 0F) {
        Interval = other.Interval;
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
            appearType_ = (global::Com.Proto.AppearTypeEnum) input.ReadEnum();
            break;
          }
          case 21: {
            Rate = input.ReadFloat();
            break;
          }
          case 29: {
            Time = input.ReadFloat();
            break;
          }
          case 32: {
            Num = input.ReadSInt32();
            break;
          }
          case 45: {
            Interval = input.ReadFloat();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///GameJumpItemRulePB GameJumpItemRule
  /// </summary>
  public sealed partial class GameJumpItemRulePB : pb::IMessage<GameJumpItemRulePB> {
    private static readonly pb::MessageParser<GameJumpItemRulePB> _parser = new pb::MessageParser<GameJumpItemRulePB>(() => new GameJumpItemRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GameJumpItemRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanGameJumpReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpItemRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpItemRulePB(GameJumpItemRulePB other) : this() {
      itemType_ = other.itemType_;
      reourceId_ = other.reourceId_;
      reource_ = other.reource_;
      effectValue_ = other.effectValue_;
      AppearInfo = other.appearInfo_ != null ? other.AppearInfo.Clone() : null;
      speed_ = other.speed_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpItemRulePB Clone() {
      return new GameJumpItemRulePB(this);
    }

    /// <summary>Field number for the "item_type" field.</summary>
    public const int ItemTypeFieldNumber = 1;
    private global::Com.Proto.ItemTypeEnum itemType_ = 0;
    /// <summary>
    ///游戏道具类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Proto.ItemTypeEnum ItemType {
      get { return itemType_; }
      set {
        itemType_ = value;
      }
    }

    /// <summary>Field number for the "reource_id" field.</summary>
    public const int ReourceIdFieldNumber = 2;
    private int reourceId_;
    /// <summary>
    ///资源ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ReourceId {
      get { return reourceId_; }
      set {
        reourceId_ = value;
      }
    }

    /// <summary>Field number for the "reource" field.</summary>
    public const int ReourceFieldNumber = 3;
    private int reource_;
    /// <summary>
    ///资源类型ItemTypeEnum为NORMAL、RARE才有意义
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Reource {
      get { return reource_; }
      set {
        reource_ = value;
      }
    }

    /// <summary>Field number for the "effect_value" field.</summary>
    public const int EffectValueFieldNumber = 4;
    private int effectValue_;
    /// <summary>
    ///效果数值
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int EffectValue {
      get { return effectValue_; }
      set {
        effectValue_ = value;
      }
    }

    /// <summary>Field number for the "appear_info" field.</summary>
    public const int AppearInfoFieldNumber = 5;
    private global::Com.Proto.GameJumpAppearInfoPB appearInfo_;
    /// <summary>
    ///道具出现规则
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Proto.GameJumpAppearInfoPB AppearInfo {
      get { return appearInfo_; }
      set {
        appearInfo_ = value;
      }
    }

    /// <summary>Field number for the "speed" field.</summary>
    public const int SpeedFieldNumber = 6;
    private int speed_;
    /// <summary>
    ///速度级别
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Speed {
      get { return speed_; }
      set {
        speed_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GameJumpItemRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GameJumpItemRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ItemType != other.ItemType) return false;
      if (ReourceId != other.ReourceId) return false;
      if (Reource != other.Reource) return false;
      if (EffectValue != other.EffectValue) return false;
      if (!object.Equals(AppearInfo, other.AppearInfo)) return false;
      if (Speed != other.Speed) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ItemType != 0) hash ^= ItemType.GetHashCode();
      if (ReourceId != 0) hash ^= ReourceId.GetHashCode();
      if (Reource != 0) hash ^= Reource.GetHashCode();
      if (EffectValue != 0) hash ^= EffectValue.GetHashCode();
      if (appearInfo_ != null) hash ^= AppearInfo.GetHashCode();
      if (Speed != 0) hash ^= Speed.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ItemType != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) ItemType);
      }
      if (ReourceId != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(ReourceId);
      }
      if (Reource != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Reource);
      }
      if (EffectValue != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(EffectValue);
      }
      if (appearInfo_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(AppearInfo);
      }
      if (Speed != 0) {
        output.WriteRawTag(48);
        output.WriteSInt32(Speed);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ItemType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ItemType);
      }
      if (ReourceId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ReourceId);
      }
      if (Reource != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Reource);
      }
      if (EffectValue != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(EffectValue);
      }
      if (appearInfo_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(AppearInfo);
      }
      if (Speed != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Speed);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GameJumpItemRulePB other) {
      if (other == null) {
        return;
      }
      if (other.ItemType != 0) {
        ItemType = other.ItemType;
      }
      if (other.ReourceId != 0) {
        ReourceId = other.ReourceId;
      }
      if (other.Reource != 0) {
        Reource = other.Reource;
      }
      if (other.EffectValue != 0) {
        EffectValue = other.EffectValue;
      }
      if (other.appearInfo_ != null) {
        if (appearInfo_ == null) {
          appearInfo_ = new global::Com.Proto.GameJumpAppearInfoPB();
        }
        AppearInfo.MergeFrom(other.AppearInfo);
      }
      if (other.Speed != 0) {
        Speed = other.Speed;
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
            itemType_ = (global::Com.Proto.ItemTypeEnum) input.ReadEnum();
            break;
          }
          case 16: {
            ReourceId = input.ReadSInt32();
            break;
          }
          case 24: {
            Reource = input.ReadSInt32();
            break;
          }
          case 32: {
            EffectValue = input.ReadSInt32();
            break;
          }
          case 42: {
            if (appearInfo_ == null) {
              appearInfo_ = new global::Com.Proto.GameJumpAppearInfoPB();
            }
            input.ReadMessage(appearInfo_);
            break;
          }
          case 48: {
            Speed = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///GameJumpLevelRulePB GameJumpLevelRule
  /// </summary>
  public sealed partial class GameJumpLevelRulePB : pb::IMessage<GameJumpLevelRulePB> {
    private static readonly pb::MessageParser<GameJumpLevelRulePB> _parser = new pb::MessageParser<GameJumpLevelRulePB>(() => new GameJumpLevelRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GameJumpLevelRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanGameJumpReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpLevelRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpLevelRulePB(GameJumpLevelRulePB other) : this() {
      itemType_ = other.itemType_;
      order_ = other.order_;
      level_ = other.level_;
      num_ = other.num_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpLevelRulePB Clone() {
      return new GameJumpLevelRulePB(this);
    }

    /// <summary>Field number for the "item_type" field.</summary>
    public const int ItemTypeFieldNumber = 1;
    private global::Com.Proto.ItemTypeEnum itemType_ = 0;
    /// <summary>
    ///道具类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Proto.ItemTypeEnum ItemType {
      get { return itemType_; }
      set {
        itemType_ = value;
      }
    }

    /// <summary>Field number for the "order" field.</summary>
    public const int OrderFieldNumber = 2;
    private int order_;
    /// <summary>
    ///量级
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Order {
      get { return order_; }
      set {
        order_ = value;
      }
    }

    /// <summary>Field number for the "level" field.</summary>
    public const int LevelFieldNumber = 3;
    private int level_;
    /// <summary>
    ///应援会等级
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Level {
      get { return level_; }
      set {
        level_ = value;
      }
    }

    /// <summary>Field number for the "num" field.</summary>
    public const int NumFieldNumber = 4;
    private int num_;
    /// <summary>
    ///单次数量
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Num {
      get { return num_; }
      set {
        num_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GameJumpLevelRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GameJumpLevelRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ItemType != other.ItemType) return false;
      if (Order != other.Order) return false;
      if (Level != other.Level) return false;
      if (Num != other.Num) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ItemType != 0) hash ^= ItemType.GetHashCode();
      if (Order != 0) hash ^= Order.GetHashCode();
      if (Level != 0) hash ^= Level.GetHashCode();
      if (Num != 0) hash ^= Num.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ItemType != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) ItemType);
      }
      if (Order != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(Order);
      }
      if (Level != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Level);
      }
      if (Num != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(Num);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ItemType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ItemType);
      }
      if (Order != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Order);
      }
      if (Level != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Level);
      }
      if (Num != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Num);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GameJumpLevelRulePB other) {
      if (other == null) {
        return;
      }
      if (other.ItemType != 0) {
        ItemType = other.ItemType;
      }
      if (other.Order != 0) {
        Order = other.Order;
      }
      if (other.Level != 0) {
        Level = other.Level;
      }
      if (other.Num != 0) {
        Num = other.Num;
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
            itemType_ = (global::Com.Proto.ItemTypeEnum) input.ReadEnum();
            break;
          }
          case 16: {
            Order = input.ReadSInt32();
            break;
          }
          case 24: {
            Level = input.ReadSInt32();
            break;
          }
          case 32: {
            Num = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///GameJumpItemPB GameJumpItem
  /// </summary>
  public sealed partial class GameJumpItemPB : pb::IMessage<GameJumpItemPB> {
    private static readonly pb::MessageParser<GameJumpItemPB> _parser = new pb::MessageParser<GameJumpItemPB>(() => new GameJumpItemPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GameJumpItemPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanGameJumpReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpItemPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpItemPB(GameJumpItemPB other) : this() {
      itemType_ = other.itemType_;
      reourceId_ = other.reourceId_;
      reource_ = other.reource_;
      count_ = other.count_;
      group_ = other.group_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameJumpItemPB Clone() {
      return new GameJumpItemPB(this);
    }

    /// <summary>Field number for the "item_type" field.</summary>
    public const int ItemTypeFieldNumber = 1;
    private global::Com.Proto.ItemTypeEnum itemType_ = 0;
    /// <summary>
    ///道具类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Proto.ItemTypeEnum ItemType {
      get { return itemType_; }
      set {
        itemType_ = value;
      }
    }

    /// <summary>Field number for the "reource_id" field.</summary>
    public const int ReourceIdFieldNumber = 2;
    private int reourceId_;
    /// <summary>
    ///资源ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ReourceId {
      get { return reourceId_; }
      set {
        reourceId_ = value;
      }
    }

    /// <summary>Field number for the "reource" field.</summary>
    public const int ReourceFieldNumber = 3;
    private int reource_;
    /// <summary>
    ///资源类型ItemTypeEnum为NORMAL、RARE才有意义
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Reource {
      get { return reource_; }
      set {
        reource_ = value;
      }
    }

    /// <summary>Field number for the "count" field.</summary>
    public const int CountFieldNumber = 4;
    private int count_;
    /// <summary>
    ///数量
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Count {
      get { return count_; }
      set {
        count_ = value;
      }
    }

    /// <summary>Field number for the "group" field.</summary>
    public const int GroupFieldNumber = 5;
    private int group_;
    /// <summary>
    ///分组0是主时间线;>0是翻倍时间线，有几个翻倍道具就有几组
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Group {
      get { return group_; }
      set {
        group_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GameJumpItemPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GameJumpItemPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ItemType != other.ItemType) return false;
      if (ReourceId != other.ReourceId) return false;
      if (Reource != other.Reource) return false;
      if (Count != other.Count) return false;
      if (Group != other.Group) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ItemType != 0) hash ^= ItemType.GetHashCode();
      if (ReourceId != 0) hash ^= ReourceId.GetHashCode();
      if (Reource != 0) hash ^= Reource.GetHashCode();
      if (Count != 0) hash ^= Count.GetHashCode();
      if (Group != 0) hash ^= Group.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ItemType != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) ItemType);
      }
      if (ReourceId != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(ReourceId);
      }
      if (Reource != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Reource);
      }
      if (Count != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(Count);
      }
      if (Group != 0) {
        output.WriteRawTag(40);
        output.WriteSInt32(Group);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ItemType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ItemType);
      }
      if (ReourceId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ReourceId);
      }
      if (Reource != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Reource);
      }
      if (Count != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Count);
      }
      if (Group != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Group);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GameJumpItemPB other) {
      if (other == null) {
        return;
      }
      if (other.ItemType != 0) {
        ItemType = other.ItemType;
      }
      if (other.ReourceId != 0) {
        ReourceId = other.ReourceId;
      }
      if (other.Reource != 0) {
        Reource = other.Reource;
      }
      if (other.Count != 0) {
        Count = other.Count;
      }
      if (other.Group != 0) {
        Group = other.Group;
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
            itemType_ = (global::Com.Proto.ItemTypeEnum) input.ReadEnum();
            break;
          }
          case 16: {
            ReourceId = input.ReadSInt32();
            break;
          }
          case 24: {
            Reource = input.ReadSInt32();
            break;
          }
          case 32: {
            Count = input.ReadSInt32();
            break;
          }
          case 40: {
            Group = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
