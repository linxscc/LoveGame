// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_clothes_goal.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_clothes_goal.proto</summary>
  public static partial class BeanClothesGoalReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_clothes_goal.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanClothesGoalReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdiZWFuX2Nsb3RoZXNfZ29hbC5wcm90bxIJY29tLnByb3RvGgpiYXNlLnBy",
            "b3RvIm0KDUNsb3RoZXNHb2FsUEISNwoRY2xvdGhlc19nb2FsX3R5cGUYASAB",
            "KA4yHC5jb20ucHJvdG8uQ2xvdGhlc0dvYWxUeXBlUEISEQoJY2hhbmdlTnVt",
            "GAIgASgREhAKCGV4dHJhTnVtGAMgASgRKoABChFDbG90aGVzR29hbFR5cGVQ",
            "QhIWChJDTEVBUkFOQ0VfTk9UX0dPQUwQABIVChFDTEVBUkFOQ0VfTk9UX0dF",
            "VBABEhAKDENMT1RIRVNfQ0FSRBACEhIKDkNMRUFSQU5DRV9HQU1FEAMSFgoS",
            "Q0xFQVJBTkNFX1ZJU0lUSU5HEARCNAofbmV0LmdhbGFzcG9ydHMuYmlnc3Rh",
            "ci5wcm90b2NvbEIRQ2xvdGhlc0dvYWxQcm90b3NiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Com.Proto.ClothesGoalTypePB), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.ClothesGoalPB), global::Com.Proto.ClothesGoalPB.Parser, new[]{ "ClothesGoalType", "ChangeNum", "ExtraNum" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum ClothesGoalTypePB {
    /// <summary>
    ///默认不需要要求
    /// </summary>
    [pbr::OriginalName("CLEARANCE_NOT_GOAL")] ClearanceNotGoal = 0,
    /// <summary>
    ///不可获得
    /// </summary>
    [pbr::OriginalName("CLEARANCE_NOT_GET")] ClearanceNotGet = 1,
    /// <summary>
    ///卡牌进化
    /// </summary>
    [pbr::OriginalName("CLOTHES_CARD")] ClothesCard = 2,
    /// <summary>
    ///通关关卡
    /// </summary>
    [pbr::OriginalName("CLEARANCE_GAME")] ClearanceGame = 3,
    /// <summary>
    ///通关探班
    /// </summary>
    [pbr::OriginalName("CLEARANCE_VISITING")] ClearanceVisiting = 4,
  }

  #endregion

  #region Messages
  /// <summary>
  ///ClothesGoalPB ClothesGoal
  /// </summary>
  public sealed partial class ClothesGoalPB : pb::IMessage<ClothesGoalPB> {
    private static readonly pb::MessageParser<ClothesGoalPB> _parser = new pb::MessageParser<ClothesGoalPB>(() => new ClothesGoalPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ClothesGoalPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanClothesGoalReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ClothesGoalPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ClothesGoalPB(ClothesGoalPB other) : this() {
      clothesGoalType_ = other.clothesGoalType_;
      changeNum_ = other.changeNum_;
      extraNum_ = other.extraNum_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ClothesGoalPB Clone() {
      return new ClothesGoalPB(this);
    }

    /// <summary>Field number for the "clothes_goal_type" field.</summary>
    public const int ClothesGoalTypeFieldNumber = 1;
    private global::Com.Proto.ClothesGoalTypePB clothesGoalType_ = 0;
    /// <summary>
    ///要求类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Proto.ClothesGoalTypePB ClothesGoalType {
      get { return clothesGoalType_; }
      set {
        clothesGoalType_ = value;
      }
    }

    /// <summary>Field number for the "changeNum" field.</summary>
    public const int ChangeNumFieldNumber = 2;
    private int changeNum_;
    /// <summary>
    ///要求描述的第一个参数
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ChangeNum {
      get { return changeNum_; }
      set {
        changeNum_ = value;
      }
    }

    /// <summary>Field number for the "extraNum" field.</summary>
    public const int ExtraNumFieldNumber = 3;
    private int extraNum_;
    /// <summary>
    ///要求描述的第二个参数
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ExtraNum {
      get { return extraNum_; }
      set {
        extraNum_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ClothesGoalPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ClothesGoalPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ClothesGoalType != other.ClothesGoalType) return false;
      if (ChangeNum != other.ChangeNum) return false;
      if (ExtraNum != other.ExtraNum) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ClothesGoalType != 0) hash ^= ClothesGoalType.GetHashCode();
      if (ChangeNum != 0) hash ^= ChangeNum.GetHashCode();
      if (ExtraNum != 0) hash ^= ExtraNum.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ClothesGoalType != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) ClothesGoalType);
      }
      if (ChangeNum != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(ChangeNum);
      }
      if (ExtraNum != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(ExtraNum);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ClothesGoalType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ClothesGoalType);
      }
      if (ChangeNum != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ChangeNum);
      }
      if (ExtraNum != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ExtraNum);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ClothesGoalPB other) {
      if (other == null) {
        return;
      }
      if (other.ClothesGoalType != 0) {
        ClothesGoalType = other.ClothesGoalType;
      }
      if (other.ChangeNum != 0) {
        ChangeNum = other.ChangeNum;
      }
      if (other.ExtraNum != 0) {
        ExtraNum = other.ExtraNum;
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
            clothesGoalType_ = (global::Com.Proto.ClothesGoalTypePB) input.ReadEnum();
            break;
          }
          case 16: {
            ChangeNum = input.ReadSInt32();
            break;
          }
          case 24: {
            ExtraNum = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code