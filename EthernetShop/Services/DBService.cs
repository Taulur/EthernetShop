using EthernetShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthernetShop.Services
{
    public class DBService
    {
        private EthernetShopContext context;
        public EthernetShopContext Context => context;
        private static DBService? instance;
        public static DBService Instance
        {
            get
            {
                if (instance == null)
                    instance = new DBService();
                return instance;
            }
        }
        private DBService()
        {
            context = new EthernetShopContext();
        }
    }
}
