// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: controller_game_config.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from controller_game_config.proto</summary>
  public static partial class ControllerGameConfigReflection {

    #region Descriptor
    /// <summary>File descriptor for controller_game_config.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ControllerGameConfigReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Chxjb250cm9sbGVyX2dhbWVfY29uZmlnLnByb3RvEgljb20ucHJvdG8aCmJh",
            "c2UucHJvdG8aFmJlYW5fZ2FtZV9jb25maWcucHJvdG8aGWJlYW5fZnVuY3Rp",
            "b25fZW50cnkucHJvdG8aG2JlYW5fZ2FtZV90aW1lX2NvbmZpZy5wcm90byK4",
            "AQoNR2FtZUNvbmZpZ1JlcxILCgNyZXQYASABKBESLQoMZ2FtZV9jb25maWdz",
            "GAIgAygLMhcuY29tLnByb3RvLkdhbWVDb25maWdQQhIzCg9mdW5jdGlvbl9l",
            "bnRyeXMYAyADKAsyGi5jb20ucHJvdG8uRnVuY3Rpb25FbnRyeVBCEjYKEWdh",
            "bWVfdGltZV9jb25maWdzGAQgAygLMhsuY29tLnByb3RvLkdhbWVUaW1lQ29u",
            "ZmlnUEJCPQofbmV0LmdhbGFzcG9ydHMuYmlnc3Rhci5wcm90b2NvbEIaR2Ft",
            "ZUNvbmZpZ0NvbnRyb2xsZXJQcm90b3NiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, global::Com.Proto.BeanGameConfigReflection.Descriptor, global::Com.Proto.BeanFunctionEntryReflection.Descriptor, global::Com.Proto.BeanGameTimeConfigReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.GameConfigRes), global::Com.Proto.GameConfigRes.Parser, new[]{ "Ret", "GameConfigs", "FunctionEntrys", "GameTimeConfigs" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///获取gameconfig列表 gameConfigC/rules
  /// </summary>
  public sealed partial class GameConfigRes : pb::IMessage<GameConfigRes> {
    private static readonly pb::MessageParser<GameConfigRes> _parser = new pb::MessageParser<GameConfigRes>(() => new GameConfigRes());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GameConfigRes> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.ControllerGameConfigReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameConfigRes() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameConfigRes(GameConfigRes other) : this() {
      ret_ = other.ret_;
      gameConfigs_ = other.gameConfigs_.Clone();
      functionEntrys_ = other.functionEntrys_.Clone();
      gameTimeConfigs_ = other.gameTimeConfigs_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameConfigRes Clone() {
      return new GameConfigRes(this);
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

    /// <summary>Field number for the "game_configs" field.</summary>
    public const int GameConfigsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Com.Proto.GameConfigPB> _repeated_gameConfigs_codec
        = pb::FieldCodec.ForMessage(18, global::Com.Proto.GameConfigPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.GameConfigPB> gameConfigs_ = new pbc::RepeatedField<global::Com.Proto.GameConfigPB>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.GameConfigPB> GameConfigs {
      get { return gameConfigs_; }
    }

    /// <summary>Field number for the "function_entrys" field.</summary>
    public const int FunctionEntrysFieldNumber = 3;
    private static readonly pb::FieldCodec<global::Com.Proto.FunctionEntryPB> _repeated_functionEntrys_codec
        = pb::FieldCodec.ForMessage(26, global::Com.Proto.FunctionEntryPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.FunctionEntryPB> functionEntrys_ = new pbc::RepeatedField<global::Com.Proto.FunctionEntryPB>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.FunctionEntryPB> FunctionEntrys {
      get { return functionEntrys_; }
    }

    /// <summary>Field number for the "game_time_configs" field.</summary>
    public const int GameTimeConfigsFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Com.Proto.GameTimeConfigPB> _repeated_gameTimeConfigs_codec
        = pb::FieldCodec.ForMessage(34, global::Com.Proto.GameTimeConfigPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.GameTimeConfigPB> gameTimeConfigs_ = new pbc::RepeatedField<global::Com.Proto.GameTimeConfigPB>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.GameTimeConfigPB> GameTimeConfigs {
      get { return gameTimeConfigs_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GameConfigRes);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GameConfigRes other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Ret != other.Ret) return false;
      if(!gameConfigs_.Equals(other.gameConfigs_)) return false;
      if(!functionEntrys_.Equals(other.functionEntrys_)) return false;
      if(!gameTimeConfigs_.Equals(other.gameTimeConfigs_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Ret != 0) hash ^= Ret.GetHashCode();
      hash ^= gameConfigs_.GetHashCode();
      hash ^= functionEntrys_.GetHashCode();
      hash ^= gameTimeConfigs_.GetHashCode();
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
      gameConfigs_.WriteTo(output, _repeated_gameConfigs_codec);
      functionEntrys_.WriteTo(output, _repeated_functionEntrys_codec);
      gameTimeConfigs_.WriteTo(output, _repeated_gameTimeConfigs_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Ret != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Ret);
      }
      size += gameConfigs_.CalculateSize(_repeated_gameConfigs_codec);
      size += functionEntrys_.CalculateSize(_repeated_functionEntrys_codec);
      size += gameTimeConfigs_.CalculateSize(_repeated_gameTimeConfigs_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GameConfigRes other) {
      if (other == null) {
        return;
      }
      if (other.Ret != 0) {
        Ret = other.Ret;
      }
      gameConfigs_.Add(other.gameConfigs_);
      functionEntrys_.Add(other.functionEntrys_);
      gameTimeConfigs_.Add(other.gameTimeConfigs_);
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
            gameConfigs_.AddEntriesFrom(input, _repeated_gameConfigs_codec);
            break;
          }
          case 26: {
            functionEntrys_.AddEntriesFrom(input, _repeated_functionEntrys_codec);
            break;
          }
          case 34: {
            gameTimeConfigs_.AddEntriesFrom(input, _repeated_gameTimeConfigs_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
