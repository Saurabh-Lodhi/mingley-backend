using AutoMapper;
using Mingley.Application.DTOs.Auth;
using Mingley.Application.DTOs.Users;
using Mingley.Application.DTOs.Subscription;
using Mingley.Domain.Entities;

namespace Mingley.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User → UserDto (auth response)
        CreateMap<User, UserDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.ToString()))
            .ForMember(d => d.ProfileComplete, o => o.MapFrom(s =>
                !string.IsNullOrEmpty(s.FullName) && !string.IsNullOrEmpty(s.Gender)));

        // User → UserProfileDto (full profile)
        CreateMap<User, UserProfileDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.ToString()))
            .ForMember(d => d.Age, o => o.MapFrom(s =>
                s.DateOfBirth.HasValue
                    ? (int?)((DateTime.UtcNow - s.DateOfBirth.Value).Days / 365)
                    : null))
            .ForMember(d => d.Interests, o => o.MapFrom(s =>
                s.Interests.Select(i => i.Interest != null ? i.Interest.Name : "").ToList()))
            .ForMember(d => d.Images, o => o.MapFrom(s => s.Images.Select(i => new ImageDto
            {
                Id = i.Id.ToString(),
                Url = i.Url,
                SortOrder = i.SortOrder
            }).ToList()))
            .ForMember(d => d.Location, o => o.MapFrom(s => s.Location == null ? null : new LocationDto
            {
                Lat = s.Location.Lat,
                Lng = s.Location.Lng,
                City = s.Location.City,
                Country = s.Location.Country
            }))
            .ForMember(d => d.Preference, o => o.MapFrom(s => s.Preference == null ? null : new PreferenceDto
            {
                InterestedIn = s.Preference.InterestedIn,
                MinAge = s.Preference.MinAge,
                MaxAge = s.Preference.MaxAge,
                MaxDistance = s.Preference.MaxDistance,
                RelationshipType = s.Preference.RelationshipType,
                NearbyOnly = s.Preference.NearbyOnly,
                OnlineOnly = s.Preference.OnlineOnly,
                VerifiedOnly = s.Preference.VerifiedOnly,
                Location = s.Preference.Location
            }));

        // SubscriptionPlan → SubscriptionPlanDto
        CreateMap<SubscriptionPlan, SubscriptionPlanDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.ToString()));
    }
}
