/*
ModelName
dbModelName
KeyFieldType
KeyEexpression
DaoKeyFieldName
UpdateFields
Mapping_Fields_A
Mapping_Fields_B
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data;
using System.Data.Objects;
using System.Net;
using Begonia.Toyota.WebSite.Enums;
using Begonia.Toyota.WebSite.Models;
using Begonia.Toyota.WebSite.DbContext;
using System.Net.Mail;
using System.Configuration;
using Begonia.Toyota.WebSite.Library;

namespace Begonia.Toyota.WebSite.Service
{
    public class ContactCatalogManagerService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public ContactCatalogManagerModel NewInstance()
        {
            ContactCatalogManagerModel newOne = new ContactCatalogManagerModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }
        
        public ContactCatalogManagerListModel GetList(string CurrentUserName, ContactCatalogManagerListModel Page)
        {
           var o_query = from p in basedb.contact_catalog_manager
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                //姓名/電子信箱/回覆人員
                o_query =
                    o_query.Where(
                        p =>
                            p.name.Contains(Page.Search) || p.email.Contains(Page.Search) ||
                            p.answer_name.Contains(Page.Search));
            }

            if (Page.SStatus != 0)
            {
                o_query = o_query.Where(p => p.status == Page.SStatus);
            }

            if (!string.IsNullOrEmpty(Page.CDate) && !string.IsNullOrEmpty(Page.RDate))
            {
                DateTime CS = Convert.ToDateTime(Page.CDate);
                DateTime CE = Convert.ToDateTime(Page.CDate).AddDays(1);

                DateTime RS = Convert.ToDateTime(Page.RDate);
                DateTime RE = Convert.ToDateTime(Page.RDate).AddDays(1);

                o_query = o_query.Where(p => p.create_date >= CS && p.create_date < CE 
                                         && p.answer_date.Value >= RS && p.answer_date.Value < RE);
            }else if (!string.IsNullOrEmpty(Page.CDate))
            {
                DateTime CS = Convert.ToDateTime(Page.CDate);
                DateTime CE = Convert.ToDateTime(Page.CDate).AddDays(1);

                o_query = o_query.Where(p => p.create_date >= CS && p.create_date < CE);
            }
            else if (!string.IsNullOrEmpty(Page.RDate))
            {
                DateTime RS = Convert.ToDateTime(Page.RDate);
                DateTime RE = Convert.ToDateTime(Page.RDate).AddDays(1);

                o_query = o_query.Where(p => p.answer_date.Value >= RS && p.answer_date.Value < RE);
            }

            switch (Page.OrderBy)
            {
                case 1:
                    if (Page.OrderField == "n")
                    {
                        o_query = o_query.OrderBy(p => p.name);
                    }
                    else if (Page.OrderField == "e")
                    {
                        o_query = o_query.OrderBy(p => p.email);
                    }
                    else
                    {
                        o_query = o_query.OrderByDescending(p => p.create_date);
                    }
                    Page.OrderBy = 2;
                    break;
                case 2:
                    if (Page.OrderField == "n")
                    {
                        o_query = o_query.OrderByDescending(p => p.name);
                    }
                    else if (Page.OrderField == "e")
                    {
                        o_query = o_query.OrderByDescending(p => p.email);
                    }
                    else
                    {
                        o_query = o_query.OrderByDescending(p => p.create_date);
                    }
                    Page.OrderBy = 1;
                    break;
                default:
                    o_query = o_query.OrderByDescending(p => p.create_date);
                    break;
            }

            var query = o_query;

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new ContactCatalogManagerModel()
                    {
						Id = o_entity.Id,
						Name = o_entity.name,
						Email = o_entity.email,
						AnswerDate = o_entity.answer_date,
						ServiceUnits = o_entity.service_units,
						Tel = o_entity.tel,
						Address = o_entity.address,
						HaveBrand = o_entity.have_brand,
						WantCatalog = o_entity.want_catalog,
						Remark = o_entity.remark,
						Answer = o_entity.answer,
						CreateDate = o_entity.create_date,
						CreateId = o_entity.create_id,
						AnswerName = o_entity.answer_name,
                        Status = o_entity.status
                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        public bool Create1(ContactCatalogManagerModel1 model)
        {

            contact_catalog_manager dbEntity = new contact_catalog_manager();
          
            dbEntity.name = model.Namea;
            dbEntity.email = model.Email;
            dbEntity.service_units = model.ServiceUnit;
            dbEntity.tel = model.Tel;
            dbEntity.address = model.selectCity+ model.selectArea + model.Address;
            if (model.ck!=null)
            {
                if (model.ck.Any())
                {
                    dbEntity.have_brand = string.Join(",", model.ck);
                }
                else
                {
                    dbEntity.have_brand = string.Empty;
                }
            }
            else
            {
                dbEntity.have_brand = string.Empty;
            }
            dbEntity.want_catalog = model.selectPt + "/" + model.selectName;
            dbEntity.remark = model.ReMarks;
            dbEntity.create_date = DateTime.Now;
            dbEntity.create_id = model.Namea;
            dbEntity.status = 1;

            basedb.contact_catalog_manager.Add(dbEntity);

            try
            {

                basedb.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool Create(string UserName, ContactCatalogManagerModel model, out string ErrMsgs)
        {


            ErrMsgs = string.Empty;

            contact_catalog_manager dbEntity = new contact_catalog_manager();
			dbEntity.Id = model.Id;
			dbEntity.name = model.Name;
			dbEntity.email = model.Email;
			dbEntity.answer_date = model.AnswerDate;
			dbEntity.service_units = model.ServiceUnits;
			dbEntity.tel = model.Tel;
			dbEntity.address = model.Address;
			dbEntity.have_brand = model.HaveBrand;
			dbEntity.want_catalog = model.WantCatalog;
			dbEntity.remark = model.Remark;
			dbEntity.answer = model.Answer;
			dbEntity.create_date = model.CreateDate;
            dbEntity.create_id = UserName;
			dbEntity.answer_name = model.AnswerName;
            dbEntity.status = 1;

            basedb.contact_catalog_manager.Add(dbEntity);

            try
            {

                basedb.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ErrMsgs = ex.InnerException.Message;
                }
                else
                {
                    ErrMsgs = ex.Message;
                }
            }

            return ErrMsgs.Length == 0;
        }

		public ContactCatalogManagerModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.contact_catalog_manager
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new ContactCatalogManagerModel()
                    {
						Id = o_entity.Id,
						Name = o_entity.name,
						Email = o_entity.email,
						AnswerDate = o_entity.answer_date,
						ServiceUnits = o_entity.service_units,
						Tel = o_entity.tel,
						Address = o_entity.address,
						HaveBrand = o_entity.have_brand,
						WantCatalog = o_entity.want_catalog,
						Remark = o_entity.remark,
						Answer = o_entity.answer,
						CreateDate = o_entity.create_date,
						CreateId = o_entity.create_id,
						AnswerName = o_entity.answer_name,
                        Status = o_entity.status,
                    };

                var d1 = (ProductType)Convert.ToInt16(model.WantCatalog.Split('/').FirstOrDefault());
                var d2 = model.WantCatalog.Split('/').LastOrDefault();
                model.WantCatalog = d1.ToString() + "/" + d2;

                return model;
            }
            return null;
        }


		public bool Update(string userId, string userAccount, ContactCatalogManagerModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            ProductType pt = (ProductType)Enum.Parse(typeof(ProductType), model.WantCatalog.Split('/').FirstOrDefault(), true);
            var d2 = model.WantCatalog.Split('/').LastOrDefault();
            model.WantCatalog = (int)pt + "/" + d2;

            try
            {
                contact_catalog_manager o_entity = new contact_catalog_manager()
                {
					Id = model.Id,
					name = model.Name,
					email = model.Email,
					answer_date = model.AnswerDate,
					service_units = model.ServiceUnits,
					tel = model.Tel,
					address = model.Address,
					have_brand = model.HaveBrand,
					want_catalog = model.WantCatalog,
					remark = model.Remark,
					answer = model.Answer,
					create_date = model.CreateDate,
					create_id = model.CreateId,
					answer_name = model.AnswerName,
                    status = model.Status
                };
				
                basedb.contact_catalog_manager.Attach(o_entity);
				basedb.Entry(o_entity).Property(x => x.name).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.email).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.service_units).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.create_date).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.create_id).IsModified = true;

                basedb.Entry(o_entity).State = EntityState.Modified;
                basedb.SaveChanges();

                string body = "回覆內容: " + model.Answer + "<br />";
                Utils.SendMailByGmail(model.Email, "【索取型錄】台灣豐田產業機械 客服回覆", body);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ErrMsgs = ex.InnerException.Message;
                }
                else
                {
                    ErrMsgs = ex.Message;
                }
            }

            return ErrMsgs.Length == 0;    
        }

        public string mailSender = ConfigurationManager.AppSettings["mailSender"];
        public string smtpHost = ConfigurationManager.AppSettings["smtpHost"];
        public string smtpPort = ConfigurationManager.AppSettings["smtpPort"];
        public string networkCredentialUserName = ConfigurationManager.AppSettings["networkCredentialUserName"];
        public string networkCredentialPassword = ConfigurationManager.AppSettings["networkCredentialPassword"];
        
        public bool Delete(string userName, int? id, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;

            var o_delete = from p in basedb.contact_catalog_manager
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.contact_catalog_manager.Remove(row);
            }

            try
            {
                basedb.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }

            return ErrorMsg.Length == 0;
        }

        
   }
}