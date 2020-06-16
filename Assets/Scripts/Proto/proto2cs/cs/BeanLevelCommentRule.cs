// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_level_comment_rule.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_level_comment_rule.proto</summary>
  public static partial class BeanLevelCommentRuleReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_level_comment_rule.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanLevelCommentRuleReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch1iZWFuX2xldmVsX2NvbW1lbnRfcnVsZS5wcm90bxIJY29tLnByb3RvGgpi",
            "YXNlLnByb3RvInAKEkxldmVsQ29tbWVudFJ1bGVQQhIKCgJpZBgBIAEoERIQ",
            "Cghncm91cF9pZBgCIAEoERILCgNpbWcYAyABKAkSDAoEbmFtZRgEIAEoCRIP",
            "Cgdjb250ZW50GAUgASgJEhAKCGxpa2VfbnVtGAYgASgRQjkKH25ldC5nYWxh",
            "c3BvcnRzLmJpZ3N0YXIucHJvdG9jb2xCFkxldmVsQ29tbWVudFJ1bGVQcm90",
            "b3NiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.LevelCommentRulePB), global::Com.Proto.LevelCommentRulePB.Parser, new[]{ "Id", "GroupId", "Img", "Name", "Content", "LikeNum" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///LevelCommentRulePB LevelCommentRule
  /// </summary>
  public sealed partial class LevelCommentRulePB : pb::IMessage<LevelCommentRulePB> {
    private static readonly pb::MessageParser<LevelCommentRulePB> _parser = new pb::MessageParser<LevelCommentRulePB>(() => new LevelCommentRulePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<LevelCommentRulePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanLevelCommentRuleReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LevelCommentRulePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LevelCommentRulePB(LevelCommentRulePB other) : this() {
      id_ = other.id_;
      groupId_ = other.groupId_;
      img_ = other.img_;
      name_ = other.name_;
      content_ = other.content_;
      likeNum_ = other.likeNum_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public LevelCommentRulePB Clone() {
      return new LevelCommentRulePB(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private int id_;
    /// <summary>
    ///评论id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "group_id" field.</summary>
    public const int GroupIdFieldNumber = 2;
    private int groupId_;
    /// <summary>
    ///星级类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int GroupId {
      get { return groupId_; }
      set {
        groupId_ = value;
      }
    }

    /// <summary>Field number for the "img" field.</summary>
    public const int ImgFieldNumber = 3;
    private string img_ = "";
    /// <summary>
    ///头像
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Img {
      get { return img_; }
      set {
        img_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 4;
    private string name_ = "";
    /// <summary>
    ///名称
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "content" field.</summary>
    public const int ContentFieldNumber = 5;
    private string content_ = "";
    /// <summary>
    ///评论内容
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Content {
      get { return content_; }
      set {
        content_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "like_num" field.</summary>
    public const int LikeNumFieldNumber = 6;
    private int likeNum_;
    /// <summary>
    ///点赞数
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int LikeNum {
      get { return likeNum_; }
      set {
        likeNum_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as LevelCommentRulePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(LevelCommentRulePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (GroupId != other.GroupId) return false;
      if (Img != other.Img) return false;
      if (Name != other.Name) return false;
      if (Content != other.Content) return false;
      if (LikeNum != other.LikeNum) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (GroupId != 0) hash ^= GroupId.GetHashCode();
      if (Img.Length != 0) hash ^= Img.GetHashCode();
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Content.Length != 0) hash ^= Content.GetHashCode();
      if (LikeNum != 0) hash ^= LikeNum.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(Id);
      }
      if (GroupId != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(GroupId);
      }
      if (Img.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Img);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Name);
      }
      if (Content.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(Content);
      }
      if (LikeNum != 0) {
        output.WriteRawTag(48);
        output.WriteSInt32(LikeNum);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Id);
      }
      if (GroupId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(GroupId);
      }
      if (Img.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Img);
      }
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (Content.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Content);
      }
      if (LikeNum != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(LikeNum);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(LevelCommentRulePB other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.GroupId != 0) {
        GroupId = other.GroupId;
      }
      if (other.Img.Length != 0) {
        Img = other.Img;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Content.Length != 0) {
        Content = other.Content;
      }
      if (other.LikeNum != 0) {
        LikeNum = other.LikeNum;
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
            Id = input.ReadSInt32();
            break;
          }
          case 16: {
            GroupId = input.ReadSInt32();
            break;
          }
          case 26: {
            Img = input.ReadString();
            break;
          }
          case 34: {
            Name = input.ReadString();
            break;
          }
          case 42: {
            Content = input.ReadString();
            break;
          }
          case 48: {
            LikeNum = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
