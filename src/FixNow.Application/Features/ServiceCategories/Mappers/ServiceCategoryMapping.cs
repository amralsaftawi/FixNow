using System;
using System.Collections.Generic;
using System.Linq;
using FixNow.Application.Features.ServiceCategories.Dtos;

namespace FixNow.Application.Features.ServiceCategories.Mappers;

public static class ServiceCategoryMapping
{
    public static ServiceCategoryDto ToDto(
        this ServiceCategory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ServiceCategoryDto(
            ServiceCategoryId: entity.Id,
            Name: entity.Name,
            Description: entity.Description,
            IconKey: entity.IconKey,
            DisplayOrder: entity.DisplayOrder,
            Price: entity.Price,
            IsActive: entity.IsActive);
    }

    public static List<ServiceCategoryDto> ToDtos(
        this IEnumerable<ServiceCategory> entities)
    {
        return entities.Select(ToDto).ToList();
    }
}