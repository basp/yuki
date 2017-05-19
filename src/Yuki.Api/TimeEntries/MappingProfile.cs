namespace Yuki.Api.TimeEntries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Yuki.Data;

    public class MappingProfile : AutoMapper.Profile
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1);

        public MappingProfile()
        {
            this.CreateMap<IDictionary<string, object>, TimeEntry>()
                // Not required but strongly suggested
                .ForMember(
                    dest => dest.Description, 
                    opts => opts.PreCondition(
                        src => src.ContainsKey(F.Description)))
                .ForMember(
                    dest => dest.Description, 
                    opts => opts.MapFrom(src => src[F.Description]))

                // Not required in case of updates or 
                // if `pid` or `tid` is specified
                .ForMember(
                    dest => dest.WorkspaceId, 
                    opts => opts.PreCondition(src => src.ContainsKey(F.Wid)))
                .ForMember(
                    dest => dest.WorkspaceId, 
                    opts => opts.Condition(
                        (src, dest) => dest.WorkspaceId == 0))
                .ForMember(
                    dest => dest.WorkspaceId, 
                    opts => opts.MapFrom(src => src[F.Wid]))

                // Not required if `wid` or `tid` is specified
                .ForMember(
                    dest => dest.ProjectId, 
                    opts => opts.PreCondition(src => src.ContainsKey(F.Pid)))
                .ForMember(
                    dest => dest.ProjectId, 
                    opts => opts.MapFrom(src => src[F.Pid]))

                // Not required if `wid` or `pid` is specified
                .ForMember(
                    dest => dest.TaskId, 
                    opts => opts.PreCondition(src => src.ContainsKey(F.Tid)))
                .ForMember(
                    dest => dest.TaskId,
                    opts => opts.MapFrom(src => src[F.Tid]))

                // Not required in case of updates or `start` API
                .ForMember(
                    dest => dest.Start, 
                    opts => opts.PreCondition(src => src.ContainsKey(F.Start)))
                .ForMember(
                    dest => dest.Start, 
                    opts => opts.MapFrom(src => src[F.Start]))

                // Not required
                .ForMember(
                    dest => dest.Stop, 
                    opts => opts.PreCondition(src => src.ContainsKey(F.Stop)))
                .ForMember(
                    dest => dest.Stop, 
                    opts => opts.MapFrom(src => src[F.Stop]))

                // Not required
                .ForMember(
                    dest => dest.Duration,
                    opts => opts.PreCondition(
                        src => src.ContainsKey(F.Duration)))
                .ForMember(
                    dest => dest.Duration, 
                    opts => opts.MapFrom(src => src[F.Duration]))

                // Not required
                .ForMember(
                    dest => dest.Tags, 
                    opts => opts.PreCondition(src => src.ContainsKey(F.Tags)))
                .ForMember(
                    dest => dest.Tags, 
                    opts => opts.MapFrom(src => src[F.Tags]));

            this.CreateMap<TimeEntry, IDictionary<string, object>>()
                .ConstructUsing(GetData);
        }

        private static int DurationInSeconds(DateTime start, DateTime stop) =>
            (int)Math.Round((stop - start).TotalSeconds);

        private static IDictionary<string, object> GetData(TimeEntry timeEntry)
        {
            var tags = timeEntry.Tags.Select(x => x.Name).ToArray();
            var data = new Dictionary<string, object>
            {
                ["description"] = timeEntry.Description,
                ["wid"] = timeEntry.WorkspaceId,
                ["start"] = timeEntry.Start.ToString(),
                ["tags"] = tags,
                ["at"] = timeEntry.LastUpdated.ToString(),
            };

            // If the timer is running, duration will be a negative value
            // denoting the start of the time entry in seconds since epoch.
            // The correct duration can be calculated by 
            // `current_time + duration` where `current_time` is the current 
            // time in seconds since epoch.
            data["duration"] = timeEntry.Stop.HasValue
                ? DurationInSeconds(timeEntry.Start, timeEntry.Stop.Value)
                : -SecondsSinceEpoch(timeEntry.Start);

            if (timeEntry.ProjectId.HasValue)
            {
                data["pid"] = timeEntry.ProjectId.Value;
            }

            if (timeEntry.TaskId.HasValue)
            {
                data["tid"] = timeEntry.TaskId.Value;
            }

            return data;
        }

        private static int SecondsSinceEpoch(DateTime dt) =>
            (int)Math.Round(dt.Subtract(epoch).TotalSeconds);
    }
}