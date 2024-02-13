using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCubeRenamer
{
    class GCDiscInfo
    {

        string path;

        public GCDiscInfo(string gamePath)
        {
            path = gamePath;
        }

        public string getGameName()
        {
            return convertHexToAscii(getHexStringFromFile(0x20, 0x3E0));
        }

        public bool setGameName(string gameName)
        {
            string hexValue = convertAsciiToHex(gameName.PadRight(0x3E0));
            return writeByteArrayToFile(this.path, 0x20, StringToByteArray(hexValue));
        }

        public string getConsoleId()
        {
            return getHexStringFromFile(0x0, 0x1);
        }

        public bool setConsoleID(string consoleID)
        {
            return writeByteArrayToFile(this.path, 0x0, StringToByteArray(consoleID.PadLeft(0x2, '0').ToUpper()));
        }

        public string getGameCode()
        {
            return convertHexToAscii(getHexStringFromFile(0x1, 0x2));
        }

        public bool setGameCode(string gameCode)
        {
            string hexValue = convertAsciiToHex(gameCode.PadLeft(0x2, '0'));
            return writeByteArrayToFile(this.path, 0x1, StringToByteArray(hexValue));
        }

        public string getCountryCode()
        {
            return getHexStringFromFile(0x3, 0x1);
        }

        public bool setCountryCode(string countryCode)
        {
            return writeByteArrayToFile(this.path, 0x3, StringToByteArray(countryCode.PadLeft(0x2, '0')));
        }

        public string getMakerCode()
        {
            return convertHexToAscii(getHexStringFromFile(0x4, 0x2));
        }

        public bool setMakerCode(string makerCode)
        {
            string hexValue = convertAsciiToHex(makerCode.PadLeft(0x2, '0'));
            return writeByteArrayToFile(this.path, 0x4, StringToByteArray(hexValue));
        }

        public string getDiscId()
        {
            return getHexStringFromFile(0x6, 0x1);
        }

        public bool setDiscId(string discId)
        {
            return writeByteArrayToFile(this.path, 0x6, StringToByteArray(discId.PadLeft(0x2, '0')));
        }

        public string getVersion()
        {
            return getHexStringFromFile(0x7, 0x1);
        }

        public bool setVersion(string version)
        {
            return writeByteArrayToFile(this.path, 0x7, StringToByteArray(version.PadLeft(0x1, '0')));
        }

        public string getDVDMagicWord()
        {
            return getHexStringFromFile(0x1C, 0x4);
        }

        public bool setDVDMagicWord(string dvdMagicWord)
        {
            return writeByteArrayToFile(this.path, 0x1C, StringToByteArray(dvdMagicWord.PadLeft(0x4, '0')));
        }

        public string getDebugMonitorOffset()
        {
            return getHexStringFromFile(0x400, 0x4);
        }

        public bool setDebugMonitorOffset(string debugMonitorOffset)
        {
            return writeByteArrayToFile(this.path, 0x400, StringToByteArray(debugMonitorOffset.PadLeft(0x4, '0')));
        }

        public string getDebugMonitorLoadAddress()
        {
            return getHexStringFromFile(0x404, 0x4);
        }

        public bool setDebugMonitorLoadAddress(string debugMonitorLoadAddress)
        {
            return writeByteArrayToFile(this.path, 0x404, StringToByteArray(debugMonitorLoadAddress.PadLeft(0x4, '0')));
        }

        public string getMainExecutableDOLOffset()
        {
            return getHexStringFromFile(0x420, 0x4);
        }

        public bool setMainExecutableDOLOffset(string mainExecutableDOLOffset)
        {
            return writeByteArrayToFile(this.path, 0x420, StringToByteArray(mainExecutableDOLOffset.PadLeft(0x4, '0')));
        }

        public string getFSTOffset()
        {
            return getHexStringFromFile(0x424, 0x4);
        }

        public bool setFSTOffset(string fstOffset)
        {
            return writeByteArrayToFile(this.path, 0x424, StringToByteArray(fstOffset.PadLeft(0x4, '0')));
        }

        public string getFSTSize()
        {
            return getHexStringFromFile(0x428, 0x4);
        }

        public bool setFSTSize(string fstSize)
        {
            return writeByteArrayToFile(this.path, 0x428, StringToByteArray(fstSize.PadLeft(0x4, '0')));
        }

        public string getMaxFSTSize()
        {
            return getHexStringFromFile(0x42C, 0x4);
        }

        public bool setMaxFSTSize(string maxFSTSize)
        {
            return writeByteArrayToFile(this.path, 0x42C, StringToByteArray(maxFSTSize.PadLeft(0x4, '0')));
        }

        private static string convertAsciiToHex(String asciiString)
        {
            char[] charValues = asciiString.ToCharArray();
            string hexValue = "";
            foreach (char c in charValues)
            {
                int value = Convert.ToInt32(c);
                hexValue += String.Format("{0:X}", value);
            }
            return hexValue;
        }

        public static string convertHexToAscii(String hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;

                }

                return ascii;
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
            }

            return string.Empty;
        }

        private string getHexStringFromFile(int startPoint, int length)
        {
            string hexString = "";
            using (FileStream fileStream = new FileStream(@path, FileMode.Open, FileAccess.Read))
            {
                long offset = fileStream.Seek(startPoint, SeekOrigin.Begin);

                for (int x = 0; x < length; x++)
                {
                    hexString += fileStream.ReadByte().ToString("X2");
                }

            }

            return hexString;
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private bool writeByteArrayToFile(string fileName, int startPoint, byte[] byteArray)
        {
            bool result = false;
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    fs.Position = startPoint;
                    fs.Write(byteArray, 0, byteArray.Length);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error writing file: {0}", ex);
                result = false;
            }

            return result;
        }
    }
}
