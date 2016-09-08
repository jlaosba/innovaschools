using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InnovaSchools.Models;

namespace InnovaSchools.Controllers
{
    public class GenerarContratoController : Controller
    {
        private InnovaSchoolsContext _db = new InnovaSchoolsContext();

        // <summary>
        // Permite generar el contrato del candidato y/o empleado
        // </summary>
        // <returns>Fecha Creacion      : 06/09/0216 | M. MAURICIO</remarks>
        // <remarks>Fecha Modificacion  : 06/09/0216 | M. MAURICIO</remarks>
        public ActionResult index()
        {
            GenerarContratoViewModel GenerarContrato = new GenerarContratoViewModel();
            GenerarContrato.PuestoList = (from entry in _db.Puesto orderby entry.idPuesto ascending select entry).Take(20).ToList();
            GenerarContrato.Candidato = new Candidato();
            GenerarContrato.Candidatos = new List<Candidato>();
            return View(GenerarContrato);            
        }

        // <summary>
        // Listar Candidatos
        // </summary>
        // <returns>Fecha Creacion      : 06/09/0216 | M. MAURICIO</remarks>
        // <remarks>Fecha Modificacion  : 06/09/0216 | M. MAURICIO</remarks>
        public ActionResult lstCandidatos(string pDocumentoIdentidad, string pNombre, string pApePaterno, string pApeMaterno, Int16 pIdPuesto)
        {
            GenerarContratoViewModel GenerarContratoViewModel = new GenerarContratoViewModel()
            {
                Candidato = new Candidato()
            };
            GenerarContratoViewModel.Candidatos = new List<Candidato>();
            
            var objPersona =
                       from cvt in _db.Convocatoria
                       join ccd in _db.ConvocatoriaCandidato on cvt.idConvocatoria equals ccd.idConvocatoria
                       join cnd in _db.Candidato on ccd.idCandidato equals cnd.idCandidato
                       join pst in _db.Puesto on cvt.Puesto.idPuesto equals pst.idPuesto
                       join per in _db.Persona on cnd.Persona.idPersona equals per.idPersona
                       select new { Candidato = cnd, Convocatoria = cvt, Puesto = pst, Persona = per };

            //var objPersona =
            //           from cnd in _db.Candidato
            //           join cvt in _db.Convocatoria on cnd.idConvocatoria equals cvt.idConvocatoria
            //           join cvt in _db.Convocatoria on cnd.idConvocatoria equals cvt.idConvocatoria
            //           join pst in _db.Puesto on cvt.Puesto.idPuesto equals pst.idPuesto
            //           join per in _db.Persona on cnd.Persona.idPersona equals per.idPersona
            //           select new { Candidato = cnd, Convocatoria = cvt, Puesto = pst, Persona = per };

            if (pDocumentoIdentidad.Trim().LongCount() > 0) { objPersona = objPersona.Where(x => x.Persona.documentoIdentidad.Contains(pDocumentoIdentidad)); }
            if (pNombre.Trim().LongCount() > 0) { objPersona = objPersona.Where(x => x.Persona.nombre.Contains(pNombre)); }
            if (pApePaterno.Trim().LongCount() > 0) { objPersona = objPersona.Where(x => x.Persona.apellidoPaterno.Contains(pApePaterno)); }
            if (pApeMaterno.Trim().LongCount() > 0) { objPersona = objPersona.Where(x => x.Persona.apellidoMaterno.Contains(pApeMaterno)); }
            if (pIdPuesto > 0) { objPersona = objPersona.Where(x => x.Convocatoria.Puesto.idPuesto == pIdPuesto); }

            foreach (var itmPrs in objPersona)
            {
                if (GenerarContratoViewModel.Candidatos.Where(w => w.Persona.idPersona == itmPrs.Persona.idPersona).ToList().Count == 0)
                {
                    GenerarContratoViewModel.Candidatos.Add(new Candidato
                    {
                        Persona = new Persona
                        {
                            fecha_inicio = itmPrs.Convocatoria.fechaInicioPublicacion.ToShortDateString(),
                            fecha_fin = itmPrs.Convocatoria.fechaFinPublicacion.ToShortDateString(),
                            idPersona = itmPrs.Persona.idPersona,
                            nombre = itmPrs.Persona.nombre,
                            apellidoPaterno = itmPrs.Persona.apellidoPaterno,
                            apellidoMaterno = itmPrs.Persona.apellidoMaterno,
                            telefono = itmPrs.Persona.telefono,
                            direccion = itmPrs.Persona.direccion,
                            documentoIdentidad = itmPrs.Persona.documentoIdentidad
                        },
                        //Convocatoria = new Convocatoria
                        //{
                        //    fechaVencimientoDocumento = itmPrs.Convocatoria.fechaVencimientoDocumento,
                        //    idPuesto = itmPrs.Convocatoria.Puesto.idPuesto,
                        //    Puesto = new Puesto { descripcionPuesto = itmPrs.Puesto.descripcionPuesto },
                        //}
                    });
                }
            }
            if (GenerarContratoViewModel.Candidatos.Count() == 0)
            {
                GenerarContratoViewModel.resultadoFind = string.Concat("No se encontraron resultado en busqueda");
            }
            else
            {
                //VerificarDocumentacionViewModel.resultadoFind = string.Concat("Resultado de busqueda: De ", fechaInicio.ToShortDateString(), " hasta ", fechaFin.ToShortDateString());
                GenerarContratoViewModel.resultadoFind = string.Concat("Resultado de busqueda");
            }
            return PartialView("_lstGenerarContrato", GenerarContratoViewModel);
        }

