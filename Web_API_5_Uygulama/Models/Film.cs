using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_API_5_Uygulama.Models
{
    public class Film
    {
        public Guid FilmID { get; set; }
        public string FilmAdi { get; set; }
        public string FilmResimYolu { get; set; }
        public string FilmUrl { get; set; }
        public string FilmPuan { get; set; }
        public int FilmYorumSayisi { get; set; }
        public DateTime FilmVizyonTarihi { get; set; }
        public string FilmTuru { get; set; }
        public string FilmZamani { get; set; }
        public string FilmOzeti { get; set; }
    }
}