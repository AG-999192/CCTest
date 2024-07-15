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

        public async Task AddData(List<ParsedDataDto> ParsedData, DBStatusDto status)
        {
            SqlConnection conn = new SqlConnection(DefaultConnection());

            // Use Batching for storing

            String query = "INSERT INTO dbo.C_C_Test_Data (MPAN, MeterSerial,DateOfInstallation,AddressLine1,PostCode) VALUES (@MPAN, @MeterSerial,@DateOfInstallation,@AddressLine1,@PostCode)";
            conn.Open();

            foreach (var data in ParsedData)
            {
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@MPAN", data.MPAN.ToString());
                    command.Parameters.AddWithValue("@MeterSerial", data.MeterSerial);
                    command.Parameters.AddWithValue("@DateOfInstallation", data.DateOfInstallation);
                    command.Parameters.AddWithValue("@AddressLine1", data.AddressLine ?? string.Empty);
                    command.Parameters.AddWithValue("@PostCode", data.PostCode ?? string.Empty);

                    int result = await command.ExecuteNonQueryAsync();

                    // Check Error
                    if (result < 0)
                    {
                        this.logger.LogError("Error inserting data into Database");
                        status.QueryStatus = "Error inserting data into Database";
                        status.FailedWrites++;
                    }
                    else
                    {
                        status.SuccessfulWrites++;
                    }

                }
            }
            conn.Close();

            if( string.IsNullOrEmpty(status.QueryStatus))
            {
                status.QueryStatus = "OK";
            }

            return;
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
