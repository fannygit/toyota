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
    public class InfoNewsService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public InfoNewsModel NewInstance()
        {
            InfoNewsModel newOne = new InfoNewsModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public void CreateLog(string UserName, string Action, int NewsId, string NewsTitle)
        {
            log_news n = new log_news();
            n.CreateTime = DateTime.Now;
            n.AccountId = UserName;
            n.Action = Action;
            n.NewsId = NewsId;
            n.NewsTitle = NewsTitle;
            basedb.log_news.Add(n);
            basedb.SaveChanges();
        }

        public InfoNewsListModel GetDicList(string CurrentUserName, InfoNewsListModel Page)
        {
            var o_query = from p in basedb.info_news
                          where p.status == true
                          select p;
            
            var query = o_query.OrderBy(p=>p.orderfield).OrderByDescending(p=>p.title_date);

            try
            {
                Page.TotalRecords = query.Select(p => p.Id).Count();
                Page.Data = query.Select(o_entity =>
                    new InfoNewsModel()
                    {
                        Id = o_entity.Id,
                        Title = o_entity.title,
                        TitleDate = o_entity.title_date,
                        ListImg = o_entity.list_img,
                        ListIntro = o_entity.list_intro,
                        Detail = o_entity.detail,
                        Orderby = o_entity.orderfield == 0 ? 999 : o_entity.orderfield,
                        Status = o_entity.status,
                        CreateTime = o_entity.create_time,
                        CreateId = o_entity.create_id,
                    }
                    ).ToList();


                Page.DicDataDL = new Dictionary<string, List<InfoNewsModel>>();
                Page.DicData = new Dictionary<string, List<InfoNewsModel>>();
                foreach (var i in Page.Data.OrderBy(p => p.Orderby).OrderByDescending(p => p.TitleDate))
                {
                    string nowKey = i.TitleDate.Year.ToString();
                    if (Page.DicDataDL.ContainsKey(nowKey))
                    {
                        var list = Page.DicDataDL[nowKey];
                        list.Add(i);
                        Page.DicDataDL[nowKey] = list;
                    }
                    else
                    {
                        List<InfoNewsModel> list = new List<InfoNewsModel>();
                        list.Add(i);
                        Page.DicDataDL.Add(nowKey, list);
                    }
                }

                if (!string.IsNullOrEmpty(Page.yy))
                {
                    int yy = Convert.ToInt32(Page.yy);
                    foreach (var i in Page.Data.Where(p=>p.TitleDate.Year == yy))
                    {
                        string nowKey = i.TitleDate.Year.ToString();
                        if (Page.DicData.ContainsKey(nowKey))
                        {
                            var list = Page.DicData[nowKey];
                            list.Add(i);
                            Page.DicData[nowKey] = list;
                        }
                        else
                        {
                            List<InfoNewsModel> list = new List<InfoNewsModel>();
                            list.Add(i);
                            Page.DicData.Add(nowKey, list);
                        }
                    }
                }
                else
                {
                    Page.DicData = Page.DicDataDL;
                }
                

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }


        public InfoNewsListModel GetList(string CurrentUserName, InfoNewsListModel Page, bool show = false)
        {
           var o_query = from p in basedb.info_news
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.title.Contains(Page.Search));
            }

            if (show)
            {
                o_query = o_query.Where(p => p.status == show);
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
                    else if (Page.OrderField == "d")
                    {
                        o_query = o_query.OrderBy(p => p.detail);
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
                    else if (Page.OrderField == "d")
                    {
                        o_query = o_query.OrderByDescending(p => p.detail);
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
                    new InfoNewsModel()
                    {
						Id = o_entity.Id,
						Title = o_entity.title,
						TitleDate = o_entity.title_date,
						ListImg = o_entity.list_img,
						ListIntro = o_entity.list_intro,
						Detail = o_entity.detail,
                        Orderby = show ? (o_entity.orderfield == 0 ? 999 : o_entity.orderfield) : o_entity.orderfield,
						Status = o_entity.status,
						CreateTime = o_entity.create_time,
						CreateId = o_entity.create_id,

                    }
                    ).ToList();

                if (show)
                    Page.Data = Page.Data.OrderBy(p => p.Orderby).Take(4).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        

        public bool Create(string UserName, InfoNewsModel model, out string ErrMsgs)
        {
            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<InfoNewsModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ListImg", model.ImgFile);
            }

            ErrMsgs = string.Empty;

            info_news dbEntity = new info_news();
			dbEntity.Id = model.Id;
			dbEntity.title = model.Title;
			dbEntity.title_date = model.TitleDate;
			dbEntity.list_img = model.ListImg;
			dbEntity.list_intro = string.Empty;
			dbEntity.detail = model.Detail;
            dbEntity.orderfield = model.Orderby;
			dbEntity.status = model.Status;
			dbEntity.create_time = DateTime.Now;
			dbEntity.create_id = UserName;


            basedb.info_news.Add(dbEntity);

            try
            {

                basedb.SaveChanges();
                CreateLog(UserName, "新增", dbEntity.Id, model.Title);
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

     


        public InfoNewsModel Get1(string userId, int? id)
        {
            var o_entity = (from p in basedb.info_news
                            where p.Id == id && p.status == true
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new InfoNewsModel()
                {
                    Id = o_entity.Id,
                    Title = o_entity.title,
                    TitleDate = o_entity.title_date,
                    ListImg = o_entity.list_img,
                    ListIntro = o_entity.list_intro,
                    Detail = o_entity.detail,
                    Orderby = o_entity.orderfield,
                    Status = o_entity.status,
                    CreateTime = o_entity.create_time,
                    CreateId = o_entity.create_id,
                };


                var all = (from p in basedb.info_news
                           where p.status == true
                           orderby p.title_date descending
                           select p.Id).ToArray();

                int nowIndex = Array.IndexOf(all, id);
                if (nowIndex == 0)//第一筆
                {
                    model.previousId = all[nowIndex];
                    model.nextId = all[nowIndex + 1];
                }
                else if (nowIndex == (all.Length - 1))//最後一筆
                {
                    model.previousId = all[nowIndex - 1];
                    model.nextId = all[nowIndex];
                }
                else
                {
                    model.previousId = all[nowIndex - 1];
                    model.nextId = all[nowIndex + 1];
                }

                

                return model;
            }
            return null;
        }

        public InfoNewsModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.info_news
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new InfoNewsModel()
                    {
						Id = o_entity.Id,
						Title = o_entity.title,
						TitleDate = o_entity.title_date,
						ListImg = o_entity.list_img,
						ListIntro = o_entity.list_intro,
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

		public bool Update(string userId, string userAccount, InfoNewsModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<InfoNewsModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "ListImg", model.ImgFile);
            }

            try
            {
                info_news o_entity = new info_news()
                {
					Id = model.Id,
					title = model.Title,
					title_date = model.TitleDate,
					list_img = model.ListImg,
					list_intro = string.Empty,
					detail = model.Detail,
                    orderfield = model.Orderby,
					status = model.Status,
                    create_time = model.CreateTime,
					create_id = model.CreateId,

                };
				
                basedb.info_news.Attach(o_entity);
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

            var o_delete = from p in basedb.info_news
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.info_news.Remove(row);
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