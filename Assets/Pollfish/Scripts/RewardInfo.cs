public class RewardInfo
{

    public string rewardName { get; }
    public double rewardConversion { get; }

    public RewardInfo(string rewardName, double rewardConversion)
    {
        this.rewardName = rewardName;
        this.rewardConversion = rewardConversion;
    }

}