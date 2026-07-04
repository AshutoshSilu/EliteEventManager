namespace EliteEvents.Application.DTOs.Review;

public class ReviewDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerImage { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public int EntityId { get; set; }
    public string? EntityName { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public string? ImageUrl { get; set; }
    public string? Reply { get; set; }
    public DateTime? RepliedAt { get; set; }
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ReviewCreateDto
{
    public string EntityType { get; set; } = string.Empty;
    public int EntityId { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public string? ImageUrl { get; set; }
}

public class ReviewReplyDto
{
    public int ReviewId { get; set; }
    public string Reply { get; set; } = string.Empty;
}
