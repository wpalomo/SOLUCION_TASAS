using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIDADES
{
    public class ENTPasajeros
    {
        private string iCLIENTEFILTRO;

        public string ICLIENTEFILTRO
        {
            get { return iCLIENTEFILTRO; }
            set { iCLIENTEFILTRO = value; }
        }

        private string iFECHAFILTRO;

        public string IFECHAFILTRO
        {
            get { return iFECHAFILTRO; }
            set { iFECHAFILTRO = value; }
        }

        private string iIDCLIENTEFILTRO;

        public string IIDCLIENTEFILTRO
        {
            get { return iIDCLIENTEFILTRO; }
            set { iIDCLIENTEFILTRO = value; }
        }

        private string iIDENTIFICACIONFILTRO;

        public string IIDENTIFICACIONFILTRO
        {
            get { return iIDENTIFICACIONFILTRO; }
            set { iIDENTIFICACIONFILTRO = value; }
        }

        private string iSQL;

        public string ISQL
        {
            get { return iSQL; }
            set { iSQL = value; }
        }
    }
}
