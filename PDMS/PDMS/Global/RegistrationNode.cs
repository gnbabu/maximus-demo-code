using System;

/// <summary>
/// Summary description for RegistrationNode
/// </summary>
[Serializable]
public class RegistrationNode
{
    public int Sequence { get; set; }
    public string MenuPath { get; set; }
    public int StatusId { get; set; }
    public int Step { get; set; }

    public int IsRequired { get; set; }

    public int ProviderStatusId { get; set; }
    public int IsEditable { get; set; }

    public int ExcludeSectionFromProvReviewCount { get; set; }

    public RegistrationNode(int inputSequence, string inputMenuPath, int inputStep, int inputStatusId, int isRequired, int providerStatusId, int isEditable, int excludeSectionFromProvReviewCount)
	{
        Sequence = inputSequence;
        MenuPath = inputMenuPath;
        Step = inputStep;
        StatusId = inputStatusId;
        IsRequired = isRequired;
        ProviderStatusId = providerStatusId;
        IsEditable = isEditable;
        ExcludeSectionFromProvReviewCount = excludeSectionFromProvReviewCount;
    }
}