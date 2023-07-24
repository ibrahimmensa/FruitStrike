
public class SurveyInfo
{
    public string SurveyClass { get; }
    public int SurveyCPA { get; }
    public int SurveyIR { get; }
    public int SurveyLOI { get; }
    public string RewardName { get; }
    public int RewardValue { get; }

    public SurveyInfo(int surveyCPA, int surveyIR, int surveyLOI, string surveyClass, string rewardName, int rewardValue)
    {
        this.SurveyClass = surveyClass;
        this.SurveyCPA = surveyCPA;
        this.SurveyIR = surveyIR;
        this.SurveyLOI = surveyLOI;
        this.RewardName = rewardName;
        this.RewardValue = rewardValue;
    }

    public override string ToString()
    {
        return "Survey Info: \n" +
            "\tReward Name: " + RewardName +
            "\tReward Value: " + RewardValue +
            "\tSurvey CPA: " + SurveyCPA +
            "\tSurvey IR: " + SurveyIR +
            "\tSurvey LOI: " + SurveyLOI +
            "\tSurvey Class: " + SurveyClass;
    }

}
