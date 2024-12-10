using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_2.Models.ViewModels
{
    public class FileContent
    {
        [JsonProperty(PropertyName = "fileId")]
        public int FileId { get; set; }
        [JsonProperty(PropertyName = "organisationName")]
        public string OrganisationName { get; set; }
        [JsonProperty(PropertyName = "periodStart")]
        public DateOnly PeriodStart { get; set; }
        [JsonProperty(PropertyName = "periodEnd")]
        public DateOnly PeriodEnd { get; set; }
        [JsonProperty(PropertyName = "dataTime")]
        public DateTime DataTime { get; set; }
        [JsonProperty(PropertyName = "uploaded")]
        public DateTime Uploaded { get; set; }
        [JsonProperty(PropertyName = "currencySymbol")]
        public string CurrencySymbol { get; set; }
        [JsonProperty(PropertyName = "accountGroups")]
        public List<AccountGroup> AccountGroups { get; set; }

        [JsonProperty(PropertyName = "openingBalanceActiveOverall")]
        public decimal OpeningBalanceActiveOverall => AccountGroups.Sum(ag => ag.OpeningBalanceActiveGroupOverall);
        [JsonProperty(PropertyName = "openingBalancePassiveOverall")]
        public decimal OpeningBalancePassiveOverall => AccountGroups.Sum(ab => ab.OpeningBalancePassiveGroupOverall);
        [JsonProperty(PropertyName = "turnoverDebitOverall")]
        public decimal TurnoverDebitOverall => AccountGroups.Sum(ab => ab.TurnoverDebitGroupOverall);
        [JsonProperty(PropertyName = "turnoverCreditOverall")]
        public decimal TurnoverCreditOverall => AccountGroups.Sum(ab => ab.TurnoverCreditGroupOverall);
        [JsonProperty(PropertyName = "closingBalanceActiveOverall")]
        public decimal ClosingBalanceActiveOverall => AccountGroups.Sum(ab => ab.ClosingBalanceActiveGroupOverall);
        [JsonProperty(PropertyName = "closingBalancePassiveOverall")]
        public decimal ClosingBalancePassiveOverall => AccountGroups.Sum(ab => ab.ClosingBalancePassiveGroupOverall);
    }

    public class AccountGroup
    {
        [JsonProperty(PropertyName = "groupNumber")]
        public string GroupNumber { get; set; }
        [JsonProperty(PropertyName = "accountBalances")]
        public List<AccountBalance> AccountBalances { get; set; }


        [JsonProperty(PropertyName = "openingBalanceActiveGroupOverall")]
        public decimal OpeningBalanceActiveGroupOverall => AccountBalances.Sum(ab => ab.OpeningBalanceActive);
        [JsonProperty(PropertyName = "openingBalancePassiveGroupOverall")]
        public decimal OpeningBalancePassiveGroupOverall => AccountBalances.Sum(ab => ab.OpeningBalancePassive);
        [JsonProperty(PropertyName = "turnoverDebitGroupOverall")]
        public decimal TurnoverDebitGroupOverall => AccountBalances.Sum(ab => ab.TurnoverDebit);
        [JsonProperty(PropertyName = "turnoverCreditGroupOverall")]
        public decimal TurnoverCreditGroupOverall => AccountBalances.Sum(ab => ab.TurnoverCredit);
        [JsonProperty(PropertyName = "closingBalanceActiveGroupOverall")]
        public decimal ClosingBalanceActiveGroupOverall => AccountBalances.Sum(ab => ab.ClosingBalanceActive);
        [JsonProperty(PropertyName = "closingBalancePassiveGroupOverall")]
        public decimal ClosingBalancePassiveGroupOverall => AccountBalances.Sum(ab => ab.ClosingBalancePassive);
    }

    public class AccountBalance
    {
        [JsonProperty(PropertyName = "accountId")]
        public int AccountId { get; set; }
        [JsonProperty(PropertyName = "accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty(PropertyName = "openingBalanceActive")]
        public decimal OpeningBalanceActive { get; set; }
        [JsonProperty(PropertyName = "openingBalancePassive")]
        public decimal OpeningBalancePassive { get; set; }
        [JsonProperty(PropertyName = "turnoverDebit")]
        public decimal TurnoverDebit { get; set; }
        [JsonProperty(PropertyName = "turnoverCredit")]
        public decimal TurnoverCredit { get; set; }
        [JsonProperty(PropertyName = "closingBalanceActive")]
        public decimal ClosingBalanceActive { get; set; }
        [JsonProperty(PropertyName = "closingBalancePassive")]
        public decimal ClosingBalancePassive { get; set; }
    }
}
