using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Begonia.Toyota.WebSite.Controllers;
using Begonia.Toyota.WebSite.DbContext;
using JamZoo.Project.WebSite.Enums;

namespace Begonia.Toyota.WebSite.Service
{
    public class BaseService
    {
        public toyota_machineryEntities basedb;

        public string INIT_ACCOUNT = "Admin";
        public string INTI_PERMISSION = "Admin";

        public BaseService()
        {
            basedb = new toyota_machineryEntities();
        }

        public bool reOrder(BaseController.DataModel r, FN fn)
        {
            try
            {
                switch (fn)
                {
                    case FN.AUsedCarInfo:
                        foreach (var item in r.data)
                        {
                            int id = Convert.ToInt32(item.key);
                            var o_entity = (from p in basedb.a_used_car_info
                                            where p.Id == id
                                            select p).FirstOrDefault();
                            o_entity.orderfield = Convert.ToInt32(item.value);
                            basedb.SaveChanges();
                        }
                        break;
                    case FN.AUsedCarEngine:
                        foreach (var item in r.data)
                        {
                            var o_entity = (from p in basedb.a_used_car_engine
                                            where p.Id == item.key
                                            select p).FirstOrDefault();
                            o_entity.orderfield = Convert.ToInt32(item.value);
                            basedb.SaveChanges();
                        }
                        break;
                    case FN.IndexBanner:
                        foreach (var item in r.data)
                        {
                            int id = Convert.ToInt32(item.key);
                            var o_entity = (from p in basedb.index_banner
                                            where p.Id == id
                                            select p).FirstOrDefault();
                            o_entity.orderfield = Convert.ToInt32(item.value);
                            basedb.SaveChanges();
                        }
                        break;
                    case FN.InfoLocation:
                        foreach (var item in r.data)
                        {
                            int id = Convert.ToInt32(item.key);
                            var o_entity = (from p in basedb.info_location
                                            where p.Id == id
                                            select p).FirstOrDefault();
                            o_entity.orderfield = Convert.ToInt32(item.value);
                            basedb.SaveChanges();
                        }
                        break;
                    case FN.InfoNews:
                        foreach (var item in r.data)
                        {
                            int id = Convert.ToInt32(item.key);
                            var o_entity = (from p in basedb.info_news
                                            where p.Id == id
                                            select p).FirstOrDefault();
                            o_entity.orderfield = Convert.ToInt32(item.value);
                            basedb.SaveChanges();
                        }
                        break;
                    case FN.InfoPrize:
                        foreach (var item in r.data)
                        {
                            int id = Convert.ToInt32(item.key);
                            var o_entity = (from p in basedb.info_prize
                                            where p.Id == id
                                            select p).FirstOrDefault();
                            o_entity.orderfield = Convert.ToInt32(item.value);
                            basedb.SaveChanges();
                        }
                        break;
                    case FN.ProductAccessories:
                        foreach (var item in r.data)
                        {
                            int id = Convert.ToInt32(item.key);
                            var o_entity = (from p in basedb.product_accessories
                                            where p.Id == id
                                            select p).FirstOrDefault();
                            o_entity.orderfield = Convert.ToInt32(item.value);
                            basedb.SaveChanges();
                        }
                        break;
                    case FN.ProductInfo:
                        foreach (var item in r.data)
                        {
                            int id = Convert.ToInt32(item.key);
                            var o_entity = (from p in basedb.product_info
                                            where p.Id == id
                                            select p).FirstOrDefault();
                            o_entity.orderfield = Convert.ToInt32(item.value);
                            basedb.SaveChanges();
                        }
                        break;
                    case FN.QuestionAndAnswer:
                        foreach (var item in r.data)
                        {
                            int id = Convert.ToInt32(item.key);
                            var o_entity = (from p in basedb.question_and_answer
                                            where p.Id == id
                                            select p).FirstOrDefault();
                            o_entity.orderfield = Convert.ToInt32(item.value);
                            basedb.SaveChanges();
                        }
                        break;
                    default:break;
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
                throw;
            }
        }

        public List<string> GetHaveRoles(string account)
        {
            var havelist = from p in basedb.T_Permission
                           join m in basedb.T_Account_Permission_Mapping on p.Id equals m.PermissionId
                           where m.AccountId == account
                           select p.Name;
            return havelist.ToList();
        }
    }
}