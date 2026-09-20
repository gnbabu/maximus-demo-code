using MAXIMUS.Core.Libraries;
using System;
using CON = MAXIMUS.Core.Libraries.Constants;

/// <summary>
/// Summary description for Screening
/// </summary>
public class ScreeningHelper
{
    public ScreeningHelper()
	{
	}

    public static bool DatabaseCheckActivityType(int activityTypeID)
    {
        bool isDBCheckActivityType = false;

        if (!Enumerations.ScreeningActivityType.IsDefined(typeof(Enumerations.ScreeningActivityType), activityTypeID))
        {
            throw new ArgumentException(string.Format("Unsupported Screening Activity Type ID: {0}", activityTypeID));
        }

        Enumerations.ScreeningActivityType activityType = (Enumerations.ScreeningActivityType)activityTypeID;

        switch (activityType)
        {
            case Enumerations.ScreeningActivityType.OIGLEIEVerification:
            case Enumerations.ScreeningActivityType.SAMVerification:
            case Enumerations.ScreeningActivityType.SSDMFVerification:
            case Enumerations.ScreeningActivityType.NPIVerification:
            case Enumerations.ScreeningActivityType.NEMEPLVerification:
            case Enumerations.ScreeningActivityType.MCSISVerification:
                isDBCheckActivityType = true;
				break;
			default:
                isDBCheckActivityType = false;
				break;
        }

        return isDBCheckActivityType;
    }

    public static bool CompletePositiveResultActivityStatus(int activityTypeID, int activityStatusID)
    {
        // a positive result to the screening
        bool isPositiveResultStatus = false;

        switch (activityStatusID)
        {
            case CON.ScreeningActivityStatusId.NotApplicable:
            case CON.ScreeningActivityStatusId.Verified:
            case CON.ScreeningActivityStatusId.MedicareEnrolled:
            case CON.ScreeningActivityStatusId.Confirmed:
                isPositiveResultStatus = true;
                break;
            case CON.ScreeningActivityStatusId.NoMatch:
                isPositiveResultStatus = MatchIsPositiveResultActivityType(activityTypeID) ? false : true;  //if match is a positive result, no match result is negative result
                break;
            default:
                isPositiveResultStatus = false;
                break;
        }

        return isPositiveResultStatus;
    }

    public static bool MatchIsPositiveResultActivityType(int activityTypeID)
    {
        bool matchIsGood = false;
        switch (activityTypeID)
        {
            //case (int)Enumerations.ScreeningActivityType.PECOSVerfication:
            //    matchIsGood = true; 
            //    break;
            default:
                matchIsGood = false;
                break;
        }
        return matchIsGood;
    }

    public static bool CompleteNegativeResultActivityStatus(int activityTypeID, int activityStatusID)
    {
        //Match Found Status = a negative result to the screening, may require action from state admin
        bool isNegativeResultStatus = false;

        switch (activityStatusID)
        {
            case CON.ScreeningActivityStatusId.Match:
            case CON.ScreeningActivityStatusId.Failed:
                isNegativeResultStatus = true;
                break;
            case CON.ScreeningActivityStatusId.NoMatch:
                isNegativeResultStatus = MatchIsPositiveResultActivityType(activityTypeID) ? true : false; //if match is a positive result, no match result is negative result
                break;
            default:
                isNegativeResultStatus = false;
                break;
        }

        return isNegativeResultStatus;
    }


    public static bool CompleteActivityStatus(int activityTypeID, int activityStatusID)
    {
        bool isCompleteStatusType = false;

        isCompleteStatusType = (CompleteNegativeResultActivityStatus(activityTypeID, activityStatusID) || CompletePositiveResultActivityStatus(activityTypeID, activityStatusID));

        return isCompleteStatusType;
    }

