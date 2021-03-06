// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_weather.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_weather.proto</summary>
  public static partial class BeanUserWeatherReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_weather.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserWeatherReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdiZWFuX3VzZXJfd2VhdGhlci5wcm90bxIJY29tLnByb3RvGgpiYXNlLnBy",
            "b3RvIt4BCg1Vc2VyV2VhdGhlclBCEg8KB3VzZXJfaWQYASABKBESGQoGcGxh",
            "eWVyGAIgASgOMgkuUGxheWVyUEISEgoKd2VhdGhlcl9pZBgDIAEoERJACg1i",
            "bGVzc19udW1fbWFwGAQgAygLMikuY29tLnByb3RvLlVzZXJXZWF0aGVyUEIu",
            "Qmxlc3NOdW1NYXBFbnRyeRIXCg9jaGFsbGVuZ2VfY291bnQYBSABKBEaMgoQ",
            "Qmxlc3NOdW1NYXBFbnRyeRILCgNrZXkYASABKBESDQoFdmFsdWUYAiABKBE6",
            "AjgBQjQKH25ldC5nYWxhc3BvcnRzLmJpZ3N0YXIucHJvdG9jb2xCEVVzZXJX",
            "ZWF0aGVyUHJvdG9zYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserWeatherPB), global::Com.Proto.UserWeatherPB.Parser, new[]{ "UserId", "Player", "WeatherId", "BlessNumMap", "ChallengeCount" }, null, null, new pbr::GeneratedClrTypeInfo[] { null, })
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserWeatherPB UserWeather
  /// </summary>
  public sealed partial class UserWeatherPB : pb::IMessage<UserWeatherPB> {
    private static readonly pb::MessageParser<UserWeatherPB> _parser = new pb::MessageParser<UserWeatherPB>(() => new UserWeatherPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserWeatherPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserWeatherReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserWeatherPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserWeatherPB(UserWeatherPB other) : this() {
      userId_ = other.userId_;
      player_ = other.player_;
      weatherId_ = other.weatherId_;
      blessNumMap_ = other.blessNumMap_.Clone();
      challengeCount_ = other.challengeCount_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserWeatherPB Clone() {
      return new UserWeatherPB(this);
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

    /// <summary>Field number for the "player" field.</summary>
    public const int PlayerFieldNumber = 2;
    private global::PlayerPB player_ = 0;
    /// <summary>
    ///男主
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::PlayerPB Player {
      get { return player_; }
      set {
        player_ = value;
      }
    }

    /// <summary>Field number for the "weather_id" field.</summary>
    public const int WeatherIdFieldNumber = 3;
    private int weatherId_;
    /// <summary>
    ///当前天气
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int WeatherId {
      get { return weatherId_; }
      set {
        weatherId_ = value;
      }
    }

    /// <summary>Field number for the "bless_num_map" field.</summary>
    public const int BlessNumMapFieldNumber = 4;
    private static readonly pbc::MapField<int, int>.Codec _map_blessNumMap_codec
        = new pbc::MapField<int, int>.Codec(pb::FieldCodec.ForSInt32(8), pb::FieldCodec.ForSInt32(16), 34);
    private readonly pbc::MapField<int, int> blessNumMap_ = new pbc::MapField<int, int>();
    /// <summary>
    ///某种天气下对应的祈福次数，这里需要注意，花费按总次数算						
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::MapField<int, int> BlessNumMap {
      get { return blessNumMap_; }
    }

    /// <summary>Field number for the "challenge_count" field.</summary>
    public const int ChallengeCountFieldNumber = 5;
    private int challengeCount_;
    /// <summary>
    ///挑战次数
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ChallengeCount {
      get { return challengeCount_; }
      set {
        challengeCount_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserWeatherPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserWeatherPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (Player != other.Player) return false;
      if (WeatherId != other.WeatherId) return false;
      if (!BlessNumMap.Equals(other.BlessNumMap)) return false;
      if (ChallengeCount != other.ChallengeCount) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (Player != 0) hash ^= Player.GetHashCode();
      if (WeatherId != 0) hash ^= WeatherId.GetHashCode();
      hash ^= BlessNumMap.GetHashCode();
      if (ChallengeCount != 0) hash ^= ChallengeCount.GetHashCode();
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
      if (Player != 0) {
        output.WriteRawTag(16);
        output.WriteEnum((int) Player);
      }
      if (WeatherId != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(WeatherId);
      }
      blessNumMap_.WriteTo(output, _map_blessNumMap_codec);
      if (ChallengeCount != 0) {
        output.WriteRawTag(40);
        output.WriteSInt32(ChallengeCount);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (Player != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Player);
      }
      if (WeatherId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(WeatherId);
      }
      size += blessNumMap_.CalculateSize(_map_blessNumMap_codec);
      if (ChallengeCount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ChallengeCount);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserWeatherPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.Player != 0) {
        Player = other.Player;
      }
      if (other.WeatherId != 0) {
        WeatherId = other.WeatherId;
      }
      blessNumMap_.Add(other.blessNumMap_);
      if (other.ChallengeCount != 0) {
        ChallengeCount = other.ChallengeCount;
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
          case 16: {
            player_ = (global::PlayerPB) input.ReadEnum();
            break;
          }
          case 24: {
            WeatherId = input.ReadSInt32();
            break;
          }
          case 34: {
            blessNumMap_.AddEntriesFrom(input, _map_blessNumMap_codec);
            break;
          }
          case 40: {
            ChallengeCount = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
