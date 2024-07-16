using C_C_Test.Conversions;
using C_C_Test.DataAccess;
using C_C_Test.Dtos;
using C_C_Test.FileIO;
using C_C_Test.Models;
using C_C_Test.Queries;
using C_C_Test.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    public class ServerTests
    {
        private readonly Mock<IMediator> mediatorMock = new();


        [SetUp]
        public void SetupForEveryTest()
        {
            mediatorMock.Reset();
        }

        [Test]
        public async Task GetDataQueryHandler_Success()
        {
            Mock<IDataRepository> dataRepositoryMock = new Mock<IDataRepository>();

            Mock <ILogger<GetDataQueryHandler>> loggerMock = new();

            Mock<IConversion> conversionMock = new Mock<IConversion>();

            List<RetrievedDataDto> repoData = new List<RetrievedDataDto> { new RetrievedDataDto { MPAN = 12345678910,
                    AddressLine = "Address line",
                    DateOfInstallation = "20101010",
                    MeterSerial = "12345",
                    PostCode = "NE1 1PT" } };

            dataRepositoryMock
               .Setup(method => method.GetData())
               .ReturnsAsync(repoData);

            List<DataViewModel> viewDatabase = new List<DataViewModel>() { new DataViewModel {  
                    MPAN = 12345678910,
                    AddressLine = "Address line",
                    DateOfInstallation = "20101010",
                    MeterSerial = "12345",
                    PostCode = "NE1 1PT" } };

            conversionMock
            .Setup(method => method.MapRetrievedDataToDataView(It.IsAny<List<RetrievedDataDto>>()))
            .Returns(viewDatabase);

            var sut = new GetDataQueryHandler(dataRepositoryMock.Object, loggerMock.Object, conversionMock.Object);

            var query = new GetDataQuery()
            {
            };

            var result = await sut.Handle(query, new CancellationToken());

            Assert.IsNotNull(result);

            Assert.AreEqual(viewDatabase.Count, result.Count);
            Assert.AreEqual(viewDatabase[0].AddressLine, result[0].AddressLine);

            dataRepositoryMock.Verify(method => method.GetData(), Times.Once);
            conversionMock.Verify(method => method.MapRetrievedDataToDataView(It.IsAny<List<RetrievedDataDto>>()), Times.Once);
        }

        [Test]
        public async Task UpdateDatabaseQueryHandler_Success()
        {
            Mock<IDataRepository> dataRepositoryMock = new Mock<IDataRepository>();

            Mock<ILogger<UpdateDatabaseQueryHandler>> loggerMock = new();

            Mock<IFileParsing> fileParsingMock = new Mock<IFileParsing>();

            List<RetrievedDataDto> repoData = new List<RetrievedDataDto> { new RetrievedDataDto { MPAN = 12345678910,
                    AddressLine = "Address line",
                    DateOfInstallation = "20101010",
                    MeterSerial = "12345",
                    PostCode = "NE1 1PT" } };

            List<ParsedDataDto> viewDatabase = new();
            DBStatusDto status = new DBStatusDto() {  SuccessfulWrites = 99 };

            dataRepositoryMock
               .Setup(method => method.AddData(viewDatabase, status));

            fileParsingMock
            .Setup(method => method.ParseFile())
            .ReturnsAsync(viewDatabase);

            var sut = new UpdateDatabaseQueryHandler(dataRepositoryMock.Object, loggerMock.Object, fileParsingMock.Object);

            var query = new UpdateDatabaseQuery()
            {
            };

            var result = await sut.Handle(query, new CancellationToken());

            Assert.IsNotNull(result);

            dataRepositoryMock.Verify(method => method.AddData(It.IsAny<List<ParsedDataDto>>(), It.IsAny<DBStatusDto>()), Times.Once);
            fileParsingMock.Verify(method => method.ParseFile(),Times.Once);
        }

        [Test]
        public async Task ValidateDataQueryHandler_Success()
        {
            Mock<IConversion> conversionMock = new Mock<IConversion>();

            Mock<ILogger<ValidateDataQueryHandler>> loggerMock = new();

            Mock<IFileParsing> fileParsingMock = new Mock<IFileParsing>();

            List<RetrievedDataDto> repoData = new List<RetrievedDataDto> { new RetrievedDataDto { MPAN = 12345678910,
                    AddressLine = "Address line",
                    DateOfInstallation = "20101010",
                    MeterSerial = "12345",
                    PostCode = "NE1 1PT" } };

            List<ParsedDataDto> viewDatabase = new();
            DBStatusDto status = new DBStatusDto() { SuccessfulWrites = 99 };

            ValidationViewModel validationViewModel = new ValidationViewModel();

            fileParsingMock
            .Setup(method => method.ParseFile(validationViewModel))
            .ReturnsAsync(viewDatabase);

            var sut = new ValidateDataQueryHandler(fileParsingMock.Object, loggerMock.Object, conversionMock.Object);

            var query = new ValidateDataQuery()
            {
            };

            var result = await sut.Handle(query, new CancellationToken());

            Assert.IsNotNull(result);

            fileParsingMock.Verify(method => method.ParseFile(It.IsAny<ValidationViewModel>()), Times.Once);
        }
    }
}
