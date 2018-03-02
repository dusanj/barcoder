//Free / Open Source Software License
//This program is free software; you can redistribute it and/or modify it under the terms of the GNU Affero General Public License version 3 as published by the Free Software Foundation with the addition of the following permission added to Section 15 as permitted in Section 7(a): FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY 1T3XT, 1T3XT DISCLAIMS THE WARRANTY OF NON INFRINGEMENT OF THIRD PARTY RIGHTS.
//This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details. You should have received a copy of the GNU Affero General Public License along with this program; if not, see http://www.gnu.org/licenses or write to the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA, 02110-1301 USA, or download the license from the following URL: http://itextpdf.com/terms-of-use/
//The interactive user interfaces in modified source and object code versions of this program must display Appropriate Legal Notices, as required under Section 5 of the GNU Affero General Public License.
//In accordance with Section 7(b) of the GNU Affero General Public License, you must retain the producer line in every PDF that is created or manipulated using iText.
//You can be released from the requirements of the license by purchasing a commercial license. Buying such a license is mandatory as soon as you develop commercial activities involving the iText software without disclosing the source code of your own applications. These activities include: offering paid services to customers as an ASP, serving PDFs on the fly in a web application, shipping iText with a closed source product. 
using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;


namespace CreatePDF
{
	public partial class _Default : System.Web.UI.Page
	{
        protected String FileName;
        protected BarcodeEAN ean;
        protected BarcodeQRCode qr;
        protected Regex regex;
		protected void Page_Load(object sender, EventArgs e)
		{
            if (rbt_ean13.Checked)
                {
                    ean = new BarcodeEAN();
                    regex = new Regex("^[0-9]{13}");
                    ean.CodeType = BarcodeEAN.EAN13;
                    FileName = "EAN13_" + TextBox1.Text + ".pdf";
                    RegularExpressionValidator2.Display = ValidatorDisplay.Dynamic;
                    RegularExpressionValidator1.Display = ValidatorDisplay.None;
                    RegularExpressionValidator2.Enabled = true;
                    RegularExpressionValidator1.Enabled = false;
                    TextBox1.MaxLength = 13;
                    TextBox1.Width = 255;
                    cbxTextAbove.Enabled = true;
                    cbxGuardBars.Enabled = true;
                    LinkButton1.Visible = true;
                }
            else if (rbt_ean8.Checked)
                {
                    ean = new BarcodeEAN();
                    regex = new Regex("^[0-9]{8}");
                    ean.CodeType = BarcodeEAN.EAN8;
                    FileName = "EAN8_" + TextBox1.Text + ".pdf";
                    RegularExpressionValidator2.Display = ValidatorDisplay.None;
                    RegularExpressionValidator1.Display = ValidatorDisplay.Dynamic;
                    RegularExpressionValidator2.Enabled = false;
                    RegularExpressionValidator1.Enabled = true;
                    TextBox1.MaxLength = 8;
                    TextBox1.Width = 157;
                    cbxTextAbove.Enabled = true;
                    cbxGuardBars.Enabled = true;
                    LinkButton1.Visible = true;
                }
            else if (rbt_qr.Checked)
            {
                regex = new Regex("^[0-9a-zA-Z:/.]+");
                FileName = TextBox1.Text; 
                FileName = FileName.Substring(0, Math.Min(24, FileName.Length));
                string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                foreach (char c in invalid)
                {
                    FileName = FileName.Replace(c.ToString(), "");
                }
                
                FileName = "QR_" + FileName + ".pdf";
                RegularExpressionValidator2.Display = ValidatorDisplay.None;
                RegularExpressionValidator1.Display = ValidatorDisplay.None;
                RegularExpressionValidator2.Enabled = false;
                RegularExpressionValidator1.Enabled = false;
                TextBox1.MaxLength = 1852;
                TextBox1.Width = 255;
                cbxTextAbove.Enabled = false;
                cbxGuardBars.Enabled = false;
                LinkButton1.Visible = false;
            }

            if (regex.Match(TextBox1.Text).Success)
            {
                if (rbt_qr.Checked)
                {
                    qr = new BarcodeQRCode(TextBox1.Text, 200, 200, null);

                }
                else
                {
                    ean.ChecksumText = true;
                    ean.GenerateChecksum = true;
                    ean.StartStopText = true;
                    BaseFont ocrb = BaseFont.CreateFont(AppPath + "Ocrb.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                    ean.Font = ocrb;
                    ean.GuardBars = cbxGuardBars.Checked;
                    ean.Code = TextBox1.Text;
                    if (cbxTextAbove.Checked && !cbxGuardBars.Checked) ean.Baseline = -1f;
                }

            }
 		}


        private void CreateBarcodePdf()
		{
            Document document = new Document(PageSize.A4, 50, 50, 50, 50);

            FileStream fs = new FileStream(AppPath + @"Files\Barcode\" + FileName, FileMode.OpenOrCreate);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            if (rbt_qr.Checked)
            {
                iTextSharp.text.Image img = qr.GetImage();
                document.Add(img);
            }
            else document.Add(ean.CreateImageWithBarcode(cb, null, null));
			document.Close();
		}

		protected void Button1_Click(object sender, EventArgs e)
		{            
            CreateBarcodePdf();
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "application/pdf";
            response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
            response.TransmitFile(AppPath + @"Files\Barcode\" + FileName);
            response.Flush();
            //File.Delete(FilePath);
            //File.Delete(FilePath + ".gif");
            response.End();
		}

        public static string AppPath
        {
            get
            {
                string APP_PATH = System.Web.HttpContext.Current.Request.ApplicationPath.ToLower();
                if (APP_PATH == "/")      //a site
                    APP_PATH = "/";
                else if (!APP_PATH.EndsWith(@"/")) //a virtual
                    APP_PATH += @"/";

                string it = System.Web.HttpContext.Current.Server.MapPath(APP_PATH);
                if (!it.EndsWith(@"\"))
                    it += @"\";
                return it;
            }
        }



        protected void cbxGuardBars_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxGuardBars.Checked) cbxTextAbove.Enabled = false;
            else cbxTextAbove.Enabled = true;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            if (regex.Match(TextBox1.Text).Success)
            {
                System.Drawing.Bitmap bc = new System.Drawing.Bitmap(ean.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
                bc.Save(AppPath + @"Files\Barcode\" + FileName + ".gif", System.Drawing.Imaging.ImageFormat.Gif);
                imgBarCode.ImageUrl = @"Files/Barcode/" + FileName + ".gif";
                Label1.Text = TextBox1.Text;
                Label1.Visible = true;
                imgBarCode.Visible = true;
                //LinkButton1.Visible = false;
            }
        }

        protected void rbt_ean8_CheckedChanged(object sender, EventArgs e)
        {
            TextBox1.Text = "";
            if (rbt_ean13.Checked)
                Button1.Text = "EAN-13 -> pdf";
            else if (rbt_ean8.Checked)
                Button1.Text = "EAN-8 -> pdf";
            else if (rbt_qr.Checked)
                Button1.Text = "QR -> pdf";
        }

	}


}