        // <summary>
        // Obtener Contrato
        // </summary>
        // <returns>Fecha Creacion      : 06/09/0216 | M. MAURICIO</remarks>
        // <remarks>Fecha Modificacion  : 06/09/0216 | M. MAURICIO</remarks>
        public ActionResult getContrato(int pIdPersona)
        {

            //var objPersona =
            //           from per in _db.Persona
            //           join cdt in _db.Candidato on per.idPersona equals cdt.Persona.idPersona
            //           join pst in _db.Puesto on cdt.Convocatoria.Puesto.idPuesto equals pst.idPuesto
            //           join emp in _db.Empleado on cdt.Empleado.idEmpleado equals emp.idEmpleado
            //           join ctt in _db.Contrato on emp.Contrato.idContrato equals ctt.idContrato
            //           where cdt.Persona.idPersona == pIdPersona
            //           select new { Persona = per, Candidato = cdt, Puest = pst };

            GenerarContratoViewModel GenerarContratoViewModel = new GenerarContratoViewModel();

            //foreach (var itm in objPersona)
            //{
            //    GenerarContratoViewModel.Candidato = new Candidato
            //    {
            //        //codigo_persona = itm.Persona.codigo_persona,
            //        rutaImgDni = itm.Candidato.rutaImgDni,
            //        rutaImgDeclaracionJurada = itm.Candidato.rutaImgDeclaracionJurada,
            //        rutaImgAntecedentesPenales = itm.Candidato.rutaImgAntecedentesPenales,
            //        rutaImgAntecedentesPoliciales = itm.Candidato.rutaImgAntecedentesPoliciales,
            //        rutaImgTituloProfesional = itm.Candidato.rutaImgTituloProfesional,
            //        Persona = new Persona
            //        {
            //            idPersona = itm.Persona.idPersona,
            //            nombre = itm.Persona.nombre,
            //            apellidoPaterno = itm.Persona.apellidoPaterno,
            //            apellidoMaterno = itm.Persona.apellidoMaterno,
            //            direccion = itm.Persona.direccion,
            //            telefono = itm.Persona.telefono,
            //            celular = itm.Persona.celular,
            //            correoElectronico = itm.Persona.correoElectronico,
            //            documentoIdentidad = itm.Persona.documentoIdentidad
            //        },
            //        Convocatoria = new Convocatoria
            //        {
            //            idPuesto = itm.Candidato.Convocatoria.idPuesto,
            //            Puesto = new Puesto { descripcionPuesto = itm.Puest.descripcionPuesto },
            //        }

            //    };
            //};

            GenerarContratoViewModel.PuestoList = (from entry in _db.Puesto orderby entry.idPuesto ascending select entry).Take(20).ToList();
            //GenerarContratoViewModel.SelectedPuestoId = GenerarContratoViewModel.Candidato.Convocatoria.idPuesto;

            return View(GenerarContratoViewModel);
        }
    }
}
