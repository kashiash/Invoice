using DevExpress.ExpressApp;
using Invoice.Module.ApiModels.VatPayers;
using Invoice.Module.ApiModels.VatPayers.Error;
using Invoice.Module.ApiModels.VatPayers.Responses;
using Invoice.Module.Extensions;
using Invoice.Module.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Services
{
    public class VatPayersService
    {
        private readonly string baseUrl;
        private readonly CustomHttpClient customHttpClient;

        public VatPayersService(string baseUrl = "https://wl-api.mf.gov.pl/api/")
        {
            customHttpClient = new CustomHttpClient();
            this.baseUrl = baseUrl;
        }

        public async Task<object> SearchVatPayers(List<string> numbers, SearchVatPayersBy searchVatPayersBy, DateTime date)
        {
            if (numbers.Count <= 0)
            {
                throw new UserFriendlyException("Please enter the number to search for VAT payers.");
            }

            var url = GetActionUrl(numbers, searchVatPayersBy);
            var queryParams = new Dictionary<string, string> { { "date", date.ToString("yyyy-MM-dd") } };
            var stringResponse = await customHttpClient.GetStringAsync(url, queryParams);

            return DeserializeVatPayersObject(numbers, stringResponse, searchVatPayersBy);
        }

        private string GetActionUrl(List<string> numbers, SearchVatPayersBy searchVatPayersBy)
        {
            var stringBuilder = new StringBuilder();

            string specificationName = searchVatPayersBy.ToString().ChangeUnderlineToDash().ToLower();

            stringBuilder.Append(numbers.FirstOrDefault());

            for (int i = 1; i < numbers.Count; i++)
            {
                stringBuilder.Append($",{numbers[i]}");
            }

            if (numbers.Count > 1)
            {
                specificationName += "s";
            }

            var specificationValue = stringBuilder.ToString();

            return $"{baseUrl}search/{specificationName}/{specificationValue}";
        }

        private object DeserializeVatPayersObject(List<string> numbers, string stringResponse, SearchVatPayersBy searchVatPayersBy)
        {
            object deserializedResponse;

            if (numbers.Count > 1)
            {
                deserializedResponse = JsonConvert.DeserializeObject<EntryListResponse>(stringResponse);
            }
            else if (searchVatPayersBy == SearchVatPayersBy.Bank_Account)
            {
                deserializedResponse = JsonConvert.DeserializeObject<EntityListResponse>(stringResponse);
            }
            else
            {
                deserializedResponse = JsonConvert.DeserializeObject<EntityResponse>(stringResponse);
            }

            deserializedResponse = ReturnExceptionIfResultIsNull(stringResponse, deserializedResponse);

            if (deserializedResponse.GetType() != typeof(ApiException))
            {
                MapAccountNumbersToBankAccountNumbers(deserializedResponse);
            }

            return deserializedResponse;
        }

        private void MapAccountNumbersToBankAccountNumbers(object response)
        {
            if (response is EntityListResponse entityListResponse)
            {
                MapEntityListResponse(entityListResponse);
            }
            else if (response is EntityResponse entityResponse)
            {
                MapEntityResponse(entityResponse);
            }
            else if (response is EntryListResponse entryListResponse)
            {
                MapEntryListResponse(entryListResponse);
            }
        }

        private void MapEntityListResponse(EntityListResponse entityListResponse)
        {
            if (entityListResponse.Result.Subjects != null && entityListResponse.Result.Subjects.Count != 0)
            {
                entityListResponse.Result.Subjects
                    .ForEach(subject => subject.AccountNumbers
                    .ForEach(accountNumber => subject.BankAccountNumbers.Add(new AccountNumber { Number = accountNumber })));
            }
        }

        private void MapEntityResponse(EntityResponse entityResponse)
        {
            if (entityResponse.Result.Subject != null)
            {
                var bankAccountNumbers = entityResponse.Result.Subject.BankAccountNumbers;

                entityResponse.Result.Subject.AccountNumbers
                    .ForEach(accountNumber => bankAccountNumbers.Add(new AccountNumber { Number = accountNumber }));
            }
        }

        private void MapEntryListResponse(EntryListResponse entryListResponse)
        {
            if (entryListResponse.Result.Entries != null && entryListResponse.Result.Entries.Count != 0)
            {
                entryListResponse.Result.Entries
                    .ForEach(entry =>
                    {
                        if (entry.Subjects != null && entry.Subjects.Count > 0)
                        {
                            entry.Subjects
                            .ForEach(subject => subject.AccountNumbers
                            .ForEach(accountNumber => subject.BankAccountNumbers.Add(new AccountNumber { Number = accountNumber })));
                        }
                    });
            }
        }

        private static object ReturnExceptionIfResultIsNull(string stringResponse, object deserializedResponse)
        {
            if (deserializedResponse.GetType().GetProperty("Result").GetValue(deserializedResponse) == null)
            {
                deserializedResponse = JsonConvert.DeserializeObject<ApiException>(stringResponse);
            }

            return deserializedResponse;
        }
    }
}