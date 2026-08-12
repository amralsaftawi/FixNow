using System.Collections.Generic;
using System.Linq;
using ApplicationTechnicianPortfolioItemResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianPortfolioItemResponse;
using ApplicationTechnicianPortfolioMediaResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianPortfolioMediaResponse;
using ContractTechnicianPortfolioItemResponse =
    FixNow.Contracts.Responses.TechnicianPortfolioItemResponse;
using ContractTechnicianPortfolioMediaResponse =
    FixNow.Contracts.Responses.TechnicianPortfolioMediaResponse;

namespace FixNow.Api.Mappings.TechnicianPortfolio;

public static class TechnicianPortfolioMapping
{
    public static ContractTechnicianPortfolioItemResponse ToContractResponse(
        this ApplicationTechnicianPortfolioItemResponse response)
        => new(
            PortfolioItemId: response.PortfolioItemId,
            TechnicianProfileId: response.TechnicianProfileId,
            Title: response.Title,
            Description: response.Description,
            Media: response.Media
                .Select(ToContractMediaResponse)
                .ToList());

    public static List<ContractTechnicianPortfolioItemResponse> ToContractResponses(
        this IEnumerable<ApplicationTechnicianPortfolioItemResponse> responses)
        => responses.Select(ToContractResponse).ToList();

    private static ContractTechnicianPortfolioMediaResponse ToContractMediaResponse(
        ApplicationTechnicianPortfolioMediaResponse response)
        => new(
            PortfolioMediaId: response.PortfolioMediaId,
            MediaKey: response.MediaKey,
            DisplayOrder: response.DisplayOrder);
}
