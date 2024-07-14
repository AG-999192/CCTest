using C_C_Test.Dtos;
using C_C_Test.Emuns;
using C_C_Test.FileIO;
using C_C_Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace C_C_Test.Pages
{
    public class UploadFileModel : PageModel
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private readonly ILogger logger;
        private readonly IFileParsing fileParsing;

        public UploadFileModel(ILogger<UploadFileModel> logger, IFileParsing fileParsing)
        {
            this.logger = logger;
            this.fileParsing = fileParsing;
        }

        public ValidationViewModel ValidationView { get; set; }

        public List<string> RejectedRows { get; set; } = null!;

        public async Task<IActionResult> OnGet(int id)
        {
            this.logger.LogDebug("Get method called with {Id}", id);
            List<ParsedDataDto> ParsedData;
            ValidationView = new ValidationViewModel();
            RejectedRows = new List<string>();

            ParsedData = await this.fileParsing.ParseFile(ValidationView, RejectedRows);

            //string FilePath = @"C:\C_C_TestData\C&CInterviewFile.txt";
            //string DirPath = @"C:\C_C_TestData";

            //List< ParsedDataDto > ParsedData = new List< ParsedDataDto >();
            //ValidationView = new ValidationViewModel();
            //RejectedRows = new List<string>();
            //int invalidRecords = 0;
            //string validationstatus = "Sucessfully Validate File";


            //if (Directory.Exists(DirPath))
            //{
            //    if(IsDataFile(FilePath))
            //    {
            //        if (System.IO.File.Exists(FilePath))
            //        {
            //            using (StreamReader sw = new StreamReader(FilePath)) //open file
            //            {
            //                bool bContinueParsing = true;

            //                while (bContinueParsing)
            //                {
            //                    string line = await sw.ReadLineAsync();

            //                    if(line != null)
            //                    {
            //                        string[] datafields = line.Split('|');

            //                        if (datafields.Length >= 3)
            //                        {
            //                            var record = ParseData(datafields, datafields.Length);
            //                            if (record != null)
            //                            {
            //                                ParsedData.Add(record);
            //                            }
            //                            else
            //                            {
            //                                RejectedRows.Add(line);
            //                                invalidRecords++;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            RejectedRows.Add(line);
            //                            invalidRecords++;
            //                            // Invalid
            //                        }
            //                    }
            //                    else
            //                    {
            //                        bContinueParsing = false;
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {         
            //            validationstatus = "Failed to access file";
            //        }
            //    }
            //}
            //else
            //{
            //    validationstatus = "Failed to access directory";
            //}

            //this.logger.LogDebug(validationstatus);

            //ValidationView.RejectedRows = invalidRecords;
            //ValidationView.SuccessfulRows = ParsedData.Count;
            //ValidationView.ValidationStatus = validationstatus;

            return this.Page();
        }

        bool IsDataFile(string filename)
        {
            // if the file ends with a known data suffix, return true
            if (filename.EndsWith(".txt") )
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

            string year =   data[(int)DataEnums.DateOfInstallation].Substring(0, 4);
            string month =  data[(int)DataEnums.DateOfInstallation].Substring(4, 2);
            string day =    data[(int)DataEnums.DateOfInstallation].Substring(6, 2);

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
