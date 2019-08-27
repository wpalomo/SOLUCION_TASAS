using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATOS;
using ENTIDADES;

namespace NEGOCIO
{
    public class manejadorPueblosHorarios
    {
        ClasePueblosHorarios puebloHorarioD = new ClasePueblosHorarios();

        // CREAMOS LA CLASE PARA LISTAR LAS OFICINAS TERMINALES 
        public List<ENTPueblosHorarios> listarPueblosHorarios(ENTPueblosHorarios dato)
        {
            return puebloHorarioD.listarPueblosHorarios(dato);
        }
    }
}
