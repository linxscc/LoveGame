// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_game_time_config.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_game_time_config.proto</summary>
  public static partial class BeanGameTimeConfigReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_game_time_config.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanGameTimeConfigReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChtiZWFuX2dhbWVfdGltZV9jb25maWcucHJvdG8SCWNvbS5wcm90bxoKYmFz",
            "ZS5wcm90byI6ChBHYW1lVGltZUNvbmZpZ1BCEhIKCmNvbmZpZ19rZXkYASAB",
            "KAkSEgoKY29uZmlnX3ZhbBgCIAEoEkI3Ch9uZXQuZ2FsYXNwb3J0cy5iaWdz",
            "dGFyLnByb3RvY29sQhRHYW1lVGltZUNvbmZpZ1Byb3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.GameTimeConfigPB), global::Com.Proto.GameTimeConfigPB.Parser, new[]{ "ConfigKey", "ConfigVal" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///GameTimeConfigPB GameTimeConfig
  /// </summary>
  public sealed partial class GameTimeConfigPB : pb::IMessage<GameTimeConfigPB> {
    private static readonly pb::MessageParser<GameTimeConfigPB> _parser = new pb::MessageParser<GameTimeConfigPB>(() => new GameTimeConfigPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GameTimeConfigPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanGameTimeConfigReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameTimeConfigPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameTimeConfigPB(GameTimeConfigPB other) : this() {
      configKey_ = other.configKey_;
      configVal_ = other.configVal_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GameTimeConfigPB Clone() {
      return new GameTimeConfigPB(this);
    }

    /// <summary>Field number for the "config_key" field.</summary>
    public const int ConfigKeyFieldNumber = 1;
    private string configKey_ = "";
    /// <summary>
    ///键
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ConfigKey {
      get { return configKey_; }
      set {
        configKey_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "config_val" field.</summary>
    public const int ConfigValFieldNumber = 2;
    private long configVal_;
    /// <summary>
    ///值
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long ConfigVal {
      get { return configVal_; }
      set {
        configVal_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GameTimeConfigPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GameTimeConfigPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ConfigKey != other.ConfigKey) return false;
      if (ConfigVal != other.ConfigVal) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ConfigKey.Length != 0) hash ^= ConfigKey.GetHashCode();
      if (ConfigVal != 0L) hash ^= ConfigVal.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ConfigKey.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(ConfigKey);
      }
      if (ConfigVal != 0L) {
        output.WriteRawTag(16);
        output.WriteSInt64(ConfigVal);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ConfigKey.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ConfigKey);
      }
      if (ConfigVal != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(ConfigVal);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GameTimeConfigPB other) {
      if (other == null) {
        return;
      }
      if (other.ConfigKey.Length != 0) {
        ConfigKey = other.ConfigKey;
      }
      if (other.ConfigVal != 0L) {
        ConfigVal = other.ConfigVal;
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
            ConfigKey = input.ReadString();
            break;
          }
          case 16: {
            ConfigVal = input.ReadSInt64();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
