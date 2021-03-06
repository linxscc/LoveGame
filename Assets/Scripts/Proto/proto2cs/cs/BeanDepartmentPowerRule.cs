// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_department_power_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_department_power_rule.proto</summary>
  public static partial class BeanDepartmentPowerRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_department_power_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanDepartmentPowerRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiBiZWFuX2RlcGFydG1lbnRfcG93ZXJfcnVsZS5wcm90bxIJY29tLnByb3Rv",
            "GgpiYXNlLnByb3RvIjoKFURlcGFydG1lbnRQb3dlclJ1bGVQQhINCgVsZXZl",
            "bBgBIAEoERISCgptYXhfZW5lcmd5GAIgASgRQjwKH25ldC5nYWxhc3BvcnRz",
            "LmJpZ3N0YXIucHJvdG9jb2xCGURlcGFydG1lbnRQb3dlclJ1bGVQcm90b3Ni",
            "BnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.DepartmentPowerRulePB), global::Com.Proto.DepartmentPowerRulePB.Parser, new[]{ "Level", "MaxEnergy" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///DepartmentPowerRulePB DepartmentPowerRule
  /// </summary>
  public sealed partial class DepartmentPowerRulePB : pb::IMessage<DepartmentPowerRulePB> {
    private static readonly pb::MessageParser<DepartmentPowerRulePB> _parser = new pb::MessageParser<DepartmentPowerRulePB>(() => new DepartmentPowerRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DepartmentPowerRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanDepartmentPowerRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentPowerRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentPowerRulePB(DepartmentPowerRulePB other) : this() {
      level_ = other.level_;
      maxEnergy_ = other.maxEnergy_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentPowerRulePB Clone() {
      return new DepartmentPowerRulePB(this);
    }

    /// <summary>Field number for the "level" field.</summary>
    public const int LevelFieldNumber = 1;
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

    /// <summary>Field number for the "max_energy" field.</summary>
    public const int MaxEnergyFieldNumber = 2;
    private int maxEnergy_;
    /// <summary>
    ///体力上限
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int MaxEnergy {
      get { return maxEnergy_; }
      set {
        maxEnergy_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DepartmentPowerRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DepartmentPowerRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Level != other.Level) return false;
      if (MaxEnergy != other.MaxEnergy) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Level != 0) hash ^= Level.GetHashCode();
      if (MaxEnergy != 0) hash ^= MaxEnergy.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Level != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(Level);
      }
      if (MaxEnergy != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(MaxEnergy);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Level != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Level);
      }
      if (MaxEnergy != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(MaxEnergy);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DepartmentPowerRulePB other) {
      if (other == null) {
        return;
      }
      if (other.Level != 0) {
        Level = other.Level;
      }
      if (other.MaxEnergy != 0) {
        MaxEnergy = other.MaxEnergy;
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
            Level = input.ReadSInt32();
            break;
          }
          case 16: {
            MaxEnergy = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