    public static bool LinkClickRequiredResultChange(int activityTypeID, Enumerations.ResultChangeType resultChangeType)
    {
        bool isRequired = false;

        //nppes - from not verified to verified
        //lei- yes, for (match-confirm to match)
        //sam - not required
        //ssdmf - not required
        //mcsis - not applicable, no link
        //nemepl - yes, for negative status response (match-confirm to match)
        //pecos - yes

        if (resultChangeType == Enumerations.ResultChangeType.ToNegativeResult)
        {
            switch (activityTypeID)
            {
                case (int)Enumerations.ScreeningActivityType.OIGLEIEVerification:
                case (int)Enumerations.ScreeningActivityType.NEMEPLVerification:
                case (int)Enumerations.ScreeningActivityType.MCSISVerification:
                case (int)Enumerations.ScreeningActivityType.PECOSVerfication:
                case (int)Enumerations.ScreeningActivityType.NPIVerification:
                case (int)Enumerations.ScreeningActivityType.LicenseVerification:
                case (int)Enumerations.ScreeningActivityType.MEDVerification:
                case (int)Enumerations.ScreeningActivityType.DODDAbuserRegistry:
                    isRequired = true;
                    break;
                default:
                    isRequired = false;
                    break;
            }
        }
        else if (resultChangeType == Enumerations.ResultChangeType.ToPositiveResult)
        {
            switch (activityTypeID)
            {
                case (int)Enumerations.ScreeningActivityType.SAVEVerification:
                case (int)Enumerations.ScreeningActivityType.PECOSVerfication:
                case (int)Enumerations.ScreeningActivityType.NPIVerification:
                case (int)Enumerations.ScreeningActivityType.LicenseVerification:
                case (int)Enumerations.ScreeningActivityType.NDENVerification:
                case (int)Enumerations.ScreeningActivityType.MEDVerification:
                case (int)Enumerations.ScreeningActivityType.DODDAbuserRegistry:
                    isRequired = true;
                    break;
                default:
                    isRequired = false;
                    break;
            }
        }

        return isRequired;
    }

    public static bool DocumentRequiredActivityStatus(int activityTypeID, Enumerations.ResultChangeType resultChangeType)
    {
        bool isRequired = false;

        if (resultChangeType == Enumerations.ResultChangeType.ToNegativeResult)
        {
            switch (activityTypeID)
            {
                case (int)Enumerations.ScreeningActivityType.OIGLEIEVerification:
                case (int)Enumerations.ScreeningActivityType.NEMEPLVerification:
                case (int)Enumerations.ScreeningActivityType.PECOSVerfication:
                case (int)Enumerations.ScreeningActivityType.SAVEVerification:
                case (int)Enumerations.ScreeningActivityType.NDENVerification:
                case (int)Enumerations.ScreeningActivityType.APSCPSVerification:
                case (int)Enumerations.ScreeningActivityType.ControlledSubstanceVerification:
                case (int)Enumerations.ScreeningActivityType.DEAVerification:
               // case (int)Enumerations.ScreeningActivityType.MEDVerification:
                    isRequired = true;
                    break;
                default:
                    isRequired = false;
                    break;
            }
        }
        else if (resultChangeType == Enumerations.ResultChangeType.ToPositiveResult)
        {
            switch (activityTypeID)
            {
                case (int)Enumerations.ScreeningActivityType.SAVEVerification:
                case (int)Enumerations.ScreeningActivityType.PECOSVerfication:
                case (int)Enumerations.ScreeningActivityType.LicenseVerification:
                case (int)Enumerations.ScreeningActivityType.NPIVerification:
                case (int)Enumerations.ScreeningActivityType.NDENVerification:
                case (int)Enumerations.ScreeningActivityType.APSCPSVerification:
                case (int)Enumerations.ScreeningActivityType.ControlledSubstanceVerification:
                case (int)Enumerations.ScreeningActivityType.DEAVerification:
                case (int)Enumerations.ScreeningActivityType.DODDAbuserRegistry:
               // case (int)Enumerations.ScreeningActivityType.MEDVerification:
                    isRequired = true;
                    break;
                default:
                    isRequired = false;
                    break;
            }
        }

        return isRequired;
    }


