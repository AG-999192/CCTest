using C_C_Test.Dtos;
using C_C_Test.Emuns;
using C_C_Test.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace C_C_Test.FileIO
{
    /// <summary>
    /// Implementation of the FileParsing class.
    /// </summary>
    public class FileParsing : IFileParsing
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<FileParsing> logger;

        /// <summary>
        /// Config.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Consts
        /// </summary>
        const int MPAN_SIZE = 13;

        const int MeterSerial_SIZE = 10;

        const int DateOfInstallation_SIZE = 8;

        /// <summary>
        /// Constructor for FileParsing.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        public FileParsing(ILogger<FileParsing> logger, IConfiguration configuration)
        {
            this.logger = logger;  
            this.configuration = configuration;
        }

        /// <summary>
        /// The ParseFile method
        /// </summary>
        /// <param name="validationViewModel"></param>
        /// <param name="RejectedRows"></param>
        /// <returns>List<ParsedDataDto></returns>
        public async Task<List<ParsedDataDto>> ParseFile(ValidationViewModel validationViewModel)
        {
            var DirPath = GetDirectory();
            var FilePath = DirPath + GetFile();

            List<ParsedDataDto> ParsedData = new List<ParsedDataDto>();
            int invalidRecords = 0;
            string validationstatus = "Sucessfully Validate File";

            try
            {
                if (Directory.Exists(DirPath))
                {
                    if (IsDataFile(FilePath))
                    {
                        if (System.IO.File.Exists(FilePath))
                        {
                            using (StreamReader sw = new StreamReader(FilePath)) //open file
                            {
                                bool bContinueParsing = true;

                                while (bContinueParsing)
                                {
                                    string line = await sw.ReadLineAsync();

                                    if (line != null)
                                    {
                                        string[] datafields = line.Split('|');

                                        if (datafields.Length >= 3)
                                        {
                                            var record = ParseData(datafields, datafields.Length);
                                            if (record != null)
                                            {
                                                ParsedData.Add(record);
                                            }
                                            else
                                            {
                                                validationViewModel.RejectedRowsList.Add(line);
                                                invalidRecords++;
                                            }
                                        }
                                        else
                                        {
                                            validationViewModel.RejectedRowsList.Add(line);
                                            invalidRecords++;
                                            // Invalid
                                        }
                                    }
                                    else
                                    {
                                        bContinueParsing = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            validationstatus = "Failed to access file";
                        }
                    }
                }
                else
                {
                    validationstatus = "Failed to access directory";
                }
            }
            catch (Exception ex)
            {
                validationstatus = "Failed to process file due to excception";
                this.logger.LogError("Exception thrown in ParseFile {0}", ex.Message);
            }
            finally
            {
                this.logger.LogDebug(validationstatus);

                validationViewModel.RejectedRows = invalidRecords;
                validationViewModel.SuccessfulRows = ParsedData.Count;
                validationViewModel.ValidationStatus = validationstatus;
            }

            return ParsedData;
        }

        /// <summary>
        /// The ParseFile method
        /// </summary>
        /// <param name="validationViewModel"></param>
        /// <param name="RejectedRows"></param>
        /// <returns>List<ParsedDataDto></returns>
        public async Task<List<ParsedDataDto>> ParseFile()
        {
            var DirPath = GetDirectory();
            var FilePath = DirPath + GetFile();

            List<ParsedDataDto> ParsedData = new List<ParsedDataDto>();
            string validationstatus = "Sucessfully Validate File";

            try
            {
                if (Directory.Exists(DirPath))
                {
                    if (IsDataFile(FilePath))
                    {
                        if (System.IO.File.Exists(FilePath))
                        {
                            using (StreamReader sw = new StreamReader(FilePath)) //open file
                            {
                                bool bContinueParsing = true;

                                while (bContinueParsing)
                                {
                                    string line = await sw.ReadLineAsync();

                                    if (line != null)
                                    {
                                        string[] datafields = line.Split('|');

                                        if (datafields.Length >= 3)
                                        {
                                            var record = ParseData(datafields, datafields.Length);
                                            if (record != null)
                                            {
                                                ParsedData.Add(record);
                                            }
                                            
                                        }
                                    }
                                    else
                                    {
                                        bContinueParsing = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            validationstatus = "Failed to access file";
                        }
                    }
                }
                else
                {
                    validationstatus = "Failed to access directory";
                }
            }
            catch (Exception ex)
            {
                validationstatus = "Failed to process file due to excception";
                this.logger.LogError("Exception thrown in ParseFile {0}", ex.Message);
            }
            finally
            {
                this.logger.LogDebug(validationstatus);
            }

            return ParsedData;
        }

        /// <summary>
        /// Check datafile is of correct type
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        bool IsDataFile(string filename)
        {
            // if the file ends with a known data suffix, return true
            if (filename.EndsWith(".txt"))
                return true;
            return false;
        }

        /// <summary>
        /// The ParseData function.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="items"></param>
        /// <returns>ParsedDataDto</returns>
        ParsedDataDto? ParseData(string[] data, int items)
        {
            var ret = new ParsedDataDto();
            CultureInfo enUK = new CultureInfo("en-GB");
            DateTime dateValue;

            if (string.IsNullOrEmpty(data[(int)DataEnums.MPAN]) || data[(int)DataEnums.MPAN].Length != MPAN_SIZE ||
                string.IsNullOrEmpty(data[(int)DataEnums.MeterSerial]) || data[(int)DataEnums.MeterSerial].Length > MeterSerial_SIZE ||
                string.IsNullOrEmpty(data[(int)DataEnums.DateOfInstallation]) || data[(int)DataEnums.DateOfInstallation].Length != DateOfInstallation_SIZE)
            {
                return null;
            }

            string year = data[(int)DataEnums.DateOfInstallation].Substring(0, 4);
            string month = data[(int)DataEnums.DateOfInstallation].Substring(4, 2);
            string day = data[(int)DataEnums.DateOfInstallation].Substring(6, 2);

            string formatted = month + "/" + day + "/" + year;

            if (DateTime.TryParseExact(formatted, "MM/dd/yyyy", enUK, DateTimeStyles.None, out dateValue))
            {
                if (dateValue >= DateTime.Now)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            decimal mpan;
            decimal.TryParse(data[(int)DataEnums.MPAN], out mpan);
            ret.MPAN = mpan;
            ret.MeterSerial = data[(int)DataEnums.MeterSerial];
            ret.DateOfInstallation = data[(int)DataEnums.DateOfInstallation];

            if (items >= 4)
            {
                if (string.IsNullOrEmpty(data[(int)DataEnums.AddressLine]))
                {
                    ret.AddressLine = data[(int)DataEnums.AddressLine];
                }
            }

            if (items >= 5)
            {
                if (string.IsNullOrEmpty(data[(int)DataEnums.PostCode]))
                {
                    ret.PostCode = data[(int)DataEnums.PostCode];
                }
            }

            return ret;
        }

        /// <summary>
        /// Get directory from config.
        /// </summary>
        /// <returns></returns>
        private string GetDirectory()
        {
            return this.configuration["IOSettings:Directory"];
        }

        /// <summary>
        /// Get directory from config.
        /// </summary>
        /// <returns></returns>
        private string GetFile()
        {
            return this.configuration["IOSettings:Datafile"];
        }
    }
}
