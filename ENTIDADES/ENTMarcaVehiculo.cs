using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class ENTMarcaVehiculo
    {
        private string iCODIGO;

        public string ICODIGO
        {
            get { return iCODIGO; }
            set { iCODIGO = value; }
        }

        private string iDESCRIPCION;

        public string IDESCRIPCION
        {
            get { return iDESCRIPCION; }
            set { iDESCRIPCION = value; }
        }

        private string iESTADO;

        public string IESTADO
        {
            get { return iESTADO; }
            set { iESTADO = value; }
        }

        private string iIDMARCAVEHICULO;

        public string IIDMARCAVEHICULO
        {
            get { return iIDMARCAVEHICULO; }
            set { iIDMARCAVEHICULO = value; }
        }

        private string sSQL;

        public string SSQL
        {
            get { return sSQL; }
            set { sSQL = value; }
        }
    }
}
