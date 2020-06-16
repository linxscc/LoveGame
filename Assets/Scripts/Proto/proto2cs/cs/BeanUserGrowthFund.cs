// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_growth_fund.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_growth_fund.proto</summary>
  public static partial class BeanUserGrowthFundReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_growth_fund.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserGrowthFundReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChtiZWFuX3VzZXJfZ3Jvd3RoX2Z1bmQucHJvdG8SCWNvbS5wcm90bxoKYmFz",
            "ZS5wcm90byIoChBVc2VyR3Jvd3RoRnVuZFBCEhQKDGF3YXJkX3N0YXRlcxgB",
            "IAMoEUI3Ch9uZXQuZ2FsYXNwb3J0cy5iaWdzdGFyLnByb3RvY29sQhRVc2Vy",
            "R3Jvd3RoRnVuZFByb3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserGrowthFundPB), global::Com.Proto.UserGrowthFundPB.Parser, new[]{ "AwardStates" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserGrowthFundPB UserGrowthFund
  /// </summary>
  public sealed partial class UserGrowthFundPB : pb::IMessage<UserGrowthFundPB> {
    private static readonly pb::MessageParser<UserGrowthFundPB> _parser = new pb::MessageParser<UserGrowthFundPB>(() => new UserGrowthFundPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserGrowthFundPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserGrowthFundReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserGrowthFundPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserGrowthFundPB(UserGrowthFundPB other) : this() {
      awardStates_ = other.awardStates_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserGrowthFundPB Clone() {
      return new UserGrowthFundPB(this);
    }

    /// <summary>Field number for the "award_states" field.</summary>
    public const int AwardStatesFieldNumber = 1;
    private static readonly pb::FieldCodec<int> _repeated_awardStates_codec
        = pb::FieldCodec.ForSInt32(10);
    private readonly pbc::RepeatedField<int> awardStates_ = new pbc::RepeatedField<int>();
    /// <summary>
    ///领取的奖励列表
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> AwardStates {
      get { return awardStates_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserGrowthFundPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserGrowthFundPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!awardStates_.Equals(other.awardStates_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= awardStates_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      awardStates_.WriteTo(output, _repeated_awardStates_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += awardStates_.CalculateSize(_repeated_awardStates_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserGrowthFundPB other) {
      if (other == null) {
        return;
      }
      awardStates_.Add(other.awardStates_);
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
            awardStates_.AddEntriesFrom(input, _repeated_awardStates_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
