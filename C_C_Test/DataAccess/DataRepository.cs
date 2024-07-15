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

        public async Task<DBStatusDto> AddData(List<ParsedDataDto> ParsedData)
        {
            var ret = new DBStatusDto();
            ret.QueryStatus = "OK";

            SqlConnection conn = new SqlConnection(DefaultConnection());

            // Use Batching for storing

            String query = "INSERT INTO dbo.C_C_Test_Data (MPAN, MeterSerial,DateOfInstallation,AddressLine1,PostCode) VALUES (@MPAN, @MeterSerial,@DateOfInstallation,@AddressLine1,@PostCode)";

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@MPAN", "1012345687466");
                command.Parameters.AddWithValue("@MeterSerial", "A00E355698");
                command.Parameters.AddWithValue("@DateOfInstallation", "19970801");
                command.Parameters.AddWithValue("@AddressLine1", "ROSE YARD");
                command.Parameters.AddWithValue("@PostCode", "NE99P66");

                conn.Open();
                int result = await command.ExecuteNonQueryAsync();

                // Check Error
                if (result < 0)
                {
                    this.logger.LogError("Error inserting data into Database");
                    ret.QueryStatus = "Error inserting data into Database";
                }
            }
            conn.Close();

            return ret;
        }

        public async Task<DBStatusDto> AddData(ParsedDataDto ParsedData)
        {
            var ret = new DBStatusDto();
            ret.QueryStatus = "OK";

            SqlConnection conn = new SqlConnection(DefaultConnection());

            // Use Batching for storing

            String query = "INSERT INTO dbo.C_C_Test_Data (MPAN, MeterSerial,DateOfInstallation,AddressLine1,PostCode) VALUES (@MPAN, @MeterSerial,@DateOfInstallation,@AddressLine1,@PostCode)";

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@MPAN", ParsedData.MPAN.ToString());
                command.Parameters.AddWithValue("@MeterSerial", ParsedData.MeterSerial);
                command.Parameters.AddWithValue("@DateOfInstallation", ParsedData.DateOfInstallation);
                command.Parameters.AddWithValue("@AddressLine1", ParsedData.AddressLine ?? string.Empty);
                command.Parameters.AddWithValue("@PostCode", ParsedData.PostCode ?? string.Empty);

                conn.Open();
                int result = await command.ExecuteNonQueryAsync();

                // Check Error
                if (result < 0)
                {
                    this.logger.LogError("Error inserting data into Database");
                    ret.QueryStatus = "Error inserting data into Database";
                }

                ret.SuccessfulWrites = result;
            }
            conn.Close();

            return ret;
        }

        public async Task<List<RetrievedData>> GetData()
        {
            var retData = new List<RetrievedData>();

            SqlConnection conn = new SqlConnection(DefaultConnection());
            conn.Open();
            string querty = "SELECT MPAN, MeterSerial,DateOfInstallation,AddressLine1,PostCode FROM dbo.C_C_Test_Data";

            using (SqlCommand command = new SqlCommand(querty, conn))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
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

            conn.Close();

            return retData;
        }



        private string DefaultConnection()
        {
            return this.configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}
