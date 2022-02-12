﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasApi.Controllers;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeliculasApi.Tests.PruebasUnitarias
{
    [TestClass]
    public class SalasDeCineControllerTests : BasePruebas
    {
        //[TestMethod]
        //public async Task ObtenerSalasDeCineA5KmOMenos()
        //{
        //    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        //    using (var context = LocalDbDatabaseInitializer.GetDbContextLocalDb(false))
        //    {
        //        var SalasDeCine = new List<SalaDeCine>()
        //        {
        //            new SalaDeCine{ Nombre = "Agora", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-69.9388777, 18.4839233))}
        //        };

        //        context.AddRange(SalasDeCine);
        //        await context.SaveChangesAsync();
        //    }

        //    var filtro = new SalaDeCineCercanoFiltroDTO()
        //    {
        //        DistanciaEnKms = 5,
        //        Latitud = 18.481139,
        //        Longitud = -69.938950
        //    };


        //    using (var context = LocalDbDatabaseInitializer.GetDbContextLocalDb(false))
        //    {
        //        var mapper = ConfigurarAutoMapper();
        //        var controller = new SalasDeCineController(context, mapper, geometryFactory);
        //        var respuesta = await controller.Cercanos(filtro);
        //        var valor = respuesta.Value;
        //        Assert.AreEqual(1, valor.Count);
        //    }
        //}
    }
}
