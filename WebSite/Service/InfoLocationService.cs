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
using System.Web.Mvc;

namespace Begonia.Toyota.WebSite.Service
{
    public class InfoLocationService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public InfoLocationModel NewInstance()
        {
            InfoLocationModel newOne = new InfoLocationModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public List<SelectListItem> GetLocation(string lid)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = Enum.GetValues(typeof(Loaction)).Cast<Loaction>().Where(v => v != Loaction.All).Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString(),
                Selected = (lid == ((int)v).ToString())
            }).ToList();
            return list;
        }

        public InfoLocationListModel GetList(string CurrentUserName, InfoLocationListModel Page, bool show = false)
        {
           var o_query = from p in basedb.info_location
                          select p;

            if (show)
            {
                o_query = o_query.Where(p => p.status == true);
            }

            if (!string.IsNullOrEmpty(Page.Search))
            {
                //標題
                o_query = o_query.Where(p => p.name.Contains(Page.Search));
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
                    if (Page.OrderField == "n")
                    {
                        o_query = o_query.OrderBy(p => p.name);
                    }
                    else if (Page.OrderField == "l")
                    {
                        o_query = o_query.OrderBy(p => p.location);
                    }
                    else
                    {
                        o_query = o_query.OrderBy(p => p.orderfield).ThenByDescending(p => p.create_time);
                    }
                    Page.OrderBy = 2;
                    break;
                case 2:
                    if (Page.OrderField == "n")
                    {
                        o_query = o_query.OrderByDescending(p => p.name);
                    }
                    else if (Page.OrderField == "l")
                    {
                        o_query = o_query.OrderByDescending(p => p.location);
                    }
                    else
                    {
                        o_query = o_query.OrderBy(p => p.orderfield).ThenByDescending(p => p.create_time);
                    }
                    Page.OrderBy = 1;
                    break;
                default:
                    Page.OrderBy++;
                    o_query = o_query.OrderByDescending(p => p.create_time);
                    break;
            }

            var query = o_query;

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new InfoLocationModel()
                    {
						Id = o_entity.Id,
						Name = o_entity.name,
						Location = o_entity.location,
						Address = o_entity.address,
						Tel = o_entity.tel,
						Fax = o_entity.fax,
                        Orderby = show ? (o_entity.orderfield == 0 ? 999 : o_entity.orderfield): o_entity.orderfield,
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

        

        public bool Create(string UserName, InfoLocationModel model, out string ErrMsgs)
        {


            ErrMsgs = string.Empty;

            info_location dbEntity = new info_location();
			dbEntity.Id = model.Id;
			dbEntity.name = model.Name;
			dbEntity.location = model.Location;
			dbEntity.address = model.Address;
			dbEntity.tel = model.Tel;
			dbEntity.fax = model.Fax;
            dbEntity.orderfield = model.Orderby;
			dbEntity.status = model.Status;
			dbEntity.create_time = model.CreateTime;
            dbEntity.create_id = UserName;


            basedb.info_location.Add(dbEntity);

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

		public InfoLocationModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.info_location
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new InfoLocationModel()
                    {
						Id = o_entity.Id,
						Name = o_entity.name,
						Location = o_entity.location,
						Address = o_entity.address,
						Tel = o_entity.tel,
						Fax = o_entity.fax,
                        Orderby = o_entity.orderfield,
						Status = o_entity.status,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,

                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, InfoLocationModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;



            try
            {
                info_location o_entity = new info_location()
                {
					Id = model.Id,
					name = model.Name,
					location = model.Location,
					address = model.Address,
					tel = model.Tel,
					fax = model.Fax,
                    orderfield = model.Orderby,
					status = model.Status,
					create_time = model.CreateTime,
					create_id = model.CreateId,

                };
				
                basedb.info_location.Attach(o_entity);
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

            var o_delete = from p in basedb.info_location
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.info_location.Remove(row);
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