using C_C_Test.Common;
using C_C_Test.Models;
using C_C_Test.Pages;
using C_C_Test.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    public class PageModelsTests
    {
        private readonly Mock<IMediator> mediatorMock = new();
        

        [SetUp]
        public void SetupForEveryTest()
        {
            mediatorMock.Reset();
        }

        [Test]
        public async Task OnGet_AllData()
        {
            Mock<ILogger<ViewDatabaseModel>> loggerMock = new();

           var data = new PaginatedList<DataViewModel>()
            {
                new DataViewModel()
                {
                    MPAN = 12345678910,
                    AddressLine = "Address line",
                    DateOfInstallation = "20101010",
                    MeterSerial = "12345",
                    PostCode = "NE1 1PT"

                },
            };

            mediatorMock
                .Setup(method => method.Send(It.IsAny<GetDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(data);

            var details = new ViewDatabaseModel(loggerMock.Object, mediatorMock.Object);

            var result = await details.OnGet();

            Assert.IsNotNull(result);
            Assert.AreEqual(data.Count, details.ViewDatabase.Count);
            mediatorMock.Verify(method => method.Send(It.IsAny<GetDataQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task OnGet_ValidateData()
        {
            Mock<ILogger<ValidateDataModel>> loggerMock = new();

            var data = new ValidationViewModel() { RejectedRows = 0, SuccessfulRows = 99, ValidationStatus = "OK", RejectedRowsList = new List<string>() { "1012345734381||20030724|" } };
            

            mediatorMock
                .Setup(method => method.Send(It.IsAny<ValidateDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(data);

            var details = new ValidateDataModel(loggerMock.Object, mediatorMock.Object);

            var result = await details.OnGet();

            Assert.IsNotNull(result);
            Assert.AreEqual(data.SuccessfulRows, details.ValidationView.SuccessfulRows);
            mediatorMock.Verify(method => method.Send(It.IsAny<ValidateDataQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task OnGet_UploadFile()
        {
            Mock<ILogger<UploadFileModel>> loggerMock = new();

            var data = new DatabaseStatusModel() {  FailedWrites = 0, SuccessfulWrites = 99, QueryStatus = "OK" };


            mediatorMock
                .Setup(method => method.Send(It.IsAny<UpdateDatabaseQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(data);

            var details = new UploadFileModel(loggerMock.Object, mediatorMock.Object);

            var result = await details.OnGet();

            Assert.IsNotNull(result);
            Assert.AreEqual(data.SuccessfulWrites, details.DBStatusView.SuccessfulWrites);
            mediatorMock.Verify(method => method.Send(It.IsAny<UpdateDatabaseQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
