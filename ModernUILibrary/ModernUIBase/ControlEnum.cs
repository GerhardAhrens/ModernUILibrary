namespace ModernIU.Base
{
    public enum DashboardSkinEnum
    {
        Speed,
        Flow,
    }

    public enum ProgressBarSkinEnum
    {
        Rectangle,
        Circle,
    }
    public enum EnumPlacement
    {
        LeftTop,
        LeftCenter,
        LeftBottom,
        RightTop,
        RightCenter,
        RightBottom,
        TopLeft,
        TopCenter,
        TopRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
    }

    public enum EnumPlacementDirection
    {
        Left,
        Top,
        Right,
        Bottom,
    }

    public enum EnumPromptType
    {
        Info,
        Warn,
        Error,
        Success,
    }

    public enum EnumCompare
    {
        Less,
        Equal,
        Large,
        None,
    }

    public enum EnumLoadingType
    {
        DoubleArc,
        DoubleRound,
        SingleRound,
        Win10,
        Android,
        Apple,
        Cogs,
        Normal,
    }

    public enum CloseBoxTypeEnum
    {
        Close,
        Hide,
    }
    
    public enum FlatButtonSkinEnum
    {
        Yes,
        No,
        Default,
        primary,
        ghost,
        dashed,
        text,
        info,
        success,
        error,
        warning,
    }

    public enum EnumTrigger
    {
        Hover,
        Click,
        Custom,
    }

    public enum EnumTabControlType
    {
        Line,
        Card,
    }

    public enum EnumIconType
    {
        Info,
        Error,
        Warning,
        Success,
        MacOS,
        Windows,
        Linux,
        Android,
        Star_Empty,
        Star_Half,
        Star_Full,
    }

    public enum EnumDatePickerType
    {
        SingleDate,
        SingleDateRange,
        Year,
        Month,
        DateTime,
        DateTimeRange,
    }

    public enum DayTitle
    {
        Sunday = 0,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
    }

    public enum EnumPlayState
    {
        Play,
        Pause,
        Stop,
    }

    public enum EnumHeadingType
    {
        H1,
        H2,
        H3,
        H4,
        H5,
        H6,
    }
    
    public enum EnumPatternType
    {
        None,
        NotEmpty,
        OnlyNumber,
        IPV4,
        IPV6,
        Email,
        IdCard15,
        IdCard18,
        MobilePhone,
        Telephone,
        OnlyChinese,
    }

    public enum EnumValidateTrigger
    {
        PropertyChanged,
        LostFocus,
    }

    public enum EnumChooseBoxType
    {
        SingleFile,
        MultiFile,
        Folder,
    }
}
