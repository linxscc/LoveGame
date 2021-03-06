// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_level_extra.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_level_extra.proto</summary>
  public static partial class BeanLevelExtraReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_level_extra.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanLevelExtraReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChZiZWFuX2xldmVsX2V4dHJhLnByb3RvEgljb20ucHJvdG8aCmJhc2UucHJv",
            "dG8iYAoMTGV2ZWxFeHRyYVBCEhAKCGxldmVsX2lkGAEgASgREhgKEGRlcGFy",
            "dG1lbnRfbGV2ZWwYAiABKBESDgoGY2FyZElkGAMgASgREhQKDGZhdm9yYWJp",
            "bGl0eRgEIAEoEUIzCh9uZXQuZ2FsYXNwb3J0cy5iaWdzdGFyLnByb3RvY29s",
            "QhBMZXZlbEV4dHJhUHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.LevelExtraPB), global::Com.Proto.LevelExtraPB.Parser, new[]{ "LevelId", "DepartmentLevel", "CardId", "Favorability" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///LevelExtraPB LevelExtra
  /// </summary>
  public sealed partial class LevelExtraPB : pb::IMessage<LevelExtraPB> {
    private static readonly pb::MessageParser<LevelExtraPB> _parser = new pb::MessageParser<LevelExtraPB>(() => new LevelExtraPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<LevelExtraPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanLevelExtraReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LevelExtraPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LevelExtraPB(LevelExtraPB other) : this() {
      levelId_ = other.levelId_;
      departmentLevel_ = other.departmentLevel_;
      cardId_ = other.cardId_;
      favorability_ = other.favorability_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LevelExtraPB Clone() {
      return new LevelExtraPB(this);
    }

    /// <summary>Field number for the "level_id" field.</summary>
    public const int LevelIdFieldNumber = 1;
    private int levelId_;
    /// <summary>
    ///通关相应章节最后关卡Id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int LevelId {
      get { return levelId_; }
      set {
        levelId_ = value;
      }
    }

    /// <summary>Field number for the "department_level" field.</summary>
    public const int DepartmentLevelFieldNumber = 2;
    private int departmentLevel_;
    /// <summary>
    ///应援会等级要求
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int DepartmentLevel {
      get { return departmentLevel_; }
      set {
        departmentLevel_ = value;
      }
    }

    /// <summary>Field number for the "cardId" field.</summary>
    public const int CardIdFieldNumber = 3;
    private int cardId_;
    /// <summary>
    ///需要解锁的星缘
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CardId {
      get { return cardId_; }
      set {
        cardId_ = value;
      }
    }

    /// <summary>Field number for the "favorability" field.</summary>
    public const int FavorabilityFieldNumber = 4;
    private int favorability_;
    /// <summary>
    ///好感度等级要求
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Favorability {
      get { return favorability_; }
      set {
        favorability_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as LevelExtraPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(LevelExtraPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (LevelId != other.LevelId) return false;
      if (DepartmentLevel != other.DepartmentLevel) return false;
      if (CardId != other.CardId) return false;
      if (Favorability != other.Favorability) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (LevelId != 0) hash ^= LevelId.GetHashCode();
      if (DepartmentLevel != 0) hash ^= DepartmentLevel.GetHashCode();
      if (CardId != 0) hash ^= CardId.GetHashCode();
      if (Favorability != 0) hash ^= Favorability.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (LevelId != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(LevelId);
      }
      if (DepartmentLevel != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(DepartmentLevel);
      }
      if (CardId != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(CardId);
      }
      if (Favorability != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(Favorability);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (LevelId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(LevelId);
      }
      if (DepartmentLevel != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(DepartmentLevel);
      }
      if (CardId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(CardId);
      }
      if (Favorability != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Favorability);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(LevelExtraPB other) {
      if (other == null) {
        return;
      }
      if (other.LevelId != 0) {
        LevelId = other.LevelId;
      }
      if (other.DepartmentLevel != 0) {
        DepartmentLevel = other.DepartmentLevel;
      }
      if (other.CardId != 0) {
        CardId = other.CardId;
      }
      if (other.Favorability != 0) {
        Favorability = other.Favorability;
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
            LevelId = input.ReadSInt32();
            break;
          }
          case 16: {
            DepartmentLevel = input.ReadSInt32();
            break;
          }
          case 24: {
            CardId = input.ReadSInt32();
            break;
          }
          case 32: {
            Favorability = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
