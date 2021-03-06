// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: api.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Account.Proto {

  /// <summary>Holder for reflection information generated from api.proto</summary>
  public static partial class ApiReflection {

    #region Descriptor
    /// <summary>File descriptor for api.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ApiReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CglhcGkucHJvdG8SEWNvbS5hY2NvdW50LnByb3RvGg5hY2NfYmFzZS5wcm90",
            "byJmCg1DaGVja1Rva2VuUmVxEg4KBmFwcF9pZBgBIAEoCRIPCgdhcHBfa2V5",
            "GAIgASgJEhIKCmFjY291bnRfaWQYAyABKAkSDQoFdG9rZW4YBCABKAkSEQoJ",
            "Z2FtZV90eXBlGAUgASgRIksKDUNoZWNrVG9rZW5SZXMSCwoDcmV0GAEgASgR",
            "EhQKDGZ1bGxfY2hhbm5lbBgCIAEoCRIXCg9jaGFubmVsX2FjY291bnQYAyAB",
            "KAkiZQoRQ2hhbm5lbE1hcHBpbmdSZXESDAoEZ2FtZRgBIAEoERIWCg5hY2Nv",
            "dW50X3ByZWZpeBgCIAEoCRIUCgxmdWxsX2NoYW5uZWwYAyABKAkSFAoMcmVh",
            "bF9jaGFubmVsGAQgASgJIlgKEUNoYW5uZWxNYXBwaW5nUmVzEgsKA3JldBgB",
            "IAEoERI2CghtYXBwaW5ncxgCIAMoCzIkLmNvbS5hY2NvdW50LnByb3RvLkNo",
            "YW5uZWxNYXBwaW5nUmVxQjEKJG5ldC5nYWxhc3BvcnRzLmFjY291bnQuYmVh",
            "bi5wcm90b2NvbEIJQXBpUHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Com.Account.Proto.AccBaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Account.Proto.CheckTokenReq), global::Com.Account.Proto.CheckTokenReq.Parser, new[]{ "AppId", "AppKey", "AccountId", "Token", "GameType" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Account.Proto.CheckTokenRes), global::Com.Account.Proto.CheckTokenRes.Parser, new[]{ "Ret", "FullChannel", "ChannelAccount" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Account.Proto.ChannelMappingReq), global::Com.Account.Proto.ChannelMappingReq.Parser, new[]{ "Game", "AccountPrefix", "FullChannel", "RealChannel" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Account.Proto.ChannelMappingRes), global::Com.Account.Proto.ChannelMappingRes.Parser, new[]{ "Ret", "Mappings" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///校验token请求
  /// </summary>
  public sealed partial class CheckTokenReq : pb::IMessage<CheckTokenReq> {
    private static readonly pb::MessageParser<CheckTokenReq> _parser = new pb::MessageParser<CheckTokenReq>(() => new CheckTokenReq());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CheckTokenReq> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Account.Proto.ApiReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CheckTokenReq() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CheckTokenReq(CheckTokenReq other) : this() {
      appId_ = other.appId_;
      appKey_ = other.appKey_;
      accountId_ = other.accountId_;
      token_ = other.token_;
      gameType_ = other.gameType_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CheckTokenReq Clone() {
      return new CheckTokenReq(this);
    }

    /// <summary>Field number for the "app_id" field.</summary>
    public const int AppIdFieldNumber = 1;
    private string appId_ = "";
    /// <summary>
    ///appId（已废弃）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string AppId {
      get { return appId_; }
      set {
        appId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "app_key" field.</summary>
    public const int AppKeyFieldNumber = 2;
    private string appKey_ = "";
    /// <summary>
    ///appKey（已废弃）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string AppKey {
      get { return appKey_; }
      set {
        appKey_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "account_id" field.</summary>
    public const int AccountIdFieldNumber = 3;
    private string accountId_ = "";
    /// <summary>
    ///用户账号Id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string AccountId {
      get { return accountId_; }
      set {
        accountId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "token" field.</summary>
    public const int TokenFieldNumber = 4;
    private string token_ = "";
    /// <summary>
    ///授权token
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Token {
      get { return token_; }
      set {
        token_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "game_type" field.</summary>
    public const int GameTypeFieldNumber = 5;
    private int gameType_;
    /// <summary>
    ///游戏TypeID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int GameType {
      get { return gameType_; }
      set {
        gameType_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CheckTokenReq);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CheckTokenReq other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (AppId != other.AppId) return false;
      if (AppKey != other.AppKey) return false;
      if (AccountId != other.AccountId) return false;
      if (Token != other.Token) return false;
      if (GameType != other.GameType) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (AppId.Length != 0) hash ^= AppId.GetHashCode();
      if (AppKey.Length != 0) hash ^= AppKey.GetHashCode();
      if (AccountId.Length != 0) hash ^= AccountId.GetHashCode();
      if (Token.Length != 0) hash ^= Token.GetHashCode();
      if (GameType != 0) hash ^= GameType.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (AppId.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(AppId);
      }
      if (AppKey.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(AppKey);
      }
      if (AccountId.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(AccountId);
      }
      if (Token.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Token);
      }
      if (GameType != 0) {
        output.WriteRawTag(40);
        output.WriteSInt32(GameType);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (AppId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AppId);
      }
      if (AppKey.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AppKey);
      }
      if (AccountId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AccountId);
      }
      if (Token.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Token);
      }
      if (GameType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(GameType);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CheckTokenReq other) {
      if (other == null) {
        return;
      }
      if (other.AppId.Length != 0) {
        AppId = other.AppId;
      }
      if (other.AppKey.Length != 0) {
        AppKey = other.AppKey;
      }
      if (other.AccountId.Length != 0) {
        AccountId = other.AccountId;
      }
      if (other.Token.Length != 0) {
        Token = other.Token;
      }
      if (other.GameType != 0) {
        GameType = other.GameType;
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
          case 10: {
            AppId = input.ReadString();
            break;
          }
          case 18: {
            AppKey = input.ReadString();
            break;
          }
          case 26: {
            AccountId = input.ReadString();
            break;
          }
          case 34: {
            Token = input.ReadString();
            break;
          }
          case 40: {
            GameType = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// 校验Token回包
  /// </summary>
  public sealed partial class CheckTokenRes : pb::IMessage<CheckTokenRes> {
    private static readonly pb::MessageParser<CheckTokenRes> _parser = new pb::MessageParser<CheckTokenRes>(() => new CheckTokenRes());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CheckTokenRes> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Account.Proto.ApiReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CheckTokenRes() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CheckTokenRes(CheckTokenRes other) : this() {
      ret_ = other.ret_;
      fullChannel_ = other.fullChannel_;
      channelAccount_ = other.channelAccount_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CheckTokenRes Clone() {
      return new CheckTokenRes(this);
    }

    /// <summary>Field number for the "ret" field.</summary>
    public const int RetFieldNumber = 1;
    private int ret_;
    /// <summary>
    /// 响应码
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Ret {
      get { return ret_; }
      set {
        ret_ = value;
      }
    }

    /// <summary>Field number for the "full_channel" field.</summary>
    public const int FullChannelFieldNumber = 2;
    private string fullChannel_ = "";
    /// <summary>
    /// 渠道
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string FullChannel {
      get { return fullChannel_; }
      set {
        fullChannel_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "channel_account" field.</summary>
    public const int ChannelAccountFieldNumber = 3;
    private string channelAccount_ = "";
    /// <summary>
    /// 渠道账号
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ChannelAccount {
      get { return channelAccount_; }
      set {
        channelAccount_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CheckTokenRes);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CheckTokenRes other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Ret != other.Ret) return false;
      if (FullChannel != other.FullChannel) return false;
      if (ChannelAccount != other.ChannelAccount) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Ret != 0) hash ^= Ret.GetHashCode();
      if (FullChannel.Length != 0) hash ^= FullChannel.GetHashCode();
      if (ChannelAccount.Length != 0) hash ^= ChannelAccount.GetHashCode();
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
      if (FullChannel.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(FullChannel);
      }
      if (ChannelAccount.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(ChannelAccount);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Ret != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Ret);
      }
      if (FullChannel.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(FullChannel);
      }
      if (ChannelAccount.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ChannelAccount);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CheckTokenRes other) {
      if (other == null) {
        return;
      }
      if (other.Ret != 0) {
        Ret = other.Ret;
      }
      if (other.FullChannel.Length != 0) {
        FullChannel = other.FullChannel;
      }
      if (other.ChannelAccount.Length != 0) {
        ChannelAccount = other.ChannelAccount;
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
            FullChannel = input.ReadString();
            break;
          }
          case 26: {
            ChannelAccount = input.ReadString();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// 查询渠道映射信息请求（按顺序优先级查询字段，为空则不查询，全空则返回所有记录）
  /// </summary>
  public sealed partial class ChannelMappingReq : pb::IMessage<ChannelMappingReq> {
    private static readonly pb::MessageParser<ChannelMappingReq> _parser = new pb::MessageParser<ChannelMappingReq>(() => new ChannelMappingReq());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ChannelMappingReq> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Account.Proto.ApiReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChannelMappingReq() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChannelMappingReq(ChannelMappingReq other) : this() {
      game_ = other.game_;
      accountPrefix_ = other.accountPrefix_;
      fullChannel_ = other.fullChannel_;
      realChannel_ = other.realChannel_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChannelMappingReq Clone() {
      return new ChannelMappingReq(this);
    }

    /// <summary>Field number for the "game" field.</summary>
    public const int GameFieldNumber = 1;
    private int game_;
    /// <summary>
    /// 游戏TypeId（可空）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Game {
      get { return game_; }
      set {
        game_ = value;
      }
    }

    /// <summary>Field number for the "account_prefix" field.</summary>
    public const int AccountPrefixFieldNumber = 2;
    private string accountPrefix_ = "";
    /// <summary>
    /// 渠道账号前缀（可空，不为空时后续字段无效）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string AccountPrefix {
      get { return accountPrefix_; }
      set {
        accountPrefix_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "full_channel" field.</summary>
    public const int FullChannelFieldNumber = 3;
    private string fullChannel_ = "";
    /// <summary>
    /// 原始渠道（可空，但与真实渠道互斥，只能有一个不为空）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string FullChannel {
      get { return fullChannel_; }
      set {
        fullChannel_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "real_channel" field.</summary>
    public const int RealChannelFieldNumber = 4;
    private string realChannel_ = "";
    /// <summary>
    /// 真实渠道（可空，但与原始渠道互斥，只能有一个不为空）
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string RealChannel {
      get { return realChannel_; }
      set {
        realChannel_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ChannelMappingReq);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ChannelMappingReq other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Game != other.Game) return false;
      if (AccountPrefix != other.AccountPrefix) return false;
      if (FullChannel != other.FullChannel) return false;
      if (RealChannel != other.RealChannel) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Game != 0) hash ^= Game.GetHashCode();
      if (AccountPrefix.Length != 0) hash ^= AccountPrefix.GetHashCode();
      if (FullChannel.Length != 0) hash ^= FullChannel.GetHashCode();
      if (RealChannel.Length != 0) hash ^= RealChannel.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Game != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(Game);
      }
      if (AccountPrefix.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(AccountPrefix);
      }
      if (FullChannel.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(FullChannel);
      }
      if (RealChannel.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(RealChannel);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Game != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Game);
      }
      if (AccountPrefix.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AccountPrefix);
      }
      if (FullChannel.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(FullChannel);
      }
      if (RealChannel.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(RealChannel);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ChannelMappingReq other) {
      if (other == null) {
        return;
      }
      if (other.Game != 0) {
        Game = other.Game;
      }
      if (other.AccountPrefix.Length != 0) {
        AccountPrefix = other.AccountPrefix;
      }
      if (other.FullChannel.Length != 0) {
        FullChannel = other.FullChannel;
      }
      if (other.RealChannel.Length != 0) {
        RealChannel = other.RealChannel;
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
            Game = input.ReadSInt32();
            break;
          }
          case 18: {
            AccountPrefix = input.ReadString();
            break;
          }
          case 26: {
            FullChannel = input.ReadString();
            break;
          }
          case 34: {
            RealChannel = input.ReadString();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  /// 查询渠道映射信息回包
  /// </summary>
  public sealed partial class ChannelMappingRes : pb::IMessage<ChannelMappingRes> {
    private static readonly pb::MessageParser<ChannelMappingRes> _parser = new pb::MessageParser<ChannelMappingRes>(() => new ChannelMappingRes());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ChannelMappingRes> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Account.Proto.ApiReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChannelMappingRes() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChannelMappingRes(ChannelMappingRes other) : this() {
      ret_ = other.ret_;
      mappings_ = other.mappings_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChannelMappingRes Clone() {
      return new ChannelMappingRes(this);
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

    /// <summary>Field number for the "mappings" field.</summary>
    public const int MappingsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Com.Account.Proto.ChannelMappingReq> _repeated_mappings_codec
        = pb::FieldCodec.ForMessage(18, global::Com.Account.Proto.ChannelMappingReq.Parser);
    private readonly pbc::RepeatedField<global::Com.Account.Proto.ChannelMappingReq> mappings_ = new pbc::RepeatedField<global::Com.Account.Proto.ChannelMappingReq>();
    /// <summary>
    /// 结果集合
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Account.Proto.ChannelMappingReq> Mappings {
      get { return mappings_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ChannelMappingRes);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ChannelMappingRes other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Ret != other.Ret) return false;
      if(!mappings_.Equals(other.mappings_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Ret != 0) hash ^= Ret.GetHashCode();
      hash ^= mappings_.GetHashCode();
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
      mappings_.WriteTo(output, _repeated_mappings_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Ret != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Ret);
      }
      size += mappings_.CalculateSize(_repeated_mappings_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ChannelMappingRes other) {
      if (other == null) {
        return;
      }
      if (other.Ret != 0) {
        Ret = other.Ret;
      }
      mappings_.Add(other.mappings_);
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
            mappings_.AddEntriesFrom(input, _repeated_mappings_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
