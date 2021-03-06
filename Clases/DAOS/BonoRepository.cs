﻿using ClinicaFrba.Clases.POJOS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TostadoPersistentKit;

namespace ClinicaFrba.Clases.DAOS
{
    public class BonoRepository : Repository
    {
        internal override Type getModelClassType()
        {
            return typeof(Bono);
        }

        internal Bono traerPorId(long numeroBono)
        {
            return (Bono)selectById(numeroBono);
        }

        internal Bono insertarBono(Compra compra)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            DataBase.Instance.agregarParametro(parametros, "@plan_medico", compra.comprador.planMedico.id);
            DataBase.Instance.agregarParametro(parametros, "@compra", compra.id);

            long id_bono = Convert.ToInt64(DataBase.Instance.ejecutarStoredConRetorno("BEMVINDO.st_insertar_bono", parametros, "@id_bono", 0));

            Bono bonoRetorno = new Bono();
            bonoRetorno.id = id_bono;
            bonoRetorno.compra = compra;

            return bonoRetorno;
        }

        internal string verificarSiBonoPuedeSerGastado(Bono bono,Afiliado cliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            DataBase.Instance.agregarParametro(parametros, "id_afiliado", cliente.usuario.id);
            DataBase.Instance.agregarParametro(parametros, "@nro_afiliado", cliente.numeroDeAfiliado);
            DataBase.Instance.agregarParametro(parametros, "@id_bono", bono.id);
            DataBase.Instance.agregarParametro(parametros, "plan_medico", cliente.planMedico.id);

            return DataBase.Instance.ejecutarStoredProcedure("BEMVINDO.st_validar_bono", parametros)[0]["resultado"].ToString();
        }
    }
}
