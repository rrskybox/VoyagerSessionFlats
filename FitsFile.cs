using AstroImage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Xml.Linq;
//using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using System.Windows.Forms;

namespace SessionFlats
{

    public class FitsFile
    {
        public ushort[] FitsVector = new ushort[0];
        public ushort[,] FitsArray = new ushort[0, 0];
        public Bitmap[] FitsBitMap = new Bitmap[0];

        byte[] headerRecord = new byte[80];
        byte[] dataUnit = new byte[2880];

        private List<string> FitsHeaderList = new List<string>();

        public string FilePath { get; set; }
        public List<FitsHeaderField> FieldList = new List<FitsHeaderField>();

        /// <summary>
        /// Class instantiation -- opens and parses fits file
        /// </summary>
        /// <param name="filepath"></param>
        public FitsFile(string filepath, bool readData = false)
        {
            //Opens file set by filepath (assumes it's a FITS formatted file)
            //Reads in header in 80 character strings, while ("END" is found
            int bCount;

            FilePath = filepath;

            int keyindex = -1;
            FileStream fitsFileStream = File.OpenRead(filepath);
            do
            {
                keyindex++;
                bCount = fitsFileStream.Read(headerRecord, 0, 80);
                //Check for empty file (file error on creation), just opt out if (so
                if (bCount == 0)
                    return;
                FitsHeaderList.Add(System.Text.Encoding.ASCII.GetString(headerRecord));
            } while (!FitsHeaderList.Last().StartsWith("END "));
            //Parse the field name/field data values out of each line and create tables
            foreach (string keyline in FitsHeaderList)
            {
                //Read each line and parse out the fits field name and fits field data
                FitsHeaderField fdf = new FitsHeaderField();
                string[] parsedField = keyline.Split('=');
                if (parsedField.Length == 2)
                {
                    fdf.FieldName = parsedField[0].Trim();
                    fdf.FieldData = parsedField[1].Split('/')[0].Replace("\'", "").Trim();
                    FieldList.Add(fdf);
                }
            }
            //If the read data flag has been set, then parse the fits data component into a vector and array of ushorts
            if (readData)
                ReadFitsData(fitsFileStream);
            //All done -- close up file stream
            fitsFileStream.Close();
        }

        /// <summary>
        /// Over writes the current fits header and data to the given filepath
        /// </summary>
        /// <returns></returns>
        public bool SaveFile()
        {
            //Read in whole data file to buffer
            int rCount;
            int byteCount = 0;
            FileStream fitsFileStreamIn = File.OpenRead(FilePath);
            FileStream fitsFileStreamOut = File.OpenWrite(FilePath + ".tmp");
            //The new file header must be written in the same field order as the old file header
            //  with "SIMPLE" field first.
            //  If a field has been added, then put it at the end after all other fields have been written out.
            //
            //Get the original fits file in order

            foreach (string keyLine in FitsHeaderList)
            {
                byte[] headerRecord = new byte[80];
                //Write the header record to the output file stream
                headerRecord = System.Text.Encoding.ASCII.GetBytes(keyLine.PadRight(80, ' '));
                fitsFileStreamOut.Write(headerRecord, 0, 80);
                byteCount += 80;
            }
            //fill in the rest of the header block with spaces
            int remainder = 2880 - (byteCount % 2880);
            for (int i = 0; i < remainder; i++)
            {
                fitsFileStreamOut.Write(new byte[1] { 32 }, 0, 1);
            }

            //Read in header of old file, read until "END" header record is found.
            byteCount = 0;
            do
            {
                rCount = fitsFileStreamIn.Read(headerRecord, 0, 80);
                byteCount += rCount;
                //Check for empty file (file error on creation), just opt out if (so
                if (rCount == 0)
                    return false;
            } while (!System.Text.Encoding.ASCII.GetString(headerRecord).StartsWith("END "));
            if (rCount == 0)
            {
                fitsFileStreamOut.Close();
                fitsFileStreamIn.Close();
                return false;
            }
            //Read in the remainder of the block
            while (byteCount % 2880 != 0)
            {
                rCount = fitsFileStreamIn.Read(headerRecord, 0, 80);
                byteCount += rCount;
            }

            //Read in and write out the data blocks until end of file
            do
            {
                rCount = fitsFileStreamIn.Read(dataUnit, 0, 2880);
                if (rCount > 0)
                    fitsFileStreamOut.Write(dataUnit, 0, 2880);
            } while (rCount > 0);
            fitsFileStreamOut.Close();
            fitsFileStreamIn.Close();
            File.Copy(FilePath + ".tmp", FilePath, true);
            File.Delete(FilePath + ".tmp");
            return true;
        }

        public string? ReadKey(string keyword)
        {
            //return;s contents of key word entry, scrubbed of extraneous characters
            foreach (string keyline in FitsHeaderList)
            {
                if (keyline.Contains(keyword))
                {
                    int startindex = keyline.IndexOf("=");

                    int endindex = keyline.IndexOf("/");
                    if (endindex == -1)
                    {
                        endindex = keyline.Length - 1;
                    }

                    string keylineN = keyline.Substring(startindex + 1, endindex - (startindex + 1));
                    // keyline = Replace(keyline, "//", " ");
                    keylineN = keylineN.Replace('/', ' ');
                    keylineN = keylineN.Replace('\'', ' ');
                    keylineN = keylineN.Trim(' ');
                    return (keylineN);
                }
            }
            return (null);
        }

