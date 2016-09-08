using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InnovaSchools.Models;
using System.IO;

namespace InnovaSchools.Controllers
{
    public class VerificarDocumentacionController : Controller
    {
        private InnovaSchoolsContext _db = new InnovaSchoolsContext();

        // <summary>
        // Verificar la documentación del Personal
        // </summary>
        // <returns>Fecha Creacion      : 29/08/0216 | C. Quiroz</remarks>
        // <remarks>Fecha Modificacion  : 29/08/0216 | C. Quiroz</remarks>
        public ActionResult index()
        {
            VerificarDocumentacionViewModel VerificarDocumentacion = new VerificarDocumentacionViewModel();
            VerificarDocumentacion.PuestoList = (from entry in _db.Puesto orderby entry.idPuesto ascending select entry).Take(20).ToList();
            VerificarDocumentacion.EstadoCandidatoList = (from entry in _db.EstadoCandidato orderby entry.idEstadoCandidato ascending select entry).Take(20).ToList();
            VerificarDocumentacion.Convocatoria = new Convocatoria();
            VerificarDocumentacion.Convocatorias = new List<Convocatoria>();
            //VerificarDocumentacion.Candidato = new Candidato();
            //VerificarDocumentacion.Candidatos = new List<Candidato>();
            return View(VerificarDocumentacion);
        }
        
