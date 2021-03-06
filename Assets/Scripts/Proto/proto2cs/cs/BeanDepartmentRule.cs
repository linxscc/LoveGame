// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_department_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_department_rule.proto</summary>
  public static partial class BeanDepartmentRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_department_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanDepartmentRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChpiZWFuX2RlcGFydG1lbnRfcnVsZS5wcm90bxIJY29tLnByb3RvGgpiYXNl",
            "LnByb3RvImkKEERlcGFydG1lbnRSdWxlUEISKgoPZGVwYXJ0bWVudF90eXBl",
            "GAEgASgOMhEuRGVwYXJ0bWVudFR5cGVQQhINCgVsZXZlbBgCIAEoERILCgNl",
            "eHAYAyABKBESDQoFcG93ZXIYBCABKBFCNwofbmV0LmdhbGFzcG9ydHMuYmln",
            "c3Rhci5wcm90b2NvbEIURGVwYXJ0bWVudFJ1bGVQcm90b3NiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.DepartmentRulePB), global::Com.Proto.DepartmentRulePB.Parser, new[]{ "DepartmentType", "Level", "Exp", "Power" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///DepartmentRulePB DepartmentRule
  /// </summary>
  public sealed partial class DepartmentRulePB : pb::IMessage<DepartmentRulePB> {
    private static readonly pb::MessageParser<DepartmentRulePB> _parser = new pb::MessageParser<DepartmentRulePB>(() => new DepartmentRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DepartmentRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanDepartmentRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentRulePB(DepartmentRulePB other) : this() {
      departmentType_ = other.departmentType_;
      level_ = other.level_;
      exp_ = other.exp_;
      power_ = other.power_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentRulePB Clone() {
      return new DepartmentRulePB(this);
    }

    /// <summary>Field number for the "department_type" field.</summary>
    public const int DepartmentTypeFieldNumber = 1;
    private global::DepartmentTypePB departmentType_ = 0;
    /// <summary>
    ///id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::DepartmentTypePB DepartmentType {
      get { return departmentType_; }
      set {
        departmentType_ = value;
      }
    }

    /// <summary>Field number for the "level" field.</summary>
    public const int LevelFieldNumber = 2;
    private int level_;
    /// <summary>
    ///level
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Level {
      get { return level_; }
      set {
        level_ = value;
      }
    }

    /// <summary>Field number for the "exp" field.</summary>
    public const int ExpFieldNumber = 3;
    private int exp_;
    /// <summary>
    ///exp
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Exp {
      get { return exp_; }
      set {
        exp_ = value;
      }
    }

    /// <summary>Field number for the "power" field.</summary>
    public const int PowerFieldNumber = 4;
    private int power_;
    /// <summary>
    ///power
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Power {
      get { return power_; }
      set {
        power_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DepartmentRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DepartmentRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (DepartmentType != other.DepartmentType) return false;
      if (Level != other.Level) return false;
      if (Exp != other.Exp) return false;
      if (Power != other.Power) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (DepartmentType != 0) hash ^= DepartmentType.GetHashCode();
      if (Level != 0) hash ^= Level.GetHashCode();
      if (Exp != 0) hash ^= Exp.GetHashCode();
      if (Power != 0) hash ^= Power.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (DepartmentType != 0) {
        output.WriteRawTag(8);
        output.WriteEnum((int) DepartmentType);
      }
      if (Level != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(Level);
      }
      if (Exp != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Exp);
      }
      if (Power != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(Power);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (DepartmentType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) DepartmentType);
      }
      if (Level != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Level);
      }
      if (Exp != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Exp);
      }
      if (Power != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Power);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DepartmentRulePB other) {
      if (other == null) {
        return;
      }
      if (other.DepartmentType != 0) {
        DepartmentType = other.DepartmentType;
      }
      if (other.Level != 0) {
        Level = other.Level;
      }
      if (other.Exp != 0) {
        Exp = other.Exp;
      }
      if (other.Power != 0) {
        Power = other.Power;
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
            departmentType_ = (global::DepartmentTypePB) input.ReadEnum();
            break;
          }
          case 16: {
            Level = input.ReadSInt32();
            break;
          }
          case 24: {
            Exp = input.ReadSInt32();
            break;
          }
          case 32: {
            Power = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
