using WorkServices.Domain.Enums;

namespace WorkServices.Domain.Entities;

public class Artisan : User
{
    private Artisan()
    {
    }

    public ServiceType ServiceType { get; private set; }

    public bool IsAvailable { get; private set; }

    public decimal AverageRating { get; private set; }

    public int TotalReviews { get; private set; }
    
      public ICollection<JobAssignment> JobAssignments = new List<JobAssignment>();
   public Artisan(
    string fullName,
    string email,
    string phoneNumber,
    string passwordHash,
    ServiceType serviceType)
    : base(
        fullName,
        email,
        phoneNumber,
        passwordHash,
        UserRole.Artisan)
    {
        ServiceType = serviceType;
        IsAvailable = true;
    }

    public void SetAvailability(bool available)
    {
        IsAvailable = available;
        MarkUpdated();
    }

    public void UpdateRating(decimal rating)
    {
        AverageRating =
            ((AverageRating * TotalReviews) + rating)
            / (TotalReviews + 1);

        TotalReviews++;

        MarkUpdated();
    }
}