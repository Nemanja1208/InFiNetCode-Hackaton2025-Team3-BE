namespace Application_Layer.IdeaSessions.DTOs
{
    public class IdeaSessionDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Status { get; set; }
        public required DateTime CreatedAt { get; set; }
        public List<MvpPlanDto>? MvpPlans { get; set; }
    }

}
