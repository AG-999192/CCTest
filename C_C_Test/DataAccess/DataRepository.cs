using C_C_Test.Dtos;
using Microsoft.Data.SqlClient;

namespace C_C_Test.DataAccess
{
    public class DataRepository : IDataRepository
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        private readonly IConfiguration configuration;


        /// <summary>
        /// Initializes a new instance of the <see cref="GarageRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">The context.</param>
        public DataRepository(ILogger<DataRepository> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public void AddData(List<ParsedDataDto> ParsedData)
        {
            SqlConnection conn = new SqlConnection(DefaultConnection());


        }

        public List<RetrievedData> GetData()
        {
            var retData = new List<RetrievedData>();

            SqlConnection conn = new SqlConnection(DefaultConnection());
            conn.Open();
            string querty = "SELECT MPAN, MeterSerial,DateOfInstallation,AddressLine1,PostCode FROM dbo.C_C_Test_Data";

            using (SqlCommand command = new SqlCommand(querty, conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retData.Add(new RetrievedData { 
                            MPAN = reader.GetDecimal(0), 
                            MeterSerial = reader.GetString(1), 
                            DateOfInstallation = reader.GetDateTime(2).ToString(),
                            AddressLine = reader.GetString(3) , 
                            PostCode = reader.GetString(4)
                        });
                    }
                }
            }

            return retData;
        }



        private string DefaultConnection()
        {
            return this.configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}
