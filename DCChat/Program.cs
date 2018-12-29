using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using System.Drawing;

namespace DCChat
{
    class Program
    {

        static void Main(string[] args)
        {
            ParseDC parseDC = new ParseDC();
            parseDC.ParseNRegex();

            Bitmap bitmap = new Bitmap(300, 300);
            Graphics graphics = Graphics.FromImage(bitmap);
            Brush brush = new SolidBrush(ColorTranslator.FromHtml("#ff00ffff"));
            graphics.FillEllipse(brush, 0, 0, 19, 19);
            graphics.DrawImage(bitmap, 300, 300);
        }
    }
    class ParseDC
    {
        List<string> DCDataList = new List<string>(); //제목, 작성자, 아이피
        public void ParseNRegex()
        {
            DCDataList.Clear();

            WebClient client = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            string downloadString = client.DownloadString("http://gall.dcinside.com/board/lists?id=maritime_university");
            string Pattern = "icon_img icon_txt\"></em>(.+?)</a>";
            Regex Reg = new Regex(Pattern);
            MatchCollection mc = Reg.Matches(downloadString);
            string tmp = null;
            foreach (Match m in mc)
            {
                tmp = m.Value.Replace("icon_img icon_txt\"></em>", string.Empty).Replace("</a>", string.Empty);
                DCDataList.Add(tmp);
            }


        }
    }
}
