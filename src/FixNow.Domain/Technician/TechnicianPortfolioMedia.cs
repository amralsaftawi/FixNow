public sealed class TechnicianPortfolioMedia : AuditableEntity
{
    public Guid TechnicianPortfolioItemId { get; private set; }

    public string MediaKey { get; private set; }

    public int DisplayOrder { get; private set; }

    // Navigation

    public TechnicianPortfolioItem TechnicianPortfolioItem { get; private set; } = null!;

#pragma warning disable CS8618
    private TechnicianPortfolioMedia()
    {
    }
#pragma warning disable CS8618

    private TechnicianPortfolioMedia(
        Guid id,
        Guid technicianPortfolioItemId,
        string mediaKey,
        int displayOrder)
        : base(id)
    {
        TechnicianPortfolioItemId = technicianPortfolioItemId;
        MediaKey = mediaKey;
        DisplayOrder = displayOrder;
    }

    public static Result<TechnicianPortfolioMedia> Create(
        Guid id,
        Guid technicianPortfolioItemId,
        string mediaKey,
        int displayOrder)
    {
        if (id == Guid.Empty)
            return TechnicianPortfolioErrors.MediaIdRequired;

        if (technicianPortfolioItemId == Guid.Empty)
            return TechnicianPortfolioErrors.TechnicianPortfolioItemIdRequired;

        mediaKey = mediaKey?.Trim();

        if (string.IsNullOrWhiteSpace(mediaKey))
            return TechnicianPortfolioErrors.MediaKeyRequired;

        if (mediaKey.Length > 500)
            return TechnicianPortfolioErrors.MediaKeyTooLong;

        if (displayOrder < 0)
            return TechnicianPortfolioErrors.InvalidDisplayOrder;

        return new TechnicianPortfolioMedia(
            id,
            technicianPortfolioItemId,
            mediaKey,
            displayOrder);
    }

    public Result<Success> UpdateDisplayOrder(int displayOrder)
    {
        if (displayOrder < 0)
            return TechnicianPortfolioErrors.InvalidDisplayOrder;

        if (DisplayOrder == displayOrder)
            return Result.Success;

        DisplayOrder = displayOrder;

        return Result.Success;
    }
}
