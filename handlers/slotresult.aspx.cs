using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

public partial class handlers_slotresult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["result"] != null)
        {
            Bitmap bitMapImage = new
            System.Drawing.Bitmap(Server.MapPath("~/Assets/slotmachine_result.jpg"));
            Graphics graphicImage = Graphics.FromImage(bitMapImage);

            graphicImage.DrawString(Request["result"].ToString().Insert(1, " ").Insert(3, "  "),
            new Font("Arial", 25, FontStyle.Regular),
            SystemBrushes.WindowText, new Point(85, 365));

            Response.ContentType = "image/jpeg";
            bitMapImage.Save(Response.OutputStream, ImageFormat.Jpeg);

            graphicImage.Dispose();
            bitMapImage.Dispose();
        }
    }
}