using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIP
{
    public class NotasFunciones
    {

        /*ARCHIVO QUE ALMACENA FUNCIONALIDADES IMPORTANTES
         
         */

        #region FUNCIONES BASICAS


         /*AUTOR                              NOMBRE FUNCIONALIDAD                                                         UBICACION                                           DESCRIPCION BREVE
         * ****************************************************************************************************************************************************************************************************************************
         *
         * RIGO                               Subir Archivo                                                 Formas/Poa/EvaluacionPoa.aspx                         se realizo una funcion que permite subir un archivo de x formato
         * 
         * MANOEL                             CARGA DE COMBOS ESCALONADOS                                   Formas/Poa/POA.aspx                                   Se realizó carga de combos escalonados con ajax toolkit. 
         * 
         * RIGO                               CREACION DE COLUMNAS Y CELDAS DINAMICAMENTE                   Formas/Poa/POAAjustadoEvaluacion.aspx                 Se crean columnas y celdas en un grid de manera dinamica
         *                                    EN GRID                                                                                                             Nombre metodos: InsertarTitulosNuevasColumnas() y InsertarNuevasCeldas() ubicados en codigo de SERVIDOR
         * 
         * RIGO                               COLOCACION DE EVENTO "CLICK" EN UN BOTON DESDE C#             Formas/POA/EvaluacionPOA.aspx                         Se crea evento "onclick" desde C# para que se ejecute en JAVASCRIPT.
         *                                    PARA EJECUTARSE EN JAVASCRIPT                                                                                       Nombre metodo: grid_RowDataBound(); Ejemplo: btnVer.Attributes["onclick"] = "fnc_AbrirArchivo('" + ruta + "'," + id + ")";
         *
         * RIGO                               PROPIEDAD PARA HABILITAR/DESHABILITAR UN CONTROL              Formas/POA/EvaluacionPOA.aspx                         Funcion para habilitar/deshabiltar un control de asp, html desde jQuery. 
         *                                    DESDE JQUERY                                                                                                        Nombre metodo: fnc_InhabilitarCampos(), ubicado en codigo de CLIENTE. Ejemplo: $("#<%= txtObservacionesPregunta.ClientID %>").prop("disabled",true);
         *
         * RIGO                               MANERA DE AVERIGUAR CUANDO UN CONTROL CHECK O                 Formas/POA/EvaluacionPOA.aspx                         Funcion para saber si un control CHECKBOX o RADIOBUTTON estan marcados o no desde JQuery
         *                                    RADIOBUTTON ESTAN MARCADOS O NO EN JQUERY                                                                           Nombre metodo: fnc_DesmarcarChecks(), ubicado en codigo de CLIENTE. Ejemplo $(sender).is(':checked')
         * 
         * 
         * REY                                ACORDEON DINAMICO                                             Formas/TechoFin/wfTechoFinanciero                     En esta forma se encuentra el componente acordion de bootstrap cargado dinamicamente en el codebehind de la forma, lo cual permite agrupar por cada movimiento de la bitacora, y ocultar y mostrar el detalle que la integra.  
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         */

        #endregion

        #region FUNCIONES LINQ

        /*  AUTOR                  NOMBRE FUNCIONALIDAD                                           UBICACION                                           DESCRIPCION BREVE
         * **************************************************************************************************************************************************************************************************
         * 
         * RIGO                    LEFT JOIN CONSULTA DE LINQ                       Formas/POA/EvaluacionPOA.aspx                                   Se realiza LEFT JOIN en una consulta de LINQ, para obtener diferencias utilizando varias listas en las uniones.            
         *                                                                                                                                          Nombre metodo: CrearPreguntasInexistentes(), ubicado en codigo de SERVIDOR
         * 
         * 
         * 
         * 
         * 
         */


        #endregion



    }
}