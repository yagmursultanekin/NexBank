using NexBank.Application.DTOs;

namespace NexBank.Application.Interfaces;

public interface IBranchService
{
    Task<List<BranchDto>> GetNearestAsync(double latitude, double longitude, int distanceLimitKm);
}