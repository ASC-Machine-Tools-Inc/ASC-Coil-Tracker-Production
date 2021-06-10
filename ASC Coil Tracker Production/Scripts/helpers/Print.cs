using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PrintLogic
{
    public class Print
    {
        public void PrintCoil(string ID, string FINISH, string MATERIAL, string GAUGE, double? THICK,
            double? WEIGHT, double? LENGTH, double? WIDTH, double? YIELD, string NOTES)
        {
            // Printer IP Address and communication port
            const string IPAddress = "192.168.0.208";
            const int Port = 9100;

            // ZPL Command(s)
            string ZPLString = "^XA"; //START LABEL
            //FORMAT BORDERS
            ZPLString += "^FO20,20^GB760,1160,5,B,1^FS"; //SEND DRAWING OBJECT FOR BOX
            ZPLString += "^FO120,20^GB100,1160,5,B,0^FS"; //SEND DRAWING OBJECT FOR BOX
            ZPLString += "^FO215,20^GB100,1160,5,B,0^FS"; //SEND DRAWING OBJECT FOR BOX
            ZPLString += "^FO310,20^GB100,1160,5,B,0^FS"; //SEND DRAWING OBJECT FOR BOX
            ZPLString += "^FO405,20^GB100,1160,5,B,0^FS"; //SEND DRAWING OBJECT FOR BOX
            //ADD DATA LABELS
            ZPLString += "^FO50,700^ADB,54,40^FDCOIL ID: ^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO150,950^ADB,36,20^FDGAUGE:  ^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO150,600^ADB,36,20^FDTHICK:  ^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO150,200^ADB,36,20^FDWEIGHT:  ^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO250,900^ADB,36,20^FDMATERIAL: ^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO250,400^ADB,36,20^FDFINISH: ^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO350,900^ADB,36,20^FDWIDTH: ^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO350,400^ADB,36,20^FDYIELD: ^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO440,750^ADB,36,20^FDCURRENT LENGTH: ^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO530,950^ADB,36,20^FDNOTES:  ^FS"; //SEND TEXT OBJECT
            //ADD DATA FIELDS
            ZPLString += "^FO50,50^ADB,54,40^FD" + ID + "^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO150,850^ADB,36,20^FD" + GAUGE + "^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO150,475^ADB,36,20^FD" + THICK + "^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO150,50^ADB,36,20^FD" + WEIGHT + "^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO250,650^ADB,36,20^FD" + MATERIAL + "^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO250,50^ADB,36,20^FD" + FINISH + "^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO350,650^ADB,36,20^FD" + WIDTH + "^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO350,50^ADB,36,20^FD" + YIELD + "^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO440,650^ADB,36,20^FD" + LENGTH + "^FS"; //SEND TEXT OBJECT
            ZPLString += "^FO530,50^ADB,36,20^FD" + NOTES + "^FS"; //SEND TEXT OBJECT
            //ADD QR CODE
            //ZPLString += "^FO435,50^BQN,2,10^FH^FDMM,B0080   " + CODE + "_0D_0A^FS"; //SEND QR CODE OBJECT
            //ZPLString += "^FO435,50^BQN,2,10^FD   " + COILS.QR_CODE + "^FS"; //SEND QR CODE OBJECT
            //ZPLString += "^FO415,50^ADB,18,10^FD" + CODE + "^FS"; //SEND TEXT OBJECT
            ZPLString += "^XZ"; //END LABEL

            try
            {
                //// Open connection
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                client.Connect(IPAddress, Port);

                //// Write ZPL String to connection
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.Write(ZPLString);
                writer.Flush();

                //// Close Connection
                writer.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                // Catch Exception
                Console.WriteLine(ex);
            }
            Console.Read();
        }
    }
}