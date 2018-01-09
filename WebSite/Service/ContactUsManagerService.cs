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
    public class ContactUsManagerService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public ContactUsManagerModel NewInstance()
        {
            ContactUsManagerModel newOne = new ContactUsManagerModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public ContactUsManagerListModel GetList(string CurrentUserName, ContactUsManagerListModel Page)
        {
           var o_query = from p in basedb.contact_us_manager
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                //姓名/電子信箱
                o_query = o_query.Where(p => p.name.Contains(Page.Search) || p.email.Contains(Page.Search));
            }

            if (Page.SStatus != 0)
            {
                o_query = o_query.Where(p => p.status == Page.SStatus);
            }

            if (!string.IsNullOrEmpty(Page.SD))
            {
                DateTime dtS = Convert.ToDateTime(Page.SD);
                DateTime dtE = Convert.ToDateTime(Page.SD).AddDays(1);
                o_query = o_query.Where(p => p.create_date >= dtS && p.create_date < dtE);
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
                    new ContactUsManagerModel()
                    {
						Id = o_entity.Id,
						Name = o_entity.name,
						Email = o_entity.email,
						AnswerDate = o_entity.answer_date,
						ServiceUnits = o_entity.service_units,
						Tel = o_entity.tel,
						ProblemInfo = o_entity.problem_info,
						Answer = o_entity.answer,
						Status = o_entity.status,
						CreateDate = o_entity.create_date,
						CreateId = o_entity.create_id,
                        AnswerName = o_entity.answer_name
                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        public bool Create1(ContactUsManagerModel1 model)
        {
            
            contact_us_manager dbEntity = new contact_us_manager();
          
            dbEntity.name = model.Namea;
            dbEntity.email = model.Email;
            dbEntity.service_units = model.ServiceUnit;
            dbEntity.tel = model.Tel;
            dbEntity.problem_info = model.Detail;
            dbEntity.status = 1;
            dbEntity.create_date = DateTime.Now;
            dbEntity.create_id = model.Namea;


            basedb.contact_us_manager.Add(dbEntity);

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

        public bool Create(string UserName, ContactUsManagerModel model, out string ErrMsgs)
        {


            ErrMsgs = string.Empty;

            contact_us_manager dbEntity = new contact_us_manager();
			dbEntity.Id = model.Id;
			dbEntity.name = model.Name;
			dbEntity.email = model.Email;
			dbEntity.answer_date = model.AnswerDate;
			dbEntity.service_units = model.ServiceUnits;
			dbEntity.tel = model.Tel;
			dbEntity.problem_info = model.ProblemInfo;
			dbEntity.answer = model.Answer;
			dbEntity.status = model.Status;
			dbEntity.create_date = model.CreateDate;
            dbEntity.create_id = UserName;


            basedb.contact_us_manager.Add(dbEntity);

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

		public ContactUsManagerModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.contact_us_manager
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new ContactUsManagerModel()
                    {
						Id = o_entity.Id,
						Name = o_entity.name,
						Email = o_entity.email,
						AnswerDate = o_entity.answer_date,
						ServiceUnits = o_entity.service_units,
						Tel = o_entity.tel,
						ProblemInfo = o_entity.problem_info,
						Answer = o_entity.answer,
						Status = o_entity.status,
						CreateDate = o_entity.create_date,
						CreateId = o_entity.create_id,
                        AnswerName = o_entity.answer_name
                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, ContactUsManagerModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;
            
            try
            {
                contact_us_manager o_entity = new contact_us_manager()
                {
					Id = model.Id,
					name = model.Name,
					email = model.Email,
					answer_date = model.AnswerDate,
					service_units = model.ServiceUnits,
					tel = model.Tel,
					problem_info = model.ProblemInfo,
					answer = model.Answer,
					status = model.Status,
					create_date = model.CreateDate,
					create_id = model.CreateId,
                    answer_name = model.AnswerName

                };
				
                basedb.contact_us_manager.Attach(o_entity);
				basedb.Entry(o_entity).Property(x => x.name).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.email).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.service_units).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.tel).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.problem_info).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.create_date).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.create_id).IsModified = true;

                basedb.Entry(o_entity).State = EntityState.Modified;
                basedb.SaveChanges();

                string body = "回覆內容: " + model.Answer + "<br />";
                Utils.SendMailByGmail(model.Email, "【聯絡我們】台灣豐田產業機械 客服回覆", body);
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

            var o_delete = from p in basedb.contact_us_manager
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.contact_us_manager.Remove(row);
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