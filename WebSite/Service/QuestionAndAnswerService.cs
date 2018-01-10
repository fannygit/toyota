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

using Begonia.Toyota.WebSite.Enums;
using Begonia.Toyota.WebSite.Models;
using Begonia.Toyota.WebSite.DbContext;

namespace Begonia.Toyota.WebSite.Service
{
    public class QuestionAndAnswerService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public QuestionAndAnswerModel NewInstance()
        {
            QuestionAndAnswerModel newOne = new QuestionAndAnswerModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public QuestionAndAnswerListModel GetList(string CurrentUserName, QuestionAndAnswerListModel Page, bool show = false)
        {
           var o_query = from p in basedb.question_and_answer
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                //標題
                o_query = o_query.Where(p => p.title.Contains(Page.Search));
            }

            if (show)
            {
                o_query = o_query.Where(p=>p.status ==true);
            }

            switch (Page.SStatus)
            {
                case 0:
                    o_query = o_query.Where(p => !p.status);
                    break;
                case 1:
                    o_query = o_query.Where(p => p.status);
                    break;
            }

            switch (Page.OrderBy)
            {
                case 1:
                    o_query = o_query.OrderBy(p => p.title);
                    Page.OrderBy = 2;
                    break;
                case 2:
                    o_query = o_query.OrderByDescending(p => p.title);
                    Page.OrderBy = 1;
                    break;
                default:
                    o_query = o_query.OrderByDescending(p=>p.create_time);
                    break;
            }

            var query = o_query;

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new QuestionAndAnswerModel()
                    {
						Id = o_entity.Id,
						Title = o_entity.title,
						Detail = o_entity.detail,
                        Orderby = show ? (o_entity.orderfield == 0 ? 999 : o_entity.orderfield) : o_entity.orderfield,
                        Status = o_entity.status,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,

                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        

        public bool Create(string UserName, QuestionAndAnswerModel model, out string ErrMsgs)
        {


            ErrMsgs = string.Empty;

            question_and_answer dbEntity = new question_and_answer();
			dbEntity.Id = model.Id;
			dbEntity.title = model.Title;
			dbEntity.detail = model.Detail;
            dbEntity.orderfield = model.Orderby;
			dbEntity.status = model.Status;
			dbEntity.create_time = DateTime.Now;
            dbEntity.create_id = UserName;


            basedb.question_and_answer.Add(dbEntity);

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

		public QuestionAndAnswerModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.question_and_answer
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new QuestionAndAnswerModel()
                    {
						Id = o_entity.Id,
						Title = o_entity.title,
						Detail = o_entity.detail,
                        Orderby = o_entity.orderfield,
						Status = o_entity.status,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,

                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, QuestionAndAnswerModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;



            try
            {
                question_and_answer o_entity = new question_and_answer()
                {
					Id = model.Id,
					title = model.Title,
					detail = model.Detail,
                    orderfield = model.Orderby,
					status = model.Status,
					create_time = model.CreateTime,
					create_id = model.CreateId,

                };
				
                basedb.question_and_answer.Attach(o_entity);
				basedb.Entry(o_entity).Property(x => x.title).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.create_time).IsModified = true;
				basedb.Entry(o_entity).Property(x => x.create_id).IsModified = true;

                basedb.Entry(o_entity).State = EntityState.Modified;
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

        public bool Delete(string userName, int? id, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;

            var o_delete = from p in basedb.question_and_answer
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.question_and_answer.Remove(row);
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