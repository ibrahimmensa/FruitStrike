using System;

namespace PollfishUnity
{
	class Utils
	{
		public static SurveyInfo GetSurveyInfoFromString(string surveyInfoString)
		{
			string[] surveyCharacteristics = surveyInfoString.Split(',');

			if (surveyCharacteristics.Length < 6)
			{
				return null;
			}

			int surveyCPA;

			try
			{
				surveyCPA = int.Parse(surveyCharacteristics[0]);
			}
			catch (Exception)
			{
				surveyCPA = -1;
			}

			int surveyIR;

			try
			{
				surveyIR = int.Parse(surveyCharacteristics[1]);
			}
			catch (Exception)
			{
				surveyIR = -1;
			}

			int surveyLOI;

			try
			{
				surveyLOI = int.Parse(surveyCharacteristics[2]);
			}
			catch (Exception)
			{
				surveyLOI = -1;
			}

			int rewardValue;

			try
			{
				rewardValue = int.Parse(surveyCharacteristics[5]);
			}
			catch (Exception)
			{
				rewardValue = -1;
			}

			return new SurveyInfo(
				surveyCPA,
				surveyIR,
				surveyLOI,
				surveyCharacteristics[3],
				surveyCharacteristics[4],
				rewardValue);
		}
	}
}