// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: bean_user_diary.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Com.Proto {

  /// <summary>Holder for reflection information generated from bean_user_diary.proto</summary>
  public static partial class BeanUserDiaryReflection {

    #region Descriptor
    /// <summary>File descriptor for bean_user_diary.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BeanUserDiaryReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChViZWFuX3VzZXJfZGlhcnkucHJvdG8SCWNvbS5wcm90bxoKYmFzZS5wcm90",
            "byKOAQoLVXNlckRpYXJ5UEISDwoHdXNlcl9pZBgBIAEoERIMCgR5ZWFyGAIg",
            "ASgREg0KBW1vbnRoGAMgASgREgwKBGRhdGUYBCABKBESEAoIbG9nX3RpbWUY",
            "BSABKBISMQoOZGlhcnlfZWxlbWVudHMYBiADKAsyGS5jb20ucHJvdG8uRGlh",
            "cnlFbGVtZW50UEIiPAoPVXNlckRpYXJ5RGF0ZVBCEgwKBHllYXIYASABKBES",
            "DQoFbW9udGgYAiABKBESDAoEZGF0ZRgDIAEoESKVAQoORGlhcnlFbGVtZW50",
            "UEISEgoKZWxlbWVudF9pZBgBIAEoBRINCgV4X3BvcxgCIAEoAhINCgV5X3Bv",
            "cxgDIAEoAhIPCgdzY2FsZV94GAQgASgCEg8KB3NjYWxlX3kYBSABKAISDwoH",
            "Y29udGVudBgGIAEoCRIQCghyb3RhdGlvbhgHIAEoAhIMCgRzaXplGAggASgF",
            "QjIKH25ldC5nYWxhc3BvcnRzLmJpZ3N0YXIucHJvdG9jb2xCD1VzZXJEaWFy",
            "eVByb3Rvc2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::BaseReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserDiaryPB), global::Com.Proto.UserDiaryPB.Parser, new[]{ "UserId", "Year", "Month", "Date", "LogTime", "DiaryElements" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.UserDiaryDatePB), global::Com.Proto.UserDiaryDatePB.Parser, new[]{ "Year", "Month", "Date" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Com.Proto.DiaryElementPB), global::Com.Proto.DiaryElementPB.Parser, new[]{ "ElementId", "XPos", "YPos", "ScaleX", "ScaleY", "Content", "Rotation", "Size" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///UserDiaryPB UserDiary
  /// </summary>
  public sealed partial class UserDiaryPB : pb::IMessage<UserDiaryPB> {
    private static readonly pb::MessageParser<UserDiaryPB> _parser = new pb::MessageParser<UserDiaryPB>(() => new UserDiaryPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserDiaryPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserDiaryReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserDiaryPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserDiaryPB(UserDiaryPB other) : this() {
      userId_ = other.userId_;
      year_ = other.year_;
      month_ = other.month_;
      date_ = other.date_;
      logTime_ = other.logTime_;
      diaryElements_ = other.diaryElements_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserDiaryPB Clone() {
      return new UserDiaryPB(this);
    }

    /// <summary>Field number for the "user_id" field.</summary>
    public const int UserIdFieldNumber = 1;
    private int userId_;
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int UserId {
      get { return userId_; }
      set {
        userId_ = value;
      }
    }

    /// <summary>Field number for the "year" field.</summary>
    public const int YearFieldNumber = 2;
    private int year_;
    /// <summary>
    ///年
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Year {
      get { return year_; }
      set {
        year_ = value;
      }
    }

    /// <summary>Field number for the "month" field.</summary>
    public const int MonthFieldNumber = 3;
    private int month_;
    /// <summary>
    ///月
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Month {
      get { return month_; }
      set {
        month_ = value;
      }
    }

    /// <summary>Field number for the "date" field.</summary>
    public const int DateFieldNumber = 4;
    private int date_;
    /// <summary>
    ///日
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Date {
      get { return date_; }
      set {
        date_ = value;
      }
    }

    /// <summary>Field number for the "log_time" field.</summary>
    public const int LogTimeFieldNumber = 5;
    private long logTime_;
    /// <summary>
    ///记录日记的时间，可以是任意时间
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long LogTime {
      get { return logTime_; }
      set {
        logTime_ = value;
      }
    }

    /// <summary>Field number for the "diary_elements" field.</summary>
    public const int DiaryElementsFieldNumber = 6;
    private static readonly pb::FieldCodec<global::Com.Proto.DiaryElementPB> _repeated_diaryElements_codec
        = pb::FieldCodec.ForMessage(50, global::Com.Proto.DiaryElementPB.Parser);
    private readonly pbc::RepeatedField<global::Com.Proto.DiaryElementPB> diaryElements_ = new pbc::RepeatedField<global::Com.Proto.DiaryElementPB>();
    /// <summary>
    ///具体内容
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Com.Proto.DiaryElementPB> DiaryElements {
      get { return diaryElements_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserDiaryPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserDiaryPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserId != other.UserId) return false;
      if (Year != other.Year) return false;
      if (Month != other.Month) return false;
      if (Date != other.Date) return false;
      if (LogTime != other.LogTime) return false;
      if(!diaryElements_.Equals(other.diaryElements_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserId != 0) hash ^= UserId.GetHashCode();
      if (Year != 0) hash ^= Year.GetHashCode();
      if (Month != 0) hash ^= Month.GetHashCode();
      if (Date != 0) hash ^= Date.GetHashCode();
      if (LogTime != 0L) hash ^= LogTime.GetHashCode();
      hash ^= diaryElements_.GetHashCode();
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
      if (Year != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(Year);
      }
      if (Month != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Month);
      }
      if (Date != 0) {
        output.WriteRawTag(32);
        output.WriteSInt32(Date);
      }
      if (LogTime != 0L) {
        output.WriteRawTag(40);
        output.WriteSInt64(LogTime);
      }
      diaryElements_.WriteTo(output, _repeated_diaryElements_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(UserId);
      }
      if (Year != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Year);
      }
      if (Month != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Month);
      }
      if (Date != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Date);
      }
      if (LogTime != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeSInt64Size(LogTime);
      }
      size += diaryElements_.CalculateSize(_repeated_diaryElements_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserDiaryPB other) {
      if (other == null) {
        return;
      }
      if (other.UserId != 0) {
        UserId = other.UserId;
      }
      if (other.Year != 0) {
        Year = other.Year;
      }
      if (other.Month != 0) {
        Month = other.Month;
      }
      if (other.Date != 0) {
        Date = other.Date;
      }
      if (other.LogTime != 0L) {
        LogTime = other.LogTime;
      }
      diaryElements_.Add(other.diaryElements_);
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
            Year = input.ReadSInt32();
            break;
          }
          case 24: {
            Month = input.ReadSInt32();
            break;
          }
          case 32: {
            Date = input.ReadSInt32();
            break;
          }
          case 40: {
            LogTime = input.ReadSInt64();
            break;
          }
          case 50: {
            diaryElements_.AddEntriesFrom(input, _repeated_diaryElements_codec);
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///UserDiaryDatePB UserDiaryDate
  /// </summary>
  public sealed partial class UserDiaryDatePB : pb::IMessage<UserDiaryDatePB> {
    private static readonly pb::MessageParser<UserDiaryDatePB> _parser = new pb::MessageParser<UserDiaryDatePB>(() => new UserDiaryDatePB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UserDiaryDatePB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserDiaryReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserDiaryDatePB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserDiaryDatePB(UserDiaryDatePB other) : this() {
      year_ = other.year_;
      month_ = other.month_;
      date_ = other.date_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UserDiaryDatePB Clone() {
      return new UserDiaryDatePB(this);
    }

    /// <summary>Field number for the "year" field.</summary>
    public const int YearFieldNumber = 1;
    private int year_;
    /// <summary>
    ///年
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Year {
      get { return year_; }
      set {
        year_ = value;
      }
    }

    /// <summary>Field number for the "month" field.</summary>
    public const int MonthFieldNumber = 2;
    private int month_;
    /// <summary>
    ///月
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Month {
      get { return month_; }
      set {
        month_ = value;
      }
    }

    /// <summary>Field number for the "date" field.</summary>
    public const int DateFieldNumber = 3;
    private int date_;
    /// <summary>
    ///哪天
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Date {
      get { return date_; }
      set {
        date_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UserDiaryDatePB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UserDiaryDatePB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Year != other.Year) return false;
      if (Month != other.Month) return false;
      if (Date != other.Date) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Year != 0) hash ^= Year.GetHashCode();
      if (Month != 0) hash ^= Month.GetHashCode();
      if (Date != 0) hash ^= Date.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Year != 0) {
        output.WriteRawTag(8);
        output.WriteSInt32(Year);
      }
      if (Month != 0) {
        output.WriteRawTag(16);
        output.WriteSInt32(Month);
      }
      if (Date != 0) {
        output.WriteRawTag(24);
        output.WriteSInt32(Date);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Year != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Year);
      }
      if (Month != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Month);
      }
      if (Date != 0) {
        size += 1 + pb::CodedOutputStream.ComputeSInt32Size(Date);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UserDiaryDatePB other) {
      if (other == null) {
        return;
      }
      if (other.Year != 0) {
        Year = other.Year;
      }
      if (other.Month != 0) {
        Month = other.Month;
      }
      if (other.Date != 0) {
        Date = other.Date;
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
            Year = input.ReadSInt32();
            break;
          }
          case 16: {
            Month = input.ReadSInt32();
            break;
          }
          case 24: {
            Date = input.ReadSInt32();
            break;
          }
        }
      }
    }

  }

  /// <summary>
  ///DiaryElementPB DiaryElement
  /// </summary>
  public sealed partial class DiaryElementPB : pb::IMessage<DiaryElementPB> {
    private static readonly pb::MessageParser<DiaryElementPB> _parser = new pb::MessageParser<DiaryElementPB>(() => new DiaryElementPB());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<DiaryElementPB> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Com.Proto.BeanUserDiaryReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DiaryElementPB() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DiaryElementPB(DiaryElementPB other) : this() {
      elementId_ = other.elementId_;
      xPos_ = other.xPos_;
      yPos_ = other.yPos_;
      scaleX_ = other.scaleX_;
      scaleY_ = other.scaleY_;
      content_ = other.content_;
      rotation_ = other.rotation_;
      size_ = other.size_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public DiaryElementPB Clone() {
      return new DiaryElementPB(this);
    }

    /// <summary>Field number for the "element_id" field.</summary>
    public const int ElementIdFieldNumber = 1;
    private int elementId_;
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int ElementId {
      get { return elementId_; }
      set {
        elementId_ = value;
      }
    }

    /// <summary>Field number for the "x_pos" field.</summary>
    public const int XPosFieldNumber = 2;
    private float xPos_;
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float XPos {
      get { return xPos_; }
      set {
        xPos_ = value;
      }
    }

    /// <summary>Field number for the "y_pos" field.</summary>
    public const int YPosFieldNumber = 3;
    private float yPos_;
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float YPos {
      get { return yPos_; }
      set {
        yPos_ = value;
      }
    }

    /// <summary>Field number for the "scale_x" field.</summary>
    public const int ScaleXFieldNumber = 4;
    private float scaleX_;
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float ScaleX {
      get { return scaleX_; }
      set {
        scaleX_ = value;
      }
    }

    /// <summary>Field number for the "scale_y" field.</summary>
    public const int ScaleYFieldNumber = 5;
    private float scaleY_;
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float ScaleY {
      get { return scaleY_; }
      set {
        scaleY_ = value;
      }
    }

    /// <summary>Field number for the "content" field.</summary>
    public const int ContentFieldNumber = 6;
    private string content_ = "";
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Content {
      get { return content_; }
      set {
        content_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "rotation" field.</summary>
    public const int RotationFieldNumber = 7;
    private float rotation_;
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Rotation {
      get { return rotation_; }
      set {
        rotation_ = value;
      }
    }

    /// <summary>Field number for the "size" field.</summary>
    public const int SizeFieldNumber = 8;
    private int size_;
    /// <summary>
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Size {
      get { return size_; }
      set {
        size_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as DiaryElementPB);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(DiaryElementPB other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ElementId != other.ElementId) return false;
      if (XPos != other.XPos) return false;
      if (YPos != other.YPos) return false;
      if (ScaleX != other.ScaleX) return false;
      if (ScaleY != other.ScaleY) return false;
      if (Content != other.Content) return false;
      if (Rotation != other.Rotation) return false;
      if (Size != other.Size) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ElementId != 0) hash ^= ElementId.GetHashCode();
      if (XPos != 0F) hash ^= XPos.GetHashCode();
      if (YPos != 0F) hash ^= YPos.GetHashCode();
      if (ScaleX != 0F) hash ^= ScaleX.GetHashCode();
      if (ScaleY != 0F) hash ^= ScaleY.GetHashCode();
      if (Content.Length != 0) hash ^= Content.GetHashCode();
      if (Rotation != 0F) hash ^= Rotation.GetHashCode();
      if (Size != 0) hash ^= Size.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ElementId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ElementId);
      }
      if (XPos != 0F) {
        output.WriteRawTag(21);
        output.WriteFloat(XPos);
      }
      if (YPos != 0F) {
        output.WriteRawTag(29);
        output.WriteFloat(YPos);
      }
      if (ScaleX != 0F) {
        output.WriteRawTag(37);
        output.WriteFloat(ScaleX);
      }
      if (ScaleY != 0F) {
        output.WriteRawTag(45);
        output.WriteFloat(ScaleY);
      }
      if (Content.Length != 0) {
        output.WriteRawTag(50);
        output.WriteString(Content);
      }
      if (Rotation != 0F) {
        output.WriteRawTag(61);
        output.WriteFloat(Rotation);
      }
      if (Size != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(Size);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ElementId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ElementId);
      }
      if (XPos != 0F) {
        size += 1 + 4;
      }
      if (YPos != 0F) {
        size += 1 + 4;
      }
      if (ScaleX != 0F) {
        size += 1 + 4;
      }
      if (ScaleY != 0F) {
        size += 1 + 4;
      }
      if (Content.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Content);
      }
      if (Rotation != 0F) {
        size += 1 + 4;
      }
      if (Size != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Size);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(DiaryElementPB other) {
      if (other == null) {
        return;
      }
      if (other.ElementId != 0) {
        ElementId = other.ElementId;
      }
      if (other.XPos != 0F) {
        XPos = other.XPos;
      }
      if (other.YPos != 0F) {
        YPos = other.YPos;
      }
      if (other.ScaleX != 0F) {
        ScaleX = other.ScaleX;
      }
      if (other.ScaleY != 0F) {
        ScaleY = other.ScaleY;
      }
      if (other.Content.Length != 0) {
        Content = other.Content;
      }
      if (other.Rotation != 0F) {
        Rotation = other.Rotation;
      }
      if (other.Size != 0) {
        Size = other.Size;
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
            ElementId = input.ReadInt32();
            break;
          }
          case 21: {
            XPos = input.ReadFloat();
            break;
          }
          case 29: {
            YPos = input.ReadFloat();
            break;
          }
          case 37: {
            ScaleX = input.ReadFloat();
            break;
          }
          case 45: {
            ScaleY = input.ReadFloat();
            break;
          }
          case 50: {
            Content = input.ReadString();
            break;
          }
          case 61: {
            Rotation = input.ReadFloat();
            break;
          }
          case 64: {
            Size = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code