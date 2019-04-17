using IronOcr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Drawing.Imaging;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Drawing;
using System.IO.IsolatedStorage;
using Ocr7siu.Models;
using Newtonsoft.Json;
using ImageProcessor;

namespace Ocr7siu.Controllers
{
  

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImagenReController : ApiController
    {
        public static int conta=1;
        public static int nuevoconta = 1;
        public  String extension = "";

        const String Storage = "ponysalvaje";
        const String key = "";
        public string tempora = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\temporal" + "\\";
        public object ViewBag { get; private set; }

       

        public void contadorin()
        {
            conta++;
        }

        public void containn ()
        {
            nuevoconta++;
        }
        

        //devuelve un string del ocr7 cuando se le envia una base64
       
        //Metodo que guarda el archivo y extrae el path para que se implemente el OCR
        public String Almacenar(String filename, String base64Image)
        {
            string destinationImgPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\EscaneoFisicoo" + "\\" + filename + extension;
            try
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\EscaneoFisicoo");
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message + " " + iox.Data);
            }
            byte[] bytes = Convert.FromBase64String(base64Image);

            using (var imageFile = new FileStream(destinationImgPath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }





            return destinationImgPath;
        }

       
        


        public String recorte(String pati)
        {
            var balonBytes = File.ReadAllBytes(pati);
            String Patricia = tempora + nuevoconta.ToString() + extension;
            using (var inStream = new MemoryStream(balonBytes))
            using (var imageFactory = new ImageFactory(false))
            {
                var m = imageFactory.Load(inStream)
             .Crop(new Rectangle(474, 115, 117, 28))
               .Save(Patricia);

            }
            containn();
                return Patricia;
        }

        public String recorte2(String pati)
        {
            var balonBytes = File.ReadAllBytes(pati);
            String Patricia = tempora + nuevoconta.ToString() + extension;
            using (var inStream = new MemoryStream(balonBytes))
            using (var imageFactory = new ImageFactory(false))
            {
                var m = imageFactory.Load(inStream)
             .Crop(new Rectangle(218, 587, 122, 34))
               .Save(Patricia);

            }
            containn();
            return Patricia;
        }

        public String recorte3(String pati)
        {
            var balonBytes = File.ReadAllBytes(pati);
            String Patricia = tempora + nuevoconta.ToString() + extension;
            using (var inStream = new MemoryStream(balonBytes))
            using (var imageFactory = new ImageFactory(false))
            {
                var m = imageFactory.Load(inStream)
             .Crop(new Rectangle(217, 98, 360, 22))
               .Save(Patricia);

            }
            containn();
            return Patricia;
        }

        public String recorte4(String pati)
        {
            var balonBytes = File.ReadAllBytes(pati);
            String Patricia = tempora + nuevoconta.ToString() + extension;
            using (var inStream = new MemoryStream(balonBytes))
            using (var imageFactory = new ImageFactory(false))
            {
                var m = imageFactory.Load(inStream)
             .Crop(new Rectangle(154, 259, 614, 643))
               .Save(Patricia);

            }
            containn();
            return Patricia;
        }








        [HttpPost]
        [Route("api/prueba")]
        public async System.Threading.Tasks.Task<string[]> jaggerAsync()
        {
            String solita = "";
            var httpRequest = HttpContext.Current.Request;
            //Upload Image
            var postedFile = httpRequest.Form["Image"];

            
            String imagenconvertida = postedFile;
            String convertida = "";

            if (imagenconvertida.Contains("jpeg"))
            {
                convertida = imagenconvertida.Replace("data:image/jpeg;base64,", String.Empty);
                extension = ".jpeg";

            }
            else
            {
                convertida = imagenconvertida.Replace("data:image/png;base64,", String.Empty);
                extension = ".png";
            }


            String CR7 = Almacenar(conta.ToString(), convertida);
            String Estandar = recorte4(CR7);
            
            String Nit = recorte(Estandar);
            String Serie = recorte2(Estandar);
            String nomb =  recorte3(Estandar);
            
            

            contadorin();
            String[] paises = new String[3] { Nit,Serie,nomb };
            String[] almacenar = new String[paises.Length];

            for (int i = 0; i < paises.Length; i++)
            {
                solita = await OcrrinAsync(paises[i]);
                String con = solita.Replace("\r\n", String.Empty);
                almacenar[i] = con;

            }


           


            

            return almacenar;
        }


        //OCR SSPACE
        public async System.Threading.Tasks.Task<string> OcrrinAsync(String path)
        {
            String Resultado = "";
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);


                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent("69bcefec0e88957"), "apikey"); //Added api key in form data
                form.Add(new StringContent("spa"), "language");


                if (string.IsNullOrEmpty(path) == false)
                {
                    byte[] imageData = File.ReadAllBytes(path);
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image"+extension);
                }


                HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);

                string strContent = await response.Content.ReadAsStringAsync();



                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(strContent);


                if (ocrResult.OCRExitCode == 1)
                {
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        Resultado = ocrResult.ParsedResults[i].ParsedText;
                    }
                }
                else
                {
                    Resultado = "error pendejo";
                }



            }
            catch (Exception exception)
            {
                Console.Write("error");
            }

            return Resultado;
        }
















        
        
        }
}
