using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class ENTTipoComprobante
    {
        private string iCODIGO;

        public string ICODIGO
        {
            get { return iCODIGO; }
            set { iCODIGO = value; }
        }

        private string iESTADO;

        public string IESTADO
        {
            get { return iESTADO; }
            set { iESTADO = value; }
        }

        private string iID_TIPO_COMPROBANTE;

        public string IID_TIPO_COMPROBANTE
        {
            get { return iID_TIPO_COMPROBANTE; }
            set { iID_TIPO_COMPROBANTE = value; }
        }

        private string iNOMBRES;

        public string INOMBRES
        {
            get { return iNOMBRES; }
            set { iNOMBRES = value; }
        }

        private string iNUMERO;

        public string INUMERO
        {
            get { return iNUMERO; }
            set { iNUMERO = value; }
        }

        private string iSQL;

        public string ISQL
        {
            get { return iSQL; }
            set { iSQL = value; }
        }
    }
}
