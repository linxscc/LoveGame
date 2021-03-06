// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user.proto</summary>
  public static partial class BeanUserReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg9iZWFuX3VzZXIucHJvdG8SCWNvbS5wcm90bxoKYmFzZS5wcm90byKYAwoG",
            "VXNlclBCEg8KB3VzZXJfaWQYASABKBESEgoKYWNjb3VudF9pZBgCIAEoCRIa",
            "ChJjaGFubmVsX2FjY291bnRfaWQYAyABKAkSEQoJdXNlcl9uYW1lGAQgASgJ",
            "EgwKBGxvZ28YBSABKAkSDQoFaW5kZXgYBiABKBESFwoPY2FyZF9nYXRoZXJf",
            "bnVtGAcgASgREi8KB2FwcGFyZWwYCCADKAsyHi5jb20ucHJvdG8uVXNlclBC",
            "LkFwcGFyZWxFbnRyeRITCgtjcmVhdGVfdGltZRgJIAEoEhIxCghiaXJ0aGRh",
            "eRgKIAMoCzIfLmNvbS5wcm90by5Vc2VyUEIuQmlydGhkYXlFbnRyeRIqCgp1",
            "c2VyX290aGVyGAsgASgLMhYuY29tLnByb3RvLlVzZXJPdGhlclBCGi4KDEFw",
            "cGFyZWxFbnRyeRILCgNrZXkYASABKBESDQoFdmFsdWUYAiABKBE6AjgBGi8K",
            "DUJpcnRoZGF5RW50cnkSCwoDa2V5GAEgASgJEg0KBXZhbHVlGAIgASgROgI4",
            "ASIxCgtVc2VyT3RoZXJQQhIOCgZhdmF0YXIYASABKBESEgoKYXZhdGFyX2Jv",
            "eBgCIAEoEUItCh9uZXQuZ2FsYXNwb3J0cy5iaWdzdGFyLnByb3RvY29sQgpV",
            "c2VyUHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserPB), global::Com.Proto.UserPB.Parser, new[]{ "UserId", "AccountId", "ChannelAccountId", "UserName", "Logo", "Index", "CardGatherNum", "Apparel", "CreateTime", "Birthday", "UserOther" }, null, null, new pbr::GeneratedClrTypeInfo[] { null, null, }),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserOtherPB), global::Com.Proto.UserOtherPB.Parser, new[]{ "Avatar", "AvatarBox" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserPB User
  /// </summary>
  public sealed partial class UserPB : pb::IMessage<UserPB> {
    private static readonly pb::MessageParser<UserPB> _parser = new pb::MessageParser<UserPB>(() => new UserPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserPB(UserPB other) : this() {
      userId_ = other.userId_;
      accountId_ = other.accountId_;
      channelAccountId_ = other.channelAccountId_;
      userName_ = other.userName_;
      logo_ = other.logo_;
      index_ = other.index_;
      cardGatherNum_ = other.cardGatherNum_;
      apparel_ = other.apparel_.Clone();
      createTime_ = other.createTime_;
      birthday_ = other.birthday_.Clone();
      UserOther = other.userOther_ != null ? other.UserOther.Clone() : null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserPB Clone() {
      return new UserPB(this);
    }

    /// <summary>Field number for the "user_id" field.</summary>
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

    /// <summary>Field number for the "account_id" field.</summary>
    public const int AccountIdFieldNumber = 2;
    private string accountId_ = "";
    /// <summary>
    ///账户id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string AccountId {
      get { return accountId_; }
      set {
        accountId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "channel_account_id" field.</summary>
    public const int ChannelAccountIdFieldNumber = 3;
    private string channelAccountId_ = "";
    /// <summary>
    ///渠道账户id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ChannelAccountId {
      get { return channelAccountId_; }
      set {
        channelAccountId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "user_name" field.</summary>
    public const int UserNameFieldNumber = 4;
    private string userName_ = "";
    /// <summary>
    ///用户名
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UserName {
      get { return userName_; }
      set {
        userName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "logo" field.</summary>
    public const int LogoFieldNumber = 5;
    private string logo_ = "";
    /// <summary>
    ///用户logo
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Logo {
      get { return logo_; }
      set {
        logo_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "index" field.</summary>
    public const int IndexFieldNumber = 6;
    private int index_;
    /// <summary>
    ///新手引导步骤
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Index {
      get { return index_; }
      set {
        index_ = value;
      }
    }

    /// <summary>Field number for the "card_gather_num" field.</summary>
    public const int CardGatherNumFieldNumber = 7;
    private int cardGatherNum_;
    /// <summary>
    ///卡牌收集度
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CardGatherNum {
      get { return cardGatherNum_; }
      set {
        cardGatherNum_ = value;
      }
    }

    /// <summary>Field number for the "apparel" field.</summary>
    public const int ApparelFieldNumber = 8;
    private static readonly pbc::MapField<int, int>.Codec _map_apparel_codec
        = new pbc::MapField<int, int>.Codec(pb::FieldCodec.ForSInt32(8), pb::FieldCodec.ForSInt32(16), 66);
    private readonly pbc::MapField<int, int> apparel_ = new pbc::MapField<int, int>();
    /// <summary>
    ///用户主界面服饰
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::MapField<int, int> Apparel {
      get { return apparel_; }
    }

    /// <summary>Field number for the "create_time" field.</summary>
    public const int CreateTimeFieldNumber = 9;
    private long createTime_;
    /// <summary>
    ///用户注册时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long CreateTime {
      get { return createTime_; }
      set {
        createTime_ = value;
      }
    }

    /// <summary>Field number for the "birthday" field.</summary>
    public const int BirthdayFieldNumber = 10;
    private static readonly pbc::MapField<string, int>.Codec _map_birthday_codec
        = new pbc::MapField<string, int>.Codec(pb::FieldCodec.ForString(10), pb::FieldCodec.ForSInt32(16), 82);
    private readonly pbc::MapField<string, int> birthday_ = new pbc::MapField<string, int>();
    /// <summary>
    ///用户生日
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::MapField<string, int> Birthday {
      get { return birthday_; }
    }

    /// <summary>Field number for the "user_other" field.</summary>
    public const int UserOtherFieldNumber = 11;
    private global::Com.Proto.UserOtherPB userOther_;
    /// <summary>
    ///用户额外信息
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Com.Proto.UserOtherPB UserOther {
      get { return userOther_; }
      set {
        userOther_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (AccountId != other.AccountId) return false;
      if (ChannelAccountId != other.ChannelAccountId) return false;
      if (UserName != other.UserName) return false;
      if (Logo != other.Logo) return false;
      if (Index != other.Index) return false;
      if (CardGatherNum != other.CardGatherNum) return false;
      if (!Apparel.Equals(other.Apparel)) return false;
      if (CreateTime != other.CreateTime) return false;
      if (!Birthday.Equals(other.Birthday)) return false;
      if (!object.Equals(UserOther, other.UserOther)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (AccountId.Length != 0) hash ^= AccountId.GetHashCode();
      if (ChannelAccountId.Length != 0) hash ^= ChannelAccountId.GetHashCode();
      if (UserName.Length != 0) hash ^= UserName.GetHashCode();
      if (Logo.Length != 0) hash ^= Logo.GetHashCode();
      if (Index != 0) hash ^= Index.GetHashCode();
      if (CardGatherNum != 0) hash ^= CardGatherNum.GetHashCode();
      hash ^= Apparel.GetHashCode();
      if (CreateTime != 0L) hash ^= CreateTime.GetHashCode();
      hash ^= Birthday.GetHashCode();
      if (userOther_ != null) hash ^= UserOther.GetHashCode();
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
      if (AccountId.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(AccountId);
      }
      if (ChannelAccountId.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(ChannelAccountId);
      }
      if (UserName.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(UserName);
      }
      if (Logo.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(Logo);
      }
      if (Index != 0) {
        output.WriteRawTag(48);
        output.WriteSInt32(Index);
      }
      if (CardGatherNum != 0) {
        output.WriteRawTag(56);
        output.WriteSInt32(CardGatherNum);
      }
      apparel_.WriteTo(output, _map_apparel_codec);
      if (CreateTime != 0L) {
        output.WriteRawTag(72);
        output.WriteSInt64(CreateTime);
      }
      birthday_.WriteTo(output, _map_birthday_codec);
      if (userOther_ != null) {
        output.WriteRawTag(90);
        output.WriteMessage(UserOther);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (AccountId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AccountId);
      }
      if (ChannelAccountId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ChannelAccountId);
      }
      if (UserName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UserName);
      }
      if (Logo.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Logo);
      }
      if (Index != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Index);
      }
      if (CardGatherNum != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(CardGatherNum);
      }
      size += apparel_.CalculateSize(_map_apparel_codec);
      if (CreateTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(CreateTime);
      }
      size += birthday_.CalculateSize(_map_birthday_codec);
      if (userOther_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(UserOther);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.AccountId.Length != 0) {
        AccountId = other.AccountId;
      }
      if (other.ChannelAccountId.Length != 0) {
        ChannelAccountId = other.ChannelAccountId;
      }
      if (other.UserName.Length != 0) {
        UserName = other.UserName;
      }
      if (other.Logo.Length != 0) {
        Logo = other.Logo;
      }
      if (other.Index != 0) {
        Index = other.Index;
      }
      if (other.CardGatherNum != 0) {
        CardGatherNum = other.CardGatherNum;
      }
      apparel_.Add(other.apparel_);
      if (other.CreateTime != 0L) {
        CreateTime = other.CreateTime;
      }
      birthday_.Add(other.birthday_);
      if (other.userOther_ != null) {
        if (userOther_ == null) {
          userOther_ = new global::Com.Proto.UserOtherPB();
        }
        UserOther.MergeFrom(other.UserOther);
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
            UserId = input.ReadSInt32();
            break;
          }
          case 18: {
            AccountId = input.ReadString();
            break;
          }
          case 26: {
            ChannelAccountId = input.ReadString();
            break;
          }
          case 34: {
            UserName = input.ReadString();
            break;
          }
          case 42: {
            Logo = input.ReadString();
            break;
          }
          case 48: {
            Index = input.ReadSInt32();
            break;
          }
          case 56: {
            CardGatherNum = input.ReadSInt32();
            break;
          }
          case 66: {
            apparel_.AddEntriesFrom(input, _map_apparel_codec);
            break;
          }
          case 72: {
            CreateTime = input.ReadSInt64();
            break;
          }
          case 82: {
            birthday_.AddEntriesFrom(input, _map_birthday_codec);
            break;
          }
          case 90: {
            if (userOther_ == null) {
              userOther_ = new global::Com.Proto.UserOtherPB();
            }
            input.ReadMessage(userOther_);
            break;
          }
        }
      }
    }

  }

  public sealed partial class UserOtherPB : pb::IMessage<UserOtherPB> {
    private static readonly pb::MessageParser<UserOtherPB> _parser = new pb::MessageParser<UserOtherPB>(() => new UserOtherPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserOtherPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserOtherPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserOtherPB(UserOtherPB other) : this() {
      avatar_ = other.avatar_;
      avatarBox_ = other.avatarBox_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserOtherPB Clone() {
      return new UserOtherPB(this);
    }

    /// <summary>Field number for the "avatar" field.</summary>
    public const int AvatarFieldNumber = 1;
    private int avatar_;
    /// <summary>
    ///头像
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Avatar {
      get { return avatar_; }
      set {
        avatar_ = value;
      }
    }

    /// <summary>Field number for the "avatar_box" field.</summary>
    public const int AvatarBoxFieldNumber = 2;
    private int avatarBox_;
    /// <summary>
    ///头像框
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int AvatarBox {
      get { return avatarBox_; }
      set {
        avatarBox_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserOtherPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserOtherPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Avatar != other.Avatar) return false;
      if (AvatarBox != other.AvatarBox) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Avatar != 0) hash ^= Avatar.GetHashCode();
      if (AvatarBox != 0) hash ^= AvatarBox.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Avatar != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(Avatar);
      }
      if (AvatarBox != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(AvatarBox);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Avatar != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Avatar);
      }
      if (AvatarBox != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(AvatarBox);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserOtherPB other) {
      if (other == null) {
        return;
      }
      if (other.Avatar != 0) {
        Avatar = other.Avatar;
      }
      if (other.AvatarBox != 0) {
        AvatarBox = other.AvatarBox;
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
            Avatar = input.ReadSInt32();
            break;
          }
          case 16: {
            AvatarBox = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
