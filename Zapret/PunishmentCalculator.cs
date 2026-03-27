using DB.Entities;

namespace Zapret;

public class PunishmentCalculator(List<List<WordAnalysisResult>> words)
{
    private List<List<WordAnalysisResult>> _words = words;

    public (double finalFreedomPunishmentInMonth, double finalMoneyPunishmentInRubles) GetPunishments(
        WordAnalysisResult.VerdictType verdictFilter = WordAnalysisResult.VerdictType.Violation)
    {
        var res = (finalFreedomPunishmentInMonth: 0.0, finalMoneyPunishment: 0.0);
        foreach (var wordAnalysisResultSentence in _words)
        {
            foreach (var wordAnalysisResult in wordAnalysisResultSentence)
            {
                if (wordAnalysisResult.Verdict == verdictFilter && wordAnalysisResult.ViolationRule != null)
                {
                    res.finalFreedomPunishmentInMonth += wordAnalysisResult.ViolationRule.FreedomPunishInMonth;
                    res.finalMoneyPunishment += wordAnalysisResult.ViolationRule.MoneyPunishmentInRubles;
                }
            }
        }

        return res;
    }
}