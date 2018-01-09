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
    public class IndexBannerService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public IndexBannerModel NewInstance()
        {
            IndexBannerModel newOne = new IndexBannerModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public IndexBannerListModel GetList(string CurrentUserName, IndexBannerListModel Page, bool show = false)
        {
           var o_query = from p in basedb.index_banner
                          select p;

            if (show)
            {
                o_query = o_query.Where(p => p.status == show);
            }

            if (!string.IsNullOrEmpty(Page.Search))
            {
                //Banner大標題
                o_query = o_query.Where(p => p.banner_title.Contains(Page.Search));
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
                    o_query = o_query.OrderBy(p => p.banner_title);
                    Page.OrderBy = 2;
                    break;
                case 2:
                    o_query = o_query.OrderByDescending(p => p.banner_title);
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
                    new IndexBannerModel()
                    {
						Id = o_entity.Id,
						BannerTitle = o_entity.banner_title,
						BannerIntro = o_entity.banner_intro,
						BtnUrl = o_entity.btn_url,
						Img = o_entity.img,
                        Orderby = show ? (o_entity.orderfield == 0 ? 999 : o_entity.orderfield): o_entity.orderfield,
						Status = o_entity.status,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,

                    }
                    ).ToList();

                if (show)
                    Page.Data = Page.Data.OrderBy(p => p.Orderby).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        

        public bool Create(string UserName, IndexBannerModel model, out string ErrMsgs)
        {

            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<IndexBannerModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Img", model.ImgFile);
            }

            ErrMsgs = string.Empty;

            index_banner dbEntity = new index_banner();
			dbEntity.Id = model.Id;
			dbEntity.banner_title = model.BannerTitle;
			dbEntity.banner_intro = model.BannerIntro;
			dbEntity.btn_url = model.BtnUrl;
			dbEntity.img = model.Img;
            dbEntity.orderfield = model.Orderby;
			dbEntity.status = model.Status;
			dbEntity.create_time = model.CreateTime;
            dbEntity.create_id = UserName;


            basedb.index_banner.Add(dbEntity);

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

		public IndexBannerModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.index_banner
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new IndexBannerModel()
                    {
						Id = o_entity.Id,
						BannerTitle = o_entity.banner_title,
						BannerIntro = o_entity.banner_intro,
						BtnUrl = o_entity.btn_url,
						Img = o_entity.img,
                        Orderby = o_entity.orderfield,
						Status = o_entity.status,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,

                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, IndexBannerModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<IndexBannerModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Img", model.ImgFile);
            }


            try
            {
                index_banner o_entity = new index_banner()
                {
					Id = model.Id,
					banner_title = model.BannerTitle,
					banner_intro = model.BannerIntro,
					btn_url = model.BtnUrl,
					img = model.Img,
                    orderfield = model.Orderby,
					status = model.Status,
					create_time = model.CreateTime,
					create_id = model.CreateId,

                };
				
                basedb.index_banner.Attach(o_entity);
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

            var o_delete = from p in basedb.index_banner
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.index_banner.Remove(row);
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