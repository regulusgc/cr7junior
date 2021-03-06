﻿using IronOcr;
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
using OCRTODOPOWER;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Ocr7siu.Controllers
{
  

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImagenReController : ApiController
    {
        public static int conta=1;
        public static int nuevoconta = 1;
        public  String extension = "sedefineluego";

        const String Storage = "ponysalvaje";
        const String key = "";
        public string tempora = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\temporal" + "\\";
        public string borrar = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\temporal";
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
             .Crop(new Rectangle(481, 112, 109, 52))
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
             .Crop(new Rectangle(221, 570, 124, 80))//y590
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
             .Crop(new Rectangle(151, 76, 440, 52))
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
            String[] splitextension = imagenconvertida.Split(',');
            if (splitextension[0].Contains("jpeg"))
            {
                convertida = imagenconvertida.Replace("data:image/jpeg;base64,", String.Empty);
                extension = ".jpeg";

            }
            else if ((splitextension[0].Contains("jpg")))
            {
                extension = ".jpg";
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
//<<<<<<< HEAD
                solita = await MakeOCRrequest(paises[i]);
                String con = solita.Replace("\r\n", String.Empty);
//=======
                //solita = await OcrrinAsync(paises[i]);
                //String con = solita.Replace("\r\n", String.Empty).Replace("!", String.Empty); ;
                
//>>>>>>> fb18e5b3efeb4f1a0591aa291fc1f25ff36fbb7d
                almacenar[i] = con;

            }


            //for (int i = 0; i < paises.Length; i++)
            //{
            //    solita = await OcrrinAsync(paises[i]);
            //    // String con = solita.Replace("\r\n", String.Empty);
            //    almacenar[i] = solita;
            //}
            //   OcrrinAsync(paises[0]).Wait();
            //System.IO.File.Delete(CR7);
            //System.IO.File.Delete(Estandar);
            //System.IO.File.Delete(Nit);
            //System.IO.File.Delete(Serie);
            //System.IO.File.Delete(nomb);




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





        //public String ElComandateCR7(String Imagen)
        //{
        //    String Resultado = "";

        //    var Ocr = new AdvancedOcr()
        //    {
        //        CleanBackgroundNoise = true,
        //        EnhanceContrast = true,
        //        EnhanceResolution = true,
        //        Language = IronOcr.Languages.Spanish.OcrLanguagePack,
        //        Strategy = IronOcr.AdvancedOcr.OcrStrategy.Advanced,
        //        ColorSpace = AdvancedOcr.OcrColorSpace.GrayScale,
        //        DetectWhiteTextOnDarkBackgrounds = false,
        //        InputImageType = AdvancedOcr.InputTypes.Document,
        //        RotateAndStraighten = true,
        //        ReadBarCodes = false,
        //        ColorDepth = 4
        //    };

        //    //  AutoOcr OCR = new AutoOcr() { ReadBarCodes = false };
        //    var Results = Ocr.Read(Imagen);

        //    Resultado = Results.Text;

        //    return Resultado;
        //}

        ////ocr que no nos dejara tirados
        //public String OCRAZURE(String path)
        //{
        //  return new OCR().HacerOCR(path);
        //}



        //[HttpPost]
        //[Route("api/IronCr7")]
        //public String[] OCRironsito()
        //{
        //    String solita = "";
        //    var httpRequest = HttpContext.Current.Request;
        //    //Upload Image
        //    var postedFile = httpRequest.Form["Image"];


        //    String imagenconvertida = postedFile;
        //    String convertida = "";

        //    if (imagenconvertida.Contains("jpeg"))
        //    {
        //        convertida = imagenconvertida.Replace("data:image/jpeg;base64,", String.Empty);
        //        extension = ".jpeg";

        //    }
        //    else if (imagenconvertida.Contains("jpg"))
        //    {
        //        extension = ".jpg";
        //    }
        //    else
        //    {
        //        convertida = imagenconvertida.Replace("data:image/png;base64,", String.Empty);
        //        extension = ".png";
        //    }


        //    String CR7 = Almacenar(conta.ToString(), convertida);
        //    String Estandar = recorte4(CR7);

        //    String Nit = recorte(Estandar);
        //    String Serie = recorte2(Estandar);
        //    String nomb = recorte3(Estandar);



        //    contadorin();
        //    String[] paises = new String[3] { Nit, Serie, nomb };
        //    String[] almacenar = new String[paises.Length];

           

        //    for (int i = 0; i < paises.Length; i++)
        //    {
        //        solita = ElComandateCR7(paises[i]);
        //        String con = solita.Replace("\r\n", String.Empty);
        //        almacenar[i] = con;
        //    }


        //    //System.IO.File.Delete(CR7);

        //    //string[] filePaths = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\temporal"+"\\");
        //    //foreach (string filePath in filePaths)
        //    //{
        //    //    File.SetAttributes(filePath, FileAttributes.Normal);
        //    //    File.Delete(filePath);

        //    //}




        //    return almacenar;
        //}









        public async System.Threading.Tasks.Task<string> MakeOCRrequest (string imageFilePath)
        {
            const string subscriptionKey = "2af84a9c8a9c4c45a8dada9fb0d48fa3";

            const string uriBase =
                "https://australiaeast.api.cognitive.microsoft.com/vision/v2.0/ocr";

            string resultado = "";

            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);


                string requestParameters = "language=es&detectOrientation=true";

                // se arma la url
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Read the contents of the specified local image
                // into a byte array.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                // Add the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(uri, content);
                }

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

        
                OcrAzureResults ocrResult = JsonConvert.DeserializeObject<OcrAzureResults>(contentString);//se coviente en un objeto del Tipo OcrAzureResults

                 //se recorreo para obtener el texto
                for (int i = 0; i < ocrResult.regions.Count(); i++)
                {
                   
                    for (int j = 0; j < ocrResult.regions[i].lines.Count(); j++)
                    {
                        for (int k = 0; k < ocrResult.regions[i].lines[j].words.Count(); k++)
                        {
                            resultado += ocrResult.regions[i].lines[j].words[k].text+" ";
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }

            return resultado.Trim();//si hay espacios al final es mejor quitarlos

        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        private byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }






    }
}
