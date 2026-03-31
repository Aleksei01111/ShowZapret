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
                if (wordAnalysisResult.Verdict != WordAnalysisResult.VerdictType.Clean && wordAnalysisResult.ViolationRule != null)
                {
                    res.finalFreedomPunishmentInMonth += wordAnalysisResult.ViolationRule.FreedomPunishInMonth;
                    res.finalMoneyPunishment += wordAnalysisResult.ViolationRule.MoneyPunishmentInRubles;
                }
            }
        }

        if (res.finalFreedomPunishmentInMonth < 0)
            res.finalFreedomPunishmentInMonth = 0;
        if(res.finalMoneyPunishment < 0)
            res.finalMoneyPunishment = 0;
        
        return res;
    }
}