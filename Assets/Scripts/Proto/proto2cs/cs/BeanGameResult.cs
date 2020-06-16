// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_game_result.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_game_result.proto</summary>
  public static partial class BeanGameResultReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_game_result.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanGameResultReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChZiZWFuX2dhbWVfcmVzdWx0LnByb3RvEgljb20ucHJvdG8aCmJhc2UucHJv",
            "dG8aEGJlYW5fYXdhcmQucHJvdG8aFGJlYW5fdXNlcl9jYXJkLnByb3RvGiVi",
            "ZWFuX3VzZXJfYWN0aXZpdHlfaG9saWRheV9pbmZvLnByb3RvIuwBCgxHYW1l",
            "UmVzdWx0UEISDgoGdXNlcklkGAEgASgREgwKBHN0YXIYAiABKBESCwoDY2Fw",
            "GAMgASgREgsKA2V4cBgEIAEoERIPCgdjYXJkRXhwGAUgASgREiIKBmF3YXJk",
            "cxgGIAMoCzISLmNvbS5wcm90by5Bd2FyZFBCEhIKCmNyZWF0ZVRpbWUYByAB",
            "KBISKQoKdXNlcl9jYXJkcxgIIAMoCzIVLmNvbS5wcm90by5Vc2VyQ2FyZFBC",
            "EjAKDWRyb3BwaW5nX2l0ZW0YCSADKAsyGS5jb20ucHJvdG8uRHJvcHBpbmdJ",
            "dGVtUEJCMwofbmV0LmdhbGFzcG9ydHMuYmlnc3Rhci5wcm90b2NvbEIQR2Ft",
            "ZVJlc3VsdFByb3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, global::Com.Proto.BeanAwardReflection.Descriptor, global::Com.Proto.BeanUserCardReflection.Descriptor, global::Com.Proto.BeanUserActivityHolidayInfoReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.GameResultPB), global::Com.Proto.GameResultPB.Parser, new[]{ "UserId", "Star", "Cap", "Exp", "CardExp", "Awards", "CreateTime", "UserCards", "DroppingItem" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///GameResultPB
  /// </summary>
  public sealed partial class GameResultPB : pb::IMessage<GameResultPB> {
    private static readonly pb::MessageParser<GameResultPB> _parser = new pb::MessageParser<GameResultPB>(() => new GameResultPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GameResultPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanGameResultReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameResultPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameResultPB(GameResultPB other) : this() {
      userId_ = other.userId_;
      star_ = other.star_;
      cap_ = other.cap_;
      exp_ = other.exp_;
      cardExp_ = other.cardExp_;
      awards_ = other.awards_.Clone();
      createTime_ = other.createTime_;
      userCards_ = other.userCards_.Clone();
      droppingItem_ = other.droppingItem_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameResultPB Clone() {
      return new GameResultPB(this);
    }

    /// <summary>Field number for the "userId" field.</summary>
    public const int UserIdFieldNumber = 1;
    private int userId_;
    /// <summary>
    ///用户id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int UserId {
      get { return userId_; }
      set {
        userId_ = value;
      }
    }

    /// <summary>Field number for the "star" field.</summary>
    public const int StarFieldNumber = 2;
    private int star_;
    /// <summary>
    ///星级
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Star {
      get { return star_; }
      set {
        star_ = value;
      }
    }

    /// <summary>Field number for the "cap" field.</summary>
    public const int CapFieldNumber = 3;
    private int cap_;
    /// <summary>
    ///比赛得分
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Cap {
      get { return cap_; }
      set {
        cap_ = value;
      }
    }

    /// <summary>Field number for the "exp" field.</summary>
    public const int ExpFieldNumber = 4;
    private int exp_;
    /// <summary>
    ///得到星级经验
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Exp {
      get { return exp_; }
      set {
        exp_ = value;
      }
    }

    /// <summary>Field number for the "cardExp" field.</summary>
    public const int CardExpFieldNumber = 5;
    private int cardExp_;
    /// <summary>
    ///得到星级-卡牌经验
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CardExp {
      get { return cardExp_; }
      set {
        cardExp_ = value;
      }
    }

    /// <summary>Field number for the "awards" field.</summary>
    public const int AwardsFieldNumber = 6;
    private static readonly pb::FieldCodec<global::Com.Proto.AwardPB> _repeated_awards_codec
        = pb::FieldCodec.ForMessage(50, global::Com.Proto.AwardPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.AwardPB> awards_ = new pbc::RepeatedField<global::Com.Proto.AwardPB>();
    /// <summary>
    ///得到道具
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.AwardPB> Awards {
      get { return awards_; }
    }

    /// <summary>Field number for the "createTime" field.</summary>
    public const int CreateTimeFieldNumber = 7;
    private long createTime_;
    /// <summary>
    ///记录时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long CreateTime {
      get { return createTime_; }
      set {
        createTime_ = value;
      }
    }

    /// <summary>Field number for the "user_cards" field.</summary>
    public const int UserCardsFieldNumber = 8;
    private static readonly pb::FieldCodec<global::Com.Proto.UserCardPB> _repeated_userCards_codec
        = pb::FieldCodec.ForMessage(66, global::Com.Proto.UserCardPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.UserCardPB> userCards_ = new pbc::RepeatedField<global::Com.Proto.UserCardPB>();
    /// <summary>
    ///玩家卡牌
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.UserCardPB> UserCards {
      get { return userCards_; }
    }

    /// <summary>Field number for the "dropping_item" field.</summary>
    public const int DroppingItemFieldNumber = 9;
    private static readonly pb::FieldCodec<global::Com.Proto.DroppingItemPB> _repeated_droppingItem_codec
        = pb::FieldCodec.ForMessage(74, global::Com.Proto.DroppingItemPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.DroppingItemPB> droppingItem_ = new pbc::RepeatedField<global::Com.Proto.DroppingItemPB>();
    /// <summary>
    ///假日活动掉落
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.DroppingItemPB> DroppingItem {
      get { return droppingItem_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GameResultPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GameResultPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (Star != other.Star) return false;
      if (Cap != other.Cap) return false;
      if (Exp != other.Exp) return false;
      if (CardExp != other.CardExp) return false;
      if(!awards_.Equals(other.awards_)) return false;
      if (CreateTime != other.CreateTime) return false;
      if(!userCards_.Equals(other.userCards_)) return false;
      if(!droppingItem_.Equals(other.droppingItem_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (Star != 0) hash ^= Star.GetHashCode();
      if (Cap != 0) hash ^= Cap.GetHashCode();
      if (Exp != 0) hash ^= Exp.GetHashCode();
      if (CardExp != 0) hash ^= CardExp.GetHashCode();
      hash ^= awards_.GetHashCode();
      if (CreateTime != 0L) hash ^= CreateTime.GetHashCode();
      hash ^= userCards_.GetHashCode();
      hash ^= droppingItem_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (UserId != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(UserId);
      }
      if (Star != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(Star);
      }
      if (Cap != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Cap);
      }
      if (Exp != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(Exp);
      }
      if (CardExp != 0) {
        output.WriteRawTag(40);
        output.WriteSInt32(CardExp);
      }
      awards_.WriteTo(output, _repeated_awards_codec);
      if (CreateTime != 0L) {
        output.WriteRawTag(56);
        output.WriteSInt64(CreateTime);
      }
      userCards_.WriteTo(output, _repeated_userCards_codec);
      droppingItem_.WriteTo(output, _repeated_droppingItem_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (Star != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Star);
      }
      if (Cap != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Cap);
      }
      if (Exp != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Exp);
      }
      if (CardExp != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(CardExp);
      }
      size += awards_.CalculateSize(_repeated_awards_codec);
      if (CreateTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(CreateTime);
      }
      size += userCards_.CalculateSize(_repeated_userCards_codec);
      size += droppingItem_.CalculateSize(_repeated_droppingItem_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GameResultPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.Star != 0) {
        Star = other.Star;
      }
      if (other.Cap != 0) {
        Cap = other.Cap;
      }
      if (other.Exp != 0) {
        Exp = other.Exp;
      }
      if (other.CardExp != 0) {
        CardExp = other.CardExp;
      }
      awards_.Add(other.awards_);
      if (other.CreateTime != 0L) {
        CreateTime = other.CreateTime;
      }
      userCards_.Add(other.userCards_);
      droppingItem_.Add(other.droppingItem_);
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
            UserId = input.ReadSInt32();
            break;
          }
          case 16: {
            Star = input.ReadSInt32();
            break;
          }
          case 24: {
            Cap = input.ReadSInt32();
            break;
          }
          case 32: {
            Exp = input.ReadSInt32();
            break;
          }
          case 40: {
            CardExp = input.ReadSInt32();
            break;
          }
          case 50: {
            awards_.AddEntriesFrom(input, _repeated_awards_codec);
            break;
          }
          case 56: {
            CreateTime = input.ReadSInt64();
            break;
          }
          case 66: {
            userCards_.AddEntriesFrom(input, _repeated_userCards_codec);
            break;
          }
          case 74: {
            droppingItem_.AddEntriesFrom(input, _repeated_droppingItem_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code