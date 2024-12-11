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
        [JsonProperty(PropertyName = "accountGroupClasses")]
        public List<AccountGroupClass> AccountGroupClasses { get; set; }

        [JsonProperty(PropertyName = "openingBalanceActiveOverall")]
        public decimal OpeningBalanceActiveOverall => AccountGroupClasses.Sum(ag => ag.OpeningBalanceActiveOverall);
        [JsonProperty(PropertyName = "openingBalancePassiveOverall")]
        public decimal OpeningBalancePassiveOverall => AccountGroupClasses.Sum(ab => ab.OpeningBalancePassiveOverall);
        [JsonProperty(PropertyName = "turnoverDebitOverall")]
        public decimal TurnoverDebitOverall => AccountGroupClasses.Sum(ab => ab.TurnoverDebitOverall);
        [JsonProperty(PropertyName = "turnoverCreditOverall")]
        public decimal TurnoverCreditOverall => AccountGroupClasses.Sum(ab => ab.TurnoverCreditOverall);
        [JsonProperty(PropertyName = "closingBalanceActiveOverall")]
        public decimal ClosingBalanceActiveOverall => AccountGroupClasses.Sum(ab => ab.ClosingBalanceActiveOverall);
        [JsonProperty(PropertyName = "closingBalancePassiveOverall")]
        public decimal ClosingBalancePassiveOverall => AccountGroupClasses.Sum(ab => ab.ClosingBalancePassiveOverall);
    }

    public class AccountGroupClass
    {
        [JsonProperty(PropertyName = "classNumber")]
        public string ClassNumber { get; set; }
        [JsonProperty(PropertyName = "className")]
        public string ClassName { get; set; }

        [JsonProperty(PropertyName = "accountGroups")]
        public List<AccountGroup> AccountGroups { get; set; }

        [JsonProperty(PropertyName = "openingBalanceActiveClassOverall")]
        public decimal OpeningBalanceActiveOverall => AccountGroups.Sum(ag => ag.OpeningBalanceActiveOverall);
        [JsonProperty(PropertyName = "openingBalancePassiveClassOverall")]
        public decimal OpeningBalancePassiveOverall => AccountGroups.Sum(ab => ab.OpeningBalancePassiveOverall);
        [JsonProperty(PropertyName = "turnoverDebitClassOverall")]
        public decimal TurnoverDebitOverall => AccountGroups.Sum(ab => ab.TurnoverDebitOverall);
        [JsonProperty(PropertyName = "turnoverCreditClassOverall")]
        public decimal TurnoverCreditOverall => AccountGroups.Sum(ab => ab.TurnoverCreditOverall);
        [JsonProperty(PropertyName = "closingBalanceActiveClassOverall")]
        public decimal ClosingBalanceActiveOverall => AccountGroups.Sum(ab => ab.ClosingBalanceActiveOverall);
        [JsonProperty(PropertyName = "closingBalancePassiveClassOverall")]
        public decimal ClosingBalancePassiveOverall => AccountGroups.Sum(ab => ab.ClosingBalancePassiveOverall);
    }

    public class AccountGroup
    {
        [JsonProperty(PropertyName = "groupNumber")]
        public string GroupNumber { get; set; }
        [JsonProperty(PropertyName = "accountBalances")]
        public List<AccountBalance> AccountBalances { get; set; }


        [JsonProperty(PropertyName = "openingBalanceActiveOverall")]
        public decimal OpeningBalanceActiveOverall => AccountBalances.Sum(ab => ab.OpeningBalanceActive);
        [JsonProperty(PropertyName = "openingBalancePassiveOverall")]
        public decimal OpeningBalancePassiveOverall => AccountBalances.Sum(ab => ab.OpeningBalancePassive);
        [JsonProperty(PropertyName = "turnoverDebitOverall")]
        public decimal TurnoverDebitOverall => AccountBalances.Sum(ab => ab.TurnoverDebit);
        [JsonProperty(PropertyName = "turnoverCreditOverall")]
        public decimal TurnoverCreditOverall => AccountBalances.Sum(ab => ab.TurnoverCredit);
        [JsonProperty(PropertyName = "closingBalanceActiveOverall")]
        public decimal ClosingBalanceActiveOverall => AccountBalances.Sum(ab => ab.ClosingBalanceActive);
        [JsonProperty(PropertyName = "closingBalancePassiveOverall")]
        public decimal ClosingBalancePassiveOverall => AccountBalances.Sum(ab => ab.ClosingBalancePassive);
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
