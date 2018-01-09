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
using Begonia.Toyota.WebSite.Library;
using System.Web.Mvc;

namespace Begonia.Toyota.WebSite.Service
{
    public class AUsedCarEngineService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public AUsedCarEngineModel NewInstance()
        {
            AUsedCarEngineModel newOne = new AUsedCarEngineModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public AUsedCarEngineListModel GetList(string CurrentUserName, AUsedCarEngineListModel Page)
        {
           var o_query = from p in basedb.a_used_car_engine
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.type.Contains(Page.Search));
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
                    o_query = o_query.OrderBy(p => p.type);
                    Page.OrderBy = 2;
                    break;
                case 2:
                    o_query = o_query.OrderByDescending(p => p.type);
                    Page.OrderBy = 1;
                    break;
                default:
                    Page.OrderBy++;
                    o_query = o_query.OrderBy(p => p.orderfield).ThenByDescending(p => p.create_time);
                    break;
            }

            var query = o_query;

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new AUsedCarEngineModel()
                    {
						Id = o_entity.Id,
						Type = o_entity.type,
                        Orderby = o_entity.orderfield,
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

        

        public bool Create(string UserName, AUsedCarEngineModel model, out string ErrMsgs)
        {


            ErrMsgs = string.Empty;

            a_used_car_engine dbEntity = new a_used_car_engine();
            dbEntity.Id = Utils.GetObjectId();
			dbEntity.type = model.Type;
            dbEntity.orderfield = model.Orderby;
			dbEntity.status = model.Status;
			dbEntity.create_time = model.CreateTime;
            dbEntity.create_id = UserName;


            basedb.a_used_car_engine.Add(dbEntity);

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

        public AUsedCarEngineModel Get(string userId, string id)
        {
            var o_entity = (from p in basedb.a_used_car_engine
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new AUsedCarEngineModel()
                    {
						Id = o_entity.Id,
						Type = o_entity.type,
                        Orderby = o_entity.orderfield,
						Status = o_entity.status,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,

                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, AUsedCarEngineModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;



            try
            {
                a_used_car_engine o_entity = new a_used_car_engine()
                {
					Id = model.Id,
					type = model.Type,
                    orderfield = model.Orderby,
					status = model.Status,
					create_time = model.CreateTime,
					create_id = model.CreateId,

                };
				
                basedb.a_used_car_engine.Attach(o_entity);
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

        public bool Delete(string userName, string id, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;

            var o_delete = from p in basedb.a_used_car_engine
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.a_used_car_engine.Remove(row);
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