        // <summary>
        // Listar Candidatos
        // </summary>
        // <returns>Fecha Creacion      : 29/08/0216 | C. Quiroz</remarks>
        // <remarks>Fecha Modificacion  : 29/08/0216 | C. Quiroz</remarks>
        public ActionResult lstCandidatos(string pFechaInicio, string pFechaFin, string pNombre, string pApePaterno, Int16 pIdPuesto, Int16 pIdEstadoCandidato)
        {
            var fechaInicio = Convert.ToDateTime(pFechaInicio);
            var fechaFin = Convert.ToDateTime(pFechaFin);

            VerificarDocumentacionViewModel VerificarDocumentacionViewModel = new VerificarDocumentacionViewModel()
            {
                //Candidato = new Candidato()
            };
            VerificarDocumentacionViewModel.Convocatorias = new List<Convocatoria>();

            var objPersona =
                       from cvt in _db.Convocatoria
                       join ccd in _db.ConvocatoriaCandidato on cvt.idConvocatoria equals ccd.idConvocatoria
                       join cnd in _db.Candidato on ccd.idCandidato equals cnd.idCandidato
                       join pst in _db.Puesto on cvt.Puesto.idPuesto equals pst.idPuesto
                       join per in _db.Persona on cnd.Persona.idPersona equals per.idPersona
                       //where (cvt.fechaInicioPublicacion >= fechaInicio && cvt.fechaFinPublicacion <= fechaFin) 
                       select new { Candidato = cnd, Convocatoria = cvt, Puesto = pst, Persona = per };

            //var objPersona =
            //           from cnd in _db.Candidato
            //           join cvt in _db.Convocatoria on cnd.idConvocatoria equals cvt.idConvocatoria
            //           join pst in _db.Puesto on cvt.Puesto.idPuesto equals pst.idPuesto
            //           join per in _db.Persona on cnd.Persona.idPersona equals per.idPersona
            //           //where (cvt.fechaInicioPublicacion >= fechaInicio && cvt.fechaFinPublicacion <= fechaFin) 
            //           select new { Candidato = cnd, Convocatoria = cvt, Puesto = pst, Persona = per };

            if (pNombre.Trim().LongCount() > 0) { objPersona = objPersona.Where(x => x.Persona.nombre.Contains(pNombre)); }
            if (pApePaterno.Trim().LongCount() > 0) { objPersona = objPersona.Where(x => x.Persona.apellidoPaterno.Contains(pApePaterno)); }
            if (pIdPuesto > 0) { objPersona = objPersona.Where(x => x.Convocatoria.Puesto.idPuesto == pIdPuesto); }
            if (pIdEstadoCandidato > 0) { objPersona = objPersona.Where(x => x.Candidato.idEstadoCandidato == pIdEstadoCandidato); }

            foreach (var itm in objPersona)
            {

                //&&
                //    _db.ProgramarPersonal.Where(w => w.fecha >= fechaInicio &&
                //                                       w.fecha <= fechaFin).Count() == 0

                //if (VerificarDocumentacionViewModel.Candidatos.Where(w => w.Persona.idPersona == itmPrs.Persona.idPersona).ToList().Count == 0)
                //{
                //    VerificarDocumentacionViewModel.Candidatos.Add(new Candidato
                //    {                        
                //        Persona = new Persona
                //        {
                //            fecha_inicio = itmPrs.Convocatoria.fechaInicioPublicacion.ToShortDateString(),
                //            fecha_fin = itmPrs.Convocatoria.fechaFinPublicacion.ToShortDateString(),
                //            idPersona = itmPrs.Persona.idPersona,
                //            nombre = itmPrs.Persona.nombre,
                //            apellidoPaterno = itmPrs.Persona.apellidoPaterno,
                //            apellidoMaterno = itmPrs.Persona.apellidoMaterno,
                //            telefono = itmPrs.Persona.telefono,
                //            direccion = itmPrs.Persona.direccion,
                //            documentoIdentidad = itmPrs.Persona.documentoIdentidad
                //        },
                //        //Convocatoria = new Convocatoria
                //        //{
                //        //    fechaVencimientoDocumento = itmPrs.Convocatoria.fechaVencimientoDocumento,
                //        //    idPuesto = itmPrs.Convocatoria.idPuesto,
                //        //    Puesto = new Puesto { descripcionPuesto = itmPrs.Puesto.descripcionPuesto },
                //        //}
                //    });
                //}


                if (VerificarDocumentacionViewModel.Convocatorias.Where(w => w.ConvocatoriaCandidato.Candidato.Persona.idPersona == itm.Persona.idPersona).ToList().Count == 0)
                {
                    VerificarDocumentacionViewModel.Convocatorias.Add(new Convocatoria
                    {
                        fechaVencimientoDocumento = itm.Convocatoria.fechaVencimientoDocumento,
                        ConvocatoriaCandidato = new ConvocatoriaCandidato
                        {
                            Candidato = new Candidato
                            {
                                Persona = new Persona
                                {
                                    fecha_inicio = itm.Convocatoria.fechaInicioPublicacion.ToShortDateString(),
                                    fecha_fin = itm.Convocatoria.fechaFinPublicacion.ToShortDateString(),
                                    idPersona = itm.Persona.idPersona,
                                    nombre = itm.Persona.nombre,
                                    apellidoPaterno = itm.Persona.apellidoPaterno,
                                    apellidoMaterno = itm.Persona.apellidoMaterno,
                                    telefono = itm.Persona.telefono,
                                    direccion = itm.Persona.direccion,
                                    documentoIdentidad = itm.Persona.documentoIdentidad,
                                },
                                //Convocatoria = new Convocatoria
                                //{
                                //    fechaVencimientoDocumento = itmPrs.Convocatoria.fechaVencimientoDocumento,
                                //    idPuesto = itmPrs.Convocatoria.idPuesto,
                                //    Puesto = new Puesto { descripcionPuesto = itmPrs.Puesto.descripcionPuesto },
                                //}
                            }
                        },
                        Puesto = new Puesto
                        {
                            idPuesto = itm.Puesto.idPuesto,
                            descripcionPuesto = itm.Puesto.descripcionPuesto,
                        }
                    });
                }






            }
            if (VerificarDocumentacionViewModel.Convocatorias.Count() == 0)
            {
                VerificarDocumentacionViewModel.resultadoFind = string.Concat("No se encontraron resultado en busqueda");
            }
            else
            {
                VerificarDocumentacionViewModel.resultadoFind = string.Concat("Resultado de busqueda: De ", fechaInicio.ToShortDateString(), " hasta ", fechaFin.ToShortDateString());
            }
            return PartialView("_lstVerificarDocumento", VerificarDocumentacionViewModel);
        }

