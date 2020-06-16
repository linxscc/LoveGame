// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_favorability_item.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_favorability_item.proto</summary>
  public static partial class BeanFavorabilityItemReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_favorability_item.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanFavorabilityItemReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChxiZWFuX2Zhdm9yYWJpbGl0eV9pdGVtLnByb3RvEgljb20ucHJvdG8aCmJh",
            "c2UucHJvdG8ijQEKEkZhdm9yYWJpbGl0eUl0ZW1QQhIPCgdpdGVtX2lkGAEg",
            "ASgREhEKCWl0ZW1fdHlwZRgCIAEoERIRCglpdGVtX2Rlc2MYAyABKAkSEgoK",
            "aXRlbV92b2ljZRgEIAEoCRIRCglpdGVtX3BoaXoYBSABKAkSGQoGcGxheWVy",
            "GAYgASgOMgkuUGxheWVyUEJCOQofbmV0LmdhbGFzcG9ydHMuYmlnc3Rhci5w",
            "cm90b2NvbEIWRmF2b3JhYmlsaXR5SXRlbVByb3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.FavorabilityItemPB), global::Com.Proto.FavorabilityItemPB.Parser, new[]{ "ItemId", "ItemType", "ItemDesc", "ItemVoice", "ItemPhiz", "Player" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///FavorabilityItemPB FavorabilityItem
  /// </summary>
  public sealed partial class FavorabilityItemPB : pb::IMessage<FavorabilityItemPB> {
    private static readonly pb::MessageParser<FavorabilityItemPB> _parser = new pb::MessageParser<FavorabilityItemPB>(() => new FavorabilityItemPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FavorabilityItemPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanFavorabilityItemReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FavorabilityItemPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FavorabilityItemPB(FavorabilityItemPB other) : this() {
      itemId_ = other.itemId_;
      itemType_ = other.itemType_;
      itemDesc_ = other.itemDesc_;
      itemVoice_ = other.itemVoice_;
      itemPhiz_ = other.itemPhiz_;
      player_ = other.player_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FavorabilityItemPB Clone() {
      return new FavorabilityItemPB(this);
    }

    /// <summary>Field number for the "item_id" field.</summary>
    public const int ItemIdFieldNumber = 1;
    private int itemId_;
    /// <summary>
    ///道具ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ItemId {
      get { return itemId_; }
      set {
        itemId_ = value;
      }
    }

    /// <summary>Field number for the "item_type" field.</summary>
    public const int ItemTypeFieldNumber = 2;
    private int itemType_;
    /// <summary>
    ///道具类型(0基础 1高级)
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ItemType {
      get { return itemType_; }
      set {
        itemType_ = value;
      }
    }

    /// <summary>Field number for the "item_desc" field.</summary>
    public const int ItemDescFieldNumber = 3;
    private string itemDesc_ = "";
    /// <summary>
    ///道具描述文案
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ItemDesc {
      get { return itemDesc_; }
      set {
        itemDesc_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "item_voice" field.</summary>
    public const int ItemVoiceFieldNumber = 4;
    private string itemVoice_ = "";
    /// <summary>
    ///道具回应语音
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ItemVoice {
      get { return itemVoice_; }
      set {
        itemVoice_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "item_phiz" field.</summary>
    public const int ItemPhizFieldNumber = 5;
    private string itemPhiz_ = "";
    /// <summary>
    ///道具表情
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ItemPhiz {
      get { return itemPhiz_; }
      set {
        itemPhiz_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "player" field.</summary>
    public const int PlayerFieldNumber = 6;
    private global::PlayerPB player_ = 0;
    /// <summary>
    ///角色ID
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::PlayerPB Player {
      get { return player_; }
      set {
        player_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FavorabilityItemPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FavorabilityItemPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ItemId != other.ItemId) return false;
      if (ItemType != other.ItemType) return false;
      if (ItemDesc != other.ItemDesc) return false;
      if (ItemVoice != other.ItemVoice) return false;
      if (ItemPhiz != other.ItemPhiz) return false;
      if (Player != other.Player) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ItemId != 0) hash ^= ItemId.GetHashCode();
      if (ItemType != 0) hash ^= ItemType.GetHashCode();
      if (ItemDesc.Length != 0) hash ^= ItemDesc.GetHashCode();
      if (ItemVoice.Length != 0) hash ^= ItemVoice.GetHashCode();
      if (ItemPhiz.Length != 0) hash ^= ItemPhiz.GetHashCode();
      if (Player != 0) hash ^= Player.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ItemId != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(ItemId);
      }
      if (ItemType != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(ItemType);
      }
      if (ItemDesc.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(ItemDesc);
      }
      if (ItemVoice.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(ItemVoice);
      }
      if (ItemPhiz.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(ItemPhiz);
      }
      if (Player != 0) {
        output.WriteRawTag(48);
        output.WriteEnum((int) Player);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ItemId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ItemId);
      }
      if (ItemType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(ItemType);
      }
      if (ItemDesc.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ItemDesc);
      }
      if (ItemVoice.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ItemVoice);
      }
      if (ItemPhiz.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ItemPhiz);
      }
      if (Player != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Player);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FavorabilityItemPB other) {
      if (other == null) {
        return;
      }
      if (other.ItemId != 0) {
        ItemId = other.ItemId;
      }
      if (other.ItemType != 0) {
        ItemType = other.ItemType;
      }
      if (other.ItemDesc.Length != 0) {
        ItemDesc = other.ItemDesc;
      }
      if (other.ItemVoice.Length != 0) {
        ItemVoice = other.ItemVoice;
      }
      if (other.ItemPhiz.Length != 0) {
        ItemPhiz = other.ItemPhiz;
      }
      if (other.Player != 0) {
        Player = other.Player;
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
            ItemId = input.ReadSInt32();
            break;
          }
          case 16: {
            ItemType = input.ReadSInt32();
            break;
          }
          case 26: {
            ItemDesc = input.ReadString();
            break;
          }
          case 34: {
            ItemVoice = input.ReadString();
            break;
          }
          case 42: {
            ItemPhiz = input.ReadString();
            break;
          }
          case 48: {
            player_ = (global::PlayerPB) input.ReadEnum();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
