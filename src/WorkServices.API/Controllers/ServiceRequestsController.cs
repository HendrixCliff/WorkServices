using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkServices.Application.Features.ServiceRequests.Commands.CreateServiceRequest;
using WorkServices.Application.Features.ServiceRequests.Queries.GetArtisanJob;
using WorkServices.Application.Features.ServiceRequests.Queries.GetCustomerRequests;
using WorkServices.Application.Features.ServiceRequests.Queries.GetServiceRequestById;

[ApiController]
[Route("api/service-requests")]
public class ServiceRequestsController
    : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceRequestsController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateServiceRequestCommand command)
    {
        var id =
            await _mediator.Send(command);

        return Ok(id);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetCustomerRequests(
        Guid customerId)
    {
        return Ok(
            await _mediator.Send(
                new GetCustomerRequestsQuery(
                    customerId)));
    }

    [HttpGet("artisan/{artisanId:guid}")]
    public async Task<IActionResult> GetArtisanJobs(
        Guid artisanId)
    {
        var jobs =
            await _mediator.Send(
                new GetArtisanJobsQuery(artisanId));

        return Ok(jobs);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(
            await _mediator.Send(
                new GetServiceRequestByIdQuery(id)));
    }
}