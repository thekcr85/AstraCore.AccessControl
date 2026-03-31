using AstraCore.AccessControl.Application.DTOs.AccessCard;
using AstraCore.AccessControl.Domain.Entities;

namespace AstraCore.AccessControl.Application.Mappings;

public static class AccessCardMappings
{
    public static AccessCardResponse ToResponse(this AccessCard card) => new()
    {
        Id = card.Id,
        CardNumber = card.CardNumber.Value,
        AccessLevel = card.AccessLevel.ToString(),
        IssuedDate = card.IssuedDate,
        ExpiryDate = card.ExpiryDate,
        IsActive = card.IsActive,
        IsValid = card.IsValid,
        IsExpired = card.IsExpired,
        EmployeeId = card.EmployeeId,
        CreatedAt = card.CreatedAt,
        UpdatedAt = card.UpdatedAt
    };

    public static IEnumerable<AccessCardResponse> ToResponseList(this IEnumerable<AccessCard> cards)
        => cards.Select(c => c.ToResponse());
}
