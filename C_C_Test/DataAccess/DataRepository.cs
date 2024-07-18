using C_C_Test.Dtos;
using C_C_Test.Emuns;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Globalization;

namespace C_C_Test.DataAccess
{
    /// <summary>
    /// The implementation of the DataRepository.
    /// </summary>
    public class DataRepository : IDataRepository
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The config.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public DataRepository(ILogger<DataRepository> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        /// <summary>
        /// Add data to the databse using list.
        /// </summary>
        /// <param name="ParsedData"></param>
        /// <param name="status"></param>
        /// <returns>Task</returns>
        public async Task AddData(List<ParsedDataDto> ParsedData, DBStatusDto status)
        {
            try
            {
                SqlConnection conn = new SqlConnection(DefaultConnection());

                // TODO use batching for storing. SqlBulkCopy? BULK INSERT with .csv? TRANSACTION?';

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

                if (string.IsNullOrEmpty(status.QueryStatus))
                {
                    status.QueryStatus = "OK";
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception thrown accessing Database {0}", ex.Message);
            }

            return;
        }

        /// <summary>
        /// Add data to database using single record.
        /// </summary>
        /// <param name="ParsedData"></param>
        /// <returns>DBStatusDto</returns>
        public async Task<DBStatusDto> AddData(ParsedDataDto ParsedData)
        {
            var ret = new DBStatusDto();

            try
            {
                ret.QueryStatus = "OK";

                SqlConnection conn = new SqlConnection(DefaultConnection());

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
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception thrown accessing Database {0}", ex.Message);
            }

            return ret;
        }

        public async Task<DBStatusDto> AddDataBulk(List<ParsedDataDto> ParsedData, DBStatusDto status)
        {
            var ret = new DBStatusDto();

            try
            {
                SqlConnection conn = new SqlConnection(DefaultConnection());

                String query = "INSERT INTO dbo.C_C_Test_Data (MPAN, MeterSerial,DateOfInstallation,AddressLine1,PostCode) VALUES (@MPAN, @MeterSerial,@DateOfInstallation,@AddressLine1,@PostCode)";

                DataTable table = new DataTable("C_C_Test_Data");
                table.Columns.Add(new DataColumn("ID", typeof(decimal)));
                table.Columns.Add(new DataColumn("MeterSerial", typeof(string)));
                table.Columns.Add(new DataColumn("DateOfInstallation", typeof(DateTime)));
                table.Columns.Add(new DataColumn("AddressLine1", typeof(string)));
                table.Columns.Add(new DataColumn("PostCode", typeof(string)));

                foreach (var data in ParsedData)
                {
                    table.Rows.Add(data.MPAN, data.MeterSerial, ParseString(data.DateOfInstallation), data.AddressLine, data.PostCode);
                }

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    conn.Open();
                    bulkCopy.DestinationTableName = "C_C_Test_Data";
                    bulkCopy.WriteToServer(table);
                }

                conn.Close();

                if (string.IsNullOrEmpty(status.QueryStatus))
                {
                    status.QueryStatus = "OK";
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception thrown accessing Database {0}", ex.Message);
            }

            return ret;
        }

        /// <summary>
        /// Returns list of records from database.
        /// TODO add pagination
        /// </summary>
        /// <returns>List<RetrievedData></returns>
        public async Task<List<RetrievedDataDto>> GetData()
        {
            var retData = new List<RetrievedDataDto>();

            try
            {
                SqlConnection conn = new SqlConnection(DefaultConnection());
                conn.Open();
                string querty = "SELECT MPAN, MeterSerial,DateOfInstallation,AddressLine1,PostCode FROM dbo.C_C_Test_Data";

                using (SqlCommand command = new SqlCommand(querty, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            retData.Add(new RetrievedDataDto
                            {
                                MPAN = reader.GetDecimal(0),
                                MeterSerial = reader.GetString(1),
                                DateOfInstallation = reader.GetDateTime(2).Date.ToString("D"),
                                AddressLine = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                PostCode = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                            });
                        }
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception thrown accessing Database {0}", ex.Message);
            }

            return retData;
        }

        /// <summary>
        /// Gets the connection string
        /// </summary>
        /// <returns>ConnectionStrings/returns>
        private string DefaultConnection()
        {
            return this.configuration["ConnectionStrings:DefaultConnection"];
        }

        /// <summary>
        /// String parsing
        /// </summary>
        /// <param name="str"></param>
        /// <returns>DateTime</returns>
        private DateTime ParseString(string str)
        {
            CultureInfo enUK = new CultureInfo("en-GB");
            DateTime dateValue;
            string year = str.Substring(0, 4);
            string month = str.Substring(4, 2);
            string day = str.Substring(6, 2);
            string formatted = month + "/" + day + "/" + year;

            if (DateTime.TryParseExact(formatted, "MM/dd/yyyy", enUK, DateTimeStyles.None, out dateValue))
            {
                return dateValue;
            }
            else
            {
                return DateTime.MinValue;
            }
        }
    }
}
