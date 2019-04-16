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
        [HttpPost]
        [Route("api/Serresiete")]
        public String DevolverOCR7()
        {


            var httpRequest = HttpContext.Current.Request;
            //Upload Image
            var postedFile = httpRequest.Form["Image"];
            String imagenconvertida = postedFile;

            String convertida = imagenconvertida.Replace("data:image/png;base64,", String.Empty);

            
            String CR7 = Almacenar(conta.ToString(), convertida);
            
          
            contadorin();



            return CR7;

        }
        //Metodo que guarda el archivo y extrae el path para que se implemente el OCR
        public String Almacenar(String filename, String base64Image)
        {
            string destinationImgPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\EscaneoFisicoo" + "\\" + filename + ".jpeg";
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

        //Metodo OCR7
        public String ElComandateCR7(String Imagen)
        {
            String Resultado ="";
            AutoOcr OCR = new AutoOcr() { ReadBarCodes = false };
            var Results = OCR.Read(Imagen);

            Resultado = Results.Text;

            return Resultado;
        }



       





        // crea el blob y haria el ocr pero nel
        [HttpPost]
        [Route("api/Autovr2")]
        public String version2()
        {
            var Stocuenta = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(Storage, key), true);
            var blobcliente = Stocuenta.CreateCloudBlobClient();
            var container = blobcliente.GetContainerReference("micontendor");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions()
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            String imageName = null;
            imageName = conta.ToString() + ".png";

            var httpRequest = HttpContext.Current.Request;
            //Upload Image
            var image = httpRequest.Files["Image"];
            //Create custom filename
             var pati = String.Format("Got image {0} of type {1} and size {2}",
            image.FileName, image.ContentType, image.ContentLength);

            string uniqueBlobName = string.Format("productimages/image_{0}",imageName
            );
            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
            blob.Properties.ContentType = image.ContentType;
            blob.UploadFromStream(image.InputStream);



            String cosa = "image_"+imageName;

            String nueva = ElComandateCR7("https://ponysalvaje.blob.core.windows.net/micontendor/productimages/"+cosa);





            return nueva;

        }


        public String recorte(String pati)
        {
            var balonBytes = File.ReadAllBytes(pati);
            String Patricia = tempora + nuevoconta.ToString() + ".jpeg";
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
            String Patricia = tempora + nuevoconta.ToString() + ".jpeg";
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
            String Patricia = tempora + nuevoconta.ToString() + ".jpeg";
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
            String Patricia = tempora + nuevoconta.ToString() + ".jpeg";
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
        public async System.Threading.Tasks.Task<string []> jaggerAsync()
        {
            String solita = "";
            var httpRequest = HttpContext.Current.Request;
            //Upload Image
            var postedFile = httpRequest.Form["Image"];
            String imagenconvertida = postedFile;

            String convertida = imagenconvertida.Replace("data:image/jpeg;base64,", String.Empty);
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
                form.Add(new StringContent("cambia el api"), "apikey"); //Added api key in form data
                form.Add(new StringContent("spa"), "language");


                if (string.IsNullOrEmpty(path) == false)
                {
                    byte[] imageData = File.ReadAllBytes(path);
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image.jpeg");
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

            }

            return Resultado;
        }

















        [HttpPost]
        [Route("api/UploadImage")]
        public String cosa()
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;
            //Upload Image
            var postedFile = httpRequest.Files["Image"];
            //Create custom filename
            imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
            var filePath = HttpContext.Current.Server.MapPath("~/Image/" + imageName);
            postedFile.SaveAs(filePath);


            Image image = new Bitmap(filePath);
            Image image2 = new Bitmap(filePath);
            Image image3 = new Bitmap(filePath);


            String soca = "0";

            return soca;
        }
        
        }
}