        // <summary>
        // Obtener Contrato
        // </summary>
        // <returns>Fecha Creacion      : 29/08/0216 | C. Quiroz</remarks>
        // <remarks>Fecha Modificacion  : 29/08/0216 | C. Quiroz</remarks>
        public ActionResult getContrato(int pIdPersona)
        {
            var objPersona =
                      from cvt in _db.Convocatoria
                      join ccd in _db.ConvocatoriaCandidato on cvt.idConvocatoria equals ccd.idConvocatoria
                      join cnd in _db.Candidato on ccd.idCandidato equals cnd.idCandidato
                      join pst in _db.Puesto on cvt.Puesto.idPuesto equals pst.idPuesto
                      join per in _db.Persona on cnd.Persona.idPersona equals per.idPersona
                      select new { Candidato = cnd, Convocatoria = cvt, Puesto = pst, Persona = per };

            //var objPersona =
            //           from per in _db.Persona
            //           join cdt in _db.Candidato on per.idPersona equals cdt.Persona.idPersona
            //           join cvt in _db.Convocatoria on cdt.idConvocatoria equals cvt.idConvocatoria
            //           join pst in _db.Puesto on cvt.Puesto.idPuesto equals pst.idPuesto
            //           where cdt.Persona.idPersona == pIdPersona
            //           select new { Persona = per, Candidato = cdt, Puest = pst };

            VerificarDocumentacionViewModel verificarDocumento = new VerificarDocumentacionViewModel();

            foreach (var itm in objPersona)
            {
                //verificarDocumento.Candidato = new Candidato
                //{
                //    rutaImgDni = itm.Candidato.rutaImgDni,
                //    rutaImgDeclaracionJurada = itm.Candidato.rutaImgDeclaracionJurada,
                //    rutaImgAntecedentesPenales = itm.Candidato.rutaImgAntecedentesPenales,
                //    rutaImgAntecedentesPoliciales = itm.Candidato.rutaImgAntecedentesPoliciales,
                //    rutaImgTituloProfesional = itm.Candidato.rutaImgTituloProfesional,
                //    Persona = new Persona
                //    {
                //        idPersona = itm.Persona.idPersona,
                //        nombre = itm.Persona.nombre,
                //        apellidoPaterno = itm.Persona.apellidoPaterno,
                //        apellidoMaterno = itm.Persona.apellidoMaterno,
                //        direccion = itm.Persona.direccion,
                //        telefono = itm.Persona.telefono,
                //        celular = itm.Persona.celular,                        
                //        correoElectronico = itm.Persona.correoElectronico,
                //        documentoIdentidad = itm.Persona.documentoIdentidad
                //    },
                //    //Convocatoria = new Convocatoria
                //    //{
                //    //    idPuesto = itm.Candidato.Convocatoria.idPuesto,
                //    //    Puesto = new Puesto { descripcionPuesto = itm.Puesto.descripcionPuesto },
                //    //}
                    
                //};                
            };

            verificarDocumento.PuestoList = (from entry in _db.Puesto orderby entry.idPuesto ascending select entry).Take(20).ToList();
            //verificarDocumento.SelectedPuestoId = verificarDocumento.Candidato.Convocatoria.idPuesto;

            return View(verificarDocumento);
            //return RedirectToAction("Index");
        }