    public static bool CommentsRequiredActivityStatus(int activityTypeID, int activityStatusID)
    {
        //nppes - no
        //lei - yes, for negative status response (match-confirm to match)
        //sam - no
        //ssdmf - no
        //mcsis - yes, for negative status response (match-confirm to match)
        //nemepl - yes, for negative status response (match-confirm to match)
        bool isRequired = false;

        if (activityStatusID == CON.ScreeningActivityStatusId.Match)
        {
            switch (activityStatusID)
            {
                case (int)Enumerations.ScreeningActivityType.OIGLEIEVerification:
                case (int)Enumerations.ScreeningActivityType.NEMEPLVerification:
                case (int)Enumerations.ScreeningActivityType.MCSISVerification:
                    isRequired = true;
                    break;
                default:
                    isRequired = false;
                    break;
            }
        }
        else if (activityStatusID == CON.ScreeningActivityStatusId.NoMatch)
        {
            switch (activityStatusID)
            {
                case (int)Enumerations.ScreeningActivityType.SAVEVerification:
                    isRequired = true;
                    break;
                default:
                    isRequired = false;
                    break;
            }
        }
        else if (activityStatusID == CON.ScreeningActivityStatusId.Failed)
        {
            switch (activityStatusID)
            {
                case (int)Enumerations.ScreeningActivityType.LicenseVerification:
                    isRequired = true;
                    break;
                default:
                    isRequired = false;
                    break;
            }
        }

        return isRequired;
    }
    public static bool ShowDynamicResultDetails(Enumerations.ScreeningActivityType activityType)
    {
        bool showDynamic = false;

        switch (activityType)
        {
            case Enumerations.ScreeningActivityType.OIGLEIEVerification:
            case Enumerations.ScreeningActivityType.SAMVerification:
            case Enumerations.ScreeningActivityType.SSDMFVerification:
            case Enumerations.ScreeningActivityType.NPIVerification:
            case Enumerations.ScreeningActivityType.NEMEPLVerification:
            case Enumerations.ScreeningActivityType.MCSISVerification:
            case Enumerations.ScreeningActivityType.LicenseVerification:
            case Enumerations.ScreeningActivityType.PECOSVerfication:
            case Enumerations.ScreeningActivityType.SAVEVerification:
            case Enumerations.ScreeningActivityType.SexOffenderVerification:
            case Enumerations.ScreeningActivityType.NDENVerification:
            case Enumerations.ScreeningActivityType.APSCPSVerification:
            case Enumerations.ScreeningActivityType.DEAVerification:
            case Enumerations.ScreeningActivityType.ControlledSubstanceVerification:
            case Enumerations.ScreeningActivityType.CriminalBackgroundCheck:
            case Enumerations.ScreeningActivityType.SiteVisitVerification:
            case Enumerations.ScreeningActivityType.MEDVerification:
            case Enumerations.ScreeningActivityType.DODDAbuserRegistry:
            case Enumerations.ScreeningActivityType.OHMedExclSuspension:
                showDynamic = true;
				break;
			default:
                showDynamic = false;
				break;
        }

        return showDynamic;
    }


    public static bool IsPendingActivityStatus(int activityStatusID)
    {
        bool IsPendingStatus = false;

        switch (activityStatusID)
        {
            case CON.ScreeningActivityStatusId.NoMatch:
            case CON.ScreeningActivityStatusId.Verified:
            case CON.ScreeningActivityStatusId.Failed:
            case CON.ScreeningActivityStatusId.Match:
                IsPendingStatus = false;
                break;
            default:
                IsPendingStatus = true;
                break;
        }

        return IsPendingStatus;
    }


    public static bool IsMatchPendingActivityStatus(int activityStatusID)
    {
        bool IsPendingStatus = false;

        switch (activityStatusID)
        {
            case CON.ScreeningActivityStatusId.MatchConfirm:
            case CON.ScreeningActivityStatusId.Match:
                IsPendingStatus = true;
                break;
            default:
                IsPendingStatus = false;
                break;
        }

        return IsPendingStatus;
    }

    public static bool ScreeningActivityIsPostScreeningStep(int activityTypeID)
    {
        bool isPostScreeningStep = false;

        if (!Enumerations.ScreeningActivityType.IsDefined(typeof(Enumerations.ScreeningActivityType), activityTypeID))
        {
            return false;
        }

        Enumerations.ScreeningActivityType activityType = (Enumerations.ScreeningActivityType)activityTypeID;

        switch (activityType)
        {
            case Enumerations.ScreeningActivityType.SiteVisit:
            case Enumerations.ScreeningActivityType.MedicaidIDAssigned:
            case Enumerations.ScreeningActivityType.WelcomeLetter:
                isPostScreeningStep = true;
				break;
			default:
                isPostScreeningStep = false;
				break;
        }

        return isPostScreeningStep;
    }
}