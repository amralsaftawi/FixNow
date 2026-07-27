using System;
using System.Collections.Generic;
using System.Linq;
using FixNow.Application.Features.TechnicianDiscovery.Queries.FindNearbyTechnicians;

namespace FixNow.Application.Features.TechnicianDiscovery.Mappers;

public static class FindNearbyTechniciansMapping
{
    public static NearbyTechnicianDto ToNearbyTechnicianDto(
        this TechnicianProfile entity,
        double distanceInKm)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new NearbyTechnicianDto(
            TechnicianProfileId: entity.Id,
            UserId: entity.UserId,
            FirstName: entity.User.FirstName,
            LastName: entity.User.LastName,
            ProfileImageKey: entity.User.ProfileImageKey,
            Bio: entity.Bio,
            YearsOfExperience: entity.YearsOfExperience,
            DistanceInKm: distanceInKm,
            Latitude: null,
            Longitude: null);
    }

    public static List<NearbyTechnicianDto> ToNearbyDtos(
        this IEnumerable<(TechnicianProfile Entity, double DistanceInKm)> items)
    {
        return items.Select(x => x.Entity.ToNearbyTechnicianDto(x.DistanceInKm)).ToList();
    }
}