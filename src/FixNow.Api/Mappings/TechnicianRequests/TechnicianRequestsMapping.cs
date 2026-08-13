using System.Linq;
using ApplicationActiveServiceRequestDto =
    FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianActiveJobs.ActiveServiceRequestDto;
using ApplicationAvailableServiceRequestDto =
    FixNow.Application.Features.TechnicianRequests.Queries.GetAvailableServiceRequests.AvailableServiceRequestDto;
using ApplicationGetAvailableServiceRequestsResponse =
    FixNow.Application.Features.TechnicianRequests.Queries.GetAvailableServiceRequests.GetAvailableServiceRequestsResponse;
using ApplicationGetServiceRequestDetailsResponse =
    FixNow.Application.Features.TechnicianRequests.Queries.GetServiceRequestDetails.GetServiceRequestDetailsResponse;
using ApplicationGetTechnicianActiveJobsResponse =
    FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianActiveJobs.GetTechnicianActiveJobsResponse;
using ApplicationGetTechnicianJobHistoryResponse =
    FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianJobHistory.GetTechnicianJobHistoryResponse;
using ApplicationHistoricalServiceRequestDto =
    FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianJobHistory.HistoricalServiceRequestDto;
using ContractActiveServiceRequestResponse =
    FixNow.Contracts.Responses.ActiveServiceRequestResponse;
using ContractAvailableServiceRequestResponse =
    FixNow.Contracts.Responses.AvailableServiceRequestResponse;
using ContractGetAvailableServiceRequestsResponse =
    FixNow.Contracts.Responses.GetAvailableServiceRequestsResponse;
using ContractGetServiceRequestDetailsResponse =
    FixNow.Contracts.Responses.GetServiceRequestDetailsResponse;
using ContractGetTechnicianActiveJobsResponse =
    FixNow.Contracts.Responses.GetTechnicianActiveJobsResponse;
using ContractGetTechnicianJobHistoryResponse =
    FixNow.Contracts.Responses.GetTechnicianJobHistoryResponse;
using ContractHistoricalServiceRequestResponse =
    FixNow.Contracts.Responses.HistoricalServiceRequestResponse;

namespace FixNow.Api.Mappings.TechnicianRequests;

public static class TechnicianRequestsMapping
{
    public static ContractGetServiceRequestDetailsResponse ToContractResponse(
        this ApplicationGetServiceRequestDetailsResponse response)
    {
        var details = response.Details;

        return new ContractGetServiceRequestDetailsResponse(
            ServiceRequestId: details.ServiceRequestId,
            ServiceCategoryId: details.ServiceCategoryId,
            ServiceCategoryName: details.ServiceCategoryName,
            ProblemTypeId: details.ProblemTypeId,
            ProblemTypeName: details.ProblemTypeName,
            Description: details.Description,
            Priority: details.Priority,
            Status: details.Status,
            RequestedAt: details.RequestedAt,
            ScheduledAt: details.ScheduledAt,
            EstimatedCost: details.EstimatedCost,
            FullAddress: details.FullAddress,
            Latitude: details.Latitude,
            Longitude: details.Longitude,
            ImageKeys: details.ImageKeys);
    }

    public static ContractGetAvailableServiceRequestsResponse ToContractResponse(
        this ApplicationGetAvailableServiceRequestsResponse response)
        => new(
            Items: response.Items
                .Select(ToContractResponse)
                .ToList(),
            PageNumber: response.PageNumber,
            PageSize: response.PageSize,
            TotalCount: response.TotalCount,
            TotalPages: response.TotalPages);

    private static ContractAvailableServiceRequestResponse ToContractResponse(
        ApplicationAvailableServiceRequestDto item)
        => new(
            ServiceRequestId: item.ServiceRequestId,
            ServiceCategoryId: item.ServiceCategoryId,
            ServiceCategoryName: item.ServiceCategoryName,
            ProblemTypeId: item.ProblemTypeId,
            ProblemTypeName: item.ProblemTypeName,
            Description: item.Description,
            Priority: item.Priority,
            RequestedAt: item.RequestedAt,
            ScheduledAt: item.ScheduledAt,
            EstimatedCost: item.EstimatedCost,
            FullAddress: item.FullAddress,
            Latitude: item.Latitude,
            Longitude: item.Longitude);

    public static ContractGetTechnicianActiveJobsResponse ToContractResponse(
        this ApplicationGetTechnicianActiveJobsResponse response)
        => new(
            Items: response.Items
                .Select(ToContractResponse)
                .ToList(),
            PageNumber: response.PageNumber,
            PageSize: response.PageSize,
            TotalCount: response.TotalCount,
            TotalPages: response.TotalPages);

    private static ContractActiveServiceRequestResponse ToContractResponse(
        ApplicationActiveServiceRequestDto item)
        => new(
            ServiceRequestId: item.ServiceRequestId,
            ServiceCategoryId: item.ServiceCategoryId,
            ServiceCategoryName: item.ServiceCategoryName,
            ProblemTypeId: item.ProblemTypeId,
            ProblemTypeName: item.ProblemTypeName,
            Description: item.Description,
            Priority: item.Priority,
            Status: item.Status,
            RequestedAt: item.RequestedAt,
            ScheduledAt: item.ScheduledAt,
            EstimatedCost: item.EstimatedCost,
            FullAddress: item.FullAddress,
            Latitude: item.Latitude,
            Longitude: item.Longitude);

    public static ContractGetTechnicianJobHistoryResponse ToContractResponse(
        this ApplicationGetTechnicianJobHistoryResponse response)
        => new(
            Items: response.Items
                .Select(ToContractResponse)
                .ToList(),
            PageNumber: response.PageNumber,
            PageSize: response.PageSize,
            TotalCount: response.TotalCount,
            TotalPages: response.TotalPages);

    private static ContractHistoricalServiceRequestResponse ToContractResponse(
        ApplicationHistoricalServiceRequestDto item)
        => new(
            ServiceRequestId: item.ServiceRequestId,
            ServiceCategoryId: item.ServiceCategoryId,
            ServiceCategoryName: item.ServiceCategoryName,
            ProblemTypeId: item.ProblemTypeId,
            ProblemTypeName: item.ProblemTypeName,
            Description: item.Description,
            Priority: item.Priority,
            Status: item.Status,
            RequestedAt: item.RequestedAt,
            ScheduledAt: item.ScheduledAt,
            EstimatedCost: item.EstimatedCost,
            FullAddress: item.FullAddress,
            Latitude: item.Latitude,
            Longitude: item.Longitude);
}