        // <summary>
        // Actualizar Documentos de Contrato
        // </summary>
        // <returns>Fecha Creacion      : 29/08/0216 | C. Quiroz</remarks>
        // <remarks>Fecha Modificacion  : 29/08/0216 | C. Quiroz</remarks>
        public JsonResult setContrato(int pIdPersona, string pNombre, string pApePaterno, string pApeMaterno, string pDireccion, string pTelefono, string pCelular, Int16 pIdPuesto, string pEmail,
                                      string pRutaImgDni, string pRutaImgDomiciliaria, string pRutaImgPenales, string pRutaImgPoliciales, string pRutaImgtitulo,
                                      HttpPostedFileBase Input_folder_Dni)
        {
            VerificarDocumentacionViewModel VerificarDocumentacion = new VerificarDocumentacionViewModel();

            var objCandidato = _db.Candidato.First(x => x.Persona.idPersona == pIdPersona);
            objCandidato.rutaImgDni = Path.GetFileName(pRutaImgDni);
            objCandidato.rutaImgDeclaracionJurada = Path.GetFileName(pRutaImgDomiciliaria);
            objCandidato.rutaImgAntecedentesPenales = Path.GetFileName(pRutaImgPenales);
            objCandidato.rutaImgAntecedentesPoliciales = Path.GetFileName(pRutaImgPoliciales);
            objCandidato.rutaImgTituloProfesional = Path.GetFileName(pRutaImgtitulo);
            
            //var objPersona = _db.Persona.First(x => x.codigo_persona == pDni);
            //objPersona.nombre = pNombre;
            //objPersona.apellido_paterno = pApePaterno;
            //objPersona.apellido_materno = pApeMaterno;
            //objPersona.direccion = pDireccion;
            //objPersona.telefono = pTelefono;
            //objPersona.celular = pCelular;
            //objPersona.id_puesto = pIdPuesto;
            //objPersona.email = pEmail;

            
            //string archivo = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + file.FileName).ToLower();
            //file.SaveAs(Server.MapPath("~/Uploads/" + archivo));

            //if (input_folder_Dni != null && input_folder_Dni.ContentLength > 0)
            //{
            //    // extract only the filename
            //    var fileName = Path.GetFileName(input_folder_Dni.FileName);
            //    // store the file inside ~/App_Data/uploads folder
            //    var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
            //    input_folder_Dni.SaveAs(path);
            //}


            //if (Request != null)
            //{
            //    HttpPostedFileBase file = Request.Files["fileDni"];

            //    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            //    {
            //        string fileName = file.FileName;
            //        string fileContentType = file.ContentType;
            //        byte[] fileBytes = new byte[file.ContentLength];
            //        file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
            //    }
            //}

            //if (pInput_folder_Dni != null)
            //{
            //    foreach (var file in pInput_folder_Dni)
            //    {
            //        // Verify that the user selected a file
            //        if (file != null && file.ContentLength > 0)
            //        {
            //            // extract only the fielname
            //            var fileName = Path.GetFileName(file.FileName);
            //            // TODO: need to define destination
            //            var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
            //            file.SaveAs(path); //upload for example
            //        }
            //    }
            //}

            _db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
            //return View(true);
        }


        [HttpPost]
        public void Upload()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];

                var fileName = Path.GetFileName(file.FileName);

                var path = Path.Combine(Server.MapPath("~/Junk/"), fileName);
                file.SaveAs(path);
            }

        }
        public JsonResult ImportCsv(IEnumerable<HttpPostedFileBase> files, String cid)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Verify that the user selected a file
                    if (file != null && file.ContentLength > 0)
                    {
                        // extract only the fielname
                        var fileName = Path.GetFileName(file.FileName);
                        // TODO: need to define destination
                        var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                        file.SaveAs(path); //upload for example
                    }
                }
            }

            //String jsonStr = JsonConvert.SerializeObject(ipRestriction);
            //return Json(jsonStr, JsonRequestBehavior.AllowGet); //Now return Json as you need.
            return Json(true, JsonRequestBehavior.AllowGet); //Now return Json as you need.
        }






    }
}
