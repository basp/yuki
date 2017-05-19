namespace Yuki.Api
{
    // Field names (keys) in the dictionary that is used as a
    // data transfer object at the external boundaries of the API.
    internal class F
    {
        public static readonly string Active = nameof(Active)
            .ToLowerInvariant();

        public const string IsPrivate = "is_private";

        public static readonly string Id = nameof(Id)
            .ToLowerInvariant();

        public static readonly string Cid = nameof(Cid)
            .ToLowerInvariant();

        public static readonly string Tid = nameof(Tid)
            .ToLowerInvariant();

        public static readonly string Name = nameof(Name)
            .ToLowerInvariant();

        public static readonly string Wid = nameof(Wid)
            .ToLowerInvariant();

        public static readonly string Pid = nameof(Pid)
            .ToLowerInvariant();

        public static readonly string Start = nameof(Start)
            .ToLowerInvariant();

        public static readonly string Stop = nameof(Stop)
            .ToLowerInvariant();

        public static readonly string Description = nameof(Description)
            .ToLowerInvariant();

        public static readonly string Duration = nameof(Duration)
            .ToLowerInvariant();

        public static readonly string Tags = nameof(Tags)
            .ToLowerInvariant();

        public static readonly string At = nameof(At)
            .ToLowerInvariant();
    }
}