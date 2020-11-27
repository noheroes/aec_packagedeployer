using AecPackageDeployer.Models;
using AecPackageDeployer.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AecPackageDeployer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Settings _settings;

        public HomeController(Settings settings, ILogger<HomeController> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Message = "Aplicación preparada.";
            return View();
        }
        [HttpPost]
        public IActionResult Cargar()
        {
            var RutaUSB = _settings.RutaUSB;
            var RutaCarga = _settings.RutaCarga;
            var PrefijoCarga = _settings.PrefijoCarga;
            var ExtensionCarga = _settings.ExtensionCarga;

            #region Validaciones
            if (string.IsNullOrWhiteSpace(RutaUSB))
            {
                ViewBag.Message = $"La aplicación no está configurada correctamente, comuniquese con un responsable de Minedu - OTIC...";
                return View("Index");
            }
            if (Directory.Exists(RutaUSB) == false)
            {
                ViewBag.Message = $"El dispositivo USB no ha sido insertado o no es accesible";
                return View("Index");
            }
            if (string.IsNullOrWhiteSpace(RutaCarga))
            {
                ViewBag.Message = $"La aplicación no está configurada correctamente, comuniquese con un responsable de Minedu - OTIC...";
                return View("Index");
            }
            if (Directory.Exists(RutaCarga) == false)
            {
                ViewBag.Message = $"La ruta de destino de carga no existe o no es accesible";
                return View("Index");
            }
            if (string.IsNullOrWhiteSpace(PrefijoCarga))
            {
                ViewBag.Message = $"La aplicación no está configurada correctamente, comuniquese con un responsable de Minedu - OTIC...";
                return View("Index");
            }
            if (string.IsNullOrWhiteSpace(ExtensionCarga))
            {
                ViewBag.Message = $"La aplicación no está configurada correctamente, comuniquese con un responsable de Minedu - OTIC...";
                return View("Index");
            }
            #endregion

            _logger.LogInformation("Iniciando la carga");

            string errorMessage = null;
            try
            {
                var files = new DirectoryInfo(RutaCarga).GetFiles(PrefijoCarga + "*" + ExtensionCarga);
                if (files.Count() > 0)
                {
                    foreach (var file in files)
                    {
                        var destino = Path.Combine(RutaUSB, Path.GetFileName(file.FullName));
                        System.IO.File.Copy(file.FullName, destino);
                    }
                }
                else
                {
                    errorMessage = $"No se encontraron archivos para cargar...";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error inesperado al realizar la carga.";
                _logger.LogError(ex.Message);
            }
            if (errorMessage == null)
            {
                ViewBag.Message = "Los datos se cargaron satisfactoriamente...";
                _logger.LogInformation("Termino la carga");
            }
            else
            {
                ViewBag.Message = errorMessage;
            }

            return View("Index");
        }

        [HttpPost]
        public IActionResult Descargar()
        {
            var RutaUSB = _settings.RutaUSB;
            var RutaDescarga = _settings.RutaDescarga;
            var PrefijoDescarga = _settings.PrefijoDescarga;
            var ExtensionDescarga = _settings.ExtensionDescarga;

            #region Validaciones
            if (string.IsNullOrWhiteSpace(RutaUSB))
            {
                ViewBag.Message = $"La aplicación no está configurada correctamente, comuniquese con un responsable de Minedu - OTIC...";
                return View("Index");
            }
            if (Directory.Exists(RutaUSB) == false)
            {
                ViewBag.Message = $"El dispositivo USB no ha sido insertado o no es accesible";
                return View("Index");
            }
            if (string.IsNullOrWhiteSpace(RutaDescarga))
            {
                ViewBag.Message = $"La aplicación no está configurada correctamente, comuniquese con un responsable de Minedu - OTIC...";
                return View("Index");
            }
            if (Directory.Exists(RutaDescarga) == false)
            {
                ViewBag.Message = $"La ruta de origen de descarga no existe o no es accesible";
                return View("Index");
            }
            if (string.IsNullOrWhiteSpace(PrefijoDescarga))
            {
                ViewBag.Message = $"La aplicación no está configurada correctamente, comuniquese con un responsable de Minedu - OTIC...";
                return View("Index");
            }
            if (string.IsNullOrWhiteSpace(ExtensionDescarga))
            {
                ViewBag.Message = $"La aplicación no está configurada correctamente, comuniquese con un responsable de Minedu - OTIC...";
                return View("Index");
            }
            #endregion

            _logger.LogInformation("Iniciando la descarga");

            string errorMessage = null;
            try
            {
                var files = new DirectoryInfo(RutaUSB).GetFiles(PrefijoDescarga + "*" + ExtensionDescarga);
                if (files.Count() > 0)
                {
                    foreach (var file in files)
                    {
                        var destino = Path.Combine(RutaDescarga, Path.GetFileName(file.FullName));
                        System.IO.File.Copy(file.FullName, destino);
                    }
                }
                else
                {
                    errorMessage = $"No se encontraron archivos para descargar...";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Error inesperado al procesar la descarga.";
                _logger.LogError(ex.Message);
            }
            if (errorMessage == null)
            {
                ViewBag.Message = "Los datos se descargaron satifactoriamente...";
                _logger.LogInformation("Termino la descarga");
            }
            else
            {
                ViewBag.Message = errorMessage;
            }

            return View("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
