﻿using C_C_Test.Dtos;
using C_C_Test.Emuns;
using C_C_Test.Models;
using System.Globalization;

namespace C_C_Test.FileIO
{
    public class FileParsing : IFileParsing
    {
        private readonly ILogger<FileParsing> logger;

        public FileParsing(ILogger<FileParsing> logger)
        {
            this.logger = logger;   
        }

        public async Task<List<ParsedDataDto>> ParseFile(ValidationViewModel validationViewModel, List<string> RejectedRows)
        {
            string FilePath = @"C:\C_C_TestData\C&CInterviewFile.txt";
            string DirPath = @"C:\C_C_TestData";

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
                                                RejectedRows.Add(line);
                                                invalidRecords++;
                                            }
                                        }
                                        else
                                        {
                                            RejectedRows.Add(line);
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
                this.logger.LogError("Exception thrown in ParseFile", ex.Message);
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

        bool IsDataFile(string filename)
        {
            // if the file ends with a known data suffix, return true
            if (filename.EndsWith(".txt"))
                return true;
            return false;
        }

        ParsedDataDto? ParseData(string[] data, int items)
        {
            var ret = new ParsedDataDto();
            CultureInfo enUK = new CultureInfo("en-GB");
            DateTime dateValue;

            if (string.IsNullOrEmpty(data[(int)DataEnums.MPAN]) || data[(int)DataEnums.MPAN].Length != 13)
            {
                return null;
            }

            if (string.IsNullOrEmpty(data[(int)DataEnums.MeterSerial]) || data[(int)DataEnums.MeterSerial].Length > 10)
            {
                return null;
            }

            if (string.IsNullOrEmpty(data[(int)DataEnums.DateOfInstallation]) || data[(int)DataEnums.DateOfInstallation].Length != 8)
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
            int mpan;
            int.TryParse(data[(int)DataEnums.MPAN], out mpan);
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
    }
}
