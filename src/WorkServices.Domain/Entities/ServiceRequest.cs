using WorkServices.Domain.Abstractions;
using WorkServices.Domain.Enums;
using WorkServices.Domain.Events;

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

    public JobStatus Status { get; private set; }

    public Customer? Customer { get; private set; }

    public decimal EstimatedCost { get; private set; }
    
    public ICollection<JobAssignment> JobAssignments
    {
        get;
        private set;
    }
        = new List<JobAssignment>();
   
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

        Status = JobStatus.Pending;
    }

public void Assign(Guid artisanId)
{
    if (Status != JobStatus.Pending)
        throw new InvalidOperationException(
            "Only pending requests can be assigned.");

    Status = JobStatus.Assigned;

    AddDomainEvent(
        new JobAssignedDomainEvent(
            Id,
            CustomerId,
            artisanId,
            ServiceType));

    MarkUpdated();
}

    public void SetEstimatedCost(decimal amount)
{
    EstimatedCost = amount;

    MarkUpdated();
}

   public void SubmitQuote()
    {
        if (Status != JobStatus.Assigned)
            throw new InvalidOperationException(
                "Only assigned jobs can receive quotes.");

        Status = JobStatus.QuoteSubmitted;

        AddDomainEvent(
        new QuoteSubmittedDomainEvent(
            Id));

        MarkUpdated();
    }

    public void ApproveQuote()
    {
        if (Status != JobStatus.QuoteSubmitted)
            throw new InvalidOperationException(
                "Quote must be submitted first.");

        Status = JobStatus.QuoteApproved;

         AddDomainEvent(
        new QuoteApprovedDomainEvent(Id));

        MarkUpdated();
    }

    public void MarkMaterialsPaid()
    {
        if (Status != JobStatus.QuoteApproved)
            throw new InvalidOperationException(
                "Quote must be approved first.");

        Status = JobStatus.MaterialsPaid;


        MarkUpdated();
    }

    public void Start()
    {
        if (Status != JobStatus.MaterialsPaid)
            throw new InvalidOperationException(
                "Materials must be paid before work starts.");

        Status = JobStatus.InProgress;

          AddDomainEvent(
        new JobStartedDomainEvent(Id));

        MarkUpdated();
    }

    public void Complete(Guid artisanId)
    {
        if (Status != JobStatus.InProgress)
            throw new InvalidOperationException(
                "Only an active job can be completed.");

        Status = JobStatus.Completed;

        AddDomainEvent(
            new JobCompletedDomainEvent(
                Id,
                CustomerId,
                artisanId,
                ServiceType));

        MarkUpdated();
    }

    public void MarkLabourPaid()
    {
        if (Status != JobStatus.Completed)
            throw new InvalidOperationException(
                "Job must be completed before labour payment.");

        Status = JobStatus.LabourPaid;


        MarkUpdated();
    }

    public void MarkReviewed()
    {
        if (Status != JobStatus.LabourPaid)
            throw new InvalidOperationException(
                "Labour payment must be completed before review.");

        Status = JobStatus.Reviewed;


        MarkUpdated();
    }
}