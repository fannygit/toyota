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
    public class InfoPrizeService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public InfoPrizeModel NewInstance()
        {
            InfoPrizeModel newOne = new InfoPrizeModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public List<string> GetYears1(string yy)
        {
            var o_query = from p in basedb.info_prize
                          where p.status ==true
                          select p;

            var list = o_query.Select(p => p.years).Distinct().OrderBy(p=>p).ToList();

            if (!string.IsNullOrEmpty(yy))
            {
                list.Remove(yy);
                list.Add(yy);
                list.Reverse();
            }

            return list;
        }

        public List<InfoPrizeModel> GetList1(string yy,bool show = false)
        {
            List<InfoPrizeModel> list = new List<InfoPrizeModel>();

            var o_query = from p in basedb.info_prize
                          select p;

            if (show)
            {
                o_query = o_query.Where(p => p.status == true);
            }

            if (!string.IsNullOrEmpty(yy))
            {
                o_query = o_query.Where(p => p.years.Equals(yy));
            }

            var query = o_query;

            try
            {
                list = query.Select(o_entity =>
                    new InfoPrizeModel()
                    {
                        Id = o_entity.Id,
                        Title = o_entity.title,
                        Years = o_entity.years,
                        Img = o_entity.img,
                        Orderby = o_entity.orderfield == 0 ? 999 : o_entity.orderfield,
                        Status = o_entity.status,
                        CreateTime = o_entity.create_time,
                        CreateId = o_entity.create_id,
                    }
                    ).ToList();

                //if (!string.IsNullOrEmpty(yy))
                //{
                //    var ri = list.Where(p => p.Years == yy).FirstOrDefault();
                //    list.Remove(ri);
                //    list.Add(ri);
                //    list.Reverse();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }


        public InfoPrizeListModel GetList(string CurrentUserName, InfoPrizeListModel Page)
        {
           var o_query = from p in basedb.info_prize
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.title.Contains(Page.Search));
            }
            if (!string.IsNullOrEmpty(Page.YearsDT))
            {
                o_query = o_query.Where(p => p.years == Page.YearsDT);
            }
            if (!string.IsNullOrEmpty(Page.SD) && !string.IsNullOrEmpty(Page.ED))
            {
                DateTime SD = Convert.ToDateTime(Page.SD);
                DateTime ED = Convert.ToDateTime(Page.ED);
                o_query = o_query.Where(p => p.create_time >= EntityFunctions.TruncateTime(SD) && p.create_time <= EntityFunctions.TruncateTime(ED));
            }
            else if (!string.IsNullOrEmpty(Page.SD))
            {
                DateTime dt = Convert.ToDateTime(Page.SD);
                o_query = o_query.Where(p => p.create_time >= dt);
            }
            else if (!string.IsNullOrEmpty(Page.ED))
            {
                DateTime dt = Convert.ToDateTime(Page.ED);
                o_query = o_query.Where(p => p.create_time <= dt);
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
                    if (Page.OrderField == "t")
                    {
                        o_query = o_query.OrderBy(p => p.title);
                    }
                    else if (Page.OrderField == "y")
                    {
                        o_query = o_query.OrderBy(p => p.years);
                    }
                    else
                    {
                        o_query = o_query.OrderByDescending(p => p.create_time);
                    }
                    Page.OrderBy = 2;
                    break;
                case 2:
                    if (Page.OrderField == "t")
                    {
                        o_query = o_query.OrderByDescending(p => p.title);
                    }
                    else if (Page.OrderField == "y")
                    {
                        o_query = o_query.OrderByDescending(p => p.years);
                    }
                    else
                    {
                        o_query = o_query.OrderByDescending(p => p.create_time);
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
                    new InfoPrizeModel()
                    {
						Id = o_entity.Id,
						Title = o_entity.title,
						Years = o_entity.years,
                        YearsDT= o_entity.years,
						Img = o_entity.img,
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

        

        public bool Create(string UserName, InfoPrizeModel model, out string ErrMsgs)
        {
            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<InfoPrizeModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Img", model.ImgFile);
            }

            ErrMsgs = string.Empty;

            info_prize dbEntity = new info_prize();
			dbEntity.Id = model.Id;
			dbEntity.title = model.Title;
			dbEntity.years = model.YearsDT;
			dbEntity.img = model.Img;
            dbEntity.orderfield = model.Orderby;
			dbEntity.status = model.Status;
			dbEntity.create_time = model.CreateTime;
			dbEntity.create_id = UserName;


            basedb.info_prize.Add(dbEntity);

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

		public InfoPrizeModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.info_prize
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new InfoPrizeModel()
                    {
						Id = o_entity.Id,
						Title = o_entity.title,
						Years = o_entity.years.ToString(),
						Img = o_entity.img,
                        Orderby = o_entity.orderfield,
						Status = o_entity.status,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,
                        YearsDT = o_entity.years.ToString()
                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, InfoPrizeModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<InfoPrizeModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Img", model.ImgFile);
            }

            try
            {
                info_prize o_entity = new info_prize()
                {
					Id = model.Id,
					title = model.Title,
					years = model.YearsDT,
					img = model.Img,
                    orderfield = model.Orderby,
					status = model.Status,
					create_time = model.CreateTime,
					create_id = model.CreateId,

                };
				
                basedb.info_prize.Attach(o_entity);
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

            var o_delete = from p in basedb.info_prize
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.info_prize.Remove(row);
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