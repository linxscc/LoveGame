// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: controller_department.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from controller_department.proto</summary>
  public static partial class ControllerDepartmentReflection {

    #region Descriptor
    /// <summary>File descriptor for controller_department.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ControllerDepartmentReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Chtjb250cm9sbGVyX2RlcGFydG1lbnQucHJvdG8SCWNvbS5wcm90bxoKYmFz",
            "ZS5wcm90bxoaYmVhbl9kZXBhcnRtZW50X3J1bGUucHJvdG8aGmJlYW5fdXNl",
            "cl9kZXBhcnRtZW50LnByb3RvGhRiZWFuX3VzZXJfaXRlbS5wcm90bxoUYmVh",
            "bl91c2VyX2ZhbnMucHJvdG8aFGJlYW5fZmFuc19ydWxlLnByb3RvGiBiZWFu",
            "X2RlcGFydG1lbnRfcG93ZXJfcnVsZS5wcm90bxogYmVhbl9kZXBhcnRtZW50",
            "X2F3YXJkX3J1bGUucHJvdG8aEGJlYW5fYXdhcmQucHJvdG8i+wEKEURlcGFy",
            "dG1lbnRSdWxlUmVzEgsKA3JldBgBIAEoERI1ChBkZXBhcnRtZW50X3J1bGVz",
            "GAIgAygLMhsuY29tLnByb3RvLkRlcGFydG1lbnRSdWxlUEISKQoKZmFuc19y",
            "dWxlcxgDIAMoCzIVLmNvbS5wcm90by5GYW5zUnVsZVBCEjUKC3Bvd2VyX3J1",
            "bGVzGAQgAygLMiAuY29tLnByb3RvLkRlcGFydG1lbnRQb3dlclJ1bGVQQhJA",
            "ChZkZXBhcnRtZW50X2F3YXJkX3J1bGVzGAUgAygLMiAuY29tLnByb3RvLkRl",
            "cGFydG1lbnRBd2FyZFJ1bGVQQiJ5Cg9NeURlcGFydG1lbnRSZXMSCwoDcmV0",
            "GAEgASgREjMKDm15X2RlcGFydG1lbnRzGAIgAygLMhsuY29tLnByb3RvLlVz",
            "ZXJEZXBhcnRtZW50UEISJAoFZmFuc3MYAyADKAsyFS5jb20ucHJvdG8uVXNl",
            "ckZhbnNQQiJDChVVcGdyYWRlRGVwYXJ0bWVudHNSZXESKgoPZGVwYXJ0bWVu",
            "dF90eXBlGAEgASgOMhEuRGVwYXJ0bWVudFR5cGVQQiKDAQoVVXBncmFkZURl",
            "cGFydG1lbnRzUmVzEgsKA3JldBgBIAEoERIzCg5teV9kZXBhcnRtZW50cxgC",
            "IAEoCzIbLmNvbS5wcm90by5Vc2VyRGVwYXJ0bWVudFBCEigKCXVzZXJfaXRl",
            "bRgDIAEoCzIVLmNvbS5wcm90by5Vc2VySXRlbVBCIkEKE0RlcGFydG1lbnRB",
            "d2FyZHNSZXESKgoPZGVwYXJ0bWVudF90eXBlGAEgASgOMhEuRGVwYXJ0bWVu",
            "dFR5cGVQQiJ6ChNEZXBhcnRtZW50QXdhcmRzUmVzEgsKA3JldBgBIAEoERIi",
            "CgZhd2FyZHMYAiADKAsyEi5jb20ucHJvdG8uQXdhcmRQQhIyCg1teV9kZXBh",
            "cnRtZW50GAMgASgLMhsuY29tLnByb3RvLlVzZXJEZXBhcnRtZW50UEJCPQof",
            "bmV0LmdhbGFzcG9ydHMuYmlnc3Rhci5wcm90b2NvbEIaRGVwYXJ0bWVudENv",
            "bnRyb2xsZXJQcm90b3NiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, global::Com.Proto.BeanDepartmentRuleReflection.Descriptor, global::Com.Proto.BeanUserDepartmentReflection.Descriptor, global::Com.Proto.BeanUserItemReflection.Descriptor, global::Com.Proto.BeanUserFansReflection.Descriptor, global::Com.Proto.BeanFansRuleReflection.Descriptor, global::Com.Proto.BeanDepartmentPowerRuleReflection.Descriptor, global::Com.Proto.BeanDepartmentAwardRuleReflection.Descriptor, global::Com.Proto.BeanAwardReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.DepartmentRuleRes), global::Com.Proto.DepartmentRuleRes.Parser, new[]{ "Ret", "DepartmentRules", "FansRules", "PowerRules", "DepartmentAwardRules" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.MyDepartmentRes), global::Com.Proto.MyDepartmentRes.Parser, new[]{ "Ret", "MyDepartments", "Fanss" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UpgradeDepartmentsReq), global::Com.Proto.UpgradeDepartmentsReq.Parser, new[]{ "DepartmentType" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UpgradeDepartmentsRes), global::Com.Proto.UpgradeDepartmentsRes.Parser, new[]{ "Ret", "MyDepartments", "UserItem" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.DepartmentAwardsReq), global::Com.Proto.DepartmentAwardsReq.Parser, new[]{ "DepartmentType" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.DepartmentAwardsRes), global::Com.Proto.DepartmentAwardsRes.Parser, new[]{ "Ret", "Awards", "MyDepartment" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class DepartmentRuleRes : pb::IMessage<DepartmentRuleRes> {
    private static readonly pb::MessageParser<DepartmentRuleRes> _parser = new pb::MessageParser<DepartmentRuleRes>(() => new DepartmentRuleRes());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DepartmentRuleRes> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.ControllerDepartmentReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentRuleRes() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentRuleRes(DepartmentRuleRes other) : this() {
      ret_ = other.ret_;
      departmentRules_ = other.departmentRules_.Clone();
      fansRules_ = other.fansRules_.Clone();
      powerRules_ = other.powerRules_.Clone();
      departmentAwardRules_ = other.departmentAwardRules_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentRuleRes Clone() {
      return new DepartmentRuleRes(this);
    }

    /// <summary>Field number for the "ret" field.</summary>
    public const int RetFieldNumber = 1;
    private int ret_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Ret {
      get { return ret_; }
      set {
        ret_ = value;
      }
    }

    /// <summary>Field number for the "department_rules" field.</summary>
    public const int DepartmentRulesFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Com.Proto.DepartmentRulePB> _repeated_departmentRules_codec
        = pb::FieldCodec.ForMessage(18, global::Com.Proto.DepartmentRulePB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.DepartmentRulePB> departmentRules_ = new pbc::RepeatedField<global::Com.Proto.DepartmentRulePB>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.DepartmentRulePB> DepartmentRules {
      get { return departmentRules_; }
    }

    /// <summary>Field number for the "fans_rules" field.</summary>
    public const int FansRulesFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Com.Proto.FansRulePB> _repeated_fansRules_codec
        = pb::FieldCodec.ForMessage(26, global::Com.Proto.FansRulePB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.FansRulePB> fansRules_ = new pbc::RepeatedField<global::Com.Proto.FansRulePB>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.FansRulePB> FansRules {
      get { return fansRules_; }
    }

    /// <summary>Field number for the "power_rules" field.</summary>
    public const int PowerRulesFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Com.Proto.DepartmentPowerRulePB> _repeated_powerRules_codec
        = pb::FieldCodec.ForMessage(34, global::Com.Proto.DepartmentPowerRulePB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.DepartmentPowerRulePB> powerRules_ = new pbc::RepeatedField<global::Com.Proto.DepartmentPowerRulePB>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.DepartmentPowerRulePB> PowerRules {
      get { return powerRules_; }
    }

    /// <summary>Field number for the "department_award_rules" field.</summary>
    public const int DepartmentAwardRulesFieldNumber = 5;
    private static readonly pb::FieldCodec<global::Com.Proto.DepartmentAwardRulePB> _repeated_departmentAwardRules_codec
        = pb::FieldCodec.ForMessage(42, global::Com.Proto.DepartmentAwardRulePB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.DepartmentAwardRulePB> departmentAwardRules_ = new pbc::RepeatedField<global::Com.Proto.DepartmentAwardRulePB>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.DepartmentAwardRulePB> DepartmentAwardRules {
      get { return departmentAwardRules_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DepartmentRuleRes);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DepartmentRuleRes other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Ret != other.Ret) return false;
      if(!departmentRules_.Equals(other.departmentRules_)) return false;
      if(!fansRules_.Equals(other.fansRules_)) return false;
      if(!powerRules_.Equals(other.powerRules_)) return false;
      if(!departmentAwardRules_.Equals(other.departmentAwardRules_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Ret != 0) hash ^= Ret.GetHashCode();
      hash ^= departmentRules_.GetHashCode();
      hash ^= fansRules_.GetHashCode();
      hash ^= powerRules_.GetHashCode();
      hash ^= departmentAwardRules_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Ret != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(Ret);
      }
      departmentRules_.WriteTo(output, _repeated_departmentRules_codec);
      fansRules_.WriteTo(output, _repeated_fansRules_codec);
      powerRules_.WriteTo(output, _repeated_powerRules_codec);
      departmentAwardRules_.WriteTo(output, _repeated_departmentAwardRules_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Ret != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Ret);
      }
      size += departmentRules_.CalculateSize(_repeated_departmentRules_codec);
      size += fansRules_.CalculateSize(_repeated_fansRules_codec);
      size += powerRules_.CalculateSize(_repeated_powerRules_codec);
      size += departmentAwardRules_.CalculateSize(_repeated_departmentAwardRules_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DepartmentRuleRes other) {
      if (other == null) {
        return;
      }
      if (other.Ret != 0) {
        Ret = other.Ret;
      }
      departmentRules_.Add(other.departmentRules_);
      fansRules_.Add(other.fansRules_);
      powerRules_.Add(other.powerRules_);
      departmentAwardRules_.Add(other.departmentAwardRules_);
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
            Ret = input.ReadSInt32();
            break;
          }
          case 18: {
            departmentRules_.AddEntriesFrom(input, _repeated_departmentRules_codec);
            break;
          }
          case 26: {
            fansRules_.AddEntriesFrom(input, _repeated_fansRules_codec);
            break;
          }
          case 34: {
            powerRules_.AddEntriesFrom(input, _repeated_powerRules_codec);
            break;
          }
          case 42: {
            departmentAwardRules_.AddEntriesFrom(input, _repeated_departmentAwardRules_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class MyDepartmentRes : pb::IMessage<MyDepartmentRes> {
    private static readonly pb::MessageParser<MyDepartmentRes> _parser = new pb::MessageParser<MyDepartmentRes>(() => new MyDepartmentRes());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<MyDepartmentRes> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.ControllerDepartmentReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MyDepartmentRes() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MyDepartmentRes(MyDepartmentRes other) : this() {
      ret_ = other.ret_;
      myDepartments_ = other.myDepartments_.Clone();
      fanss_ = other.fanss_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MyDepartmentRes Clone() {
      return new MyDepartmentRes(this);
    }

    /// <summary>Field number for the "ret" field.</summary>
    public const int RetFieldNumber = 1;
    private int ret_;
    /// <summary>
    ///响应码
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Ret {
      get { return ret_; }
      set {
        ret_ = value;
      }
    }

    /// <summary>Field number for the "my_departments" field.</summary>
    public const int MyDepartmentsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Com.Proto.UserDepartmentPB> _repeated_myDepartments_codec
        = pb::FieldCodec.ForMessage(18, global::Com.Proto.UserDepartmentPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.UserDepartmentPB> myDepartments_ = new pbc::RepeatedField<global::Com.Proto.UserDepartmentPB>();
    /// <summary>
    ///应援会信息
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.UserDepartmentPB> MyDepartments {
      get { return myDepartments_; }
    }

    /// <summary>Field number for the "fanss" field.</summary>
    public const int FanssFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Com.Proto.UserFansPB> _repeated_fanss_codec
        = pb::FieldCodec.ForMessage(26, global::Com.Proto.UserFansPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.UserFansPB> fanss_ = new pbc::RepeatedField<global::Com.Proto.UserFansPB>();
    /// <summary>
    ///粉丝信息
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.UserFansPB> Fanss {
      get { return fanss_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as MyDepartmentRes);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(MyDepartmentRes other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Ret != other.Ret) return false;
      if(!myDepartments_.Equals(other.myDepartments_)) return false;
      if(!fanss_.Equals(other.fanss_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Ret != 0) hash ^= Ret.GetHashCode();
      hash ^= myDepartments_.GetHashCode();
      hash ^= fanss_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Ret != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(Ret);
      }
      myDepartments_.WriteTo(output, _repeated_myDepartments_codec);
      fanss_.WriteTo(output, _repeated_fanss_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Ret != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Ret);
      }
      size += myDepartments_.CalculateSize(_repeated_myDepartments_codec);
      size += fanss_.CalculateSize(_repeated_fanss_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(MyDepartmentRes other) {
      if (other == null) {
        return;
      }
      if (other.Ret != 0) {
        Ret = other.Ret;
      }
      myDepartments_.Add(other.myDepartments_);
      fanss_.Add(other.fanss_);
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
            Ret = input.ReadSInt32();
            break;
          }
          case 18: {
            myDepartments_.AddEntriesFrom(input, _repeated_myDepartments_codec);
            break;
          }
          case 26: {
            fanss_.AddEntriesFrom(input, _repeated_fanss_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class UpgradeDepartmentsReq : pb::IMessage<UpgradeDepartmentsReq> {
    private static readonly pb::MessageParser<UpgradeDepartmentsReq> _parser = new pb::MessageParser<UpgradeDepartmentsReq>(() => new UpgradeDepartmentsReq());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UpgradeDepartmentsReq> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.ControllerDepartmentReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpgradeDepartmentsReq() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpgradeDepartmentsReq(UpgradeDepartmentsReq other) : this() {
      departmentType_ = other.departmentType_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpgradeDepartmentsReq Clone() {
      return new UpgradeDepartmentsReq(this);
    }

    /// <summary>Field number for the "department_type" field.</summary>
    public const int DepartmentTypeFieldNumber = 1;
    private global::DepartmentTypePB departmentType_ = 0;
    /// <summary>
    ///升级的属性类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::DepartmentTypePB DepartmentType {
      get { return departmentType_; }
      set {
        departmentType_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UpgradeDepartmentsReq);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UpgradeDepartmentsReq other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (DepartmentType != other.DepartmentType) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (DepartmentType != 0) hash ^= DepartmentType.GetHashCode();
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
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (DepartmentType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) DepartmentType);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UpgradeDepartmentsReq other) {
      if (other == null) {
        return;
      }
      if (other.DepartmentType != 0) {
        DepartmentType = other.DepartmentType;
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
        }
      }
    }

  }

  public sealed partial class UpgradeDepartmentsRes : pb::IMessage<UpgradeDepartmentsRes> {
    private static readonly pb::MessageParser<UpgradeDepartmentsRes> _parser = new pb::MessageParser<UpgradeDepartmentsRes>(() => new UpgradeDepartmentsRes());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UpgradeDepartmentsRes> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.ControllerDepartmentReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpgradeDepartmentsRes() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpgradeDepartmentsRes(UpgradeDepartmentsRes other) : this() {
      ret_ = other.ret_;
      MyDepartments = other.myDepartments_ != null ? other.MyDepartments.Clone() : null;
      UserItem = other.userItem_ != null ? other.UserItem.Clone() : null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpgradeDepartmentsRes Clone() {
      return new UpgradeDepartmentsRes(this);
    }

    /// <summary>Field number for the "ret" field.</summary>
    public const int RetFieldNumber = 1;
    private int ret_;
    /// <summary>
    ///响应码
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Ret {
      get { return ret_; }
      set {
        ret_ = value;
      }
    }

    /// <summary>Field number for the "my_departments" field.</summary>
    public const int MyDepartmentsFieldNumber = 2;
    private global::Com.Proto.UserDepartmentPB myDepartments_;
    /// <summary>
    ///升级属性
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Proto.UserDepartmentPB MyDepartments {
      get { return myDepartments_; }
      set {
        myDepartments_ = value;
      }
    }

    /// <summary>Field number for the "user_item" field.</summary>
    public const int UserItemFieldNumber = 3;
    private global::Com.Proto.UserItemPB userItem_;
    /// <summary>
    ///道具信息
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Proto.UserItemPB UserItem {
      get { return userItem_; }
      set {
        userItem_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UpgradeDepartmentsRes);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UpgradeDepartmentsRes other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Ret != other.Ret) return false;
      if (!object.Equals(MyDepartments, other.MyDepartments)) return false;
      if (!object.Equals(UserItem, other.UserItem)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Ret != 0) hash ^= Ret.GetHashCode();
      if (myDepartments_ != null) hash ^= MyDepartments.GetHashCode();
      if (userItem_ != null) hash ^= UserItem.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Ret != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(Ret);
      }
      if (myDepartments_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(MyDepartments);
      }
      if (userItem_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(UserItem);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Ret != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Ret);
      }
      if (myDepartments_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(MyDepartments);
      }
      if (userItem_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(UserItem);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UpgradeDepartmentsRes other) {
      if (other == null) {
        return;
      }
      if (other.Ret != 0) {
        Ret = other.Ret;
      }
      if (other.myDepartments_ != null) {
        if (myDepartments_ == null) {
          myDepartments_ = new global::Com.Proto.UserDepartmentPB();
        }
        MyDepartments.MergeFrom(other.MyDepartments);
      }
      if (other.userItem_ != null) {
        if (userItem_ == null) {
          userItem_ = new global::Com.Proto.UserItemPB();
        }
        UserItem.MergeFrom(other.UserItem);
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
            Ret = input.ReadSInt32();
            break;
          }
          case 18: {
            if (myDepartments_ == null) {
              myDepartments_ = new global::Com.Proto.UserDepartmentPB();
            }
            input.ReadMessage(myDepartments_);
            break;
          }
          case 26: {
            if (userItem_ == null) {
              userItem_ = new global::Com.Proto.UserItemPB();
            }
            input.ReadMessage(userItem_);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///领取奖励  departmentC/departmentAward
  /// </summary>
  public sealed partial class DepartmentAwardsReq : pb::IMessage<DepartmentAwardsReq> {
    private static readonly pb::MessageParser<DepartmentAwardsReq> _parser = new pb::MessageParser<DepartmentAwardsReq>(() => new DepartmentAwardsReq());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DepartmentAwardsReq> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.ControllerDepartmentReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentAwardsReq() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentAwardsReq(DepartmentAwardsReq other) : this() {
      departmentType_ = other.departmentType_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentAwardsReq Clone() {
      return new DepartmentAwardsReq(this);
    }

    /// <summary>Field number for the "department_type" field.</summary>
    public const int DepartmentTypeFieldNumber = 1;
    private global::DepartmentTypePB departmentType_ = 0;
    /// <summary>
    ///属性类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::DepartmentTypePB DepartmentType {
      get { return departmentType_; }
      set {
        departmentType_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DepartmentAwardsReq);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DepartmentAwardsReq other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (DepartmentType != other.DepartmentType) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (DepartmentType != 0) hash ^= DepartmentType.GetHashCode();
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
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (DepartmentType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) DepartmentType);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DepartmentAwardsReq other) {
      if (other == null) {
        return;
      }
      if (other.DepartmentType != 0) {
        DepartmentType = other.DepartmentType;
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
        }
      }
    }

  }

  public sealed partial class DepartmentAwardsRes : pb::IMessage<DepartmentAwardsRes> {
    private static readonly pb::MessageParser<DepartmentAwardsRes> _parser = new pb::MessageParser<DepartmentAwardsRes>(() => new DepartmentAwardsRes());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DepartmentAwardsRes> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.ControllerDepartmentReflection.Descriptor.MessageTypes[5]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentAwardsRes() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentAwardsRes(DepartmentAwardsRes other) : this() {
      ret_ = other.ret_;
      awards_ = other.awards_.Clone();
      MyDepartment = other.myDepartment_ != null ? other.MyDepartment.Clone() : null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DepartmentAwardsRes Clone() {
      return new DepartmentAwardsRes(this);
    }

    /// <summary>Field number for the "ret" field.</summary>
    public const int RetFieldNumber = 1;
    private int ret_;
    /// <summary>
    ///响应码
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Ret {
      get { return ret_; }
      set {
        ret_ = value;
      }
    }

    /// <summary>Field number for the "awards" field.</summary>
    public const int AwardsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Com.Proto.AwardPB> _repeated_awards_codec
        = pb::FieldCodec.ForMessage(18, global::Com.Proto.AwardPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.AwardPB> awards_ = new pbc::RepeatedField<global::Com.Proto.AwardPB>();
    /// <summary>
    ///奖励
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.AwardPB> Awards {
      get { return awards_; }
    }

    /// <summary>Field number for the "my_department" field.</summary>
    public const int MyDepartmentFieldNumber = 3;
    private global::Com.Proto.UserDepartmentPB myDepartment_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Proto.UserDepartmentPB MyDepartment {
      get { return myDepartment_; }
      set {
        myDepartment_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DepartmentAwardsRes);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DepartmentAwardsRes other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Ret != other.Ret) return false;
      if(!awards_.Equals(other.awards_)) return false;
      if (!object.Equals(MyDepartment, other.MyDepartment)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Ret != 0) hash ^= Ret.GetHashCode();
      hash ^= awards_.GetHashCode();
      if (myDepartment_ != null) hash ^= MyDepartment.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Ret != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(Ret);
      }
      awards_.WriteTo(output, _repeated_awards_codec);
      if (myDepartment_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(MyDepartment);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Ret != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Ret);
      }
      size += awards_.CalculateSize(_repeated_awards_codec);
      if (myDepartment_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(MyDepartment);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DepartmentAwardsRes other) {
      if (other == null) {
        return;
      }
      if (other.Ret != 0) {
        Ret = other.Ret;
      }
      awards_.Add(other.awards_);
      if (other.myDepartment_ != null) {
        if (myDepartment_ == null) {
          myDepartment_ = new global::Com.Proto.UserDepartmentPB();
        }
        MyDepartment.MergeFrom(other.MyDepartment);
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
            Ret = input.ReadSInt32();
            break;
          }
          case 18: {
            awards_.AddEntriesFrom(input, _repeated_awards_codec);
            break;
          }
          case 26: {
            if (myDepartment_ == null) {
              myDepartment_ = new global::Com.Proto.UserDepartmentPB();
            }
            input.ReadMessage(myDepartment_);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
