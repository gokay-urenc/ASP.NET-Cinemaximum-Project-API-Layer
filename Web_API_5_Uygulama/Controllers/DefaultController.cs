using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Web_API_5_Uygulama.Models;

namespace Web_API_5_Uygulama.Controllers
{
    public class DefaultController : ApiController
    {
        List<Film> filmler = new List<Film>();

        [NonAction] // Dışarıdan tetiklenmemesi ve çağırılmaması için NonAction Attributesi'ni ekledik. MVC'de de yapılabilir. (Sadece bizim kullanacağımız bir methoddur.)
        public void FilmGetir()
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8; // Türkçe karakter sıkıntısı için karakter setini UTF8 yaptık.

            string okunanSayfa = client.DownloadString("https://www.cinemaximum.com.tr/vizyondakiler");
            okunanSayfa = HttpUtility.HtmlDecode(okunanSayfa);

            HtmlDocument dokuman = new HtmlDocument();
            dokuman.LoadHtml(okunanSayfa);

            for (int i = 1; i <= 20; i++)
            {
                HtmlNode okunanFilm = dokuman.DocumentNode.SelectSingleNode("//*[@id='movie-list']/div[1]/div["+i+"]");
                Film f = new Film();
                f.FilmID = Guid.Parse(okunanFilm.Attributes["data-id"].Value);
                f.FilmYorumSayisi = int.Parse(okunanFilm.Attributes["data-comments"].Value);
                f.FilmResimYolu = okunanFilm.SelectSingleNode("//*[@id='movie-list']/div[1]/div["+i+"]/div[1]/img").Attributes["src"].Value;
                f.FilmAdi = okunanFilm.SelectSingleNode("//*[@id='movie-list']/div[1]/div["+i+"]/h4").InnerText;
                f.FilmUrl = "https://www.cinemaximum.com.tr" + okunanFilm.SelectSingleNode("//*[@id='movie-list']/div[1]/div["+i+"]/div[1]/div/div[2]/a").Attributes["href"].Value;
                WebClient detay = new WebClient();
                detay.Encoding = Encoding.UTF8;
                string okunanDetay = detay.DownloadString(f.FilmUrl);
                okunanDetay = HttpUtility.HtmlDecode(okunanDetay);
                HtmlDocument detayDokuman = new HtmlDocument();
                detayDokuman.LoadHtml(okunanDetay);
                string tarih = detayDokuman.DocumentNode.SelectSingleNode("/html/body/section[3]/div/div/div[1]/div[1]").InnerHtml;
                tarih = tarih.Substring(tarih.LastIndexOf("</strong>") + 10);
                f.FilmVizyonTarihi = DateTime.Parse(tarih);
                filmler.Add(f);
            }
        }

        public IHttpActionResult Get()
        {
            FilmGetir();
            return Ok(filmler);
        }
    }
}