        public bool ReplaceKey(string keyword, string newval)
        {
            // Replaces the value associated with the fits keyword with a new value
            //
            // find the position of the keyword
            for (int i = 0; i < FitsHeaderList.Count; i++)
            {
                //if the keyword is found, then substitute the new value for old
                if (FitsHeaderList[i].Contains(keyword))
                {
                    //find start and end indexes for the given keyword
                    int startindex = FitsHeaderList[i].IndexOf("=");
                    int endindex = FitsHeaderList[i].IndexOf("/");
                    if (endindex == -1)
                        endindex = FitsHeaderList[i].Length - 1;
                    //Replace the string between these two indices with the new string in single quotes
                    string keylineN = FitsHeaderList[i].Substring(startindex + 1, endindex - (startindex + 1));
                    //if string is not numeric, then add single quotes, otherwise just bracket in spaces
                    if (decimal.TryParse(newval, out decimal throwaway))
                        FitsHeaderList[i] = FitsHeaderList[i].Replace(keylineN, " " + newval + " ");
                    else
                        FitsHeaderList[i] = FitsHeaderList[i].Replace(keylineN, "\' " + newval + " \'");
                    return (true);
                }
            }
            return (false);
        }

        public bool AddKey(string keyword, string newval)
        {
            // Adds a new fits header field
            //
            // find the position of the keyword
            for (int i = 0; i < FitsHeaderList.Count; i++)
            {
                //if the keyword is found, notify user of error
                if (FitsHeaderList[i].Contains(keyword))
                {
                    MessageBox.Show("Add Header Field Error", "Header field already exists!", MessageBoxButtons.OK);
                    //find start and end indexes for the given keyword
                    return (false);
                }
            }
            //Find the END field
            int end = FitsHeaderList.FindIndex(s => s.StartsWith("END"));
            if (end == -1)
                return false;
            else
                FitsHeaderList.Insert(end, keyword + "=\' " + newval + " \'");
            return (true);
        }

        public bool DeleteKey(string keyword)
        {
            // Replaces the value associated with the fits keyword with a new value
            //
            // find the position of the keyword
            for (int i = 0; i < FitsHeaderList.Count; i++)
            {
                //if the keyword is found, then substitute the new value for old
                if (FitsHeaderList[i].Contains(keyword))
                {
                    FitsHeaderList.RemoveAt(i);
                    return (true);
                }
            }
            MessageBox.Show("Field not found!");
            return (false);
        }

        public DateTime ParseDateTime(string fitsUTC)
        {
            string[] dsts = fitsUTC.Split('T');
            string[] ds = dsts[0].Split('-');
            string[] dt = dsts[1].Split(':');
            int year = Convert.ToInt16(ds[0]);
            int month = Convert.ToInt16(ds[1]);
            int day = Convert.ToInt16(ds[2]);
            int hour = Convert.ToInt16(dt[0]);
            int minute = Convert.ToInt16(dt[1]) % 60;
            int second = Convert.ToInt16(dt[2]);
            DateTime utcDT = new DateTime(year, month, day, hour, minute, second);
            string FitsUTCDate = utcDT.Date.ToShortDateString();
            string FitsUTCTime = utcDT.TimeOfDay.ToString();
            return utcDT;

        }

        public void ReadFitsData(FileStream fs)
        {
            int Xaxis = Convert.ToInt32(ReadKey("NAXIS1"));
            int Yaxis = Convert.ToInt32(ReadKey("NAXIS2"));

            int bCount;
            int num = Xaxis * Yaxis;
            int num2 = 0;
            int xaxis = Xaxis;
            int yaxis = Yaxis;
            Array.Resize(ref FitsVector, num);
            int num3 = 0;
            int num4 = 0;
            do
            {
                bCount = fs.Read(dataUnit, 0, 2880);
                for (int j = 0; j <= bCount - 1; j += 2)
                {
                    if (num2 < num)
                    {
                        ushort num5 = TwosComplementBytesToInteger(dataUnit[j], dataUnit[j + 1]);
                        FitsVector[num2] = num5;
                    }

                    num2++;
                    num3++;
                    if (num3 >= xaxis)
                    {
                        num4++;
                        num3 = 0;
                    }
                }
            } while (bCount != 0);

            FitsArray = new ushort[Xaxis, Yaxis];
            for (int k = 0; k < Yaxis; k++)
            {
                for (int l = 0; l < Xaxis; l++)
                {
                    FitsArray[l, k] = (ushort)FitsVector[l + k * Xaxis];
                }
            }
        }

        private ushort TwosComplementBytesToInteger(ushort highbyte, ushort lowbyte)
        {
            return (ushort)Math.Min(((short)((highbyte << 8) + lowbyte) + 32768) / 256, 255);
        }

        public Bitmap GetImageBitmap()
        {
            Histogram hg = new Histogram(ref FitsVector);
            return hg.NormalStretch(ref FitsArray, Convert.ToInt16(ReadKey("NAXIS1")), Convert.ToInt16(ReadKey("NAXIS2")));
        }

    }

    public struct FitsHeaderField
    {
        public string FieldName;
        public string FieldData;
    }
}




