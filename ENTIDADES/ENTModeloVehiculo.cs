﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class ENTModeloVehiculo
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

        private string iIDMODELOVEHICULO;

        public string IIDMODELOVEHICULO
        {
            get { return iIDMODELOVEHICULO; }
            set { iIDMODELOVEHICULO = value; }
        }

        private string iSSQL;

        public string ISSQL
        {
            get { return iSSQL; }
            set { iSSQL = value; }
        }
    }
}
