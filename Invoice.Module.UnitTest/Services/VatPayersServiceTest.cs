using DevExpress.ExpressApp;
using FluentAssertions;
using Invoice.Module.ApiModels.VatPayers.Responses;
using Invoice.Module.Services;
using Invoice.Module.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.UnitTest.Services
{
    public class VatPayersServiceTest
    {
        private VatPayersService vatPayersService;

        [SetUp]
        public void Setup()
        {
            vatPayersService = new VatPayersService("https://wl-test.mf.gov.pl/api/");
        }


        [Test]
        public void ShouldThrowException()
        {
            Func<Task> action = async () => await vatPayersService.SearchVatPayers(null, SearchVatPayersBy.Bank_Account, DateTime.Today);
            action.Should().ThrowAsync<UserFriendlyException>();
        }

        [Test]
        public async Task ShouldReturnEntityListResponseByBankAccount()
        {
            List<string> numbers = new() { "49635393242218223941769740" };

            var response = await vatPayersService.SearchVatPayers(numbers, SearchVatPayersBy.Bank_Account, DateTime.Today);
            response.Should().BeOfType(typeof(EntityListResponse));

            var entityListResponse = response as EntityListResponse;
            entityListResponse.Result.Should().NotBeNull();

            var subject = entityListResponse.Result.Subjects.FirstOrDefault();
            subject.Should().NotBeNull();
            subject.Pesel.Should().Be("12341201012");
            subject.Nip.Should().Be("6735593109");
            subject.Regon.Should().Be("26748477255870");
            subject.AccountNumbers.Count.Should().Be(14);
        }

        [Test]
        public async Task ShouldReturnEntryListResponseByBankAccount()
        {
            List<string> numbers = new() { "49635393242218223941769740", "70506405335016096312945164" };

            var response = await vatPayersService.SearchVatPayers(numbers, SearchVatPayersBy.Bank_Account, DateTime.Today);
            response.Should().BeOfType(typeof(EntryListResponse));

            var entryListResponse = response as EntryListResponse;
            entryListResponse.Result.Should().NotBeNull();

            var entry = entryListResponse.Result.Entries.FirstOrDefault();
            entry.Should().NotBeNull();

            var subject = entry.Subjects.FirstOrDefault();
            subject.Should().NotBeNull();
            subject.Pesel.Should().Be("12341201012");
            subject.Nip.Should().Be("6735593109");
            subject.Regon.Should().Be("26748477255870");
            subject.AccountNumbers.Count.Should().Be(14);
        }

        [Test]
        public async Task ShouldReturnEntityResponseByNip()
        {
            List<string> numbers = new() { "6735593109" };

            var response = await vatPayersService.SearchVatPayers(numbers, SearchVatPayersBy.Nip, DateTime.Today);
            response.Should().BeOfType(typeof(EntityResponse));

            var entityResponse = response as EntityResponse;
            entityResponse.Result.Should().NotBeNull();

            var subject = entityResponse.Result.Subject;
            subject.Should().NotBeNull();
            subject.Pesel.Should().Be("12341201012");
            subject.Nip.Should().Be("6735593109");
            subject.Regon.Should().Be("26748477255870");
            subject.AccountNumbers.Count.Should().Be(14);
        }

        [Test]
        public async Task ShouldReturnEntityResponseByRegon()
        {
            List<string> numbers = new() { "26748477255870" };

            var response = await vatPayersService.SearchVatPayers(numbers, SearchVatPayersBy.Regon, DateTime.Today);
            response.Should().BeOfType(typeof(EntityResponse));

            var entityResponse = response as EntityResponse;
            entityResponse.Result.Should().NotBeNull();

            var subject = entityResponse.Result.Subject;
            subject.Should().NotBeNull();
            subject.Pesel.Should().Be("12341201012");
            subject.Nip.Should().Be("6735593109");
            subject.Regon.Should().Be("26748477255870");
            subject.AccountNumbers.Count.Should().Be(14);
        }
    }
}
