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
    public class IndexVideoService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public IndexVideoModel NewInstance()
        {
            IndexVideoModel newOne = new IndexVideoModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public IndexVideoListModel GetList(string CurrentUserName, IndexVideoListModel Page)
        {
           var o_query = from p in basedb.index_video
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                //影片標題
                o_query = o_query.Where(p => p.video_title.Contains(Page.Search));
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
                    o_query = o_query.OrderBy(p => p.video_title);
                    Page.OrderBy = 2;
                    break;
                case 2:
                    o_query = o_query.OrderByDescending(p => p.video_title);
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
                    new IndexVideoModel()
                    {
						Id = o_entity.Id,
						VideoTitle = o_entity.video_title,
						VideoIntro = o_entity.video_intro,
						ReviewUrl = o_entity.review_url,
						FullUrl = o_entity.full_url,
						MobileImg = o_entity.mobile_img,
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

        

        public bool Create(string UserName, IndexVideoModel model, out string ErrMsgs)
        {
            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<IndexVideoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "MobileImg", model.ImgFile);
            }

            ErrMsgs = string.Empty;

            index_video dbEntity = new index_video();
			dbEntity.Id = model.Id;
			dbEntity.video_title = model.VideoTitle;
			dbEntity.video_intro = model.VideoIntro;
			dbEntity.review_url = model.ReviewUrl;
			dbEntity.full_url = model.FullUrl;
			dbEntity.mobile_img = model.MobileImg;
			dbEntity.status = model.Status;
			dbEntity.create_time = DateTime.Now;
            dbEntity.create_id = UserName;


            basedb.index_video.Add(dbEntity);

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

        public IndexVideoModel Get1(int id)
        {
            var o_entity = (from p in basedb.index_video
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new IndexVideoModel()
                {
                    Id = o_entity.Id,
                    VideoTitle = o_entity.video_title,
                    VideoIntro = o_entity.video_intro,
                    ReviewUrl = o_entity.review_url,
                    FullUrl = o_entity.full_url,
                    MobileImg = o_entity.mobile_img,
                    Status = o_entity.status,
                    CreateTime = o_entity.create_time,
                    CreateId = o_entity.create_id,

                };

                return model;
            }
            return null;
        }

		public IndexVideoModel Get(string userId, int? id , bool show = false)
        {
            var o_entity = (from p in basedb.index_video
                            select p).FirstOrDefault();

            if (show)
            {
                if (o_entity.status == false)
                    o_entity = null;
            }

            if (o_entity != null)
            {
                var model = new IndexVideoModel()
                    {
						Id = o_entity.Id,
						VideoTitle = o_entity.video_title,
						VideoIntro = o_entity.video_intro,
						ReviewUrl = o_entity.review_url,
						FullUrl = o_entity.full_url,
						MobileImg = o_entity.mobile_img,
						Status = o_entity.status,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,

                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, IndexVideoModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<IndexVideoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "MobileImg", model.ImgFile);
            }

            try
            {
                index_video o_entity = new index_video()
                {
					Id = model.Id,
					video_title = model.VideoTitle,
					video_intro = model.VideoIntro,
					review_url = model.ReviewUrl,
					full_url = model.FullUrl,
					mobile_img = model.MobileImg,
					status = model.Status,
					create_time = model.CreateTime,
					create_id = model.CreateId,

                };
				
                basedb.index_video.Attach(o_entity);
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

            var o_delete = from p in basedb.index_video
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.index_video.Remove(row);
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