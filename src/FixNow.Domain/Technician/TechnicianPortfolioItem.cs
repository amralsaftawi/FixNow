public sealed class TechnicianPortfolioItem : AuditableEntity
{
    public Guid TechnicianProfileId { get; private set; }

    public string Title { get; private set; }

    public string? Description { get; private set; }

    // Navigation

    public TechnicianProfile TechnicianProfile { get; private set; } = null!;

    private readonly List<TechnicianPortfolioMedia> _media = [];

    public IReadOnlyCollection<TechnicianPortfolioMedia> Media =>
        _media.AsReadOnly();

#pragma warning disable CS8618
    private TechnicianPortfolioItem()
    {
    }
#pragma warning disable CS8618

    private TechnicianPortfolioItem(
        Guid id,
        Guid technicianProfileId,
        string title,
        string? description)
        : base(id)
    {
        TechnicianProfileId = technicianProfileId;
        Title = title;
        Description = description;
    }

    public static Result<TechnicianPortfolioItem> Create(
        Guid id,
        Guid technicianProfileId,
        string title,
        string? description,
        IReadOnlyCollection<string>? mediaKeys)
    {
        if (id == Guid.Empty)
            return TechnicianPortfolioErrors.IdRequired;

        if (technicianProfileId == Guid.Empty)
            return TechnicianPortfolioErrors.TechnicianProfileIdRequired;

        title = title?.Trim();

        if (string.IsNullOrWhiteSpace(title))
            return TechnicianPortfolioErrors.TitleRequired;

        if (title.Length > 150)
            return TechnicianPortfolioErrors.TitleTooLong;

        description = description?.Trim();

        if (description?.Length > 1000)
            return TechnicianPortfolioErrors.DescriptionTooLong;

        var normalizedMediaKeys = NormalizeMediaKeys(mediaKeys);

        if (normalizedMediaKeys is null)
        {
            return TechnicianPortfolioErrors.MediaKeyRequired;
        }

        var duplicateMediaKey = FindDuplicateMediaKey(normalizedMediaKeys);

        if (duplicateMediaKey is not null)
        {
            return TechnicianPortfolioErrors.DuplicateMediaKey;
        }

        if (normalizedMediaKeys.Any(key => key.Length > 500))
        {
            return TechnicianPortfolioErrors.MediaKeyTooLong;
        }

        var item = new TechnicianPortfolioItem(
            id,
            technicianProfileId,
            title,
            description);

        var mediaErrors = item.CreateMedia(normalizedMediaKeys);

        if (mediaErrors is not null)
        {
            return mediaErrors;
        }

        item.AddDomainEvent(
            new TechnicianPortfolioItemCreatedDomainEvent(
                item.Id,
                item.TechnicianProfileId));

        return item;
    }

    public Result<Success> Update(
        string title,
        string? description,
        IReadOnlyCollection<string>? mediaKeys)
    {
        title = title?.Trim();

        if (string.IsNullOrWhiteSpace(title))
            return TechnicianPortfolioErrors.TitleRequired;

        if (title.Length > 150)
            return TechnicianPortfolioErrors.TitleTooLong;

        description = description?.Trim();

        if (description?.Length > 1000)
            return TechnicianPortfolioErrors.DescriptionTooLong;

        var normalizedMediaKeys = NormalizeMediaKeys(mediaKeys);

        if (normalizedMediaKeys is null)
        {
            return TechnicianPortfolioErrors.MediaKeyRequired;
        }

        var duplicateMediaKey = FindDuplicateMediaKey(normalizedMediaKeys);

        if (duplicateMediaKey is not null)
        {
            return TechnicianPortfolioErrors.DuplicateMediaKey;
        }

        if (normalizedMediaKeys.Any(key => key.Length > 500))
        {
            return TechnicianPortfolioErrors.MediaKeyTooLong;
        }

        Title = title;
        Description = description;

        var mediaErrors = ReconcileMedia(normalizedMediaKeys);

        if (mediaErrors is not null)
        {
            return mediaErrors;
        }

        AddDomainEvent(
            new TechnicianPortfolioItemUpdatedDomainEvent(
                Id,
                TechnicianProfileId));

        return Result.Success;
    }

    private List<Error>? CreateMedia(
        IReadOnlyCollection<string> mediaKeys)
    {
        var displayOrder = 0;

        foreach (var mediaKey in mediaKeys)
        {
            var mediaResult = TechnicianPortfolioMedia.Create(
                id: Guid.NewGuid(),
                technicianPortfolioItemId: Id,
                mediaKey: mediaKey,
                displayOrder: displayOrder);

            if (mediaResult.IsError)
            {
                return mediaResult.Errors;
            }

            _media.Add(mediaResult.Value);

            displayOrder++;
        }

        return null;
    }

    private List<Error>? ReconcileMedia(
        IReadOnlyCollection<string> mediaKeys)
    {
        var keysToKeep = mediaKeys.ToHashSet(StringComparer.Ordinal);

        _media.RemoveAll(media => !keysToKeep.Contains(media.MediaKey));

        var displayOrder = 0;

        foreach (var mediaKey in mediaKeys)
        {
            var media = _media.FirstOrDefault(x => x.MediaKey == mediaKey);

            if (media is null)
            {
                var mediaResult = TechnicianPortfolioMedia.Create(
                    id: Guid.NewGuid(),
                    technicianPortfolioItemId: Id,
                    mediaKey: mediaKey,
                    displayOrder: displayOrder);

                if (mediaResult.IsError)
                {
                    return mediaResult.Errors;
                }

                _media.Add(mediaResult.Value);
            }
            else
            {
                var updateResult = media.UpdateDisplayOrder(displayOrder);

                if (updateResult.IsError)
                {
                    return updateResult.Errors;
                }
            }

            displayOrder++;
        }

        return null;
    }

    private static List<string>? NormalizeMediaKeys(
        IReadOnlyCollection<string>? mediaKeys)
    {
        if (mediaKeys is null)
        {
            return [];
        }

        var normalizedMediaKeys = mediaKeys
            .Select(key => key?.Trim() ?? string.Empty)
            .ToList();

        if (normalizedMediaKeys.Any(string.IsNullOrWhiteSpace))
        {
            return null;
        }

        return normalizedMediaKeys;
    }

    private static string? FindDuplicateMediaKey(
        List<string> normalizedMediaKeys)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);

        foreach (var mediaKey in normalizedMediaKeys)
        {
            if (!seen.Add(mediaKey))
            {
                return mediaKey;
            }
        }

        return null;
    }
}
