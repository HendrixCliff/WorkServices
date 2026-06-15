using WorkServices.Domain.Abstractions;
using WorkServices.Domain.Enums;

namespace WorkServices.Domain.Entities;

public class ServiceRequest : Entity
{
    private ServiceRequest()
    {
    }

    public Guid CustomerId { get; private set; }

    public ServiceType ServiceType { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public string Address { get; private set; } = string.Empty;

    public RequestStatus Status { get; private set; }

    public Customer? Customer { get; private set; }

    public ServiceRequest(
        Guid customerId,
        ServiceType serviceType,
        string description,
        string address)
    {
        CustomerId = customerId;
        ServiceType = serviceType;
        Description = description;
        Address = address;

        Status = RequestStatus.Pending;
    }

    public void Assign()
    {
        Status = RequestStatus.Assigned;
        MarkUpdated();
    }

    public void Start()
    {
        Status = RequestStatus.InProgress;
        MarkUpdated();
    }

    public void Complete()
    {
        Status = RequestStatus.Completed;
        MarkUpdated();
    }
}